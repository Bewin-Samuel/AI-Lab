# 📁 Project Structure Overview

## Directory Layout

```
quiz_app/
│
├── main.py                          # 🎯 Streamlit entry point - orchestrates everything
│
├── config/                          # ⚙️ Configuration & Constants Layer
│   ├── __init__.py
│   ├── constants.py                 # Enums: ModelVendor, AgeGroup, DifficultyLevel, Field, Theme
│   │                                 # Dataclasses: QuizQuestion, QuizCriteria, EvaluationResult
│   └── models.py                    # ModelRegistry - maps vendors to their available models
│
├── models/                          # 🤖 Model Vendor Implementation Layer
│   ├── __init__.py
│   ├── base.py                      # BaseModelVendor - abstract interface
│   ├── huggingface.py               # HuggingFace Inference API implementation
│   ├── openai_vendor.py             # OpenAI API implementation
│   ├── azure_openai.py              # Azure OpenAI API implementation
│   ├── openrouter.py                # OpenRouter API implementation
│   └── vendor_factory.py            # Factory pattern - creates vendor instances
│
├── services/                        # 🧠 Business Logic & Service Layer
│   ├── __init__.py
│   ├── prompt_builder.py            # QuizPromptBuilder & EvaluationPromptBuilder
│   │                                 # Constructs prompts for quiz generation & evaluation
│   ├── quiz_generator.py            # QuizGenerator - orchestrates quiz question generation
│   └── quiz_evaluator.py            # QuizEvaluator - evaluates answers & provides feedback
│
├── ui/                              # 🎨 User Interface Components
│   ├── __init__.py
│   ├── theme_manager.py             # ThemeManager - applies Light/Dark/High Contrast themes
│   ├── sidebar.py                   # SidebarConfigurator - renders config sidebar (2 sections)
│   │                                 # Section 1: Model vendor setup
│   │                                 # Section 2: Quiz criteria
│   ├── quiz_display.py              # QuizDisplay - renders questions & captures answers
│   └── results.py                   # ResultsDisplay - shows evaluation results & feedback
│
├── state/                           # 💾 Session State Management Layer
│   ├── __init__.py
│   └── quiz_state.py                # QuizSessionState - wrapper around st.session_state
│                                     # Manages quiz progress, answers, results
│
├── prompts/                         # 📝 Prompt Templates (future expansion)
│
├── .streamlit/                      # ⚙️ Streamlit Configuration
│   └── config.toml                  # Theme & server settings for Streamlit
│
├── requirements.txt                 # 📦 Python dependencies
├── .gitignore                       # 🚫 Git ignore rules
├── .env.example                     # 🔑 Example environment variables
├── README.md                        # 📖 Full documentation
├── QUICKSTART.md                    # 🚀 Quick start guide
└── PROJECT_STRUCTURE.md             # 📁 This file
```

---

## 🗂️ File Descriptions

### `main.py` (85 lines)
**Purpose**: Streamlit entry point that orchestrates the entire application

**Key Functions**:
- `setup_page()` - Configures page layout and settings
- `render_welcome_screen()` - Shows welcome/instructions when quiz not started
- `render_quiz_screen()` - Displays quiz questions
- `render_evaluation_screen()` - Shows results
- `main()` - Main orchestration loop

**Flow**:
1. Initialize theme & session state
2. Render sidebar for configuration
3. If quiz not started → show welcome or generate quiz
4. If quiz started → show questions
5. If quiz complete → evaluate answers
6. If evaluated → show results

---

### `config/constants.py` (60 lines)
**Purpose**: Centralizes all enums and dataclasses for type safety

**Key Items**:
- `ModelVendor` enum - OPENAI, AZURE_OPENAI, OPENROUTER, HUGGINGFACE
- `AgeGroup` enum - 3-5, 6-12, 13-19, Adult
- `DifficultyLevel` enum - Beginner, Practitioner, Expert
- `Field` enum - General Knowledge, C# Programming, Science
- `Theme` enum - Light, Dark, High Contrast
- `QuizQuestion` dataclass - Question with 4 options & correct answer
- `QuizCriteria` dataclass - User's quiz configuration
- `EvaluationResult` dataclass - Score, summary, feedback

**Benefits**:
- Type hints throughout app
- No magic strings
- Single source of truth for options

---

### `config/models.py` (45 lines)
**Purpose**: Model availability mapping per vendor

**Key Class**: `ModelRegistry`
- `OPENAI_MODELS` - gpt-4o, gpt-4-turbo, gpt-3.5-turbo
- `AZURE_OPENAI_MODELS` - Same but Azure deployments
- `OPENROUTER_MODELS` - 100+ models including open-source
- `HUGGINGFACE_MODELS` - Popular HF models
- `get_models_for_vendor()` - Returns models for selected vendor

**Usage**: Sidebar dropdowns pull from this registry

---

