"""
author: Fabian Oppermann
file_name: squareroot.py
"""

import math

def squareroot(a):
    x0 = a
    x1 = (x0 + a/x0)/2
    x2 = (x1 + a/x1)/2
    x3 = (x2 + a/x2)/2
    print("The square root of", a, "is approx.", x3)
    print("The square root of", a, "is", math.sqrt(a))
    print("The error is", abs(x3 - math.sqrt(a)))

squareroot(float(input("Enter a number: ")))