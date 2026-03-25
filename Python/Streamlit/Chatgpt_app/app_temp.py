# Open router API key for using a free model , paid models
# How can we mimic the streaming behavior of ChatGPT in Streamlit?
# How can we have all the session_state stored for our conversation. 


import streamlit as st
from openai import OpenAI
from openai import AuthenticationError, APIStatusError
from openai import APIConnectionError, APITimeoutError

# Configure the page
st.set_page_config(page_title="My ChatBottt", page_icon="🤖")

AVAILABLE_MODELS = {
    "Mistral 7B Free": "mistralai/mistral-7b-instruct:free",
    "Llama 3.1 8B Free": "meta-llama/llama-3.1-8b-instruct:free",
    "Gemma 2 9B Free": "google/gemma-2-9b-it:free",
    "OpenAi gpt oss": "openai/gpt-oss-120b"
}

# Initialize the OpenAI client with OpenRouter
if "OPENROUTER_API_KEY" in st.secrets:
    api_key = st.secrets["OPENROUTER_API_KEY"].strip()
else:
    api_key = st.sidebar.text_input("Enter OpenRouter API Key", type="password").strip()

if "messages" not in st.session_state:
    st.session_state.messages = []

if "selected_model" not in st.session_state:
    st.session_state.selected_model = AVAILABLE_MODELS["OpenAi gpt oss"]

if "data_collection" not in st.session_state:
    st.session_state.data_collection = "deny"


with st.sidebar:
    st.header("Chat Settings")
    st.caption("Configure the chatbot before sending a message.")

    #secrets_api_key = st.secrets.get("OPENROUTER_API_KEY", "")
    # api_key = secrets_api_key or st.text_input(
    #     secrets_api_key,
    #     type="password",
    #     placeholder="Paste your API key here",
    # )

    selected_model_label = st.selectbox(
        "Model",
        options=list(AVAILABLE_MODELS.keys()),
        index=list(AVAILABLE_MODELS.values()).index(st.session_state.selected_model),
    )
    st.session_state.selected_model = AVAILABLE_MODELS[selected_model_label]

    privacy_option = st.selectbox(
        "Prompt Retention",
        options=["Do not retain prompts", "Allow provider retention"],
        index=0 if st.session_state.data_collection == "deny" else 1,
        help="Choose whether the provider is allowed to retain prompt data.",
    )
    st.session_state.data_collection = (
        "deny" if privacy_option == "Do not retain prompts" else "allow"
    )

    if st.button("Clear Chat", use_container_width=True):
        st.session_state.messages = []
        st.rerun()

    st.divider()
    st.write(f"Messages in session: {len(st.session_state.messages)}")
    # if secrets_api_key:
    #     st.success("Using API key from Streamlit secrets.")
    # else:
    #     st.info("Add your API key here for local development.")

if not api_key:
    st.warning("Please enter your OpenRouter API key to continue.")
    st.stop()

client = OpenAI(
    base_url="https://openrouter.ai/api/v1",
    api_key=api_key,
    timeout=45.0,
    max_retries=2,
    default_headers={
        "HTTP-Referer": "http://localhost:8501",  # Optional: shows on OpenRouter rankings
        "X-Title": "My ChatBot",                  # Optional: shows on OpenRouter rankings
    }
)

# App title
st.title("🤖 My ChatBot")

# Display chat history
for message in st.session_state.messages:
    with st.chat_message(message["role"]):
        st.markdown(message["content"])

# Handle user input
if prompt := st.chat_input("What would you like to know?"):
    # Add user message to chat history
    st.session_state.messages.append({"role": "user", "content": prompt})

    # Display user message
    with st.chat_message("user"):
        st.markdown(prompt)

# Training models on your inputs or prompts - Open router consent or privacy page
# Retention - How much time you'd want to store your input prompts to these models.

    # Generate AI response
    with st.chat_message("assistant"):
        try:
            response = client.chat.completions.create(
                model=st.session_state.selected_model,  # safer free model
                messages=st.session_state.messages,
                stream=True,
                extra_headers={
                    "HTTP-Referer": "http://localhost:8501",
                    "X-Title": "My ChatBot"
                },
                extra_body={
                    "provider": {
                        "data_collection": st.session_state.data_collection
                    }
                }
            )
# Hello how can I help you today ? 
# Hello how can I help you today ?


            # Stream the response
            response_text = ""
            response_placeholder = st.empty()

            for chunk in response:
                if chunk.choices[0].delta.content is not None:
                    # Clean up unwanted tokens
                    content = chunk.choices[0].delta.content
                    content = (
                        content.replace('<s>', '')
                        .replace('<|im_start|>', '')
                        .replace('<|im_end|>', '')
                        .replace("<|OUT|>", "")
                    )
                    response_text += content
                    response_placeholder.markdown(response_text + "▌")

            # Final cleanup of response text
            response_text = (
                response_text.replace('<s>', '')
                .replace('<|im_start|>', '')
                .replace('<|im_end|>', '')
                .replace("<|OUT|>", "")
                .strip()
            )
            response_placeholder.markdown(response_text)

            # Add assistant response to chat history
            st.session_state.messages.append(
                {"role": "assistant", "content": response_text}
            )

        except AuthenticationError as e:
            st.error("Authentication failed: OpenRouter API key was rejected.")
            st.info("Verify the key value and ensure it is active in your OpenRouter account.")
            st.caption(f"Details: {str(e)}")
        except APIStatusError as e:
            st.error(f"OpenRouter returned HTTP {e.status_code}.")
            st.info("This can be caused by model access, quota, or request validation issues.")
            st.caption(f"Details: {str(e)}")
        except APITimeoutError as e:
            st.error("Request timed out while waiting for OpenRouter.")
            st.info("Try again, switch to a free model, or retry with a shorter prompt.")
            st.caption(f"Details: {str(e)}")
        except APIConnectionError as e:
            st.error("Could not establish a stable connection to OpenRouter.")
            st.info("Your internet is reachable, but SSL/proxy/firewall/VPN can still interrupt SDK requests.")
            st.caption(f"Details: {str(e)}")
        except Exception as e:
            st.error(f"Unexpected error: {str(e)}")
            st.info("If this persists, switch model and retry with a short prompt.")
