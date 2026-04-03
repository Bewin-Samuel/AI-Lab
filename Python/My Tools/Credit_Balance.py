import os
import sys
from pathlib import Path

import requests
from dotenv import load_dotenv

API_KEY_ENV_VAR = "OPENROUTER_API_KEY"
OPENROUTER_KEY_INFO_URL = "https://openrouter.ai/api/v1/auth/key"
PLACEHOLDER_API_KEY = "REPLACE_WITH_FRESH_OPENROUTER_API_KEY"


def get_env_path() -> Path:
    """Return the .env path next to this script."""
    return Path(__file__).resolve().parent / ".env"


def mask_api_key(api_key: str) -> str:
    """Mask the API key while keeping enough characters for verification."""
    if len(api_key) < 12:
        return "[too short]"
    return f"{api_key[:6]}...{api_key[-4:]}"


def load_environment() -> None:
    """Load environment variables from .env in this folder."""
    env_path = get_env_path()
    load_dotenv(dotenv_path=env_path)


def enable_system_trust_store() -> None:
    """Enable OS trust store for requests on Windows when truststore is available."""
    if os.name != "nt":
        return

    try:
        import truststore  # type: ignore[import-not-found]

        truststore.inject_into_ssl()
    except ImportError:
        pass


def get_api_key() -> str:
    """Read API key from environment and fail fast if missing."""
    api_key = os.environ.get(API_KEY_ENV_VAR, "").strip()
    if not api_key:
        print(f"Error: {API_KEY_ENV_VAR} not found in .env or environment")
        print(f"Expected .env path: {get_env_path()}")
        sys.exit(1)
    if api_key == PLACEHOLDER_API_KEY:
        print(f"Error: {API_KEY_ENV_VAR} is still set to the placeholder value")
        print(f"Update this file: {get_env_path()}")
        sys.exit(1)
    return api_key


def main() -> None:
    load_environment()
    enable_system_trust_store()
    api_key = get_api_key()

    headers = {
        "Authorization": f"Bearer {api_key}",
        "Content-Type": "application/json",
    }

    try:
        response = requests.get(OPENROUTER_KEY_INFO_URL, headers=headers, timeout=20)
        response.raise_for_status()
    except requests.exceptions.SSLError as error:
        print("SSL error: certificate verification failed")
        print(f"Details: {error}")
        print("Hint: install truststore in your venv with 'pip install truststore'.")
        sys.exit(1)
    except requests.exceptions.HTTPError as error:
        response = error.response
        print(f"Request failed: {response.status_code} {response.reason}")
        print(f"Python executable: {sys.executable}")
        print(f".env path: {get_env_path()}")
        print(f"Key preview: {mask_api_key(api_key)}")
        try:
            print(f"Response body: {response.text}")
        except Exception:
            pass
        if response.status_code == 401:
            print("Action: replace OPENROUTER_API_KEY in .env with a fresh key from OpenRouter.")
        sys.exit(1)
    except requests.exceptions.RequestException as error:
        print(f"Request failed: {error}")
        sys.exit(1)

    data = response.json()
    limit = data.get("limit", 0)
    usage = data.get("usage", 0)

    print("Credit Balance Response:")
    print(response.text)
    print(f"Total limit: ${limit}")
    print(f"Used: ${usage}")
    print(f"Remaining: ${limit - usage}")


if __name__ == "__main__":
    main()