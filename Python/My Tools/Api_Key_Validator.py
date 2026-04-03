"""OpenRouter API Key Validator - validates API connectivity and lists available models."""

import os
import sys
import traceback
import json
from pathlib import Path
from urllib.error import HTTPError, URLError
from urllib.request import Request, urlopen

from dotenv import load_dotenv

# Constants
API_KEY_ENV_VAR = "OPENROUTER_API_KEY"
API_BASE_URL = "https://openrouter.ai/api/v1"
AUTH_KEY_URL = f"{API_BASE_URL}/auth/key"
DEBUG_ENV_VAR = "DEBUG_API_VALIDATOR"
USE_CERTIFI_ENV_VAR = "USE_CERTIFI_BUNDLE"
PLACEHOLDER_API_KEY = "REPLACE_WITH_FRESH_OPENROUTER_API_KEY"


def get_env_path() -> Path:
    """Return the .env path next to this script."""
    return Path(__file__).resolve().parent / ".env"


def mask_api_key(api_key: str) -> str:
    """Mask the API key while keeping enough characters for verification."""
    if len(api_key) < 12:
        return "[too short]"
    return f"{api_key[:6]}...{api_key[-4:]}"


def setup_tls_certificates() -> None:
    """Configure TLS certificate handling.

    By default, this keeps the system trust store unchanged (best for corporate
    TLS inspection environments on Windows). Set USE_CERTIFI_BUNDLE=1 to force
    certifi's CA bundle.
    """
    if os.environ.get(USE_CERTIFI_ENV_VAR, "").strip() != "1":
        return

    try:
        import certifi
        cert_path = certifi.where()
        os.environ.setdefault("SSL_CERT_FILE", cert_path)
        os.environ.setdefault("REQUESTS_CA_BUNDLE", cert_path)
    except ImportError:
        pass


def load_environment() -> None:
    """Load environment variables from .env file in Tools folder."""
    env_path = get_env_path()
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
        print(f"  - Expected .env path: {get_env_path()}")
        sys.exit(1)
    if api_key.strip() == PLACEHOLDER_API_KEY:
        print(f"✗ Error: {API_KEY_ENV_VAR} is still set to the placeholder value")
        print(f"  - .env path: {get_env_path()}")
        sys.exit(1)
    return api_key


def validate_and_display_key_details(api_key: str) -> None:
    """
    Validate the API key against OpenRouter and display credit details.
    
    Args:
        api_key: The OpenRouter API key
    """
    try:
        request = Request(
            AUTH_KEY_URL,
            headers={
                "Authorization": f"Bearer {api_key}",
                "Content-Type": "application/json",
            },
            method="GET",
        )
        with urlopen(request, timeout=20) as response:
            payload = json.loads(response.read().decode("utf-8"))

        limit = payload.get("limit", 0)
        usage = payload.get("usage", 0)

        print("✓ API key is valid")
        print(f"✓ Label: {payload.get('label', 'N/A')}")
        print(f"✓ Total limit: ${limit}")
        print(f"✓ Used: ${usage}")
        print(f"✓ Remaining: ${limit - usage}")

    except HTTPError as e:
        if e.code == 401:
            print("✗ Error: Invalid or unauthorized API key")
            print(f"  - Python executable: {sys.executable}")
            print(f"  - .env path: {get_env_path()}")
            print(f"  - Key preview: {mask_api_key(api_key)}")
            print("  - Action: replace OPENROUTER_API_KEY in .env with a fresh key from OpenRouter.")
        else:
            print("✗ API responded with an error status")
            print(f"  - Status code: {e.code}")

        try:
            error_body = e.read().decode("utf-8")
            print(f"  - Response body: {error_body}")
        except Exception:
            pass

        if os.environ.get(DEBUG_ENV_VAR, "").strip() == "1":
            print("\nDetailed traceback:")
            traceback.print_exc()

        sys.exit(1)
    except URLError as e:
        print("✗ Connection error")
        print(f"  - Exception type: {type(e.reason).__name__ if getattr(e, 'reason', None) else type(e).__name__}")
        print(f"  - Message: {e}")
        if getattr(e, "reason", None):
            print(f"  - Root cause: {e.reason}")

        if "CERTIFICATE_VERIFY_FAILED" in str(e.reason if getattr(e, 'reason', None) else e):
            print("  - Hint: TLS certificate trust failed.")
            print("          Try installing truststore: pip install truststore")
            print("          If on a corporate network, set REQUESTS_CA_BUNDLE to your org root CA PEM.")

        # Enable full traceback by setting DEBUG_API_VALIDATOR=1 in .env.
        if os.environ.get(DEBUG_ENV_VAR, "").strip() == "1":
            print("\nDetailed traceback:")
            traceback.print_exc()

        sys.exit(1)
    except Exception as e:
        print("✗ Unexpected error")
        print(f"  - Exception type: {type(e).__name__}")
        print(f"  - Message: {e}")

        if os.environ.get(DEBUG_ENV_VAR, "").strip() == "1":
            print("\nDetailed traceback:")
            traceback.print_exc()

        sys.exit(1)


def main() -> None:
    """Main entry point for the API validator."""
    setup_tls_certificates()
    load_environment()
    api_key = validate_api_key()
    validate_and_display_key_details(api_key)


if __name__ == "__main__":
    main()