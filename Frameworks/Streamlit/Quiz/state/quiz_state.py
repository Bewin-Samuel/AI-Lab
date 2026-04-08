"""Session state management for quiz application."""

import streamlit as st
from typing import List, Optional
from datetime import datetime
from config.constants import (
    QuizQuestion, EvaluationResult, QuizCriteria, Theme
)


class QuizSessionState:
    """Manages quiz session state in Streamlit's session state store."""

    # Session state keys
    QUIZ_STARTED = "quiz_started"
    QUIZ_QUESTIONS = "quiz_questions"
    USER_ANSWERS = "user_answers"
    QUIZ_EVALUATED = "quiz_evaluated"
    EVALUATION_RESULT = "evaluation_result"
    START_TIME = "start_time"
    END_TIME = "end_time"
    QUIZ_SUBMITTED = "quiz_submitted"
    CURRENT_QUESTION_IDX = "current_question_idx"
    QUIZ_CRITERIA = "quiz_criteria"
    THEME = "theme"

    @staticmethod
    def initialize():
        """Initialize default session state values."""
        if QuizSessionState.QUIZ_STARTED not in st.session_state:
            st.session_state[QuizSessionState.QUIZ_STARTED] = False
        if QuizSessionState.QUIZ_QUESTIONS not in st.session_state:
            st.session_state[QuizSessionState.QUIZ_QUESTIONS] = []
        if QuizSessionState.USER_ANSWERS not in st.session_state:
            st.session_state[QuizSessionState.USER_ANSWERS] = []
        if QuizSessionState.QUIZ_EVALUATED not in st.session_state:
            st.session_state[QuizSessionState.QUIZ_EVALUATED] = False
        if QuizSessionState.EVALUATION_RESULT not in st.session_state:
            st.session_state[QuizSessionState.EVALUATION_RESULT] = None
        if QuizSessionState.START_TIME not in st.session_state:
            st.session_state[QuizSessionState.START_TIME] = None
        if QuizSessionState.END_TIME not in st.session_state:
            st.session_state[QuizSessionState.END_TIME] = None
        if QuizSessionState.QUIZ_SUBMITTED not in st.session_state:
            st.session_state[QuizSessionState.QUIZ_SUBMITTED] = False
        if QuizSessionState.CURRENT_QUESTION_IDX not in st.session_state:
            st.session_state[QuizSessionState.CURRENT_QUESTION_IDX] = 0
        if QuizSessionState.QUIZ_CRITERIA not in st.session_state:
            st.session_state[QuizSessionState.QUIZ_CRITERIA] = None
        if QuizSessionState.THEME not in st.session_state:
            st.session_state[QuizSessionState.THEME] = Theme.LIGHT.value

    @staticmethod
    def start_quiz(criteria: QuizCriteria, questions: List[QuizQuestion]):
        """
        Mark quiz as started and initialize quiz data.
        
        Args:
            criteria: QuizCriteria with selected options
            questions: List of generated QuizQuestion objects
        """
        st.session_state[QuizSessionState.QUIZ_STARTED] = True
        st.session_state[QuizSessionState.QUIZ_QUESTIONS] = questions
        st.session_state[QuizSessionState.USER_ANSWERS] = [None] * len(questions)
        st.session_state[QuizSessionState.START_TIME] = datetime.now()
        st.session_state[QuizSessionState.QUIZ_SUBMITTED] = False
        st.session_state[QuizSessionState.QUIZ_CRITERIA] = criteria
        st.session_state[QuizSessionState.CURRENT_QUESTION_IDX] = 0

    @staticmethod
    def submit_answer(question_idx: int, answer_idx: int):
        """
        Record user's answer for a specific question.
        
        Args:
            question_idx: Index of the question (0-based)
            answer_idx: Index of selected answer (0-3)
        """
        if 0 <= question_idx < len(st.session_state[QuizSessionState.USER_ANSWERS]):
            st.session_state[QuizSessionState.USER_ANSWERS][question_idx] = answer_idx

    @staticmethod
    def complete_quiz():
        """Mark quiz as complete and record end time."""
        st.session_state[QuizSessionState.END_TIME] = datetime.now()
        st.session_state[QuizSessionState.QUIZ_SUBMITTED] = True

    @staticmethod
    def set_evaluation_result(result: EvaluationResult):
        """
        Store evaluation result in session state.
        
        Args:
            result: EvaluationResult from evaluator service
        """
        st.session_state[QuizSessionState.EVALUATION_RESULT] = result
        st.session_state[QuizSessionState.QUIZ_EVALUATED] = True

    @staticmethod
    def get_is_quiz_started() -> bool:
        """Check if quiz has been started."""
        QuizSessionState.initialize()
        return st.session_state.get(QuizSessionState.QUIZ_STARTED, False)

    @staticmethod
    def get_is_quiz_complete() -> bool:
        """Check if all questions have been answered."""
        QuizSessionState.initialize()
        answers = st.session_state.get(QuizSessionState.USER_ANSWERS, [])
        return all(ans is not None for ans in answers) if answers else False

    @staticmethod
    def get_is_quiz_evaluated() -> bool:
        """Check if quiz has been evaluated."""
        QuizSessionState.initialize()
        return st.session_state.get(QuizSessionState.QUIZ_EVALUATED, False)

    @staticmethod
    def get_is_quiz_submitted() -> bool:
        """Check if quiz was submitted and is awaiting evaluation/results."""
        QuizSessionState.initialize()
        return st.session_state.get(QuizSessionState.QUIZ_SUBMITTED, False)

    @staticmethod
    def get_quiz_questions() -> List[QuizQuestion]:
        """Get current quiz questions."""
        QuizSessionState.initialize()
        return st.session_state.get(QuizSessionState.QUIZ_QUESTIONS, [])

    @staticmethod
    def get_user_answers() -> List[int]:
        """Get user's submitted answers."""
        QuizSessionState.initialize()
        return st.session_state.get(QuizSessionState.USER_ANSWERS, [])

    @staticmethod
    def get_current_question_index() -> int:
        """Get current question index."""
        QuizSessionState.initialize()
        return st.session_state.get(QuizSessionState.CURRENT_QUESTION_IDX, 0)

    @staticmethod
    def set_current_question_index(idx: int):
        """Set current question index."""
        st.session_state[QuizSessionState.CURRENT_QUESTION_IDX] = idx

    @staticmethod
    def get_evaluation_result() -> Optional[EvaluationResult]:
        """Get evaluation result if available."""
        QuizSessionState.initialize()
        return st.session_state.get(QuizSessionState.EVALUATION_RESULT)

    @staticmethod
    def get_elapsed_time() -> float:
        """
        Get elapsed time in seconds from quiz start to end (or now if not ended).
        
        Returns:
            Elapsed time in seconds, or 0 if quiz not started
        """
        QuizSessionState.initialize()
        start = st.session_state.get(QuizSessionState.START_TIME)
        end = st.session_state.get(QuizSessionState.END_TIME)

        if not start:
            return 0.0

        if end:
            elapsed = (end - start).total_seconds()
        else:
            elapsed = (datetime.now() - start).total_seconds()

        return max(0.0, elapsed)

    @staticmethod
    def get_quiz_criteria() -> Optional[QuizCriteria]:
        """Get quiz criteria."""
        QuizSessionState.initialize()
        return st.session_state.get(QuizSessionState.QUIZ_CRITERIA)

    @staticmethod
    def get_theme() -> Theme:
        """Get current theme."""
        QuizSessionState.initialize()
        raw_theme = st.session_state.get(QuizSessionState.THEME, Theme.LIGHT.value)
        if isinstance(raw_theme, Theme):
            return raw_theme
        try:
            return Theme(raw_theme)
        except Exception:
            return Theme.LIGHT

    @staticmethod
    def set_theme(theme: Theme):
        """Set current theme."""
        st.session_state[QuizSessionState.THEME] = theme.value

    @staticmethod
    def reset_quiz():
        """Reset all quiz state for a new quiz."""
        st.session_state[QuizSessionState.QUIZ_STARTED] = False
        st.session_state[QuizSessionState.QUIZ_QUESTIONS] = []
        st.session_state[QuizSessionState.USER_ANSWERS] = []
        st.session_state[QuizSessionState.QUIZ_EVALUATED] = False
        st.session_state[QuizSessionState.EVALUATION_RESULT] = None
        st.session_state[QuizSessionState.START_TIME] = None
        st.session_state[QuizSessionState.END_TIME] = None
        st.session_state[QuizSessionState.QUIZ_SUBMITTED] = False
        st.session_state[QuizSessionState.CURRENT_QUESTION_IDX] = 0
        st.session_state[QuizSessionState.QUIZ_CRITERIA] = None
