"""
author: Oppermann Fabian
file_name: kgv.py
"""

from utils import ggt

a = int(input("Erste Zahl: "))
b = int(input("Zweite Zahl: "))


print("kgv({}, {}) = {}".format(a, b, (a*b)/ggt(a,b)))
