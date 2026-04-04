"""
Orchestrator — coordinates all agents in sequence and returns a unified result.
Flow: DocumentReader → ExpenseClassifier → DebtAnalyzer → SavingsStrategist → ReportBuilder
"""

from agents.document_reader import DocumentReaderAgent
from agents.expense_classifier import ExpenseClassifierAgent
from agents.debt_analyzer import DebtAnalyzerAgent
from agents.savings_strategist import SavingsStrategistAgent
from agents.report_builder import ReportBuilderAgent


def run_financial_analysis(
    file_content: bytes,
    file_type: str,
    monthly_income: int,
    savings_goal: str,
    api_key: str,
    vendor: str,
    model_name: str,
) -> dict:
    """
    Main entry point. Runs all agents in sequence and collects their outputs.
    vendor and model_name come from the Streamlit UI — no config.json needed.
    """

    # Shared kwargs passed to every agent
    llm_kwargs = dict(api_key=api_key, vendor=vendor, model_name=model_name)

    agent_outputs = []

    # ── Step 1: Document Reader ───────────────────────────────────────────────
    reader = DocumentReaderAgent(**llm_kwargs)
    raw_text = reader.extract_text(file_content, file_type)

    # print("Raw extracted text (truncated):")
    # print(raw_text[:500])  # Print first 500 chars for debugging
    
    parsed = reader.parse_transactions(raw_text)
    print("Orchestrator: After reader.parse_transactions")

    agent_outputs.append({
        "agent": "Document Reader Agent",
        "output": f"Extracted {parsed['transaction_count']} transactions. "
                  f"Date range: {parsed.get('date_range', 'N/A')}."
    })

    # ── Step 2: Expense Classifier ────────────────────────────────────────────
    classifier = ExpenseClassifierAgent(**llm_kwargs)
    categorized = classifier.categorize(parsed["transactions"])
    agent_outputs.append({
        "agent": "Expense Classifier Agent",
        "output": classifier.summary_text(categorized)
    })

    # ── Step 3: Debt Analyzer ─────────────────────────────────────────────────
    debt_agent = DebtAnalyzerAgent(**llm_kwargs)
    debt_analysis = debt_agent.analyze(parsed["transactions"], monthly_income)
    agent_outputs.append({
        "agent": "Debt Analyzer Agent",
        "output": debt_analysis["summary"]
    })

    # ── Step 4: Savings Strategist ────────────────────────────────────────────
    strategist = SavingsStrategistAgent(**llm_kwargs)
    strategy = strategist.build_strategy(
        categorized=categorized,
        monthly_income=monthly_income,
        goal=savings_goal,
        debt_info=debt_analysis
    )
    agent_outputs.append({
        "agent": "Savings Strategist Agent",
        "output": strategy["advice"]
    })

    # ── Step 5: Report Builder ────────────────────────────────────────────────
    reporter = ReportBuilderAgent(**llm_kwargs)
    report = reporter.build(
        categorized=categorized,
        strategy=strategy,
        debt_info=debt_analysis,
        monthly_income=monthly_income
    )
    agent_outputs.append({
        "agent": "Report Builder Agent",
        "output": report["summary"]
    })

    # ── Compile final result ──────────────────────────────────────────────────
    total_expenses = sum(
        sum(v) for v in categorized.get("categories", {}).values()
        if isinstance(v, list)
    )

    return {
        "total_expenses": int(total_expenses),
        "monthly_income": monthly_income,
        "categorized": categorized,
        "debt_analysis": debt_analysis,
        "strategy": strategy,
        "report": report,
        "agent_outputs": agent_outputs,
    }