### `models/base.py` (50 lines)
**Purpose**: Abstract base class defining vendor interface

**Key Methods** (all abstract):
- `validate_credentials()` - Test API key validity
- `generate_quiz()` - Create quiz questions
- `evaluate_answers()` - Score and provide feedback

**Why Abstract**:
- Ensures all vendors implement same contract
- Enables dependency injection
- Makes swapping vendors trivial in tests

---

### `models/huggingface.py` (120 lines)
**Purpose**: HuggingFace Inference API implementation

**Key Features**:
- Sends requests to HuggingFace Inference API
- Parses JSON responses from model
- Handles markdown code block extraction
- Validates credentials before quiz starts

**Used For**: Open-source models, cost-free inference

---

### `models/openai_vendor.py` (120 lines)
**Purpose**: OpenAI API implementation

**Key Features**:
- Uses official OpenAI Python SDK
- Supports GPT-4o, GPT-4-Turbo, GPT-3.5-Turbo
- Chat completions endpoint
- Credential validation with test request

**Used For**: High-quality, production-grade responses

---

### `models/azure_openai.py` (130 lines)
**Purpose**: Azure OpenAI API implementation

**Key Features**:
- AzureOpenAI SDK (different from OpenAI SDK)
- Requires endpoint URL + API key + deployment name
- Uses specific API version for compatibility
- Identical response format to OpenAI

**Used For**: Enterprise Azure deployments

---

### `models/openrouter.py` (130 lines)
**Purpose**: OpenRouter API implementation

**Key Features**:
- HTTP-based API (not SDK)
- Single API key for 100+ models
- Good for cost optimization (cheapest model per usage)
- Supports many open-source models

**Used For**: Cost-effective, model variety

---

### `models/vendor_factory.py` (35 lines)
**Purpose**: Factory pattern for vendor instantiation

**Key Method**: `create_vendor()`
- Takes ModelVendor enum value
- Returns appropriate vendor instance
- Handles initialization details (Azure needs endpoint, etc.)

**Benefits**:
- Main.py doesn't depend on vendor implementations
- Easy to test with mock vendors
- Adding new vendors requires minimal changes

---

### `services/prompt_builder.py` (85 lines)
**Purpose**: Generates prompts for quiz generation and evaluation

**Key Classes**:
- `QuizPromptBuilder` - Generates prompt for question creation
- `EvaluationPromptBuilder` - Generates prompt for answer evaluation

**Why Separate**:
- Prompt quality is critical - centralize for easy tweaking
- Reusable across all vendors
- Single Responsibility Principle

---

### `services/quiz_generator.py` (40 lines)
**Purpose**: Orchestrates quiz generation using a vendor and criteria

**Key Method**: `generate(criteria: QuizCriteria)`
1. Builds prompt using QuizPromptBuilder
2. Calls vendor's generate_quiz()
3. Returns list of QuizQuestion objects

**Dependency Injection**: Vendor passed in constructor, not imported

---

### `services/quiz_evaluator.py` (55 lines)
**Purpose**: Orchestrates quiz evaluation using a vendor

**Key Method**: `evaluate(questions, user_answers)`
1. Validates answer count and indices
2. Builds evaluation prompt
3. Calls vendor's evaluate_answers()
4. Returns EvaluationResult

**Error Handling**: Validates input before calling vendor

---

### `ui/theme_manager.py` (95 lines)
**Purpose**: Applies visual themes using Streamlit markdown/CSS

**Themes**:
- **Light**: Clean blue and white
- **Dark**: Purple/teal on dark background
- **High Contrast**: Black/yellow for accessibility

**Key Method**: `apply_theme(theme: Theme)`
- Injects CSS into Streamlit app
- Colors UI elements (buttons, text, background)

---

### `ui/sidebar.py` (110 lines)
**Purpose**: Renders entire sidebar configuration

**Structure**:
- **Section 1: Model Configuration**
  - Model Vendor (dropdown)
  - Model ID (dynamic dropdown based on vendor)
  - API Key (password field)

- **Section 2: Quiz Criteria**
  - Age Group, Difficulty, Field (dropdowns)
  - Number of Questions (slider, 1-10)
  - Theme selector (radio buttons)

**Returns**: QuizCriteria object only if:
- All mandatory fields filled
- API credentials validated
- "START QUIZ" button clicked

---

### `ui/quiz_display.py` (95 lines)
**Purpose**: Renders quiz questions and navigation

**Key Methods**:
- `render_question()` - Shows single question with 4 options (radio buttons)
  - Progress bar
  - Question text
  - 4 options
  - Saves answer to session state

- `render_question_navigation()` - Prev/Next buttons
  - Disabled if answer not selected
  - Disabled at start/end
  - Shows question progress

- `render_final_submission()` - Final question submit button
  - Warning message
  - Returns True if submit clicked

---

### `ui/results.py` (105 lines)
**Purpose**: Displays quiz results and detailed feedback

