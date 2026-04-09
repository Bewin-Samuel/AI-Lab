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
            --primary-color: #4aa8ff;
            --secondary-color: #14b8a6;
            --success-color: #22c55e;
            --warning-color: #f59e0b;
            --error-color: #ef4444;
            --background-color: #0b1220;
            --surface-color: #111a2d;
            --surface-elevated: #17233b;
            --text-color: #e5edf7;
            --text-muted: #9db0c7;
            --border-color: #2a3a55;
        }
        
        html, body, [class*="css"] {
            color: var(--text-color);
        }
        
        .stApp {
            background:
                radial-gradient(1200px 600px at -5% -10%, #12335f 0%, rgba(18, 51, 95, 0) 60%),
                radial-gradient(900px 500px at 110% -20%, #0f766e 0%, rgba(15, 118, 110, 0) 60%),
                linear-gradient(180deg, #0a1120 0%, #0b1220 45%, #0a1324 100%);
            color: var(--text-color);
        }

        [data-testid="stAppViewContainer"],
        [data-testid="stMain"],
        [data-testid="stHeader"] {
            background: transparent;
        }

        [data-testid="stHeader"] {
            background-color: rgba(10, 17, 32, 0.86);
            border-bottom: 1px solid var(--border-color);
            backdrop-filter: blur(6px);
        }

        [data-testid="stToolbar"],
        [data-testid="stStatusWidget"],
        [data-testid="stDecoration"] {
            background-color: transparent;
        }

        [data-testid="stSidebar"] {
            background: linear-gradient(180deg, #0d1628 0%, #0f1a30 100%);
            border-right: 1px solid var(--border-color);
        }

        [data-testid="stSidebar"] [data-testid="stMarkdownContainer"] p,
        [data-testid="stSidebar"] label,
        [data-testid="stSidebar"] h1,
        [data-testid="stSidebar"] h2,
        [data-testid="stSidebar"] h3 {
            color: var(--text-color);
        }

        h1, h2, h3 {
            color: #f3f8ff;
            letter-spacing: 0.2px;
        }

        p, li, span, div {
            color: var(--text-color);
        }

        hr {
            border-color: var(--border-color);
        }

        [data-testid="stAlert"] {
            border: 1px solid var(--border-color);
            border-radius: 12px;
            background-color: rgba(23, 35, 59, 0.85);
        }

        .stTextInput > div > div > input,
        div[data-baseweb="input"] input,
        div[data-baseweb="textarea"] textarea {
            background-color: var(--surface-color);
            color: var(--text-color);
            border: 1px solid var(--border-color);
            border-radius: 10px;
        }

        .stTextInput > div > div > input::placeholder,
        div[data-baseweb="textarea"] textarea::placeholder {
            color: var(--text-muted);
        }

        div[data-baseweb="select"] > div {
            background-color: var(--surface-color);
            color: var(--text-color);
            border: 1px solid var(--border-color);
            border-radius: 10px;
        }

        /* Dropdown menu panel + option rows */
        div[data-baseweb="popover"] {
            background: transparent;
        }

        div[data-baseweb="menu"] {
            background-color: #0f1b30 !important;
            border: 1px solid var(--border-color) !important;
            border-radius: 10px !important;
            box-shadow: 0 12px 24px rgba(0, 0, 0, 0.35) !important;
        }

        div[data-baseweb="menu"] ul,
        div[data-baseweb="menu"] li,
        div[data-baseweb="menu"] [role="option"],
        ul[role="listbox"],
        li[role="option"] {
            background-color: #0f1b30 !important;
            color: var(--text-color) !important;
        }

        div[data-baseweb="menu"] [role="option"]:hover,
        li[role="option"]:hover,
        div[data-baseweb="menu"] li:hover {
            background-color: #173055 !important;
            color: #f3f8ff !important;
        }

        div[data-baseweb="menu"] [aria-selected="true"],
        li[aria-selected="true"] {
            background-color: #1e3a66 !important;
            color: #ffffff !important;
        }

        .stNumberInput input,
        [data-baseweb="slider"] {
            color: var(--text-color);
        }

        .stRadio [role="radiogroup"] label {
            background-color: rgba(17, 26, 45, 0.7);
            border: 1px solid var(--border-color);
            border-radius: 10px;
            padding: 8px 10px;
            margin-bottom: 6px;
        }

        .stProgress > div > div > div > div {
            background: linear-gradient(90deg, #2563eb 0%, #14b8a6 100%);
        }
        
        .stButton > button {
            background: linear-gradient(135deg, #2563eb 0%, #0ea5e9 100%);
            color: #f8fbff;
            border-radius: 10px;
            border: 1px solid #3b82f6;
            box-shadow: 0 6px 14px rgba(37, 99, 235, 0.28);
            transition: transform 120ms ease, box-shadow 120ms ease;
        }
        
        .stButton > button:hover {
            transform: translateY(-1px);
            box-shadow: 0 10px 18px rgba(14, 165, 233, 0.25);
            border-color: #38bdf8;
        }

        .stButton > button:disabled {
            background: #24324a;
            color: #8ea1b7;
            border-color: #31445f;
            box-shadow: none;
        }

        [data-testid="stMetric"] {
            background-color: rgba(17, 26, 45, 0.72);
            border: 1px solid var(--border-color);
            border-radius: 12px;
            padding: 8px 12px;
        }

        code, pre {
            background-color: #101a2e !important;
            color: #cde3ff !important;
            border-radius: 8px;
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
