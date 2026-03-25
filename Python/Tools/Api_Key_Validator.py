"""OpenRouter API Key Validator - validates API connectivity and lists available models."""

import os
import sys
from pathlib import Path

from dotenv import load_dotenv
from openai import OpenAI, APIConnectionError, AuthenticationError

# Constants
API_KEY_ENV_VAR = "OPENROUTER_API_KEY"
API_BASE_URL = "https://openrouter.ai/api/v1"
MODELS_TO_DISPLAY = 10


def setup_tls_certificates() -> None:
    """Configure TLS certificate handling for Windows environments."""
    try:
        import certifi
        cert_path = certifi.where()
        os.environ.setdefault("SSL_CERT_FILE", cert_path)
        os.environ.setdefault("REQUESTS_CA_BUNDLE", cert_path)
    except ImportError:
        pass


def load_environment() -> None:
    """Load environment variables from .env file in Tools folder."""
    env_path = Path(__file__).resolve().parent / ".env"
    load_dotenv(dotenv_path=env_path)


def validate_api_key() -> str:
    """
    Retrieve and validate the API key.
    
    Returns:
        str: The API key
        
    Raises:
        SystemExit: If API key is not found
    """
    api_key = os.environ.get(API_KEY_ENV_VAR)
    if not api_key:
        print(f"✗ Error: {API_KEY_ENV_VAR} not found in environment variables")
        sys.exit(1)
    return api_key


def fetch_and_display_models(api_key: str) -> None:
    """
    Fetch models from OpenRouter API and display a summary.
    
    Args:
        api_key: The OpenRouter API key
    """
    try:
        client = OpenAI(api_key=api_key, base_url=API_BASE_URL)
        models = client.models.list()
        
        print(f"✓ Successfully connected to OpenRouter API!")
        print(f"✓ Available models: {len(models.data)}")
        print(f"\nFirst {MODELS_TO_DISPLAY} models:")
        for model in models.data[:MODELS_TO_DISPLAY]:
            print(f"  - {model.id}")
            
    except AuthenticationError:
        print("✗ Error: Invalid API key")
        sys.exit(1)
    except APIConnectionError as e:
        print(f"✗ Connection error: {e}")
        sys.exit(1)


def main() -> None:
    """Main entry point for the API validator."""
    setup_tls_certificates()
    load_environment()
    api_key = validate_api_key()
    fetch_and_display_models(api_key)


if __name__ == "__main__":
    main()