t = (1,2,3,4,5)
print(t) # Output: (1, 2, 3, 4, 5)
print(type(t)) # Output: <class 'tuple'>
print(len(t)) # Output: 5
print(t[0]) # Output: 1
print(t[3]) # Output: 4
print(t[-1]) # Output: 5
print(t[1:4]) # Output: (2, 3, 4)
print(t[:3]) # Output: (1, 2, 3)
print(t[3:]) # Output: (4, 5)
# Tuples are immutable, so we cannot modify them
# t[0] = 10 # This will raise a TypeError                                                                                               
# However, we can concatenate tuples to create a new tuple
t2 = t + (6, 7)
print(t2) # Output: (1, 2, 3, 4, 5, 6, 7)
# We can also use the tuple() constructor to create a tuple from a list
myList = [1, 2, 3]  
t3 = tuple(myList)
print(t3) # Output: (1, 2, 3)   
# We can use the count() method to count the occurrences of a value in a tuple
print(t.count(2)) # Output: 1   
# We can use the index() method to find the index of the first occurrence of a value in a tuple
print(t.index(3)) # Output: 2
# print(t.index(6)) # This will raise a ValueError since 6 is not in the tuple
print(2 in t) # Output: True
print(6 in t) # Output: False   
print(2 not in t) # Output: False
print(6 not in t) # Output: True    
print(t * 3) # Output: (1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5)
