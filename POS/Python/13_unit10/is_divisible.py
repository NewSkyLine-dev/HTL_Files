"""
author: Oppermann Fabian
file_name: is_divisible.py
"""

a = int(input("Zahl 1: "))
b  = int(input("Zahl 2: "))


def is_divisible(a, b):
    if b == 0:
        return False
    else:
        return True


print(is_divisible(a, b))

