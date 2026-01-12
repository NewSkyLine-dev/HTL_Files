"""
author: Oppermann Fabian
file_name: minimum.py
"""


def minimum(a, b, c, d):
    if a < b and a < c and a < d:
        return a
    elif b < a and b < c and b < d:
        return b
    elif c < a and c < b and c < d:
        return c
    else:
        return d

print(minimum(int(input("Bitte gib eine Zahl ein: ")), int(input("Bitte gib eine Zahl ein: ")), int(input("Bitte gib eine Zahl ein: ")), int(input("Bitte gib eine Zahl ein: "))))