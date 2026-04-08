"""Main Streamlit application for Quiz Application."""

import streamlit as st
from config.constants import QuizCriteria, Theme
from state.quiz_state import QuizSessionState
from models.vendor_factory import VendorFactory
from services.quiz_generator import QuizGenerator
from services.quiz_evaluator import QuizEvaluator
from ui.theme_manager import ThemeManager
from ui.sidebar import SidebarConfigurator
from ui.quiz_display import QuizDisplay
from ui.results import ResultsDisplay


def setup_page():
    """Configure Streamlit page settings."""
    st.set_page_config(
        page_title="Project CooL Quiz Master",
        page_icon="🎓",
        layout="wide",
        initial_sidebar_state="expanded"
    )


def render_welcome_screen():
    """Render welcome screen when quiz hasn't started."""
    col1, col2 = st.columns([1, 2])

    with col1:
        st.markdown("## 🎓")
    with col2:
        st.title("**Project CooL Quiz Master**")

    st.markdown("""
    ### Welcome to the AI-Powered Quiz Application!
    
    Test your knowledge across multiple domains with AI-generated questions.
    
    **Features:**
    - 🤖 Multi-vendor AI support (OpenAI, Azure, HuggingFace, OpenRouter)
    - 📚 Multiple subjects (General Knowledge, C# Programming, Science)
    - 🎯 Adaptive difficulty levels based on age group
    - 📊 Instant evaluation with detailed feedback
    - ⏱️ Time tracking for your quiz session
    - 🎨 Customizable themes
    
    **How to use:**
    1. Configure your AI model in the sidebar (Section 1)
    2. Set your quiz preferences (Section 2)
    3. Click "START QUIZ" to begin
    4. Answer all questions and submit for evaluation
    5. Review your results and feedback
    
    **Good luck!** 🚀
    """)


def render_quiz_screen():
    """Render quiz questions screen."""
    questions = QuizSessionState.get_quiz_questions()
    current_idx = QuizSessionState.get_current_question_index()
    user_answers = QuizSessionState.get_user_answers()

    if not questions:
        st.error("❌ No quiz questions loaded")
        return

    # Render current question
    question = questions[current_idx]
    selected_answer = QuizDisplay.render_question(
        question=question,
        question_idx=current_idx,
        total_questions=len(questions)
    )

    # Save answer if selected
    if selected_answer is not None:
        QuizSessionState.submit_answer(current_idx, selected_answer)

    st.markdown("---")

    # Check if this is the last question
    is_last_question = current_idx == len(questions) - 1

    if is_last_question:
        # Final submission button
        submit_disabled = QuizSessionState.get_is_quiz_submitted()
        if QuizDisplay.render_final_submission(submit_disabled=submit_disabled):
            QuizSessionState.complete_quiz()
            st.rerun()
    else:
        # Navigation buttons
        answer_recorded = (current_idx < len(user_answers) and
                          user_answers[current_idx] is not None)

        nav = QuizDisplay.render_question_navigation(
            question_idx=current_idx,
            total_questions=len(questions),
            answer_selected=answer_recorded
        )

        if nav == "next":
            QuizSessionState.set_current_question_index(current_idx + 1)
            st.rerun()
        elif nav == "prev":
            QuizSessionState.set_current_question_index(current_idx - 1)
            st.rerun()


def render_evaluation_screen():
    """Render results screen with evaluation."""
    result = QuizSessionState.get_evaluation_result()
    elapsed_time = QuizSessionState.get_elapsed_time()

    if result:
        ResultsDisplay.render(result, elapsed_time)
    else:
        st.error("❌ Evaluation result not available")


def main():
    """Main application orchestrator."""
    setup_page()

    # Initialize session state
    QuizSessionState.initialize()

    # Read current theme from session state
    current_theme = QuizSessionState.get_theme()

    # Render sidebar configuration (includes theme selector)
    quiz_criteria = SidebarConfigurator.render(current_theme)

    # Apply selected theme from widget state
    selected_theme_value = st.session_state.get("theme_selector")
    if selected_theme_value:
        # Convert selected string value to Theme enum safely.
        try:
            selected_theme_enum = Theme(selected_theme_value)
            if selected_theme_enum != current_theme:
                QuizSessionState.set_theme(selected_theme_enum)
            current_theme = selected_theme_enum
        except Exception:
            pass

    ThemeManager.apply_theme(current_theme)

    # STEP 1: Render welcome or start quiz generation
    if not QuizSessionState.get_is_quiz_started():
        if quiz_criteria is None:
            # Show welcome screen
            render_welcome_screen()
        else:
            # Start quiz with criteria
            try:
                st.info("🔄 Generating quiz questions...")

                # Create vendor and generate questions
                vendor = VendorFactory.create_vendor(
                    vendor=quiz_criteria.model_vendor,
                    api_key=quiz_criteria.api_key,
                    model_id=quiz_criteria.model_id
                )

                generator = QuizGenerator(vendor=vendor)
                questions = generator.generate(criteria=quiz_criteria)

                # Store in session and start quiz
                QuizSessionState.start_quiz(quiz_criteria, questions)
                st.success("✅ Quiz loaded successfully!")
                st.rerun()

            except Exception as e:
                st.error(f"❌ Failed to generate quiz: {str(e)}")

    # STEP 2: Evaluate answers if quiz is complete and not yet evaluated
    elif (
        QuizSessionState.get_is_quiz_started()
        and QuizSessionState.get_is_quiz_complete()
        and not QuizSessionState.get_is_quiz_evaluated()
    ):
        try:
            st.info("📊 Evaluating your answers...")

            questions = QuizSessionState.get_quiz_questions()
            user_answers = QuizSessionState.get_user_answers()

            # Create vendor and evaluate
            criteria = QuizSessionState.get_quiz_criteria()
            vendor = VendorFactory.create_vendor(
                vendor=criteria.model_vendor,
                api_key=criteria.api_key,
                model_id=criteria.model_id
            )

            evaluator = QuizEvaluator(vendor=vendor)
            result = evaluator.evaluate(questions, user_answers)

            # Store result and rerun
            QuizSessionState.set_evaluation_result(result)
            st.success("✅ Evaluation complete!")
            st.rerun()

        except Exception as e:
            st.error(f"❌ Evaluation failed: {str(e)}")

    # STEP 3: Render quiz if started, incomplete, and not evaluated
    elif QuizSessionState.get_is_quiz_started() and not QuizSessionState.get_is_quiz_evaluated():
        render_quiz_screen()

    # STEP 4: Render results if evaluation is complete
    elif QuizSessionState.get_is_quiz_evaluated():
        render_evaluation_screen()


if __name__ == "__main__":
    main()
