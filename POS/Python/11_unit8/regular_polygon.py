"""
author: Oppermann Fabian
file_name regular_polygon2.py
"""

import turtle as tr

length = int(input("Seitenlänge: "))
sides = int(input("Anzahl der Seiten: "))


def regular_polygon2():
    colT = []
    tr.colormode(255)
    for i in range(sides):
        red = int(input("Rodanteil: "))
        green = int(input("Grünanteil: "))
        blue = int(input("Blauanteil: "))
        colT.append((red, green, blue))
    for i in colT:
        tr.pencolor(i)
        tr.fd(length)
        tr.lt(360 / sides)


regular_polygon2()