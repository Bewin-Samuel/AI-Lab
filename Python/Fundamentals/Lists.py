myList = [1,3,5,'a','b','c']
print(myList) # Output: [1, 3, 5, 'a', 'b', 'c']
print(type(myList)) # Output: <class 'list'>   
print(len(myList)) # Output: 6
print(myList[0]) # Output: 1    
print(myList[3]) # Output: 'a'
print(myList[-1]) # Output: 'c'
print(myList[1:4]) # Output: [3, 5, 'a']
print(myList[:3]) # Output: [1, 3, 5]
print(myList[3:]) # Output: ['a', 'b', 'c']
myList.append(7) # Output: [1, 3, 5, 'a', 'b', 'c', 7]
print(myList) # Output: [1, 3, 5, 'a', 'b', 'c', 7]
myList.insert(2, 4) # Output: [1, 3, 4, 5, 'a', 'b', 'c', 7]
print(myList) # Output: [1, 3, 4, 5, 'a', 'b', 'c', 7]
myList.remove('a') # Output: [1, 3, 4, 5, 'b', 'c', 7]
print(myList) # Output: [1, 3, 4, 5, 'b', 'c', 7]
myList.pop(2) # Output: [1, 3, 5, 'b', 'c', 7]
print(myList) # Output: [1, 3, 5, 'b', 'c', 7]
myList[1] = 2 # Output: [1, 2, 5, 'b', 'c', 7]
print(myList) # Output: [1, 2, 5, 'b', 'c', 7]
# Note: sort() and reverse() cannot be used on mixed-type lists (int and str)  
