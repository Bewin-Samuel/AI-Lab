"""
Debt Analyzer Agent
- Identifies EMIs, loan payments, credit card dues from transactions
- Calculates debt-to-income ratio
- Flags high-risk patterns
"""

import json
from langchain_core.messages import HumanMessage, SystemMessage
from agents.llm_factory import get_llm


SYSTEM_PROMPT = """You are a debt analysis expert.
Given a transaction list and monthly income, identify:
1. All EMI / loan / credit card payments
2. Total monthly debt obligations
3. Debt-to-income (DTI) ratio
4. Risk level: LOW (<30% DTI), MEDIUM (30-50%), HIGH (>50%)
5. Specific actionable advice

Return ONLY valid JSON:
{
  "debt_payments": [
    {"description": "Home Loan EMI", "amount": 12000}
  ],
  "total_debt_monthly": 15000,
  "dti_percent": 30.0,
  "risk_level": "MEDIUM",
  "summary": "Your monthly debt payments are ₹15,000 (30% of income). Consider..."
}
"""


class DebtAnalyzerAgent:
    def __init__(self, api_key: str, vendor: str, model_name: str):
        self.llm = get_llm(api_key=api_key, vendor=vendor, model_name=model_name, temperature=0.2)

    def analyze(self, transactions: list, monthly_income: int) -> dict:
        if not transactions:
            return {
                "debt_payments": [],
                "total_debt_monthly": 0,
                "dti_percent": 0,
                "risk_level": "LOW",
                "summary": "No transactions found to analyze for debt."
            }

        tx_text = json.dumps(transactions[:50], indent=2)

        messages = [
            SystemMessage(content=SYSTEM_PROMPT),
            HumanMessage(content=(
                f"Monthly income: ₹{monthly_income:,}\n\n"
                f"Transactions:\n{tx_text}"
            ))
        ]

        response = self.llm.invoke(messages)
        try:
            return json.loads(response.content.strip())
        except json.JSONDecodeError:
            return {
                "debt_payments": [],
                "total_debt_monthly": 0,
                "dti_percent": 0,
                "risk_level": "LOW",
                "summary": "Could not analyze debt from provided data."
            }