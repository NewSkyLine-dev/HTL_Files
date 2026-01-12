"""
author: Oppermann Fabian
file_name: mathelib.py
"""


def arith_mean(x, y, z):
    print((x + y + z) / 3)


def geom_mean(a, b, solution):
    if (a * b) < 0:
        return solution
    else:
        return (a * b)**0.5


def sum(x, y, op):
    if op == 'add':
        return x + y
    elif op == 'zusammen':
        return "{}{}".format(x, y)


def v(s):
    result = 9.81 * s
    if result < 0:
        return -1
    else:
        return result


def s(t):
    if t < 0:
        return -1
    else:
        return t * 9.81


def mod(a, b):
    return a - b * int(a / b)


def square_root(x):
    try:
        return x ** 0.5
    except Exception:
        return -1


def div(x, y):
    try:
        return x / y
    except Exception:
        return 0