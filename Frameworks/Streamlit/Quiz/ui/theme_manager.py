"""Theme manager for applying visual themes to the application."""

import streamlit as st
from config.constants import Theme


class ThemeManager:
    """Manages and applies UI themes (Light, Dark, High Contrast)."""

    @staticmethod
    def apply_theme(theme: Theme):
        """
        Apply theme styling to the Streamlit application.
        
        Args:
            theme: Theme enum value to apply
        """
        if theme == Theme.LIGHT:
            ThemeManager._apply_light_theme()
        elif theme == Theme.DARK:
            ThemeManager._apply_dark_theme()
        elif theme == Theme.HIGH_CONTRAST:
            ThemeManager._apply_high_contrast_theme()

    @staticmethod
    def _apply_light_theme():
        """Apply light theme styling."""
        st.markdown("""
        <style>
        :root {
            --primary-color: #0066cc;
            --secondary-color: #00a8e8;
            --success-color: #06a77d;
            --warning-color: #d4a574;
            --error-color: #d62828;
            --background-color: #ffffff;
            --text-color: #1e1e1e;
            --border-color: #e0e0e0;
        }
        
        body {
            background-color: #f8f9fa;
            color: #1e1e1e;
        }
        
        .stApp {
            background-color: #f8f9fa;
        }
        
        .stButton > button {
            background-color: #0066cc;
            color: white;
            border-radius: 4px;
            border: none;
        }
        
        .stButton > button:hover {
            background-color: #0052a3;
        }
        </style>
        """, unsafe_allow_html=True)

    @staticmethod
    def _apply_dark_theme():
        """Apply dark theme styling."""
        st.markdown("""
        <style>
        :root {
            --primary-color: #bb86fc;
            --secondary-color: #03dac6;
            --success-color: #1db679;
            --warning-color: #ffb74d;
            --error-color: #cf6679;
            --background-color: #121212;
            --text-color: #ffffff;
            --border-color: #333333;
        }
        
        body {
            background-color: #121212;
            color: #ffffff;
        }
        
        .stApp {
            background-color: #121212;
        }
        
        .stButton > button {
            background-color: #bb86fc;
            color: #000000;
            border-radius: 4px;
            border: none;
        }
        
        .stButton > button:hover {
            background-color: #9a67d8;
        }
        </style>
        """, unsafe_allow_html=True)

    @staticmethod
    def _apply_high_contrast_theme():
        """Apply high contrast theme for accessibility."""
        st.markdown("""
        <style>
        :root {
            --primary-color: #000000;
            --secondary-color: #ffcc00;
            --success-color: #00aa00;
            --warning-color: #ff6600;
            --error-color: #ff0000;
            --background-color: #ffffff;
            --text-color: #000000;
            --border-color: #000000;
        }
        
        body {
            background-color: #ffffff;
            color: #000000;
            font-weight: bold;
        }
        
        .stApp {
            background-color: #ffffff;
        }
        
        .stButton > button {
            background-color: #000000;
            color: #ffcc00;
            border: 2px solid #000000;
            border-radius: 2px;
            font-weight: bold;
            font-size: 16px;
        }
        
        .stButton > button:hover {
            background-color: #ffcc00;
            color: #000000;
        }
        </style>
        """, unsafe_allow_html=True)

    @staticmethod
    def render_theme_selector(current_theme: Theme) -> Theme:
        """
        Render theme selector in sidebar and return selected theme.

        Args:
            current_theme: Theme currently stored in session state
        
        Returns:
            Selected Theme enum value
        """
        options = [theme.value for theme in Theme]
        default_index = options.index(current_theme.value)
        selected = st.sidebar.radio(
            "🎨 **Theme**",
            options=options,
            horizontal=True,
            index=default_index,
            key="theme_selector",
        )
        return Theme(selected)
