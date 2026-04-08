# 🎓 AI Quiz Master

A sophisticated, multi-vendor AI-powered quiz application built with Streamlit. Test your knowledge across multiple domains with adaptive questions generated and evaluated by leading AI models.

## ✨ Features

- **Multi-Vendor AI Support**: OpenAI, Azure OpenAI, OpenRouter, Hugging Face
- **Multiple Knowledge Fields**: General Knowledge, C# Programming, Science
- **Adaptive Difficulty**: Beginner, Practitioner, Expert levels
- **Age-Aware Generation**: Customized for age groups 3-5, 6-12, 13-19, Adults
- **AI Evaluation**: Instant feedback with performance analysis
- **Time Tracking**: Monitor how long you take to complete the quiz
- **Flexible Themes**: Light, Dark, High Contrast for accessibility
- **Modular Architecture**: SOLID principles with clean separation of concerns
- **Dependency Injection**: Easy to test and extend with new vendors

## 🏗️ Architecture

The application follows **SOLID principles** with distinct layers:

### Layer Structure

```
quiz_app/
├── config/              # Configuration & Constants
│   ├── constants.py     # Enums & Dataclasses
│   └── models.py        # Model registry per vendor
├── models/              # AI Model Vendor Implementations
│   ├── base.py          # Abstract BaseModelVendor
│   ├── huggingface.py   # HuggingFace Inference API
│   ├── openai_vendor.py # OpenAI API
│   ├── azure_openai.py  # Azure OpenAI API
│   ├── openrouter.py    # OpenRouter API
│   └── vendor_factory.py# Vendor creation & DI
├── services/            # Business Logic
│   ├── prompt_builder.py # Quiz & Evaluation prompt generation
│   ├── quiz_generator.py # Quiz question generation
│   └── quiz_evaluator.py # Answer evaluation & scoring
├── ui/                  # UI Components (Streamlit)
│   ├── theme_manager.py # Theme application
│   ├── sidebar.py       # Configuration sidebar
│   ├── quiz_display.py  # Question rendering
│   └── results.py       # Results page
├── state/               # Session State Management
│   └── quiz_state.py    # QuizSessionState wrapper
└── main.py              # Streamlit entry point
```

### Design Principles

1. **Single Responsibility**: Each class has one clear job
2. **Abstraction**: BaseModelVendor interface for vendor independence
3. **Dependency Injection**: Services receive vendors, not import them
4. **No Code Duplication**: Shared logic in services & builders
5. **Proper Naming**: Clear, self-documenting names at all levels

## 🚀 Installation

### Prerequisites
- Python 3.9+
- pip or pip3

### Setup Steps

1. **Clone the repository**
   ```bash
   cd "c:\Win\Learn\Artificial Intelligence\Code\Repos\AI-Lab\Frameworks\Streamlit\Quiz"
   ```

2. **Create a Python virtual environment**
   ```bash
   python -m venv venv
   ```

3. **Activate the virtual environment**
   - **Windows:**
     ```bash
     venv\Scripts\activate
     ```
   - **macOS/Linux:**
     ```bash
     source venv/bin/activate
     ```

4. **Install dependencies**
   ```bash
   pip install -r requirements.txt
   ```

## 🔑 API Credentials

### OpenAI
- Get API key from: https://platform.openai.com/api-keys
- Models: `gpt-4o`, `gpt-4-turbo`, `gpt-3.5-turbo`

### Azure OpenAI
- Get credentials from Azure Portal
- Format Model ID as: `https://your-resource.openai.azure.com|your-deployment-name`
- Models: `gpt-4o`, `gpt-4-turbo`, `gpt-35-turbo`

### OpenRouter
- Get API key from: https://openrouter.ai/keys
- Models include various open-source (Llama, Mistral, etc.) and commercial models

### Hugging Face
- Get API key from: https://huggingface.co/settings/tokens
- Models: `mistralai/Mistral-7B-Instruct-v0.1`, `meta-llama/Llama-2-70b-chat-hf`, etc.

## 📝 Usage

1. **Start the Streamlit app**
   ```bash
   streamlit run main.py
   ```

2. **Configure in the sidebar:**
   - **Section 1 - AI Model Setup:**
     - Select Model Vendor (OpenAI, Azure, OpenRouter, Hugging Face)
     - Choose Model ID from dropdown
     - Enter API Key
   
   - **Section 2 - Quiz Criteria:**
     - Select Age Group
     - Select Difficulty Level
     - Select Knowledge Field
     - Set Number of Questions (1-10)
     - Choose Theme (Light, Dark, High Contrast)

3. **Click "START QUIZ"**
   - App validates credentials
   - AI generates questions based on your criteria
   - Quiz begins

4. **Answer Questions**
   - Read question carefully
   - Select one of 4 options
   - Click "NEXT" to proceed
   - Navigate with Previous/Next buttons

