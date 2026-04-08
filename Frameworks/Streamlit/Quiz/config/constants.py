"""Application constants, enums, and configuration values."""

from enum import Enum
from dataclasses import dataclass
from typing import List


class ModelVendor(str, Enum):
    """Supported AI model vendors."""
    OPENAI = "OpenAI"
    AZURE_OPENAI = "Azure OpenAI"
    OPENROUTER = "OpenRouter"
    HUGGINGFACE = "Hugging Face"


class AgeGroup(str, Enum):
    """Age groups for quiz difficulty calibration."""
    YOUNG_CHILDREN = "3 - 5"
    CHILDREN = "6 - 12"
    TEENAGERS = "13 - 19"
    ADULT = "Adult"


class DifficultyLevel(str, Enum):
    """Quiz difficulty levels."""
    BEGINNER = "Beginner"
    PRACTITIONER = "Practitioner"
    EXPERT = "Expert"


class Field(str, Enum):
    """Knowledge fields/subjects."""
    GENERAL_KNOWLEDGE = "General Knowledge"
    CSHARP_PROGRAMMING = "C# Programming"
    SCIENCE = "Science"


class Theme(str, Enum):
    """UI themes for the application."""
    LIGHT = "Light"
    DARK = "Dark"
    HIGH_CONTRAST = "High Contrast"


@dataclass
class QuizQuestion:
    """Represents a single quiz question with multiple choice options."""
    question_id: int
    question_text: str
    options: List[str]  # Exactly 4 options
    correct_answer_index: int  # 0-3 index of correct option


@dataclass
class QuizCriteria:
    """Represents user-selected quiz configuration."""
    model_vendor: ModelVendor
    model_id: str
    api_key: str
    age_group: AgeGroup
    difficulty_level: DifficultyLevel
    field: Field
    num_questions: int  # 1-10
    theme: Theme


@dataclass
class EvaluationResult:
    """Represents quiz evaluation results."""
    score_percentage: float  # 0-100
    knowledge_summary: str  # Brief summary of performance
    category_scores: dict  # Field-specific breakdown (e.g., {"concept_1": 80, "concept_2": 60})
    feedback: str  # Detailed feedback on strengths/weaknesses
