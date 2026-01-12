"""
author: Fabian Oppermann
file_name: triangle6.py
"""

import math

a = float(input("Enter the first cathetus: "))
b = float(input("Enter the second cathetus: "))

c = math.sqrt(a**2 + b**2)
p = a + b + c
s = a * b / 2

print("The hypotenuse is", c)
print("The perimeter is", p)
print("The area is", s)