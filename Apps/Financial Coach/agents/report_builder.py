"""
Report Builder Agent
- Synthesizes all previous agent outputs
- Produces a concise executive summary
"""

import json
from langchain_openai import ChatOpenAI
from langchain_core.messages import HumanMessage, SystemMessage
from agents.config import MODEL_NAME


SYSTEM_PROMPT = """You are a financial report writer.
Given analysis from multiple agents, write a crisp, encouraging executive summary
that a non-finance person can act on immediately.

Return ONLY valid JSON:
{
  "summary": "3-4 sentence summary of the user's financial health and top 2 actions to take this month",
  "health_score": 72,
  "health_label": "Fair",
  "top_actions": [
    "Action 1 with ₹ impact",
    "Action 2 with ₹ impact",
    "Action 3 with ₹ impact"
  ]
}
health_score: 0-100 (>75 = Good, 50-75 = Fair, <50 = Needs Attention)
Be warm and motivating, not alarming.
"""


class ReportBuilderAgent:
    def __init__(self, api_key: str):
        self.llm = ChatOpenAI(
            model=MODEL_NAME,
            api_key=api_key,
            temperature=0.4
        )

    def build(
        self,
        categorized: dict,
        strategy: dict,
        debt_info: dict,
        monthly_income: int
    ) -> dict:
        context = {
            "monthly_income": monthly_income,
            "dti_percent": debt_info.get("dti_percent", 0),
            "risk_level": debt_info.get("risk_level", "LOW"),
            "monthly_savings_target": strategy.get("monthly_savings_target", 0),
            "quick_wins": strategy.get("quick_wins", []),
            "investment_suggestion": strategy.get("investment_suggestion", ""),
        }

        messages = [
            SystemMessage(content=SYSTEM_PROMPT),
            HumanMessage(content=f"Build report from:\n{json.dumps(context, indent=2)}")
        ]

        response = self.llm.invoke(messages)
        try:
            return json.loads(response.content.strip())
        except json.JSONDecodeError:
            return {
                "summary": "Analysis complete. Review the agent outputs above for details.",
                "health_score": 50,
                "health_label": "Fair",
                "top_actions": []
            }
