"""
author: Oppermann Fabian
file_name: egyptian.py
"""

a = int(input("Gib die erste Zahl ein: "))
b = int(input("Gib die zweite Zahl ein: "))


def egyptian_mul(a, b):
    c=0
    while a >= 1:
        if a % 2 != 0:
            c += b
        a //= 2
        b *= 2
    return c


print(egyptian_mul(a, b))