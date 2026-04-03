"""
Savings Strategist Agent
- Takes categorized expenses + income + goal
- Produces a personalized savings plan with specific ₹ recommendations
"""

import json
from langchain_openai import ChatOpenAI
from langchain_core.messages import HumanMessage, SystemMessage
from agents.config import MODEL_NAME


SYSTEM_PROMPT = """You are a personal finance strategist specializing in the Indian market.
Given a user's expense categories, income, financial goal, and debt info,
create a practical, specific savings strategy.

Return ONLY valid JSON:
{
  "suggested_budget": {
    "Food & Dining": 4000,
    "Transport": 2000
  },
  "monthly_savings_target": 10000,
  "quick_wins": [
    "Cut Zomato/Swiggy orders to 4x/month — save ~₹1,200",
    "Switch OTT plan to family sharing — save ~₹300"
  ],
  "investment_suggestion": "Put ₹5,000/month in a SIP (Nifty 50 index fund)",
  "advice": "2-3 sentence personalized advice paragraph here"
}

Be specific with Indian context: mention SIP, PPF, FD, UPI cashback, etc. where relevant.
"""


class SavingsStrategistAgent:
    def __init__(self, api_key: str):
        self.llm = ChatOpenAI(
            model=MODEL_NAME,
            api_key=api_key,
            temperature=0.5
        )

    def build_strategy(
        self,
        categorized: dict,
        monthly_income: int,
        goal: str,
        debt_info: dict
    ) -> dict:
        cats = categorized.get("categories", {})
        category_totals = {
            cat: sum(amounts)
            for cat, amounts in cats.items()
            if isinstance(amounts, list)
        }

        prompt_data = {
            "monthly_income": monthly_income,
            "goal": goal,
            "expense_categories": category_totals,
            "total_debt_monthly": debt_info.get("total_debt_monthly", 0),
            "dti_percent": debt_info.get("dti_percent", 0),
        }

        messages = [
            SystemMessage(content=SYSTEM_PROMPT),
            HumanMessage(content=f"Build a savings strategy:\n{json.dumps(prompt_data, indent=2)}")
        ]

        response = self.llm.invoke(messages)
        try:
            return json.loads(response.content.strip())
        except json.JSONDecodeError:
            return {
                "suggested_budget": {},
                "monthly_savings_target": 0,
                "quick_wins": [],
                "investment_suggestion": "",
                "advice": "Could not generate strategy. Please try again."
            }
