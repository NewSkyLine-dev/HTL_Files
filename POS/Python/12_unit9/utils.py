"""
author: Oppermann Fabian
file_name: utils.py
"""


def ggt(a, b):
    """
    Berechnet den größten gemeinsamen Teiler zweier Zahlen.
    """
    if a == b:
        return a
    elif a > b:
        return ggt(a - b, b)
    else:
        return ggt(a, b - a)
