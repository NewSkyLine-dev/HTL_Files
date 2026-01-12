"""
author: Oppermann Fabian
file_name: ggt.py
"""

from utils import ggt

a = int(input("Erste Zahl: "))
b = int(input("Zweite Zahl: "))
print("ggt({}, {}) = {}".format(a, b, ggt(a, b)))
