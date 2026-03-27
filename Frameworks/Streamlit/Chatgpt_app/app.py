import streamlit as st
from openai import OpenAI
import os
from dotenv import load_dotenv

# Load environment variables
load_dotenv("../.env")

# Initialize Streamlit page config
st.set_page_config(
    page_title="LLM Chat Interface",
    page_icon="💬",
    layout="wide",
    initial_sidebar_state="expanded"
)

# Initialize OpenAI client
@st.cache_resource
def get_openai_client():
    return OpenAI(api_key=os.getenv("OPENAI_API_KEY"))

client = get_openai_client()

# Initialize session state
if "messages" not in st.session_state:
    st.session_state.messages = []

if "model" not in st.session_state:
    st.session_state.model = "gpt-5"

if "temperature" not in st.session_state:
    st.session_state.temperature = 0.7

if "max_tokens" not in st.session_state:
    st.session_state.max_tokens = 2000

# Sidebar configuration
st.sidebar.markdown("## ⚙️ Settings")

# Model selection
available_models = ["gpt-5", "gpt-5.4", "gpt-5.4-nano", "o3-mini"]
st.session_state.model = st.sidebar.selectbox(
    "Model",
    available_models,
    index=available_models.index(st.session_state.model)
)

# Temperature slider
st.session_state.temperature = st.sidebar.slider(
    "Temperature",
    min_value=0.0,
    max_value=2.0,
    value=st.session_state.temperature,
    step=0.1,
    help="Lower = more deterministic, Higher = more creative"
)

# Max tokens input
st.session_state.max_tokens = st.sidebar.number_input(
    "Max Tokens",
    min_value=100,
    max_value=4000,
    value=st.session_state.max_tokens,
    step=100,
    help="Maximum length of the response"
)

# Clear history button
if st.sidebar.button("🗑️ Clear History", use_container_width=True):
    st.session_state.messages = []
    st.rerun()

# Display model info in sidebar
st.sidebar.markdown("---")
st.sidebar.markdown(
    f"""
    **Current Config:**
    - Model: {st.session_state.model}
    - Temperature: {st.session_state.temperature}
    - Max Tokens: {st.session_state.max_tokens}
    """
)

# Main chat interface
st.markdown("# 💬 LLM Chat Interface")
st.markdown("Chat with your AI assistant. Messages are stored during this session.")

# Display chat history
for message in st.session_state.messages:
    with st.chat_message(message["role"]):
        st.markdown(message["content"])

# Chat input
user_input = st.chat_input("Type your message here...")

if user_input:
    # Add user message to history
    st.session_state.messages.append({
        "role": "user",
        "content": user_input
    })
    
    # Display user message
    with st.chat_message("user"):
        st.markdown(user_input)
    
    # Get AI response
    try:
        with st.chat_message("assistant"):
            message_placeholder = st.empty()
            full_response = ""
            
            # Call OpenAI API with streaming
            with message_placeholder.container():
                stream = client.responses.create(
                    model=st.session_state.model,
                    input=[
                        {
                            "role": "user",
                            "content": user_input,
                        }
                    ],
                    stream=True,
                )
                
                # Process streaming response
                for event in stream:
                    if hasattr(event, 'delta') and event.delta:
                        full_response += event.delta
                        message_placeholder.markdown(full_response + "▌")
                
                # Display final response
                message_placeholder.markdown(full_response)
            
            # Add assistant message to history
            st.session_state.messages.append({
                "role": "assistant",
                "content": full_response
            })
    
    except Exception as e:
        st.error(f"⚠️ Error: {str(e)}")
        st.info("Make sure your OPENAI_API_KEY is set in your environment.")
        # Remove the user message if API call failed
        st.session_state.messages.pop()

# Footer
st.markdown("---")
st.markdown(
    """
    **Note:** Conversation history is stored only during this session. 
    Refresh the page or close the app to start a new conversation.
    """
)