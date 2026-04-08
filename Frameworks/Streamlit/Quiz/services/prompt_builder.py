"""Service for building prompts for quiz generation and evaluation."""

from config.constants import AgeGroup, DifficultyLevel, Field, QuizQuestion
from typing import List
import json


class QuizPromptBuilder:
    """Builds prompts for quiz generation given user criteria."""

    @staticmethod
    def build_quiz_prompt(
        age_group: AgeGroup,
        difficulty_level: DifficultyLevel,
        field: Field,
        num_questions: int
    ) -> str:
        """
        Build a prompt for quiz generation.
        
        Args:
            age_group: Target age group
            difficulty_level: Difficulty level
            field: Subject field
            num_questions: Number of questions to generate
            
        Returns:
            Formatted prompt string for quiz generation
        """
        return f"""Generate a {num_questions}-question quiz for the following criteria:

Age Group: {age_group.value}
Difficulty Level: {difficulty_level.value}
Subject Field: {field.value}
Number of Questions: {num_questions}

Return the quiz in valid JSON format with this exact structure:
{{
    "questions": [
        {{
            "question": "Question text here?",
            "options": ["Option A", "Option B", "Option C", "Option D"],
            "correct_answer": 0
        }}
    ]
}}

Important:
- Each question must have exactly 4 options
- correct_answer is 0-indexed (0=first option, 3=last option)
- Ensure questions are appropriate for the specified age group and difficulty
- Make questions clear and unambiguous
- Return ONLY valid JSON, no additional text

Generate the quiz now:"""

    @staticmethod
    def build_evaluation_prompt(
        quiz_questions: List[QuizQuestion],
        user_answers: List[int]
    ) -> str:
        """
        Build a prompt for evaluating quiz answers.
        
        Args:
            quiz_questions: List of quiz questions with correct answers
            user_answers: List of user-selected answer indices (0-3)
            
        Returns:
            Formatted prompt string for evaluation
        """
        # Build quiz data for evaluation context
        quiz_summary = []
        for idx, question in enumerate(quiz_questions):
            user_answer_text = question.options[user_answers[idx]]
            correct_answer_text = question.options[question.correct_answer_index]
            is_correct = user_answers[idx] == question.correct_answer_index

            quiz_summary.append({
                "question_num": idx + 1,
                "question": question.question_text,
                "user_answer": user_answer_text,
                "correct_answer": correct_answer_text,
                "is_correct": is_correct
            })

        # Calculate raw score
        correct_count = sum(1 for q in quiz_summary if q["is_correct"])
        score_percentage = (correct_count / len(quiz_questions)) * 100

        # Build evaluation prompt
        prompt = f"""You are an expert quiz evaluator. Analyze the following quiz responses and provide detailed evaluation.

Quiz Results Summary:
- Total Questions: {len(quiz_questions)}
- Correct Answers: {correct_count}
- Score: {score_percentage:.1f}%

Detailed Results:
{json.dumps(quiz_summary, indent=2)}

Based on the user's responses, provide evaluation in this exact JSON format:
{{
    "score_percentage": {score_percentage},
    "summary": "A brief (1-2 sentence) summary of overall performance",
    "category_scores": {{}},
    "feedback": "Detailed feedback (3-5 sentences) on strengths and areas for improvement"
}}

Important:
- score_percentage should match the calculated score
- summary should be encouraging yet honest
- feedback should be constructive and specific to the topics covered
- Return ONLY valid JSON, no additional text

Evaluate now:"""

        return prompt
