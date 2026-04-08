"""Hugging Face model vendor implementation."""

import json
import re
import time
from typing import List
import httpx
from models.base import BaseModelVendor
from config.constants import QuizQuestion, EvaluationResult


class HuggingFaceVendor(BaseModelVendor):
    """Hugging Face Inference API implementation."""

    ROUTER_CHAT_COMPLETIONS_URL = "https://router.huggingface.co/v1/chat/completions"
    CONNECT_TIMEOUT_SECONDS = 20.0
    READ_TIMEOUT_SECONDS = 180.0
    MAX_RETRIES = 3

    def __init__(self, api_key: str, model_id: str):
        """Initialize with Hugging Face API key and model ID."""
        super().__init__(api_key, model_id)

    def validate_credentials(self) -> bool:
        """
        Validate Hugging Face API credentials by attempting a simple request.
        
        Returns:
            True if credentials are valid
            
        Raises:
            Exception with descriptive error message
        """
        try:
            # Test with a minimal chat completion against router v1.
            self._chat_completion(
                messages=[{"role": "user", "content": "Reply with OK"}],
                max_tokens=5,
                temperature=0.0,
            )
            return True
        except Exception as e:
            error_text = str(e)
            if "404" in error_text:
                raise Exception(
                    "Selected Hugging Face model is not available via the current router path. "
                    "Please choose another Hugging Face model from the dropdown."
                )
            raise Exception(
                f"Hugging Face API validation failed: {error_text}. "
                f"Please verify your API key and model ID."
            )

    def generate_quiz(
        self,
        age_group: str,
        difficulty_level: str,
        field: str,
        num_questions: int,
        quiz_prompt: str
    ) -> List[QuizQuestion]:
        """
        Generate quiz questions using Hugging Face model.
        
        Args:
            age_group: Target age group
            difficulty_level: Difficulty level
            field: Subject field
            num_questions: Number of questions
            quiz_prompt: Formatted prompt for generation
            
        Returns:
            List of parsed QuizQuestion objects
        """
        try:
            # Keep token budget bounded to reduce long-running responses and timeout risk.
            generation_max_tokens = min(2200, max(700, num_questions * 220))
            response_text = self._generate_text_response(
                prompt=quiz_prompt,
                max_tokens=generation_max_tokens,
                temperature=0.7,
            )

            # Extract JSON from response (handle markdown code blocks)
            json_str = self._extract_json(response_text)
            quiz_data = json.loads(json_str)

            # Parse questions
            questions = []
            for idx, q_data in enumerate(quiz_data.get("questions", [])):
                question = QuizQuestion(
                    question_id=idx + 1,
                    question_text=q_data["question"],
                    options=q_data["options"],
                    correct_answer_index=q_data["correct_answer"]
                )
                questions.append(question)

            return questions

        except json.JSONDecodeError as e:
            raise Exception(
                f"Failed to parse quiz response as JSON: {str(e)}"
            )
        except Exception as e:
            raise Exception(
                f"Hugging Face quiz generation failed: {str(e)}"
            )

    def evaluate_answers(
        self,
        quiz_questions: List[QuizQuestion],
        user_answers: List[int],
        evaluation_prompt: str
    ) -> EvaluationResult:
        """
        Evaluate quiz answers using Hugging Face model.
        
        Args:
            quiz_questions: Quiz questions with correct answers
            user_answers: User's selected answer indices
            evaluation_prompt: Formatted evaluation prompt
            
        Returns:
            EvaluationResult with score and feedback
        """
        try:
            response_text = self._generate_text_response(
                prompt=evaluation_prompt,
                max_tokens=2048,
                temperature=0.5,
            )

            # Extract and parse evaluation JSON
            json_str = self._extract_json(response_text)
            eval_data = json.loads(json_str)

            result = EvaluationResult(
                score_percentage=eval_data.get("score_percentage", 0),
                knowledge_summary=eval_data.get("summary", ""),
                category_scores=eval_data.get("category_scores", {}),
                feedback=eval_data.get("feedback", "")
            )
            return result

        except json.JSONDecodeError as e:
            raise Exception(
                f"Failed to parse evaluation response as JSON: {str(e)}"
            )
        except Exception as e:
            raise Exception(
                f"Hugging Face evaluation failed: {str(e)}"
            )

    def _extract_json(self, text: str) -> str:
        """
        Extract JSON from text, handling markdown code blocks.
        
        Args:
            text: Text potentially containing JSON
            
        Returns:
            Extracted JSON string
            
        Raises:
            ValueError if no JSON found
        """
        # Try to find JSON in markdown code blocks
        match = re.search(r"```(?:json)?\s*([\s\S]*?)```", text)
        if match:
            return match.group(1).strip()

        # Otherwise assume the entire response is JSON
        return text.strip()

    def _generate_text_response(self, prompt: str, max_tokens: int, temperature: float) -> str:
        """
        Generate model output using Hugging Face router chat completions.
        """
        response = self._chat_completion(
            messages=[
                {"role": "system", "content": "Return only valid JSON."},
                {"role": "user", "content": prompt},
            ],
            max_tokens=max_tokens,
            temperature=temperature,
        )
        return response

    def _chat_completion(self, messages: List[dict], max_tokens: int, temperature: float) -> str:
        """Call Hugging Face router OpenAI-compatible chat completions endpoint."""
        headers = {
            "Authorization": f"Bearer {self.api_key}",
            "Content-Type": "application/json",
        }
        payload = {
            "model": self.model_id,
            "messages": messages,
            "max_tokens": max_tokens,
            "temperature": temperature,
        }

        timeout = httpx.Timeout(
            connect=self.CONNECT_TIMEOUT_SECONDS,
            read=self.READ_TIMEOUT_SECONDS,
            write=30.0,
            pool=30.0,
        )

        last_exception: Exception | None = None
        for attempt in range(1, self.MAX_RETRIES + 1):
            try:
                with httpx.Client(timeout=timeout) as client:
                    response = client.post(
                        self.ROUTER_CHAT_COMPLETIONS_URL,
                        headers=headers,
                        json=payload,
                    )
                    response.raise_for_status()
                    data = response.json()
                return data.get("choices", [{}])[0].get("message", {}).get("content", "") or ""
            except (httpx.ReadTimeout, httpx.ConnectTimeout, httpx.TransportError) as e:
                last_exception = e
                if attempt < self.MAX_RETRIES:
                    # Small exponential backoff for transient network/queue delays.
                    time.sleep(2 ** (attempt - 1))
                    continue
                raise Exception(
                    "Request timed out while waiting for Hugging Face model response. "
                    "Please retry or choose a faster/smaller model."
                ) from e
            except httpx.HTTPStatusError as e:
                # Non-transient API errors should surface immediately.
                raise Exception(f"{e.response.status_code} Client Error: {e.response.text}") from e

        if last_exception:
            raise Exception(str(last_exception))
        raise Exception("Unknown error while calling Hugging Face router.")
