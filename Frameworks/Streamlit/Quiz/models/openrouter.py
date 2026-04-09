"""OpenRouter model vendor implementation."""

import json
import os
import re
import ssl
from typing import List
import httpx
from models.base import BaseModelVendor
from config.constants import QuizQuestion, EvaluationResult


class OpenRouterVendor(BaseModelVendor):
    """OpenRouter API implementation."""

    OPENROUTER_API_URL = "https://openrouter.ai/api/v1/chat/completions"

    def __init__(self, api_key: str, model_id: str):
        """Initialize with OpenRouter API key and model ID."""
        super().__init__(api_key, model_id)

    def _get_tls_verify(self):
        """Build TLS verification config with optional custom CA bundle."""
        ca_bundle = os.getenv("OPENROUTER_CA_BUNDLE", "").strip()
        if ca_bundle:
            if not os.path.exists(ca_bundle):
                raise Exception(
                    f"OPENROUTER_CA_BUNDLE path not found: {ca_bundle}"
                )
            return ssl.create_default_context(cafile=ca_bundle)

        # Prefer OS trust store when truststore is available.
        try:
            import truststore
            return truststore.SSLContext(ssl.PROTOCOL_TLS_CLIENT)
        except Exception:
            return ssl.create_default_context()

    def validate_credentials(self) -> bool:
        """
        Validate OpenRouter API credentials.
        
        Returns:
            True if credentials are valid
            
        Raises:
            Exception with descriptive error message
        """
        try:
            headers = {
                "Authorization": f"Bearer {self.api_key}",
                "Content-Type": "application/json",
            }
            payload = {
                "model": self.model_id,
                "messages": [
                    {"role": "user", "content": "Hello"}
                ],
                "max_tokens": 10,
            }

            with httpx.Client(verify=self._get_tls_verify()) as client:
                response = client.post(
                    self.OPENROUTER_API_URL,
                    headers=headers,
                    json=payload,
                    timeout=30.0
                )
                response.raise_for_status()
            return True

        except httpx.TransportError as e:
            if "CERTIFICATE_VERIFY_FAILED" in str(e).upper():
                raise Exception(
                    "OpenRouter SSL certificate verification failed. "
                    "If your network uses a custom/corporate CA, set OPENROUTER_CA_BUNDLE "
                    "to your CA bundle path, or install the truststore package."
                )
            raise Exception(
                f"OpenRouter API validation failed: {str(e)}. "
                f"Please verify your network connectivity and TLS settings."
            )

        except httpx.HTTPStatusError as e:
            if e.response.status_code == 405:
                raise Exception(
                    "OpenRouter returned 405 Method Not Allowed. "
                    "Please ensure the request is sent to https://openrouter.ai/api/v1/chat/completions "
                    "and retry validation."
                )
            raise Exception(
                f"OpenRouter API validation failed: {str(e)}. "
                f"Please verify your API key and model ID."
            )

        except Exception as e:
            raise Exception(
                f"OpenRouter API validation failed: {str(e)}. "
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
        Generate quiz questions using OpenRouter model.
        
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
            headers = {
                "Authorization": f"Bearer {self.api_key}",
                "Content-Type": "application/json",
            }
            payload = {
                "model": self.model_id,
                "messages": [
                    {"role": "user", "content": quiz_prompt}
                ],
                "temperature": 0.7,
                "max_tokens": 4096,
            }

            with httpx.Client(verify=self._get_tls_verify()) as client:
                response = client.post(
                    self.OPENROUTER_API_URL,
                    headers=headers,
                    json=payload,
                    timeout=60.0
                )
                response.raise_for_status()
                data = response.json()

            # Extract response text
            response_text = data["choices"][0]["message"]["content"]

            # Extract JSON from response
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
                f"OpenRouter quiz generation failed: {str(e)}"
            )

    def evaluate_answers(
        self,
        quiz_questions: List[QuizQuestion],
        user_answers: List[int],
        evaluation_prompt: str
    ) -> EvaluationResult:
        """
        Evaluate quiz answers using OpenRouter model.
        
        Args:
            quiz_questions: Quiz questions with correct answers
            user_answers: User's selected answer indices
            evaluation_prompt: Formatted evaluation prompt
            
        Returns:
            EvaluationResult with score and feedback
        """
        try:
            headers = {
                "Authorization": f"Bearer {self.api_key}",
                "Content-Type": "application/json",
            }
            payload = {
                "model": self.model_id,
                "messages": [
                    {"role": "user", "content": evaluation_prompt}
                ],
                "temperature": 0.5,
                "max_tokens": 2048,
            }

            with httpx.Client(verify=self._get_tls_verify()) as client:
                response = client.post(
                    self.OPENROUTER_API_URL,
                    headers=headers,
                    json=payload,
                    timeout=60.0
                )
                response.raise_for_status()
                data = response.json()

            # Extract response text
            response_text = data["choices"][0]["message"]["content"]

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
                f"OpenRouter evaluation failed: {str(e)}"
            )

    def _extract_json(self, text: str) -> str:
        """
        Extract JSON from text, handling markdown code blocks.
        
        Args:
            text: Text potentially containing JSON
            
        Returns:
            Extracted JSON string
        """
        # Try to find JSON in markdown code blocks
        match = re.search(r"```(?:json)?\s*([\s\S]*?)```", text)
        if match:
            return match.group(1).strip()

        # Otherwise assume the entire response is JSON
        return text.strip()
