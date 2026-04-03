import streamlit as st
import os
import tempfile
from dotenv import load_dotenv
from agent import build_agent, build_rag_chain
from langchain_core.messages import HumanMessage, AIMessage

load_dotenv()


def _friendly_error_message(err: Exception) -> str:
    """Convert provider errors into actionable user-facing messages."""
    msg = str(err)
    msg_l = msg.lower()

    if (
        "insufficient_quota" in msg_l
        or "exceeded your current quota" in msg_l
        or "error code: 429" in msg_l
    ):
        return (
            "OpenAI quota exceeded (429: insufficient_quota).\n\n"
            "Fix options:\n"
            "1. Add billing/credits to your OpenAI account.\n"
            "2. Verify OPENAI_API_KEY in .env points to the correct paid project key.\n"
            "3. Restart Streamlit after updating .env so new credentials are loaded."
        )

    return msg

# ── Page config ──────────────────────────────────────────────────────────────
st.set_page_config(
    page_title="Agentic RAG",
    page_icon="🧠",
    layout="wide",
    initial_sidebar_state="expanded",
)

# ── Custom CSS ────────────────────────────────────────────────────────────────
st.markdown("""
<style>
@import url('https://fonts.googleapis.com/css2?family=IBM+Plex+Mono:wght@400;600&family=Space+Grotesk:wght@300;400;600;700&display=swap');

:root {
    --bg:        #0d0f14;
    --surface:   #161922;
    --border:    #262c38;
    --accent:    #5b8dee;
    --accent2:   #3ecf8e;
    --accent3:   #f0883e;
    --text:      #e2e8f0;
    --muted:     #8892a4;
    --danger:    #f87171;
}

html, body, [class*="css"] {
    font-family: 'Space Grotesk', sans-serif;
    background-color: var(--bg) !important;
    color: var(--text) !important;
}

/* ---- Sidebar ---- */
section[data-testid="stSidebar"] {
    background-color: var(--surface) !important;
    border-right: 1px solid var(--border);
}

/* ---- Header ---- */
.rag-header {
    display: flex;
    align-items: center;
    gap: 14px;
    padding: 0 0 24px 0;
    border-bottom: 1px solid var(--border);
    margin-bottom: 28px;
}
.rag-header .logo {
    width: 42px; height: 42px;
    background: linear-gradient(135deg, var(--accent), var(--accent2));
    border-radius: 10px;
    display: flex; align-items: center; justify-content: center;
    font-size: 22px;
}
.rag-header h1 {
    font-size: 1.5rem; font-weight: 700;
    background: linear-gradient(90deg, var(--accent), var(--accent2));
    -webkit-background-clip: text; -webkit-text-fill-color: transparent;
    margin: 0;
}
.rag-header .sub {
    font-family: 'IBM Plex Mono', monospace;
    font-size: 0.72rem; color: var(--muted); margin-top: 2px;
}

/* ---- Source badge ---- */
.source-badge {
    display: inline-block;
    padding: 2px 10px;
    border-radius: 20px;
    font-size: 0.7rem;
    font-family: 'IBM Plex Mono', monospace;
    font-weight: 600;
    letter-spacing: 0.05em;
    margin-right: 6px;
    margin-bottom: 4px;
}
.badge-pdf      { background: #1a3a2a; color: var(--accent2); border: 1px solid var(--accent2); }
.badge-wikipedia{ background: #1a2a3a; color: var(--accent);  border: 1px solid var(--accent); }
.badge-tavily   { background: #2a2a1a; color: var(--accent3); border: 1px solid var(--accent3); }
.badge-arxiv    { background: #2a1a1a; color: var(--danger);  border: 1px solid var(--danger); }
.badge-agent    { background: #1f1a2a; color: #a78bfa;        border: 1px solid #a78bfa; }

/* ---- Chat bubbles ---- */
.chat-user {
    background: linear-gradient(135deg, #1e2a45, #1a2238);
    border: 1px solid var(--accent);
    border-radius: 16px 16px 4px 16px;
    padding: 14px 18px;
    margin: 10px 0;
    max-width: 75%;
    margin-left: auto;
    box-shadow: 0 2px 12px rgba(91,141,238,0.12);
}
.chat-ai {
    background: var(--surface);
    border: 1px solid var(--border);
    border-radius: 16px 16px 16px 4px;
    padding: 14px 18px;
    margin: 10px 0;
    max-width: 85%;
    box-shadow: 0 2px 12px rgba(0,0,0,0.25);
}
.chat-label {
    font-family: 'IBM Plex Mono', monospace;
    font-size: 0.65rem;
    color: var(--muted);
    letter-spacing: 0.08em;
    margin-bottom: 6px;
    text-transform: uppercase;
}

/* ---- Tool call pill ---- */
.tool-pill {
    display: inline-flex; align-items: center; gap: 6px;
    background: #1a1f2e;
    border: 1px solid var(--border);
    border-radius: 8px;
    padding: 4px 12px;
    font-family: 'IBM Plex Mono', monospace;
    font-size: 0.72rem;
    color: var(--muted);
    margin: 4px 4px 4px 0;
}
.tool-pill .dot { width:7px; height:7px; border-radius:50%; }

/* ---- Upload area ---- */
.upload-hint {
    background: #0f1620;
    border: 1px dashed var(--border);
    border-radius: 10px;
    padding: 14px 16px;
    font-size: 0.82rem;
    color: var(--muted);
    line-height: 1.6;
}

/* ---- Streamlit overrides ---- */
.stTextInput > div > div > input,
.stChatInput > div > div > textarea {
    background: var(--surface) !important;
    color: var(--text) !important;
    border-color: var(--border) !important;
    font-family: 'Space Grotesk', sans-serif !important;
}
.stButton > button {
    background: linear-gradient(135deg, var(--accent), #3b6fd4) !important;
    color: white !important;
    border: none !important;
    border-radius: 8px !important;
    font-weight: 600 !important;
}
div[data-testid="stFileUploader"] {
    background: var(--surface);
    border: 1px dashed var(--border);
    border-radius: 10px;
}
.stExpander {
    background: var(--surface) !important;
    border: 1px solid var(--border) !important;
    border-radius: 8px !important;
}
hr { border-color: var(--border) !important; }
</style>
""", unsafe_allow_html=True)

