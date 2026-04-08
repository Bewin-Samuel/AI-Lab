"""Service for evaluating quiz answers using model vendors."""

from typing import List
from config.constants import QuizQuestion, EvaluationResult
from models.base import BaseModelVendor
from services.prompt_builder import QuizPromptBuilder


class QuizEvaluator:
    """Evaluates quiz answers and provides score and feedback."""

    def __init__(self, vendor: BaseModelVendor):
        """
        Initialize with a model vendor instance.
        
        Args:
            vendor: Instance of BaseModelVendor to use for evaluation
        """
        self.vendor = vendor

    def evaluate(
        self,
        quiz_questions: List[QuizQuestion],
        user_answers: List[int]
    ) -> EvaluationResult:
        """
        Evaluate user answers against correct answers.
        
        Args:
            quiz_questions: List of QuizQuestion objects with correct answers
            user_answers: List of user-selected answer indices (0-3 per question)
            
        Returns:
            EvaluationResult with score, summary, and feedback
            
        Raises:
            ValueError if user_answers length doesn't match questions
            Exception if evaluation fails
        """
        # Validate input
        if len(user_answers) != len(quiz_questions):
            raise ValueError(
                f"Expected {len(quiz_questions)} answers, got {len(user_answers)}"
            )

        # Check all answers are valid indices (0-3)
        for idx, answer in enumerate(user_answers):
            if not isinstance(answer, int) or answer < 0 or answer > 3:
                raise ValueError(
                    f"Invalid answer at question {idx + 1}: {answer}. "
                    f"Must be 0-3."
                )

        # Build evaluation prompt
        prompt = QuizPromptBuilder.build_evaluation_prompt(
            quiz_questions=quiz_questions,
            user_answers=user_answers
        )

        # Call vendor to evaluate
        result = self.vendor.evaluate_answers(
            quiz_questions=quiz_questions,
            user_answers=user_answers,
            evaluation_prompt=prompt
        )

        return result
