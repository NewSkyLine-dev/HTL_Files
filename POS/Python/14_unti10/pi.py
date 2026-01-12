"""
author: Oppermann Fabian
file_name: pi.py
"""

import math

n = int(input("Bitte geben Sie n ein: "))
pi = 0


for i in range(n):
    pi += 4 / (2 * i + 1) * (-1) ** i


print("pi = ", pi)
print("Fehler = ", abs(pi - math.pi))