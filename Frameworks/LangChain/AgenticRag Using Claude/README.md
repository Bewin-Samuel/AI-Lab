.# 🧠 Agentic RAG System

A LangChain-powered Agentic RAG with a Streamlit UI. Follows the flow from the architecture diagram:

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

## Setup

### 1. Install dependencies
```bash
pip install -r requirements.txt
```

### 2. Set API keys (or enter in the sidebar)
```bash
export OPENAI_API_KEY="sk-..."
export TAVILY_API_KEY="tvly-..."   # optional but recommended
```

### 3. Run
```bash
streamlit run app.py
```

## Project Structure

```
agentic_rag/
├── app.py            # Streamlit UI
├── agent.py          # RAG chain + LangChain agent + tools
├── requirements.txt
└── README.md
```

## Tools

| Tool | Purpose |
|------|---------|
| **FAISS + PDF** | Semantic search over uploaded PDF (RAG) |
| **Wikipedia** | Encyclopedic background knowledge |
| **Tavily** | Real-time web search & current events |
| **ArXiv** | Academic papers & scientific research |

## Notes

- Uses `gpt-4o-mini` by default (cost-efficient). Change in `agent.py → _get_llm()`.
- The RAG chain falls back to the agent if the PDF answer score is low.
- Tavily tool is only activated when `TAVILY_API_KEY` is set.
