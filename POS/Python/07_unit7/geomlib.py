"""
author = Oppermann Fabian
file_name: geomlib.py
"""

from turtle import *

def distance1(x, y):
    return "\nDie Entfernung zwischen dem Punkt {} und dem Punkt {} beträgt {}.".format(x, y, abs(x - y))


def distance2(x1, y1, x2, y2):
    return "\nDie Entfernung zwischen dem Punkt ({},{}) und dem Punkt ({},{}) beträgt {}.".format(x1, y1, x2, y2, round(((x1 - x2) ** 2 + (y1 - y2) ** 2) ** 0.5, 2))


def distance3(x1, y1, z1, x2, y2, z2):
    return "\nDie Entfernung zwwischen dem Punkt ({},{},{}) und dem Punkt ({},{},{}) beträgt {}.".format(x1, y1, z1, x2, y2, z2, round(((x1 - x2) ** 2 + (y1 - y2) ** 2 + (z1 - z2) ** 2) ** 0.5, 2))


def regular_polygon(x, y, laenge, ecken):
    pu()
    goto(x, y)
    pd()
    for _ in range(ecken):
        fd(laenge)
        lt(360 / ecken)
    goto(x, y)
    print(f"Der Umfang ist {laenge**2} m^2.")


def polygon(x1, y1, x2, y2, x3, y3, x4 = None, y4 = None, x5 = None, y5 = None):
    pu()
    goto(x1, y1)
    pd()
    goto(x2, y2)
    goto(x3, y3)
    if x4 is not None and y4 is not None:
        goto(x4, y4)
    if x5 is not None and y5 is not None:
        goto(x5, y5)
    goto(x1, y1)
    done()
    return distance2(x1, y1, x2, y2) + distance2(x2, y2, x3, y3) + distance2(x3, y3, x1, y1)
