"""
author: Fabian Oppermann
file_name: triangle7.py
"""

import math

a = float(input("Enter the length of the first cathetus: "))
c = float(input("Enter the length of the hypotenuse: "))
b = math.sqrt(c**2 - a**2)
p = a + b + c
s = (a + b + c) / 2
A = math.sqrt(s * (s - a) * (s - b) * (s - c))

print("The length of the second cathetus is:", b)
print("The perimeter of the triangle is:", p)
print("The area of the triangle is:", A)