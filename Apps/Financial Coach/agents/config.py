"""Centralized app configuration for agent settings."""

import json
from pathlib import Path


DEFAULT_MODEL_NAME = "gpt-4o-mini"
_ROOT_DIR = Path(__file__).resolve().parent.parent
_CONFIG_PATH = _ROOT_DIR / "config.json"


def _load_config() -> dict:
    if not _CONFIG_PATH.exists():
        return {}

    with _CONFIG_PATH.open("r", encoding="utf-8") as file:
        return json.load(file)


_CONFIG = _load_config()
MODEL_NAME = _CONFIG.get("model_name", DEFAULT_MODEL_NAME)
