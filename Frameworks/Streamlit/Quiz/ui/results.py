"""Results display component for showing quiz evaluation."""

import streamlit as st
from config.constants import EvaluationResult, Theme
from state.quiz_state import QuizSessionState


class ResultsDisplay:
    """Displays quiz evaluation results and provides retake option."""

    @staticmethod
    def render(evaluation_result: EvaluationResult, elapsed_time: float):
        """
        Render results page with score, feedback, and retake option.
        
        Args:
            evaluation_result: EvaluationResult from evaluator service
            elapsed_time: Time taken to complete quiz in seconds
        """
        st.markdown("---")
        st.markdown("# 🏆 **Quiz Results**")
        is_dark_theme = QuizSessionState.get_theme() == Theme.DARK

        # Score display (large, prominent)
        col1, col2, col3 = st.columns(3)

        with col1:
            # Score percentage with color coding
            score = evaluation_result.score_percentage
            if score >= 80:
                color = "green"
                emoji = "🌟"
            elif score >= 60:
                color = "orange"
                emoji = "👍"
            else:
                color = "red"
                emoji = "💪"

            score_card_bg = "rgba(17, 26, 45, 0.82)" if is_dark_theme else "transparent"

            st.markdown(f"""
            <div style='text-align: center; padding: 20px; border: 2px solid {color}; border-radius: 10px; background-color: {score_card_bg};'>
                <h2>{emoji} Score: {score:.1f}%</h2>
            </div>
            """, unsafe_allow_html=True)

        with col2:
            # Time taken
            minutes = int(elapsed_time // 60)
            seconds = int(elapsed_time % 60)
            time_card_bg = "rgba(17, 26, 45, 0.82)" if is_dark_theme else "#f0f2f6"
            time_card_text = "#e5edf7" if is_dark_theme else "#1e1e1e"
            st.markdown(f"""
            <div style='text-align: center; padding: 20px; background-color: {time_card_bg}; color: {time_card_text}; border-radius: 10px; border: 1px solid rgba(74, 168, 255, 0.22);'>
                <h3>⏱️ **Time Taken**</h3>
                <h2>{minutes}:{seconds:02d}</h2>
            </div>
            """, unsafe_allow_html=True)

        with col3:
            # Performance level
            if score >= 80:
                level = "Excellent"
                level_color = "#06a77d"
            elif score >= 60:
                level = "Good"
                level_color = "#00a8e8"
            else:
                level = "Needs Work"
                level_color = "#d62828"

            st.markdown(f"""
            <div style='text-align: center; padding: 20px; background-color: {level_color}; color: white; border-radius: 10px;'>
                <h3>**Performance Level**</h3>
                <h2>{level}</h2>
            </div>
            """, unsafe_allow_html=True)

        st.markdown("---")

        # Knowledge summary
        st.subheader("📚 **Knowledge Summary**")
        st.info(f"💡 {evaluation_result.knowledge_summary}")

        # Detailed feedback
        st.subheader("📝 **Detailed Feedback**")
        st.write(evaluation_result.feedback)

        # Category scores (if available)
        if evaluation_result.category_scores:
            st.subheader("📊 **Performance by Category**")
            
            # Create columns for category breakdown
            cols = st.columns(len(evaluation_result.category_scores))
            for idx, (category, score_val) in enumerate(evaluation_result.category_scores.items()):
                with cols[idx]:
                    st.metric(
                        label=category,
                        value=f"{score_val}%",
                        delta=None
                    )

        # Action buttons
        st.markdown("---")
        col1, col2 = st.columns(2)

        with col1:
            if st.button("🔄 **RETAKE QUIZ**", use_container_width=True):
                # Reset only quiz flow data so theme and UI preferences are preserved.
                QuizSessionState.reset_quiz()
                st.rerun()

        with col2:
            st.button(
                "📊 **HOME**",
                use_container_width=True,
                disabled=True  # Can add home functionality later
            )

        # Additional stats
        st.markdown("---")
        st.markdown("""
        ### 📌 **Quick Tips for Next Time**
        - Review the topics you found challenging
        - Try a higher difficulty level if you scored 80% or above
        - Practice more with the concepts you struggled with
        """)
