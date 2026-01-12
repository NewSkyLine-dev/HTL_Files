"""
author: Fabian Oppermann
file_name: power.py
"""
x = int(input("Enter x: "))
y = int(input("Enter y: "))

if x == 1:
    print("1")
elif x == -1 and y % 2 == 0:
    print("1")
elif x == -1 and y % 2 != 0:
    print("-1")
elif y == 0:
    print("1")
elif y == 1:
    print(x)
elif y > 1:
    print(x * y)
else:
    print("y < 0 or x and y equal 0")