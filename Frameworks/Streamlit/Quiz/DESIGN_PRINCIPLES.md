# 🏛️ Design Principles & Best Practices

This document explains the architectural decisions and design patterns used in the AI Quiz Master application, demonstrating SOLID principles and professional software engineering practices.

---

## 📋 Table of Contents

1. [SOLID Principles Implementation](#solid-principles)
2. [Design Patterns Used](#design-patterns)
3. [Code Reusability Strategy](#code-reusability)
4. [Proper Naming Conventions](#naming-conventions)
5. [Architecture Layers](#architecture-layers)
6. [Extension Guidelines](#extension-guidelines)

---

## <a name="solid-principles"></a>🔹 SOLID Principles Implementation

### 1. **S**ingle Responsibility Principle (SRP)

**Definition**: Each class should have only one reason to change.

**Implementation**:

```
┌─────────────────────┐
│ config/constants.py │ → Responsibility: Define types
├─────────────────────┤
│ models/*.py         │ → Responsibility: Vendor-specific API calls
├─────────────────────┤
│ services/*.py       │ → Responsibility: Business logic orchestration
├─────────────────────┤
│ ui/*.py             │ → Responsibility: Rendering & user interaction
├─────────────────────┤
│ state/quiz_state.py │ → Responsibility: Session state management
└─────────────────────┘
```

**Examples**:

- `config/constants.py` - ONLY defines enums and dataclasses
- `models/base.py` - ONLY defines the interface
- `models/huggingface.py` - ONLY HuggingFace API logic
- `services/prompt_builder.py` - ONLY builds prompts (doesn't call APIs)
- `services/quiz_generator.py` - ONLY orchestrates generation
- `ui/sidebar.py` - ONLY renders config UI
- `state/quiz_state.py` - ONLY manages session state

**Changes are isolated**:
- Need to add new field? → Only change `config/constants.py`
- Fix prompt format? → Only change `services/prompt_builder.py`
- New vendor? → Only add `models/new_vendor.py`
- UI redesign? → Only change `ui/*.py`

---

### 2. **O**pen/Closed Principle (OCP)

**Definition**: Classes should be open for extension, closed for modification.

**Implementation**:

**Before OCP Violation**:
```python
# BAD: Adding new vendor requires modifying multiple places
if vendor == "openai":
    from models.openai_vendor import OpenAIVendor
    # ...
elif vendor == "huggingface":
    from models.huggingface import HuggingFaceVendor
    # ...
```

**After OCP Applied**:
```python
# GOOD: Adding new vendor only requires creating new file + 1 factory method
class VendorFactory:
    @staticmethod
    def create_vendor(vendor: ModelVendor, api_key: str, model_id: str):
        if vendor == ModelVendor.HUGGINGFACE:
            return HuggingFaceVendor(api_key, model_id)
        elif vendor == ModelVendor.OPENAI:
            return OpenAIVendor(api_key, model_id)
        # NEW VENDOR: Just add 1 elif block (closed for major changes, open for extension)
```

**Benefits**:
- Adding OpenRouter vendor required 1 new file + 1 factory line
- No changes to quiz_generator.py, main.py, etc.
- Entire UI works without modification

---

### 3. **L**iskov Substitution Principle (LSP)

**Definition**: Objects of subclasses should be substitutable for superclass objects.

**Implementation**:

```python
# Interface: BaseModelVendor defines contract
class BaseModelVendor(ABC):
    def generate_quiz(...) -> List[QuizQuestion]: pass
    def evaluate_answers(...) -> EvaluationResult: pass

# All implementations follow same contract
class OpenAIVendor(BaseModelVendor):
    def generate_quiz(...) -> List[QuizQuestion]: ...
    def evaluate_answers(...) -> EvaluationResult: ...

class HuggingFaceVendor(BaseModelVendor):
    def generate_quiz(...) -> List[QuizQuestion]: ...
    def evaluate_answers(...) -> EvaluationResult: ...

# Usage: Interchangeable!
vendor: BaseModelVendor = OpenAIVendor(key, model)  # Works
vendor: BaseModelVendor = HuggingFaceVendor(key, model)  # Also works
generator = QuizGenerator(vendor)  # No changes needed
```

**Benefits**:
- Services don't care which vendor is used
- Can swap vendors at runtime without breaking code
- Perfect for testing (inject mock vendor)

---

### 4. **I**nterface Segregation Principle (ISP)

**Definition**: Clients shouldn't depend on interfaces they don't use.

**Implementation**:

```python
# ✅ GOOD: Lean, focused interface
class BaseModelVendor(ABC):
    @abstractmethod
    def validate_credentials(self) -> bool: pass
    
    @abstractmethod
    def generate_quiz(...) -> List[QuizQuestion]: pass
    
    @abstractmethod
    def evaluate_answers(...) -> EvaluationResult: pass

# Services only depend on what they need
class QuizGenerator:
    def __init__(self, vendor: BaseModelVendor):
        # Only uses generate_quiz method
        self.vendor = vendor
```

**vs ❌ BAD**:
```python
# BAD: Bloated interface with unused methods
class AIModel(ABC):
    @abstractmethod
    def generate_quiz(...): pass
    
    @abstractmethod
    def evaluate_answers(...): pass
    
    @abstractmethod
    def generate_image(...): pass  # We don't need this!
    
    @abstractmethod  
    def translate_text(...): pass   # We don't need this either!
```

---

### 5. **D**ependency Inversion Principle (DIP)

**Definition**: Depend on abstractions, not concretions.

**Implementation**:

```python
# ✅ GOOD: Service depends on abstract interface
class QuizGenerator:
    def __init__(self, vendor: BaseModelVendor):  # Abstract!
        self.vendor = vendor

# ❌ BAD: Service depends on concrete class
class QuizGenerator:
    def __init__(self, vendor: HuggingFaceVendor):  # Concrete!
        self.vendor = vendor
```

**Benefits**:
- Easy to test with mock vendors
- Easy to swap vendors
- Services don't care about vendor implementation details

**Example: Testing with Dependency Inversion**:
```python
# Test with mock
mock_vendor = Mock(spec=BaseModelVendor)
mock_vendor.generate_quiz.return_value = [test_question]

generator = QuizGenerator(vendor=mock_vendor)
result = generator.generate(criteria)
# Works without real API

# Production with real vendor
real_vendor = OpenAIVendor(api_key, model_id)
generator = QuizGenerator(vendor=real_vendor)
result = generator.generate(criteria)  # Same code!
```

---

## <a name="design-patterns"></a>🎨 Design Patterns Used

### 1. **Abstract Factory Pattern** (Vendor Factory)

**Problem**: Creating different vendor instances based on type requires conditional logic scattered throughout code.

**Solution**: Centralize vendor creation in factory.

```python
# models/vendor_factory.py
class VendorFactory:
    @staticmethod
    def create_vendor(vendor: ModelVendor, api_key, model_id, **kwargs):
        if vendor == ModelVendor.OPENAI:
            return OpenAIVendor(api_key, model_id)
        elif vendor == ModelVendor.AZURE_OPENAI:
            return AzureOpenAIVendor(api_key, model_id, **kwargs)
        # ... etc
```

**Usage**:
```python
# main.py - Just one place to know about all vendors
vendor = VendorFactory.create_vendor(vendor_type, api_key, model_id)
```

**Benefits**:
- Centralized vendor instantiation
- Adding new vendor doesn't scatter changes
- Handles vendor-specific parameters (Azure needs endpoint)

---

### 2. **Strategy Pattern** (Vendor Implementation)

**Problem**: Different vendors have different APIs, but we need them to behave identically.

**Solution**: Define common interface (BaseModelVendor), each vendor implements it with vendor-specific logic.

```python
# All follow same interface
class HuggingFaceVendor(BaseModelVendor):
    def generate_quiz(self, ...):
        # HuggingFace specific implementation
        
class OpenAIVendor(BaseModelVendor):
    def generate_quiz(self, ...):
        # OpenAI specific implementation
```

**Usage**:
```python
# Same code works with any vendor
generator = QuizGenerator(vendor)
generator.generate(criteria)  # Works regardless of vendor
```

---

### 3. **Builder Pattern** (Prompt Construction)

**Problem**: Constructing complex prompts with many variables.

**Solution**: Separate method handles prompt building.

```python
# services/prompt_builder.py
class QuizPromptBuilder:
    @staticmethod
    def build_quiz_prompt(age_group, difficulty, field, num_questions):
        return f"""Generate {num_questions} questions for...
        Age: {age_group.value}
        Difficulty: {difficulty.value}
        Field: {field.value}
        ..."""
```

**Benefits**:
- Prompt logic centralized & testable
- Easy to modify prompt format
- Reused by all vendors

---

### 4. **Facade Pattern** (Session State)

**Problem**: Streamlit's `st.session_state` is a dict, prone to key typos and inconsistency.

**Solution**: Create facade that wraps session_state with typed methods.

```python
# state/quiz_state.py
class QuizSessionState:
    QUIZ_STARTED = "quiz_started"  # Single source of truth
    
    @staticmethod
    def start_quiz(criteria, questions):
        st.session_state[QuizSessionState.QUIZ_STARTED] = True
        # ... other state updates
    
    @staticmethod
    def get_is_quiz_started() -> bool:
        return st.session_state.get(QuizSessionState.QUIZ_STARTED, False)
```

**Benefits**:
- No string key typos
- Type hints (get_is_quiz_started returns bool, not Any)
- Single place to add/remove state fields

---

### 5. **Template Method Pattern** (Vendor Base Class)

**Problem**: All vendors follow same sequence (validate → generate/evaluate) but details differ.

**Solution**: Base class defines template, subclasses implement details.

```python
class BaseModelVendor(ABC):
    def __init__(self, api_key, model_id):
        # Template: All vendors need these
        self.api_key = api_key
        self.model_id = model_id
    
    @abstractmethod
    def validate_credentials(self) -> bool:
        pass  # Subclasses implement details
    
    @abstractmethod
    def generate_quiz(self, ...):
        pass  # Each vendor calls different API
```

---

## <a name="code-reusability"></a>📦 Code Reusability Strategy

### Rule: DRY - Don't Repeat Yourself

**Principle**: Common logic lives in ONE place.

### Example 1: JSON Extraction

**Before (Duplicated in every vendor)**:
```python
# ❌ In huggingface.py
def _extract_json(self, text):
    match = re.search(r"```(?:json)?\s*([\s\S]*?)```", text)
    if match:
        return match.group(1).strip()
    return text.strip()

# ❌ In openai_vendor.py  
def _extract_json(self, text):
    match = re.search(r"```(?:json)?\s*([\s\S]*?)```", text)
    if match:
        return match.group(1).strip()
    return text.strip()

# ❌ In azure_openai.py
def _extract_json(self, text):
    match = re.search(r"```(?:json)?\s*([\s\S]*?)```", text)
    if match:
        return match.group(1).strip()
    return text.strip()
```

**After (Centralized in base or utility)**:
```python
# ✅ In models/base.py (could add utility method) OR
# ✅ Create utils/json_parser.py
# All vendors using same implementation = easier maintenance
```

**Current App**: Each vendor implements `_extract_json()` (small enough) but larger logic is shared:

```python
# ✅ Prompt building shared across all vendors
QuizPromptBuilder.build_quiz_prompt()

# ✅ Quiz generation orchestration shared
class QuizGenerator:
    def generate(self, criteria):
        prompt = QuizPromptBuilder.build_quiz_prompt(...)  # Shared
        questions = self.vendor.generate_quiz(prompt, ...)  # Vendor-specific
        return questions
```

### Example 2: Answer Evaluation

```python
# ✅ Prompt building shared
evaluation_prompt = QuizPromptBuilder.build_evaluation_prompt(questions, answers)

# ✅ Evaluation orchestration shared
class QuizEvaluator:
    def evaluate(self, questions, answers):
        prompt = QuizPromptBuilder.build_evaluation_prompt(...)  # Shared
        result = self.vendor.evaluate_answers(prompt, ...)  # Vendor-specific
        return result
```

**Result**: 
- Prompt generation changes → 1 file (QuizPromptBuilder)
- Vendor API calls → Only that vendor's file
- Business logic → Services, not UI code

---

## <a name="naming-conventions"></a>📝 Proper Naming Conventions

### Naming Rules Applied

#### 1. **Classes**: PascalCase, Clear Purpose

```python
# ✅ GOOD: Clear what it does
class QuizGenerator: pass
class QuizEvaluator: pass
class SidebarConfigurator: pass
class ThemeManager: pass
class VendorFactory: pass

# ❌ BAD: Unclear
class Quiz: pass  # Too generic
class Gen: pass  # Abbreviation
class Manager: pass  # Too vague
```

#### 2. **Methods**: snake_case, Verb-Starting

```python
# ✅ GOOD: Start with verb
def generate_quiz(): pass
def evaluate_answers(): pass
def render_question(): pass
def submit_answer(): pass
def validate_credentials(): pass

# ❌ BAD: No verb or vague
def quiz(): pass
def process(): pass
def do_stuff(): pass
```

#### 3. **Enums**: UPPER_CASE with PascalCase class

```python
# ✅ GOOD
class ModelVendor(str, Enum):
    OPENAI = "OpenAI"
    AZURE_OPENAI = "Azure OpenAI"
    HUGGINGFACE = "Hugging Face"

# ❌ BAD
class model_vendor(str, Enum):
    openai = "openai"

# ✅ Usage is clear
if vendor == ModelVendor.OPENAI:  # Self-documenting
```

#### 4. **Constants**: UPPER_CASE

```python
# ✅ GOOD
OPENROUTER_API_URL = "https://openrouter.io/api/v1/chat/completions"
MAX_QUESTIONS = 10
MIN_QUESTIONS = 1

# ❌ BAD
url = "https://..."
max_q = 10
```

#### 5. **Parameters**: snake_case, Descriptive

```python
# ✅ GOOD
def build_quiz_prompt(age_group, difficulty_level, field, num_questions):
    pass

# ❌ BAD
def build_prompt(ag, d, f, n):
    pass
```

#### 6. **Boolean Methods**: is_ or has_ prefix

```python
# ✅ GOOD
def is_quiz_started() -> bool: pass
def is_quiz_complete() -> bool: pass
def has_answered() -> bool: pass
def is_valid() -> bool: pass

# ❌ BAD  
def quiz_started() -> bool: pass  # Ambiguous
def check() -> bool: pass  # Too vague
```

#### 7. **Private Methods**: _leading_underscore

```python
# ✅ GOOD
def _extract_json(self, text):
    """Private helper"""
    pass

def _apply_light_theme(self):
    """Private helper"""
    pass

# ❌ BAD
def extract_json(self, text):  # Looks public
def apply_light_theme(self):  # Looks public
```

---

## <a name="architecture-layers"></a>🏗️ Architecture Layers

### Clean Layered Architecture

```
┌─────────────────────────────────────────────────┐
│  UI Layer                                       │
│  (Streamlit Components: sidebar, quiz, results) │
└─────────────────────────────────────────────────┘
         │
         └─> state/ (Session State Management)
         
┌─────────────────────────────────────────────────┐
│  Business Logic Layer                           │
│  (Quiz Generation, Evaluation, Prompts)         │
└─────────────────────────────────────────────────┘
         │
         └─> models/ (Vendor Abstraction)
         
┌─────────────────────────────────────────────────┐
│  Configuration Layer                            │
│  (Constants, Enums, Dataclasses, Registries)    │
└─────────────────────────────────────────────────┘
         │
         └─> External APIs (OpenAI, HuggingFace, etc)
```

### Dependency Rules

```
UI Layer          ← Depends on State + Services + Config
Business Logic    ← Depends on Models + Config
Configuration     ← Depends on nothing (all external)
Models/Vendors    ← Depends on Config only
```

**Benefits**:
- **Testable**: Each layer testable independently
- **Maintainable**: Change one layer, others unaffected (if deps followed)
- **Clear Responsibility**: Each layer has specific job
- **Flexible**: Swap vendors without changing UI

---

## <a name="extension-guidelines"></a>🚀 Extension Guidelines

### Adding a New Feature: Example Flow

**Scenario**: Add support for quiz hints

**Step 1: Configuration Layer**
```python
# config/constants.py
@dataclass
class QuizQuestion:
    question_id: int
    question_text: str
    options: List[str]
    correct_answer_index: int
    hint: str  # NEW
```

**Step 2: Business Logic Layer**
```python
# services/prompt_builder.py
def build_quiz_prompt(self, ...):
    # Add instruction: "Include a helpful hint (1-2 words) for each question"
    return f"... each question must include a 'hint' field with JSON response"
```

**Step 3: Vendor Layer** (No changes! They just parse the JSON)

**Step 4: UI Layer**
```python
# ui/quiz_display.py
def render_question(self, question):
    # ... show question and options
    with st.expander("💡 Show Hint"):
        st.write(question.hint)  # NEW
```

**Step 5: State Layer** (No changes needed!)

**Result**: Feature added with 3 file changes, clear separation!

---

### Adding a New Vendor

**Step 1**: Create `models/new_vendor.py` inheriting `BaseModelVendor`
```python
class NewVendorClass(BaseModelVendor):
    def validate_credentials(self): ...
    def generate_quiz(self, ...): ...
    def evaluate_answers(self, ...): ...
```

**Step 2**: Add to enum in `config/constants.py`
```python
class ModelVendor(str, Enum):
    # ... existing
    NEW_VENDOR = "New Vendor"
```

**Step 3**: Add models to `config/models.py`
```python
class ModelRegistry:
    NEW_VENDOR_MODELS = ["model1", "model2"]
    VENDOR_MODELS = {
        # ... existing
        ModelVendor.NEW_VENDOR: NEW_VENDOR_MODELS,
    }
```

**Step 4**: Add factory method in `models/vendor_factory.py`
```python
elif vendor == ModelVendor.NEW_VENDOR:
    return NewVendorClass(api_key, model_id, **kwargs)
```

**Done!** No changes to UI, services, or state. Sidebar automatically shows new vendor.

---

## 📚 Summary Table

| Principle | Implementation | Benefit |
|-----------|----------------|---------|
| **SRP** | Each class has one job | Easy to understand, test, modify |
| **OCP** | Vendors follow interface | Add vendors without changing code |
| **LSP** | All vendors interchangeable | Services work with any vendor |
| **ISP** | Lean, focused interfaces | Services don't depend on unused methods |
| **DIP** | Depend on abstractions | Easy to test, swap vendors |
| **DRY** | Shared logic in services | One place to fix, maintain |
| **Patterns** | Factory, Strategy, Builder, etc. | Proven, familiar structures |
| **Naming** | Clear, descriptive names | Code is self-documenting |
| **Layering** | Config → Services → UI | Changes isolated, testable |

---

## 🎓 Conclusion

This application demonstrates **professional software engineering practices**:

✅ **SOLID Principles** - Every one applied thoughtfully
✅ **Design Patterns** - Factory, Strategy, Builder, Facade, Template Method
✅ **Code Reusability** - DRY principle: no duplication
✅ **Proper Naming** - Self-documenting code
✅ **Clean Architecture** - Clear layer separation
✅ **Type Safety** - Full type hints throughout
✅ **Easy Extension** - Adding features/vendors is straightforward
✅ **Testability** - Each layer independently testable

Perfect example to learn from and build upon! 🚀