# ── Session state ─────────────────────────────────────────────────────────────
if "messages" not in st.session_state:
    st.session_state.messages = []
if "rag_chain" not in st.session_state:
    st.session_state.rag_chain = None
if "pdf_name" not in st.session_state:
    st.session_state.pdf_name = None
if "agent" not in st.session_state:
    st.session_state.agent = None
if "tool_calls_log" not in st.session_state:
    st.session_state.tool_calls_log = {}

# ── Sidebar ───────────────────────────────────────────────────────────────────
with st.sidebar:
    st.markdown("""
    <div style="padding:0 0 20px 0; border-bottom:1px solid var(--border); margin-bottom:20px">
        <div style="font-family:'IBM Plex Mono',monospace; font-size:0.7rem; color:var(--muted); letter-spacing:0.1em; text-transform:uppercase; margin-bottom:6px">Configuration</div>
        <div style="font-size:1.1rem; font-weight:700; color:var(--text)">🧠 Agentic RAG</div>
    </div>
    """, unsafe_allow_html=True)

    st.markdown("**🔑 API Keys**")
    openai_key = os.getenv("OPENAI_API_KEY", "")
    tavily_key = os.getenv("TAVILY_API_KEY", "")
    st.caption("Keys are loaded from your .env file.")

    st.markdown("---")
    st.markdown("**📄 PDF Upload** *(optional)*")
    uploaded_pdf = st.file_uploader("Upload PDF for RAG", type=["pdf"], label_visibility="collapsed")

    if uploaded_pdf:
        if uploaded_pdf.name != st.session_state.pdf_name:
            with st.spinner("🔍 Indexing PDF with FAISS..."):
                try:
                    with tempfile.NamedTemporaryFile(delete=False, suffix=".pdf") as tmp:
                        tmp.write(uploaded_pdf.read())
                        tmp_path = tmp.name
                    os.environ["OPENAI_API_KEY"] = openai_key
                    from agent import build_rag_chain
                    st.session_state.rag_chain = build_rag_chain(tmp_path)
                    st.session_state.pdf_name = uploaded_pdf.name
                    st.success(f"✅ Indexed: **{uploaded_pdf.name}**")
                except Exception as e:
                    st.error(f"❌ {_friendly_error_message(e)}")
    else:
        st.markdown("""
        <div class="upload-hint">
        No PDF uploaded.<br>
        Agent will use <b>Wikipedia</b>, <b>Tavily</b>, and <b>ArXiv</b> to answer queries.
        </div>
        """, unsafe_allow_html=True)

    st.markdown("---")
    st.markdown("**🛠️ Active Tools**")
    pdf_active = st.session_state.rag_chain is not None
    col1, col2 = st.columns(2)
    with col1:
        st.markdown(f"{'🟢' if pdf_active else '⚫'} RAG/PDF")
        st.markdown("🟢 Wikipedia")
    with col2:
        st.markdown("🟢 Tavily")
        st.markdown("🟢 ArXiv")

    st.markdown("---")
    if st.button("🗑️ Clear Chat"):
        st.session_state.messages = []
        st.session_state.tool_calls_log = {}
        st.rerun()

# ── Main area ─────────────────────────────────────────────────────────────────
st.markdown("""
<div class="rag-header">
    <div class="logo">🧠</div>
    <div>
        <h1>Agentic RAG System</h1>
        <div class="sub">LangChain · FAISS · Wikipedia · Tavily · ArXiv</div>
    </div>
</div>
""", unsafe_allow_html=True)

