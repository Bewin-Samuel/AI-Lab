"""
Expense Classifier Agent
- Takes transaction list
- Categorizes into: Food, Rent, Transport, Entertainment, EMI/Loan, Health, Utilities, Other
- Returns categorized dict with amounts per category
"""

import json
from langchain_core.messages import HumanMessage, SystemMessage
from agents.llm_factory import get_llm


SYSTEM_PROMPT = """You are an expense categorization expert.
Given a list of transactions, classify each debit/expense into one of these categories:
Food & Dining, Rent & Housing, Transport, Entertainment, EMI & Loans,
Health & Medical, Utilities & Bills, Shopping, Savings & Investments, Other.

Return ONLY valid JSON (no markdown):
{
  "categories": {
    "Food & Dining": [450, 320, 180],
    "Rent & Housing": [15000],
    "Transport": [500, 200],
    ...
  },
  "top_category": "Rent & Housing",
  "biggest_expense": 15000
}
Only include debit transactions (expenses). Ignore credits/income.
"""


class ExpenseClassifierAgent:
    def __init__(self, api_key: str, vendor: str, model_name: str):
        self.llm = get_llm(api_key=api_key, vendor=vendor, model_name=model_name, temperature=0)

    def categorize(self, transactions: list) -> dict:
        if not transactions:
            return {"categories": {}, "top_category": "N/A", "biggest_expense": 0}

        # Only send debits to classify
        debits = [t for t in transactions if t.get("type") == "debit"]
        if not debits:
            debits = transactions  # fallback if types not tagged

        print("ExpenseClassifierAgent: debit count", len(debits))

        tx_text = json.dumps(debits[:50], indent=2)  # cap at 50 for tokens

        messages = [
            SystemMessage(content=SYSTEM_PROMPT),
            HumanMessage(content=f"Categorize these transactions:\n{tx_text}")
        ]

        response = self.llm.invoke(messages)
        print("ExpenseClassifierAgent - LLM response")
        print(response.content.strip()[:500])  # Print first 500 chars for debugging
        
        try:
            return json.loads(response.content.strip())
        except json.JSONDecodeError:
            return {"categories": {}, "top_category": "N/A", "biggest_expense": 0}

    def summary_text(self, categorized: dict) -> str:
        cats = categorized.get("categories", {})
        if not cats:
            return "No expense data found."

        lines = []
        for cat, amounts in cats.items():
            total = sum(amounts)
            lines.append(f"**{cat}**: ₹{total:,}")

        top = categorized.get("top_category", "N/A")
        biggest = categorized.get("biggest_expense", 0)
        lines.append(f"\n📌 Biggest category: **{top}** (₹{biggest:,})")
        return " &nbsp;|&nbsp; ".join(lines[:5]) + (f"\n\n{lines[-1]}" if len(lines) > 5 else "")