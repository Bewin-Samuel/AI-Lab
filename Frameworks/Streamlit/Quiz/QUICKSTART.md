# 🚀 Quick Start Guide

## Step 1: Navigate to Project Directory
```bash
cd "c:\Win\Learn\Artificial Intelligence\Code\Repos\AI-Lab\Frameworks\Streamlit\Quiz"
```

## Step 2: Create Virtual Environment
```bash
python -m venv venv
```

## Step 3: Activate Virtual Environment
```bash
venv\Scripts\activate
```

## Step 4: Install Dependencies
```bash
pip install -r requirements.txt
```

## Step 5: Run the App
```bash
streamlit run main.py
```

The app will open in your browser at `http://localhost:8501`

---

## 🔑 Get Your API Key

Choose one vendor and get the API key:

### Option 1: OpenAI (Recommended for Quality)
1. Go to https://platform.openai.com/api-keys
2. Create a new API key
3. Copy and paste it in the sidebar

### Option 2: HuggingFace (Free, No Card Required)
1. Go to https://huggingface.co/settings/tokens
2. Create a new token (read permission)
3. Copy and paste it in the sidebar

### Option 3: Azure OpenAI
1. Create Azure account
2. Create OpenAI resource in Azure Portal
3. Get API key and endpoint from Keys & Endpoint section
4. Format: `https://resource.openai.azure.com|deployment-name`

### Option 4: OpenRouter
1. Go to https://openrouter.ai/keys
2. Create API key
3. Copy and paste it in the sidebar

---

## 📋 Test the App

1. **In the sidebar (Section 1):**
   - Vendor: Choose one (e.g., OpenAI)
   - Model: Select a model
   - API Key: Paste your API key

2. **In the sidebar (Section 2):**
   - Age Group: Select your age group
   - Difficulty: Select "Beginner" (easier to test)
   - Field: Select "General Knowledge"
   - Questions: Set to 3 (quick test)
   - Theme: Choose Light/Dark/High Contrast

3. **Click "🚀 START QUIZ"**

4. **Answer the 3 questions** and submit

5. **View your results** with score, feedback, and time taken

---

## ✅ Verification Checklist

After installation:
- [ ] Virtual environment activated
- [ ] All packages installed: `pip list | grep -E "streamlit|openai|huggingface"`
- [ ] API key obtained and ready
- [ ] App starts without errors: `streamlit run main.py`
- [ ] Welcome screen displays
- [ ] Can populate all sidebar fields
- [ ] Can click "START QUIZ"

---

## 🆘 Troubleshooting

### "ModuleNotFoundError: No module named 'streamlit'"
→ Run: `pip install -r requirements.txt`

### "Invalid API Key"
→ Check your API key is correct in the vendor's dashboard

### "Model not found"
→ Verify the model ID is available for your vendor in the dropdown

### App runs slow
→ Use a faster model (gpt-3.5-turbo, mistral-7b) or reduce number of questions

---

## 📚 Project Structure

```
quiz_app/
├── main.py              ← Start here
├── config/              ← Enums & Constants
├── models/              ← AI Vendor Implementations
├── services/            ← Business Logic
├── ui/                  ← UI Components
├── state/               ← Session State
├── requirements.txt     ← Dependencies
└── README.md            ← Full Documentation
```

---

## 💡 Pro Tips

1. **Start with HuggingFace** - No cost, no credit card needed
2. **Use OpenAI gpt-3.5-turbo** - Best balance of quality and cost
3. **Test with 1-3 questions first** - Faster to develop/test
4. **Read the README.md** - Full documentation and extending guide
5. **Keep your API key safe** - Never commit it to git

---

## 🎯 Next Steps

Once the basic app works:
1. Try different vendors to see quality differences
2. Test different difficulty levels and age groups
3. Experiment with higher number of questions (8-10)
4. Try different themes to find your preference
5. Check out `README.md` for extending with new vendors/fields

**Happy Quizzing!** 🎓
