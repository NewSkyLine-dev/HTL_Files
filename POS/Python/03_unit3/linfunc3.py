"""
author: Fabian Oppermann
file_name: linfunc3.py
"""


def linfunc3(x):
    return 2 * x + 1


for x in range(11):
    print("f({0}) = {1}".format(x, linfunc3(x)))