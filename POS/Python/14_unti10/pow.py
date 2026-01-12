"""
author: Oppermann Fabian
file_name: pow.py
"""


def pow(a, b):
    x = a
    y = b
    z = 1
    while y > 0:
        if y % 2 == 1:
            z = z * x
        y = y // 2
        x = x * x
    return z


print(pow(2, 3))