# Flow diagram expander
with st.expander("📊 View Agent Flow Logic", expanded=False):
    st.markdown("""
    ```
    User Query
        │
        ├─ PDF uploaded? ──Yes──▶ RAG (FAISS + PDF)
        │                              │
        │                         Answer found? ──Yes──▶ Return PDF Answer
        │                              │
        │                             No ──────────────────┐
        │                                                   │
        └─ No ──────────────────────────────────────────────▶
                                                            │
                                                    LangChain Agent
                                                  (Decides which tool)
                                                   /       |       \
                                              Wikipedia  Tavily   ArXiv
                                                   \       |       /
                                                    Response + Sources
    ```
    """)

# ── Chat display ──────────────────────────────────────────────────────────────
for i, msg in enumerate(st.session_state.messages):
    if msg["role"] == "user":
        st.markdown(f"""
        <div class="chat-user">
            <div class="chat-label">You</div>
            {msg['content']}
        </div>
        """, unsafe_allow_html=True)
    else:
        tool_html = ""
        if i in st.session_state.tool_calls_log:
            tools_used = st.session_state.tool_calls_log[i]
            pills = ""
            colors = {"pdf": "#3ecf8e", "wikipedia": "#5b8dee",
                      "tavily": "#f0883e", "arxiv": "#f87171", "agent": "#a78bfa"}
            for t in tools_used:
                c = colors.get(t.lower(), "#8892a4")
                pills += f'<span class="tool-pill"><span class="dot" style="background:{c}"></span>{t}</span>'
            tool_html = f'<div style="margin-bottom:8px">{pills}</div>'

        source_html = ""
        if "sources" in msg:
            for src in msg["sources"]:
                badge_class = f"badge-{src['type'].lower()}"
                source_html += f'<span class="source-badge {badge_class}">{src["type"]}</span>'
            if source_html:
                source_html = f'<div style="margin-top:10px; padding-top:8px; border-top:1px solid var(--border)">{source_html}</div>'

        st.markdown(f"""
        <div class="chat-ai">
            <div class="chat-label">Assistant</div>
            {tool_html}
            {msg['content']}
            {source_html}
        </div>
        """, unsafe_allow_html=True)

# ── Chat input ────────────────────────────────────────────────────────────────
if prompt := st.chat_input("Ask anything — PDF context or web search..."):

    if not openai_key:
        st.error("❌ OPENAI_API_KEY not found. Add it to your .env file.")
        st.stop()

    os.environ["OPENAI_API_KEY"] = openai_key
    if tavily_key:
        os.environ["TAVILY_API_KEY"] = tavily_key

    st.session_state.messages.append({"role": "user", "content": prompt})
    st.rerun()

# ── Process last user message if no assistant reply yet ───────────────────────
if (st.session_state.messages and
        st.session_state.messages[-1]["role"] == "user"):

    user_q = st.session_state.messages[-1]["content"]
    msg_idx = len(st.session_state.messages)  # index for the upcoming assistant msg

    with st.spinner("🤔 Thinking..."):
        try:
            os.environ["OPENAI_API_KEY"] = openai_key
            if tavily_key:
                os.environ["TAVILY_API_KEY"] = tavily_key

            tools_used = []
            answer = ""
            sources = []

            # ── Step 1: Try RAG if PDF is loaded ────────────────────────────
            if st.session_state.rag_chain is not None:
                tools_used.append("PDF/RAG")
                rag_result = st.session_state.rag_chain.invoke({"query": user_q})
                rag_answer = rag_result.get("result", "").strip()
                rag_docs = rag_result.get("source_documents", [])

                if rag_answer and len(rag_answer) > 30 and "don't know" not in rag_answer.lower():
                    answer = rag_answer
                    sources = [{"type": "PDF", "text": d.page_content[:80]} for d in rag_docs[:3]]
                else:
                    # Fall through to agent
                    tools_used.append("Agent")
                    agent = build_agent()
                    result = agent.invoke({"input": user_q})
                    answer = result.get("output", "No answer found.")
                    # detect which tools were used from intermediate steps
                    for step in result.get("intermediate_steps", []):
                        tool_name = step[0].tool if hasattr(step[0], "tool") else ""
                        if tool_name and tool_name not in tools_used:
                            tools_used.append(tool_name.capitalize())
                            sources.append({"type": tool_name.capitalize()})

            else:
                # ── Step 2: No PDF — go directly to agent ───────────────────
                tools_used.append("Agent")
                agent = build_agent()
                result = agent.invoke({"input": user_q})
                answer = result.get("output", "No answer found.")
                for step in result.get("intermediate_steps", []):
                    tool_name = step[0].tool if hasattr(step[0], "tool") else ""
                    if tool_name and tool_name not in tools_used:
                        tools_used.append(tool_name.capitalize())
                        sources.append({"type": tool_name.capitalize()})

            st.session_state.tool_calls_log[msg_idx] = tools_used
            st.session_state.messages.append({
                "role": "assistant",
                "content": answer,
                "sources": sources,
            })

        except Exception as e:
            st.session_state.messages.append({
                "role": "assistant",
                "content": (
                    "⚠️ Error:\n"
                    f"{_friendly_error_message(e)}\n\n"
                    "Please check your API keys and dependencies."
                ),
                "sources": [],
            })

    st.rerun()
