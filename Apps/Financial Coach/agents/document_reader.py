"""
Document Reader Agent
- Extracts raw text from PDF / CSV / TXT
- Sends to LLM to parse into structured transaction list
"""

import io
import csv
import json
from langchain_openai import ChatOpenAI
from langchain.schema import HumanMessage, SystemMessage


SYSTEM_PROMPT = """You are a financial document parser.
Given raw text from a bank statement or expense file, extract ALL transactions.
Return ONLY valid JSON in this exact format (no markdown, no explanation):
{
  "transactions": [
    {"date": "YYYY-MM-DD", "description": "string", "amount": 1234.56, "type": "debit|credit"}
  ],
  "date_range": "Jan 2024 - Mar 2024",
  "transaction_count": 42
}
If amounts are ambiguous, treat positive as credit (income) and negative as debit (expense).
"""


class DocumentReaderAgent:
    def __init__(self, api_key: str):
        self.llm = ChatOpenAI(
            model="gpt-4o-mini",
            api_key=api_key,
            temperature=0
        )

    def extract_text(self, file_content: bytes, file_type: str) -> str:
        """Extract raw text from the uploaded file."""
        if file_type == "pdf":
            return self._extract_pdf(file_content)
        elif file_type == "csv":
            return self._extract_csv(file_content)
        else:  # txt
            return file_content.decode("utf-8", errors="ignore")

    def _extract_pdf(self, content: bytes) -> str:
        try:
            import pdfplumber
            with pdfplumber.open(io.BytesIO(content)) as pdf:
                return "\n".join(
                    page.extract_text() or "" for page in pdf.pages
                )
        except ImportError:
            # Fallback: try pypdf
            try:
                import pypdf
                reader = pypdf.PdfReader(io.BytesIO(content))
                return "\n".join(
                    page.extract_text() or "" for page in reader.pages
                )
            except Exception as e:
                return f"Could not extract PDF: {e}"

    def _extract_csv(self, content: bytes) -> str:
        text = content.decode("utf-8", errors="ignore")
        reader = csv.reader(io.StringIO(text))
        rows = [", ".join(row) for row in reader]
        return "\n".join(rows)

    def parse_transactions(self, raw_text: str) -> dict:
        """Use LLM to parse raw text into structured transactions."""
        # Truncate to avoid token limits (keep first ~3000 chars)
        truncated = raw_text[:3000]

        messages = [
            SystemMessage(content=SYSTEM_PROMPT),
            HumanMessage(content=f"Parse this financial document:\n\n{truncated}")
        ]

        response = self.llm.invoke(messages)
        raw = response.content.strip()

        try:
            data = json.loads(raw)
        except json.JSONDecodeError:
            # Fallback: return empty structure
            data = {
                "transactions": [],
                "date_range": "Unknown",
                "transaction_count": 0
            }

        return data
