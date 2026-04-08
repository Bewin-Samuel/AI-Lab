"""Abstract base class for all model vendor implementations."""

from abc import ABC, abstractmethod
from typing import List, Optional
from config.constants import QuizQuestion, EvaluationResult


class BaseModelVendor(ABC):
    """
    Abstract base class defining the interface all model vendors must implement.
    
    Vendors implement this interface to provide quiz generation and evaluation
    capabilities. The abstraction enables dependency injection and makes it
    easy to swap vendors without changing client code.
    """

    def __init__(self, api_key: str, model_id: str):
        """
        Initialize vendor with credentials and model selection.
        
        Args:
            api_key: API key/token for authentication with the vendor
            model_id: Specific model identifier to use from this vendor
        """
        self.api_key = api_key
        self.model_id = model_id

    @abstractmethod
    def validate_credentials(self) -> bool:
        """
        Validate that the API credentials are valid and the model is accessible.
        
        Returns:
            True if credentials are valid, False otherwise
            
        Raises:
            Exception with user-friendly error message if validation fails
        """
        pass

    @abstractmethod
    def generate_quiz(
        self,
        age_group: str,
        difficulty_level: str,
        field: str,
        num_questions: int,
        quiz_prompt: str
    ) -> List[QuizQuestion]:
        """
        Generate quiz questions using the model.
        
        Args:
            age_group: Target age group (e.g., "Adult")
            difficulty_level: Difficulty level (e.g., "Beginner")
            field: Subject field (e.g., "Science")
            num_questions: Number of questions to generate (1-10)
            quiz_prompt: Formatted prompt for quiz generation
            
        Returns:
            List of QuizQuestion objects with questions and correct answers
            
        Notes:
            - Implementation should handle JSON parsing and validation
            - If model response is malformed, should raise descriptive exception
            - Can optionally stream questions one-by-one for better UX
        """
        pass

    @abstractmethod
    def evaluate_answers(
        self,
        quiz_questions: List[QuizQuestion],
        user_answers: List[int],  # 0-3 index per question
        evaluation_prompt: str
    ) -> EvaluationResult:
        """
        Evaluate user answers and provide score and feedback.
        
        Args:
            quiz_questions: List of quiz questions with correct answers
            user_answers: List of user-selected answer indices (0-3 per question)
            evaluation_prompt: Formatted prompt for evaluation
            
        Returns:
            EvaluationResult with score %, summary, and detailed feedback
        """
        pass
