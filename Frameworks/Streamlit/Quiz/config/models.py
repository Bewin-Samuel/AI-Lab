"""Model ID mappings for each vendor."""

from config.constants import ModelVendor
from typing import Dict, List
import httpx


class ModelRegistry:
    """Central registry of available models per vendor."""

    # OpenAI models
    OPENAI_MODELS = [
        "gpt-4o",
        "gpt-4-turbo",
        "gpt-3.5-turbo",
    ]

    # Azure OpenAI models (requires deployment name matching model ID)
    AZURE_OPENAI_MODELS = [
        "gpt-4o",
        "gpt-4-turbo",
        "gpt-35-turbo",
    ]

    # OpenRouter models (popular open-source and commercial)
    OPENROUTER_MODELS = [
        "meta-llama/llama-2-70b-chat",
        "meta-llama/llama-3-70b-instruct",
        "mistralai/mistral-7b-instruct",
        "openai/gpt-4-turbo",
        "openai/gpt-3.5-turbo",
    ]

    # Hugging Face models (supports both API and local inference)
    HUGGINGFACE_MODELS = [
        "meta-llama/Llama-3.1-8B-Instruct",
        "Qwen/Qwen2.5-7B-Instruct",
        "openai/gpt-oss-20b",
        "google/gemma-3-27b-it",
        "meta-llama/Meta-Llama-3-8B-Instruct",
    ]

    VENDOR_MODELS: Dict[ModelVendor, List[str]] = {
        ModelVendor.OPENAI: OPENAI_MODELS,
        ModelVendor.AZURE_OPENAI: AZURE_OPENAI_MODELS,
        ModelVendor.OPENROUTER: OPENROUTER_MODELS,
        ModelVendor.HUGGINGFACE: HUGGINGFACE_MODELS,
    }

    @classmethod
    def _get_huggingface_router_models(cls, limit: int = 30) -> List[str]:
        """
        Fetch currently available model IDs from Hugging Face router.

        Falls back to static defaults if the API call fails.

        Args:
            limit: Maximum number of model IDs to return

        Returns:
            List of model IDs from router catalog
        """
        try:
            response = httpx.get("https://router.huggingface.co/v1/models", timeout=15.0)
            response.raise_for_status()
            data = response.json().get("data", [])

            model_ids: List[str] = []
            for item in data:
                model_id = item.get("id")
                if isinstance(model_id, str) and "/" in model_id:
                    model_ids.append(model_id)
                if len(model_ids) >= limit:
                    break

            return model_ids
        except Exception:
            return cls.HUGGINGFACE_MODELS

    @classmethod
    def get_models_for_vendor(cls, vendor: ModelVendor) -> List[str]:
        """
        Get list of available models for a given vendor.
        
        Args:
            vendor: The model vendor
            
        Returns:
            List of available model IDs for the vendor
        """
        if vendor == ModelVendor.HUGGINGFACE:
            return cls._get_huggingface_router_models()

        return cls.VENDOR_MODELS.get(vendor, [])

    @classmethod
    def get_all_vendors(cls) -> List[str]:
        """Get list of all available vendors."""
        return [v.value for v in ModelVendor]
