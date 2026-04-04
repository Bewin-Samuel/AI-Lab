import sys
import os
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))

import streamlit as st
import json
from agents.orchestrator import run_financial_analysis

st.set_page_config(
    page_title="AI Financial Coach",   
    page_icon="💰",
    layout="wide"
)

# ── Styling ───────────────────────────────────────────────────────────────────
st.markdown("""
<style>
    .main { background: #0f1117; }
    .stApp { background: #0f1117; }
    h1 { color: #00d4aa; font-size: 2.4rem; font-weight: 700; }
    h2, h3 { color: #e0e0e0; }
    .metric-card {
        background: #1a1d2e; border: 1px solid #2a2d3e;
        border-radius: 12px; padding: 1.2rem; text-align: center;
    }
    .metric-val { font-size: 2rem; font-weight: 700; color: #00d4aa; }
    .metric-label { font-size: 0.85rem; color: #888; margin-top: 4px; }
    .agent-card {
        background: #1a1d2e; border-left: 3px solid #00d4aa;
        border-radius: 8px; padding: 1rem 1.2rem; margin-bottom: 1rem;
    }
    .agent-name { font-size: 0.75rem; color: #00d4aa; font-weight: 600;
                  text-transform: uppercase; letter-spacing: 1px; }
    .stButton > button {
        background: #00d4aa; color: #0f1117; font-weight: 700;
        border: none; border-radius: 8px; padding: 0.6rem 2rem;
        font-size: 1rem; width: 100%; margin-top: 1rem;
    }
    .stButton > button:hover { background: #00b894; }
    .hint-box {
        background: #12162a; border: 1px solid #2a2d3e; border-radius: 8px;
        padding: 0.7rem 1rem; margin-top: 0.4rem; font-size: 0.78rem; color: #888;
    }
</style>
""", unsafe_allow_html=True)

# ── Vendor config — defined here, used everywhere below ───────────────────────
VENDOR_OPTIONS = {
    "HuggingFace":  {
        "key": "huggingface",
        "emoji": "🟡",
        "placeholder": "hf_...",
        "hint": "Get key: huggingface.co → Settings → Access Tokens (free)",
        "default_model": "Qwen/Qwen2.5-72B-Instruct",
        "model_hint": "e.g. mistralai/Mistral-7B-Instruct-v0.3, HuggingFaceH4/zephyr-7b-beta, Qwen/Qwen2.5-72B-Instruct"
    },    
    "OpenAI":       {
        "key": "openai",
        "emoji": "🟢",
        "placeholder": "sk-...",
        "hint": "Get key: platform.openai.com → API keys",
        "default_model": "gpt-4o-mini",
        "model_hint": "e.g. gpt-4o-mini, gpt-4o, gpt-4-turbo"
    },
    "Gemini":       {
        "key": "gemini",
        "emoji": "🔵",
        "placeholder": "AIza...",
        "hint": "Get key: aistudio.google.com → Get API key (free)",
        "default_model": "gemini-2.0-flash",
        "model_hint": "e.g. gemini-2.0-flash, gemini-1.5-pro"
    },
    "OpenRouter":   {
        "key": "openrouter",
        "emoji": "🟠",
        "placeholder": "sk-or-...",
        "hint": "Get key: openrouter.ai → Keys  |  300+ models, pay-as-you-go",
        "default_model": "google/gemini-2.0-flash-lite-001",
        "model_hint": "e.g. openai/gpt-4o-mini, google/gemini-2.0-flash-lite-001, meta-llama/llama-3.1-8b-instruct:free"
    },
}

# ── Header ────────────────────────────────────────────────────────────────────
st.markdown("# 💰 AI Financial Coach")
st.markdown("##  Powered by Outskill C5:Group 3")

st.markdown("*Upload your bank statement or salary slip — our agents will analyze and advise.*")
st.divider()

