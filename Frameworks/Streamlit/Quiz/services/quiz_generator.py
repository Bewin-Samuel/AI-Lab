"""Service for generating quizzes using model vendors."""

from typing import List
from config.constants import QuizCriteria, QuizQuestion, AgeGroup, DifficultyLevel, Field
from models.base import BaseModelVendor
from services.prompt_builder import QuizPromptBuilder


class QuizGenerator:
    """Generates quiz questions using a model vendor."""

    def __init__(self, vendor: BaseModelVendor):
        """
        Initialize with a model vendor instance.
        
        Args:
            vendor: Instance of BaseModelVendor to use for generation
        """
        self.vendor = vendor

    def generate(self, criteria: QuizCriteria) -> List[QuizQuestion]:
        """
        Generate quiz questions based on provided criteria.
        
        Args:
            criteria: QuizCriteria containing all quiz configuration
            
        Returns:
            List of QuizQuestion objects
            
        Raises:
            Exception if generation fails (will include vendor-specific error message)
        """
        # Build the prompt
        prompt = QuizPromptBuilder.build_quiz_prompt(
            age_group=criteria.age_group,
            difficulty_level=criteria.difficulty_level,
            field=criteria.field,
            num_questions=criteria.num_questions
        )

        # Call vendor to generate quiz
        questions = self.vendor.generate_quiz(
            age_group=criteria.age_group.value,
            difficulty_level=criteria.difficulty_level.value,
            field=criteria.field.value,
            num_questions=criteria.num_questions,
            quiz_prompt=prompt
        )

        return questions
