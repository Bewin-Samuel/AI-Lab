"""
agent.py  –  Core logic for the Agentic RAG system.

Flow (mirrors the screenshot):
  1. If a PDF is uploaded → build FAISS retriever → try to answer from PDF.
  2. If no PDF, or PDF answer is unsatisfactory → hand off to LangChain agent.
  3. LangChain agent decides between: Wikipedia | Tavily Search | ArXiv.
  4. Returns final answer + source metadata.
"""

from __future__ import annotations

import os
from functools import lru_cache
from typing import Any

# ── LangChain core ────────────────────────────────────────────────────────────
from langgraph.prebuilt import create_react_agent
from langchain_community.document_loaders import PyPDFLoader
from langchain_community.tools import ArxivQueryRun, WikipediaQueryRun
from langchain_community.tools.tavily_search import TavilySearchResults
from langchain_community.utilities import ArxivAPIWrapper, WikipediaAPIWrapper
from langchain_community.vectorstores import FAISS
from langchain_core.prompts import PromptTemplate
from langchain_core.runnables import RunnableLambda
from langchain_openai import ChatOpenAI, OpenAIEmbeddings
from langchain_text_splitters import RecursiveCharacterTextSplitter


# ─────────────────────────────────────────────────────────────────────────────
# 1.  RAG chain  (PDF → FAISS → RetrievalQA)
# ─────────────────────────────────────────────────────────────────────────────

def build_rag_chain(pdf_path: str="file:///C:/Users/bewin.samuel/Downloads/survey_of_self_evolving_agents.pdf") -> RunnableLambda:
    """
    Loads a PDF, splits it, embeds with OpenAI, stores in FAISS,
    and wraps in a RunnableLambda that returns {result, source_documents}.
    """
    loader = PyPDFLoader(pdf_path)
    pages = loader.load()

    splitter = RecursiveCharacterTextSplitter(
        chunk_size=1000,
        chunk_overlap=150,
        separators=["\n\n", "\n", ".", " ", ""],
    )
    docs = splitter.split_documents(pages)

    embeddings = _get_embeddings()
    vectorstore = FAISS.from_documents(docs, embeddings)
    retriever = vectorstore.as_retriever(
        search_type="similarity",
        search_kwargs={"k": 4},
    )

    llm = _get_llm()
    prompt = _rag_prompt()

    def _invoke(inputs: dict) -> dict:
        query = inputs["query"]
        source_docs = retriever.invoke(query)
        context = "\n\n".join(d.page_content for d in source_docs)
        prompt_text = prompt.format(context=context, question=query)
        response = llm.invoke(prompt_text)
        return {
            "result": response.content,
            "source_documents": source_docs,
        }

    return RunnableLambda(_invoke)


def _rag_prompt() -> PromptTemplate:
    template = """You are an expert assistant. Use the following context extracted from
a PDF document to answer the question.

If the answer is not clearly present in the context, reply with exactly:
"I don't have enough information in the PDF to answer this question."

Context:
{context}

Question: {question}

Answer (be concise and cite the relevant part of the context):"""
    return PromptTemplate(template=template, input_variables=["context", "question"])


# ─────────────────────────────────────────────────────────────────────────────
# 2.  Tools
# ─────────────────────────────────────────────────────────────────────────────

def _build_tools() -> list:
    tools = []

    # ── Wikipedia ────────────────────────────────────────────────────────────
    wiki_wrapper = WikipediaAPIWrapper(top_k_results=3, doc_content_chars_max=2000)
    wiki_tool = WikipediaQueryRun(
        api_wrapper=wiki_wrapper,
        description=(
            "Useful for encyclopedic background knowledge, definitions, historical facts, "
            "or general information about people, places, concepts, and events. "
            "Input should be a concise search term."
        ),
    )
    tools.append(wiki_tool)

    # ── Tavily web search ─────────────────────────────────────────────────────
    tavily_key = os.getenv("TAVILY_API_KEY", "")
    if tavily_key:
        tavily_tool = TavilySearchResults(
            max_results=5,
            description=(
                "Useful for current events, recent news, live data, product information, "
                "or anything that requires up-to-date web information. "
                "Input should be a clear search query."
            ),
        )
        tools.append(tavily_tool)

    # ── ArXiv ─────────────────────────────────────────────────────────────────
    arxiv_wrapper = ArxivAPIWrapper(top_k_results=3, doc_content_chars_max=2000)
    arxiv_tool = ArxivQueryRun(
        api_wrapper=arxiv_wrapper,
        description=(
            "Useful for scientific research questions, academic papers, machine learning, "
            "physics, mathematics, computer science, and technical topics. "
            "Input should be a research topic or paper title/keywords."
        ),
    )
    tools.append(arxiv_tool)

    return tools