# ── Sidebar ───────────────────────────────────────────────────────────────────
with st.sidebar:
    st.markdown("### ⚙️ Configuration")

    # Vendor dropdown
    selected_vendor_label = st.selectbox(
        "AI Vendor",
        options=list(VENDOR_OPTIONS.keys()),
        index=0,
        help="Choose which AI provider to use"
    )
    cfg = VENDOR_OPTIONS[selected_vendor_label]

    # Model name text input — pre-filled with a good default
    model_name = st.text_input(
        "Model name",
        value=cfg["default_model"],
        placeholder=cfg["default_model"],
        help="Type the exact model name for the selected vendor"
    )
    st.markdown(f'<div class="hint-box">💡 {cfg["model_hint"]}</div>', unsafe_allow_html=True)

    st.markdown("")

    # API key input — label and placeholder match selected vendor
    api_key = st.text_input(
        f"{cfg['emoji']} {selected_vendor_label} API Key",
        type="password",
        placeholder=cfg["placeholder"],
        help=cfg["hint"]
    )
    st.markdown(f'<div class="hint-box">🔑 {cfg["hint"]}</div>', unsafe_allow_html=True)

    st.markdown("---")
    st.markdown("**Agents active:**")
    st.markdown("🔍 Document Reader")
    st.markdown("📊 Expense Classifier")
    st.markdown("💡 Debt Analyzer")
    st.markdown("🎯 Savings Strategist")
    st.markdown("📋 Report Builder")

# ── Main layout ───────────────────────────────────────────────────────────────
col1, col2 = st.columns([1, 1.8], gap="large")

with col1:
    st.markdown("### 📂 Upload Your Data")
    uploaded_file = st.file_uploader(
        "Bank statement, salary slip, or expense CSV",
        type=["pdf", "csv", "txt"],
        help="Supported: PDF, CSV, TXT"
    )

    if uploaded_file:
        st.success(f"✅ `{uploaded_file.name}` uploaded")

        monthly_income = st.number_input(
            "Monthly income (₹)", min_value=0, value=50000, step=1000
        )
        savings_goal = st.selectbox(
            "Primary goal",
            ["Pay off debt faster", "Build emergency fund",
             "Save for a goal", "Invest more", "General budgeting"]
        )
        analyze_btn = st.button("🚀 Analyze with AI Agents")

with col2:
    st.markdown("### 📊 Analysis Results")

    if "results" not in st.session_state:
        st.info("Upload a file and click **Analyze** to see results here.")
    else:
        r = st.session_state.results

        # Metrics row
        m1, m2, m3 = st.columns(3)
        with m1:
            st.markdown(f"""<div class="metric-card">
                <div class="metric-val">₹{r['total_expenses']:,}</div>
                <div class="metric-label">Total Expenses</div>
            </div>""", unsafe_allow_html=True)
        with m2:
            surplus = r['monthly_income'] - r['total_expenses']
            color = "#00d4aa" if surplus >= 0 else "#ff6b6b"
            st.markdown(f"""<div class="metric-card">
                <div class="metric-val" style="color:{color}">₹{abs(surplus):,}</div>
                <div class="metric-label">{'Surplus' if surplus >= 0 else 'Deficit'}</div>
            </div>""", unsafe_allow_html=True)
        with m3:
            savings_pct = max(0, round(surplus / r['monthly_income'] * 100, 1)) if r['monthly_income'] > 0 else 0
            st.markdown(f"""<div class="metric-card">
                <div class="metric-val">{savings_pct}%</div>
                <div class="metric-label">Savings Rate</div>
            </div>""", unsafe_allow_html=True)

        st.markdown("---")

        # Agent outputs
        for agent_output in r.get("agent_outputs", []):
            st.markdown(f"""<div class="agent-card">
                <div class="agent-name">🤖 {agent_output['agent']}</div>
                <div style="color:#d0d0d0; margin-top:8px; font-size:0.95rem;">
                    {agent_output['output']}
                </div>
            </div>""", unsafe_allow_html=True)


# ── Run analysis on button click ──────────────────────────────────────────────
if uploaded_file and "analyze_btn" in dir() and analyze_btn:
    if not api_key:
        st.error(f"⚠️ Please enter your {selected_vendor_label} API key in the sidebar.")
    elif not model_name:
        st.error("⚠️ Please enter a model name in the sidebar.")
    else:
        with st.spinner("🤖 Agents are analyzing your finances..."):
            file_content = uploaded_file.read()
            file_type = uploaded_file.name.split(".")[-1].lower()

            results = run_financial_analysis(
                file_content=file_content,
                file_type=file_type,
                monthly_income=monthly_income,
                savings_goal=savings_goal,
                api_key=api_key,
                vendor=cfg["key"],
                model_name=model_name,
            )
            st.session_state.results = results
            st.rerun()