5. **Submit Quiz**
   - On the last question, click "SUBMIT QUIZ"
   - AI evaluates your answers
   - Results display with score %, feedback, and time taken

6. **Review Results**
   - Score percentage with performance level
   - Knowledge summary
   - Detailed feedback on strengths/weaknesses
   - Category-specific scores
   - Time taken to complete

## 🔧 Configuration

### Supported Models

#### OpenAI
- **gpt-4o**: Most capable, recommended for quality
- **gpt-4-turbo**: Faster than GPT-4, good balance
- **gpt-3.5-turbo**: Most economical, still good quality

#### Azure OpenAI
- Same models as OpenAI but deployed in Azure
- Requires both API key and endpoint URL

#### OpenRouter
- Access to 100+ models (open-source & commercial)
- Single API key for multiple model families
- Good for cost optimization

#### Hugging Face
- Access to thousands of open-source models
- No costs (with rate limits)
- Great for local/self-hosted scenarios

### Quiz Parameters

| Parameter | Min | Max | Default |
|-----------|-----|-----|---------|
| Number of Questions | 1 | 10 | 5 |

## 📊 Quiz Generation

The app generates multiple-choice questions based on:
- **Age Group**: Content complexity calibrated to age
- **Difficulty**: Beginner, Practitioner, Expert
- **Subject**: Domain-specific questions
- **Format**: 4 options per question, JSON-structured

## 🎯 Evaluation

After submission, the AI evaluator provides:
- **Score Percentage**: Percentage of correct answers
- **Knowledge Summary**: 1-2 sentence performance overview
- **Category Breakdown**: Performance by topic (if applicable)
- **Detailed Feedback**: Specific strengths and areas for improvement
- **Time Taken**: Total minutes:seconds for the quiz

## 🧪 Testing the App

### Test Flow
1. Use a test API key for your vendor
2. Select Beginner difficulty with a simple field (General Knowledge)
3. Start with 3 questions to verify quick end-to-end flow
4. Check results render properly

### Common Issues

| Issue | Solution |
|-------|----------|
| **Invalid API Key** | Verify key in vendor dashboard, check permissions |
| **Model not found** | Confirm model ID is available in selected vendor |
| **JSON parse error** | Some models may return malformed JSON; try again or switch vendor |
| **Slow generation** | Larger models take longer; try gpt-3.5-turbo or mistral-7b |
| **Theme not applied** | Theme applies on next question/navigation |

## 📚 Extending the App

### Adding a New Vendor

1. Create `models/new_vendor.py`:
   ```python
   from models.base import BaseModelVendor
   
   class NewVendorVendor(BaseModelVendor):
       def validate_credentials(self) -> bool: ...
       def generate_quiz(self, ...): ...
       def evaluate_answers(self, ...): ...
   ```

2. Add to `ModelVendor` enum in `config/constants.py`

3. Update `VendorFactory.create_vendor()` in `models/vendor_factory.py`

4. Add model list to `ModelRegistry` in `config/models.py`

### Customizing Prompts

Edit `services/prompt_builder.py`:
- `QuizPromptBuilder.build_quiz_prompt()` - Customize quiz generation
- `QuizPromptBuilder.build_evaluation_prompt()` - Customize evaluation

### Adding New Fields

1. Add to `Field` enum in `config/constants.py`
2. Update `ModelRegistry` with example fields
3. Sidebar automatically updates

## 📝 Project Philosophy

This application demonstrates:
- **No Code Duplication**: Shared logic centralized in services
- **SOLID Principles**: Especially Single Responsibility
- **Clean Architecture**: Clear separation between UI, business logic, and vendors
- **Dependency Injection**: Loose coupling between components
- **Proper Naming**: Self-documenting code at all levels
- **Type Safety**: Full type hints throughout
- **Scalability**: Easy to add new vendors, fields, and features

## 🤝 Contributing

To contribute improvements:
1. Follow the existing architecture patterns
2. Maintain SOLID principles
3. Add type hints to all functions
4. Use descriptive names
5. Add docstrings to classes and methods

## 📄 License

MIT License - Feel free to use and modify

## 🆘 Troubleshooting

### Installation Issues
- Ensure Python 3.9+ is installed: `python --version`
- Create fresh venv: `python -m venv venv_new && venv_new\Scripts\activate`
- Reinstall packages: `pip install --refresh-deps -r requirements.txt`

### Runtime Issues
- Check API credentials in sidebar
- Verify internet connection
- Check Streamlit logs: `streamlit run main.py --logger.level=debug`
- Try a different model vendor

### Performance Issues
- Use faster models (gpt-3.5-turbo, mistral-7b)
- Reduce number of questions
- Check network latency to vendor API

## 📞 Support

For issues or questions:
1. Check the troubleshooting section above
2. Review the code comments and docstrings
3. Test with a simpler configuration (fewer questions, simpler field)

---

**Happy Learning!** 🚀📚
