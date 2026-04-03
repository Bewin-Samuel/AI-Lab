"""
LLM Factory
-----------
Returns the correct LangChain LLM based on vendor + model_name.
Vendor and model are now passed at runtime from the Streamlit UI —
no need to edit config.json to switch models.

Supported vendors:
  openai       → langchain-openai        (pip install langchain-openai)
  gemini       → langchain-google-genai  (pip install langchain-google-genai)
  openrouter   → langchain-openai        (same package, different base_url)
  huggingface  → langchain-huggingface   (pip install langchain-huggingface)

HuggingFace free models to try:
  mistralai/Mistral-7B-Instruct-v0.3
  HuggingFaceH4/zephyr-7b-beta
  Qwen/Qwen2.5-72B-Instruct
  microsoft/Phi-3.5-mini-instruct
"""


def get_llm(api_key: str, vendor: str, model_name: str, temperature: float = 0):
    """
    Return the correct LangChain chat model.

    Args:
        api_key:    API key for the selected vendor
        vendor:     One of: openai, gemini, openrouter, huggingface
        model_name: Exact model name string for that vendor
        temperature: 0 = deterministic, 1 = creative
    """
    vendor = vendor.lower().strip()

    # ── OpenAI ────────────────────────────────────────────────────────────────
    if vendor == "openai":
        try:
            from langchain_openai import ChatOpenAI
        except ImportError:
            raise ImportError("Run: pip install langchain-openai")
        return ChatOpenAI(
            model=model_name,
            api_key=api_key,
            temperature=temperature,
        )

    # ── OpenRouter (OpenAI-compatible, 300+ models) ───────────────────────────
    elif vendor == "openrouter":
        try:
            from langchain_openai import ChatOpenAI
        except ImportError:
            raise ImportError("Run: pip install langchain-openai")
        return ChatOpenAI(
            model=model_name,
            api_key=api_key,
            base_url="https://openrouter.ai/api/v1",
            temperature=temperature,
        )

    # ── Google Gemini (direct) ────────────────────────────────────────────────
    elif vendor == "gemini":
        try:
            from langchain_google_genai import ChatGoogleGenerativeAI
        except ImportError:
            raise ImportError("Run: pip install langchain-google-genai")
        return ChatGoogleGenerativeAI(
            model=model_name,
            google_api_key=api_key,
            temperature=temperature,
        )

    # ── HuggingFace Inference API ─────────────────────────────────────────────
    elif vendor == "huggingface":
        try:
            from langchain_huggingface import ChatHuggingFace, HuggingFaceEndpoint
        except ImportError:
            raise ImportError("Run: pip install langchain-huggingface huggingface_hub")
        endpoint = HuggingFaceEndpoint(
            repo_id=model_name,
            huggingfacehub_api_token=api_key,
            temperature=max(temperature, 0.01),  # HF requires temperature > 0
            max_new_tokens=1024,
        )
        return ChatHuggingFace(llm=endpoint)

    else:
        raise ValueError(
            f"Unsupported vendor '{vendor}'.\n"
            f"Allowed: 'openai', 'gemini', 'openrouter', 'huggingface'."
        )