**Sections**:
1. **Score Card** (3 columns)
   - Large score % with color coding (red/orange/green)
   - Time taken (minutes:seconds)
   - Performance level (Excellent/Good/Needs Work)

2. **Knowledge Summary**
   - 1-2 sentence summary from model

3. **Detailed Feedback**
   - 3-5 sentence specific feedback

4. **Category Breakdown** (if available)
   - Score per topic/concept

5. **Action Buttons**
   - Retake Quiz (clears session)
   - Home (future feature)

---

### `state/quiz_state.py` (210 lines)
**Purpose**: Manages Streamlit session state for the quiz

**Key Features**:
- Wraps st.session_state with typed methods
- No direct session_state access in UI code
- Clear initialization and reset patterns

**Key Methods**:
- `initialize()` - Set default values
- `start_quiz()` - Initialize quiz data
- `submit_answer()` - Record user answer
- `complete_quiz()` - Mark as done
- `get_*()` - Typed getters
- `reset_quiz()` - Clear all state

**Benefits**:
- Type safety - no `.get()` strings
- Single source of truth for state keys
- Reusable across components

---

## 🏗️ Architecture Patterns Used

### 1. **Dependency Injection**
```python
# Services receive vendors, don't import them
def __init__(self, vendor: BaseModelVendor):
    self.vendor = vendor
```

### 2. **Abstract Base Classes**
```python
# All vendors must implement same interface
class BaseModelVendor(ABC):
    @abstractmethod
    def validate_credentials(self) -> bool: pass
```

### 3. **Factory Pattern**
```python
# Easy vendor creation without conditional logic
vendor = VendorFactory.create_vendor(vendor_type, api_key, model_id)
```

### 4. **Dataclasses for Contracts**
```python
# Clear data structures passed between layers
@dataclass
class QuizCriteria:
    model_vendor: ModelVendor
    api_key: str
    # ... other fields
```

### 5. **Separation of Concerns**
```
Config   → Never changes (enums, constants)
Models   → Vendor-specific logic only
Services → Business logic (generation, evaluation)
UI       → Rendering only
State    → Session management only
```

---

## 🔄 Data Flow

```
1. Sidebar Configuration
   └─> QuizCriteria object

2. Quiz Generation
   └─> QuizPromptBuilder.build_quiz_prompt()
   └─> BaseModelVendor.generate_quiz()
   └─> QuizQuestion list
   └─> QuizSessionState.start_quiz()

3. Quiz Display
   └─> QuizDisplay.render_question()
   └─> User selects answer
   └─> QuizSessionState.submit_answer()

4. Quiz Evaluation
   └─> QuizEvaluator.evaluate()
   └─> QuizPromptBuilder.build_evaluation_prompt()
   └─> BaseModelVendor.evaluate_answers()
   └─> EvaluationResult object
   └─> QuizSessionState.set_evaluation_result()

5. Results Display
   └─> ResultsDisplay.render()
   └─> User clicks Retake
   └─> QuizSessionState.reset_quiz()
```

---

## 📊 Key Design Principles

| Principle | Implementation |
|-----------|-----------------|
| **Single Responsibility** | Each class has one job (config, model, service, UI, state) |
| **Open/Closed** | Open for extension (add vendors), closed for modification |
| **Liskov Substitution** | All vendors interchangeable via BaseModelVendor |
| **Interface Segregation** | Services only depend on BaseModelVendor, not concrete vendors |
| **Dependency Inversion** | Services depend on abstractions, not concrete classes |
| **DRY** | Shared logic in services & builders, not duplicated in UI |
| **Type Safety** | Full type hints, dataclasses, enums (no magic strings) |

---

## 🧪 Testing Approach

Each layer can be tested independently:

```python
# Test services with mock vendor
mock_vendor = Mock(spec=BaseModelVendor)
generator = QuizGenerator(vendor=mock_vendor)

# Test UI with mock services
mock_generator = Mock()
# ... render sidebar without API calls

# Test state independently
QuizSessionState.initialize()
QuizSessionState.start_quiz(criteria, questions)
# ... verify state changes
```

---

## 🚀 Extending the App

### Add a New Vendor

1. Create `models/new_vendor.py`
2. Inherit from `BaseModelVendor`
3. Implement 3 methods
4. Add to `ModelVendor` enum
5. Update `VendorFactory.create_vendor()`
6. Add models to `ModelRegistry`

### Add a New Field/Subject

1. Add to `Field` enum in `config/constants.py`
2. Add example models to `ModelRegistry` if needed
3. Sidebar automatically updates

### Customize Prompts

1. Edit `services/prompt_builder.py`
2. Modify `build_quiz_prompt()` or `build_evaluation_prompt()`
3. Change prompt format/instructions
4. All vendors use updated prompts

---

This modular, SOLID-principle-based architecture makes the codebase maintainable, testable, and easy to extend! 🎯
