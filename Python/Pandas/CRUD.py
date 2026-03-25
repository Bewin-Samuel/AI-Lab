import pandas as pd

# Load the CSV file into a DataFrame
df = pd.read_csv('Data/Employee.csv', skipinitialspace=True)

""" df['Gender'] = df['Gender'].str.strip()
df.columns = df.columns.str.strip() """

"""
df = df.head(10)
# Display the DataFrame
print(df)
"""

""" df.loc[3, 'SpendingScore'] = 80
print(df) """   

""" #Filters
filtered_df = df[(df['Age'] > 30) & (df['Gender'] == 'Male')]
print(filtered_df)
 """
""" #grouping
grouped_df = df.groupby('Gender')['Gender'].count()
print(grouped_df)

#aggregation -> Grouped By Gender then 2 aggregations -> count of EmployeeId and sum of AnnualIncome
grouped_df = df.groupby('Gender').agg({'EmployeeId': 'count', 'AnnualIncome': 'sum'})
print(grouped_df)

#aggregation with Alias name
grouped_df = df.groupby('Gender').agg(EmployeeCount=('EmployeeId', 'count'), TotalIncome=('AnnualIncome', 'sum'))
print(grouped_df) """

""" null_df = df.isnull().sum()
print(null_df)
 """
#few static functions
#drop duplicates
"""df.drop_duplicates(inplace=True)
print(df.shape)
print(df.info())"""

#sort values
df.sort_values(by='Age', ascending=False, inplace=True)
print(df.head(10))  