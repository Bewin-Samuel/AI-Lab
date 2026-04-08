"""Factory for creating model vendor instances."""

from config.constants import ModelVendor
from models.base import BaseModelVendor
from models.huggingface import HuggingFaceVendor
from models.openai_vendor import OpenAIVendor
from models.azure_openai import AzureOpenAIVendor
from models.openrouter import OpenRouterVendor


class VendorFactory:
    """Factory for creating vendor instances based on ModelVendor enum."""

    @staticmethod
    def create_vendor(
        vendor: ModelVendor,
        api_key: str,
        model_id: str,
        **kwargs
    ) -> BaseModelVendor:
        """
        Create a vendor instance based on the selected vendor type.
        
        Args:
            vendor: The ModelVendor enum value
            api_key: API key/token for the vendor
            model_id: Model ID or deployment name
            **kwargs: Additional vendor-specific parameters (e.g., endpoint for Azure)
            
        Returns:
            An instance of the appropriate BaseModelVendor subclass
            
        Raises:
            ValueError if vendor is not supported
        """
        if vendor == ModelVendor.HUGGINGFACE:
            return HuggingFaceVendor(api_key, model_id)
        
        elif vendor == ModelVendor.OPENAI:
            return OpenAIVendor(api_key, model_id)
        
        elif vendor == ModelVendor.AZURE_OPENAI:
            endpoint = kwargs.get("endpoint")
            return AzureOpenAIVendor(api_key, model_id, endpoint=endpoint)
        
        elif vendor == ModelVendor.OPENROUTER:
            return OpenRouterVendor(api_key, model_id)
        
        else:
            raise ValueError(
                f"Unsupported vendor: {vendor}. "
                f"Supported vendors: {[v.value for v in ModelVendor]}"
            )
