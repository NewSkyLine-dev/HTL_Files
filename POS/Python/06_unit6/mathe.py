"""
author: Oppermann Fabian
file_name: mathe.py
"""

from mathelib import *


def sequence(x):
    if x == "arith":
        arith_mean(float(input("Geben Sie eine Zahl ein: ")), float(input("Geben Sie eine Zahl ein: ")), float(input("Geben Sie eine Zahl ein: ")))
    elif x == "geom":
        a = float(input("Geben Sie eine Zahl ein: "))
        b = float(input("Geben Sie eine Zahl ein: "))
        print(geom_mean(a, b, "Das geometrische Mittel von {} und {} kann nicht berechnet werden".format(str(a), str(b))))
    elif x == "sum":
        print(sum(int(input("Geben Sie eine Zahl ein: ")), int(input("Geben Sie eine Zahl ein: ")), input("Geben Sie einen Operator ein: ")))
    elif x == "v":
        print(v(float(input("Geben Sie die Zeit ein: "))))
    elif x == "t":
        print(s(float(input("Geben Sie die Zeit ein: "))))
    elif x == "mod":
        print(mod(float(input("Geben Sie eine Zahl ein: ")), float(input("Geben Sie eine Zahl ein: "))))
    elif x == "sqrt":
        print(square_root(float(input("Geben Sie eine Zahl ein: "))))
    elif x == "div":
        print(div(float(input("Geben Sie eine Zahl ein: ")), float(input("Geben Sie eine Zahl ein: "))))



sequence(input("Operator: "))