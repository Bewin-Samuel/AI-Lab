"""Sidebar configuration component for quiz setup."""

import streamlit as st
from config.constants import (
    ModelVendor, AgeGroup, DifficultyLevel, Field, Theme, QuizCriteria
)
from config.models import ModelRegistry
from models.vendor_factory import VendorFactory
from ui.theme_manager import ThemeManager
from typing import Optional


class SidebarConfigurator:
    """Renders sidebar configuration sections and returns quiz criteria."""

    @staticmethod
    def render(current_theme: Theme) -> Optional[QuizCriteria]:
        """
        Render sidebar with two sections:
        - Section 1: Model vendor configuration (Vendor, Model ID, API Key)
        - Section 2: Quiz criteria (Age Group, Difficulty, Field, Num Questions, Theme)

        Args:
            current_theme: Theme currently stored in session state
        
        Returns:
            QuizCriteria if "Start" button is clicked and all validations pass
            None if button not clicked or validation fails
        """
        st.sidebar.title("⚙️ **Quiz Configuration**")

        # ===== SECTION 1: Model Configuration =====
        st.sidebar.subheader("🤖 Section 1: AI Model Configuration")

        # Model Vendor dropdown
        vendor_options = [v.value for v in ModelVendor]
        default_vendor_index = vendor_options.index(ModelVendor.HUGGINGFACE.value)
        vendor_name = st.sidebar.selectbox(
            label="Model Vendor",
            options=vendor_options,
            index=default_vendor_index,
            help="Select the AI model provider"
        )
        selected_vendor = ModelVendor(vendor_name)

        # Model ID dropdown (dynamic based on vendor)
        available_models = ModelRegistry.get_models_for_vendor(selected_vendor)
        model_id = st.sidebar.selectbox(
            label="Model ID",
            options=available_models,
            help=f"Available models for {vendor_name}"
        )

        # API Key field
        api_key = st.sidebar.text_input(
            label="API Key",
            type="password",
            help="Enter your API key for the selected vendor"
        )

        # ===== SECTION 2: Quiz Criteria =====
        st.sidebar.markdown("---")
        st.sidebar.subheader("📝 Section 2: Quiz Criteria")

        age_group = st.sidebar.selectbox(
            label="Age Group",
            options=[ag.value for ag in AgeGroup],
            help="Target age group for quiz difficulty"
        )
        selected_age_group = AgeGroup(age_group)

        difficulty = st.sidebar.selectbox(
            label="Difficulty Level",
            options=[dl.value for dl in DifficultyLevel],
            help="Quiz difficulty level"
        )
        selected_difficulty = DifficultyLevel(difficulty)

        field = st.sidebar.selectbox(
            label="Field/Subject",
            options=[f.value for f in Field],
            help="Subject area for the quiz"
        )
        selected_field = Field(field)

        num_questions = st.sidebar.slider(
            label="Number of Questions",
            min_value=1,
            max_value=10,
            value=5,
            help="Total questions in the quiz"
        )

        # ===== THEME SELECTOR =====
        st.sidebar.markdown("---")
        selected_theme = ThemeManager.render_theme_selector(current_theme)

        # ===== START BUTTON & VALIDATION =====
        st.sidebar.markdown("---")

        # Check mandatory field
        button_enabled = bool(api_key and model_id)

        start_button = st.sidebar.button(
            label="🚀 **START QUIZ**",
            disabled=not button_enabled,
            use_container_width=True
        )

        if not api_key and st.session_state.get("show_api_error"):
            st.sidebar.error("⚠️ API Key is required")

        if start_button:
            # Validate API credentials
            try:
                st.sidebar.info("🔐 Validating credentials...")
                vendor = VendorFactory.create_vendor(
                    vendor=selected_vendor,
                    api_key=api_key,
                    model_id=model_id
                )
                vendor.validate_credentials()
                st.sidebar.success("✅ Credentials validated!")

                # Create and return QuizCriteria
                criteria = QuizCriteria(
                    model_vendor=selected_vendor,
                    model_id=model_id,
                    api_key=api_key,
                    age_group=selected_age_group,
                    difficulty_level=selected_difficulty,
                    field=selected_field,
                    num_questions=num_questions,
                    theme=selected_theme
                )
                return criteria

            except Exception as e:
                st.sidebar.error(f"❌ Validation failed: {str(e)}")
                return None

        return None
