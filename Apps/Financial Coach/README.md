# 💰 AI Financial Coach — Hackathon Project

A multi-agent financial advisor built with LangChain + Streamlit.

## 🏗️ Project Structure

```
financial_coach/
├── app.py                          ← Streamlit UI (run this)
├── requirements.txt
├── sample_data/
│   └── sample_bank_statement.csv  ← Use this for demo
└── agents/
    ├── orchestrator.py             ← Coordinates all agents
    ├── document_reader.py          ← Parses PDF/CSV/TXT
    ├── expense_classifier.py       ← Categorizes spending
    ├── debt_analyzer.py            ← Debt-to-income analysis
    ├── savings_strategist.py       ← Personalized savings plan
    └── report_builder.py           ← Final executive summary
```

## ⚡ Quick Setup (10 minutes)

### 1. Install Python 3.10+
```bash
python --version   # should be 3.10 or higher
```

### 2. Create virtual environment
```bash
cd financial_coach
python -m venv venv

# Mac/Linux:
source venv/bin/activate

# Windows:
venv\Scripts\activate
```

### 3. Install dependencies
```bash
pip install -r requirements.txt
```

### 4. Run the app
```bash
streamlit run app.py
```

The app opens at **http://localhost:8501**

## 🎯 Demo Instructions

1. Open the app in browser
2. Enter your **OpenAI API key** in the sidebar (get one at platform.openai.com)
3. Upload `sample_data/sample_bank_statement.csv`
4. Set monthly income to **₹55,000**
5. Choose a savings goal
6. Click **🚀 Analyze with AI Agents**
7. Watch 5 agents work in sequence!

## 🤖 Agent Flow

```
Document Reader → Expense Classifier → Debt Analyzer → Savings Strategist → Report Builder
```

Each agent has one clear job and passes structured output to the next.

## 🛠️ Tech Stack

- **UI**: Streamlit
- **AI Agents**: LangChain + OpenAI GPT-4o-mini
- **PDF Parsing**: pdfplumber
- **Language**: Python

## 💡 Extend Ideas (if time permits)

- Add a **chart** showing spending by category (use `st.bar_chart`)
- Add **n8n webhook** to email the report
- Add a **chat window** at the bottom for follow-up questions

## ⚠️ Notes

- GPT-4o-mini is used (cheaper, fast — ideal for hackathon)
- API key is entered in UI, not stored anywhere
- Works with PDF bank statements, CSV exports, or plain text files
