import streamlit as st
import pandas as pd
import numpy as np

st.write("Got lots of data? Great! Streamlit can show [dataframes](https://docs.streamlit.io/develop/api-reference/data) with hundred thousands of rows, images, sparklines – and even supports editing! ✍️")

num_rows = st.slider("Number of rows", 1, 10000, 5000)
np.random.seed(500)
data = []
for i in range(num_rows):
    data.append(
        {
            "Preview": f"https://picsum.photos/400/200?lock={i}",
            "Views": np.random.randint(0, 1000),
            "Active": np.random.choice([True, False]),
            "Category": np.random.choice(["🤖 LLM", "📊 Data", "⚙️ Tool"]),
            "Progress": np.random.randint(1, 100),
        }
    )
data = pd.DataFrame(data)

config = {
    "Preview": st.column_config.ImageColumn(),
    "Progress": st.column_config.ProgressColumn(),
}

if st.toggle("Enable editing"):
    edited_data = st.data_editor(data, column_config=config, use_container_width=True)
else:
    st.dataframe(data, column_config=config, use_container_width=True)