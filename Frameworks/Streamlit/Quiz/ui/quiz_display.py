"""Quiz display component for showing questions and answering."""

import streamlit as st
from config.constants import QuizQuestion
from state.quiz_state import QuizSessionState
from typing import Optional


class QuizDisplay:
    """Renders quiz questions and captures user responses."""

    @staticmethod
    def render_question(
        question: QuizQuestion,
        question_idx: int,
        total_questions: int
    ) -> Optional[int]:
        """
        Render a single quiz question with 4 options.
        
        Args:
            question: The QuizQuestion to display
            question_idx: Current question index (0-based)
            total_questions: Total number of questions
            
        Returns:
            Selected answer index (0-3) if user selects, None otherwise
        """
        # Progress bar
        progress = (question_idx + 1) / total_questions
        st.progress(progress, text=f"Question {question_idx + 1} of {total_questions}")

        # Question header
        st.markdown(f"## 📌 Question {question_idx + 1}")
        st.markdown(f"### {question.question_text}")

        # Options as radio buttons
        selected_option = st.radio(
            label="Select your answer:",
            options=range(4),
            format_func=lambda i: question.options[i],
            key=f"question_{question_idx}",
            label_visibility="collapsed"
        )

        # Check if answer has been saved
        user_answers = QuizSessionState.get_user_answers()
        answer_saved = (
            selected_option is not None and
            question_idx < len(user_answers) and
            user_answers[question_idx] == selected_option
        )

        # Show confirmation
        if answer_saved:
            st.success(f"✅ Answer saved: {question.options[selected_option]}")

        return selected_option

    @staticmethod
    def render_final_submission(submit_disabled: bool = False) -> bool:
        """
        Render the final submission confirmation for the last question.

        Args:
            submit_disabled: Disable submit button when quiz is already submitted
        
        Returns:
            True if user clicks submit, False otherwise
        """
        st.markdown("---")
        st.warning("⚠️ This is the last question. After submission, your quiz will be evaluated.")
        if submit_disabled:
            st.info("⏳ Submission received. Evaluating your answers, please wait...")

        col1, col2 = st.columns(2)
        with col1:
            submit_clicked = st.button(
                "✅ **SUBMIT QUIZ**",
                use_container_width=True,
                type="primary",
                disabled=submit_disabled,
            )
        with col2:
            st.button(
                "← **BACK**",
                use_container_width=True,
                disabled=True  # Can add back navigation later
            )

        return submit_clicked

    @staticmethod
    def render_question_navigation(
        question_idx: int,
        total_questions: int,
        answer_selected: bool
    ) -> str:
        """
        Render navigation buttons for moving between questions.
        
        Args:
            question_idx: Current question index
            total_questions: Total questions
            answer_selected: Whether current question has been answered
            
        Returns:
            "next" if next clicked, "prev" if prev clicked, None otherwise
        """
        col1, col2, col3 = st.columns([1, 1, 1])

        with col1:
            if question_idx > 0:
                if st.button("← **PREVIOUS**", use_container_width=True):
                    return "prev"
            else:
                st.button("← **PREVIOUS**", use_container_width=True, disabled=True)

        with col2:
            # Question indicator in center
            st.markdown(f"<div style='text-align: center; padding: 10px;'><b>{question_idx + 1} / {total_questions}</b></div>", unsafe_allow_html=True)

        with col3:
            if question_idx < total_questions - 1:
                if st.button("**NEXT** →", use_container_width=True, disabled=not answer_selected):
                    return "next"
            else:
                st.button("**NEXT** →", use_container_width=True, disabled=True)

        return None
