"""
author: Oppermann Fabian
file_name: factorial.py
"""

n = int(input("Geben Sie eine Zahl ein: "))


def fak(n):
    if n == 0:
        return 1
    else:
        return n * fak(n-1)


print(fak(n))