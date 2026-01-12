"""
author: Fabian Oppermann
file_name: division.py
"""

a = int(input("Enter the first number: "))
b = int(input("Enter the second number: "))

if (a - b) == 0:
    print("denominator must not become zero")
else:
    print("The result is " + str((a*b) / (a - b)))