# ─────────────────────────────────────────────────────────────────────────────
# 3.  Agent
# ─────────────────────────────────────────────────────────────────────────────

def build_agent():
    """
    Builds a LangGraph react agent with Wikipedia, Tavily, and ArXiv tools.
    Returns a wrapper whose .invoke({"input": ...}) returns
    {"output": str, "intermediate_steps": list} to match app.py expectations.
    """
    llm = _get_llm()
    tools = _build_tools()

    system_prompt = """You are an intelligent research assistant with access to three tools:

1. **wikipedia** – For encyclopedic background, definitions, history, biographies.
2. **tavily_search_results_json** – For current events, news, and real-time web data.
3. **arxiv** – For academic papers, scientific research, and technical topics.

Strategy:
- Choose the MOST APPROPRIATE tool for the query.
- For factual/conceptual questions → Wikipedia first.
- For recent/current events → Tavily.
- For scientific/technical research → ArXiv.
- You may call multiple tools if needed to give a comprehensive answer.
- Always cite your sources in the final answer.
- Be concise yet thorough."""

    graph = create_react_agent(llm, tools, prompt=system_prompt)

    class _AgentWrapper:
        def invoke(self, inputs: dict) -> dict:
            from langchain_core.messages import HumanMessage, AIMessage, ToolMessage
            result = graph.invoke({"messages": [HumanMessage(content=inputs["input"])]})
            messages = result.get("messages", [])
            # Last AIMessage is the final answer
            output = ""
            for m in reversed(messages):
                if isinstance(m, AIMessage) and m.content:
                    output = m.content
                    break
            # Build intermediate_steps compatible with app.py
            intermediate_steps = []
            for m in messages:
                if isinstance(m, AIMessage) and getattr(m, "tool_calls", None):
                    for tc in m.tool_calls:
                        intermediate_steps.append(
                            (type("ToolCall", (), {"tool": tc["name"]})(), "")
                        )
            return {"output": output, "intermediate_steps": intermediate_steps}

    return _AgentWrapper()


# ─────────────────────────────────────────────────────────────────────────────
# 4.  Helpers
# ─────────────────────────────────────────────────────────────────────────────

def _get_llm(temperature: float = 0.0) -> ChatOpenAI:
    api_key = os.getenv("OPENAI_API_KEY", "")
    is_openrouter = api_key.startswith("sk-or-v1-")
    base_url = os.getenv("OPENAI_BASE_URL") or (
        "https://openrouter.ai/api/v1" if is_openrouter else None
    )
    model = os.getenv("OPENAI_MODEL") or (
        "openai/gpt-4o-mini" if is_openrouter else "gpt-4o-mini"
    )

    return ChatOpenAI(
        model=model,
        base_url=base_url,
        temperature=temperature,
        streaming=False,
    )


def _get_embeddings() -> OpenAIEmbeddings:
    api_key = os.getenv("OPENAI_API_KEY", "")
    is_openrouter = api_key.startswith("sk-or-v1-")
    base_url = os.getenv("OPENAI_EMBEDDINGS_BASE_URL") or os.getenv("OPENAI_BASE_URL") or (
        "https://openrouter.ai/api/v1" if is_openrouter else None
    )
    model = os.getenv("OPENAI_EMBEDDINGS_MODEL") or (
        "openai/text-embedding-3-small" if is_openrouter else "text-embedding-3-small"
    )

    return OpenAIEmbeddings(
        model=model,
        base_url=base_url,
    )
