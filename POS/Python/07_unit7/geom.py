"""
author = Oppermann Fabian
file_name: geom.py
"""

from geomlib import *
from math import sqrt


print("Willkommen beim geometrischen Wunderzwerg!\n")
eingabe = input("(1) Berchnung der Entfernung: eindimensional\n(2) Berchnung der Entfernung: zweidimensional\n(3) Berchnung der Entfernung: dreidimensional\n(4) Zechenen eines Vielecks\n(5) Dreieck Zeichenen mit min. 3 oder max. x-y-Werten:\n(6) Stern aus mal Zeichen")


def eindimensional():
    x1 = input("Bitte geben Sie den x-Wert von Punkt 1 ein: ")
    y1 = input("Bitte geben Sie den y-Wert von Punkt 1 ein: ")
    try:
        val = float(x1), float(y1)
    except ValueError:
        print("Das Sind keine Zahlen. ValueError")
    print(distance1(float(x1), float(y1)))


def zweidimensional():
    x1 = input("Bitte geben Sie den x-Wert von Punkt 1 ein: ")
    y1 = input("Bitte geben Sie den y-Wert von Punkt 1 ein: ")
    x2 = input("Bitte geben Sie den x-Wert von Punkt 2 ein: ")
    y2 = input("Bitte geben Sie den y-Wert von Punkt 2 ein: ")
    try:
        val = float(x1), float(y1), float(x2), float(y2)
    except ValueError:
        print("Mindestens eine Eingabe ist keine Zahl. ValueError")
    print(distance2(float(x1), float(y1), float(x2), float(y2)))


def dreidimensional():
    x1 = input("Bitte geben Sie den x-Wert von Punkt 1 ein: ")
    y1 = input("Bitte geben Sie den y-Wert von Punkt 1 ein: ")
    z1 = input("Bitte geben Sie den z-Wert von Punkt 1 ein: ")
    x2 = input("Bitte geben Sie den x-Wert von Punkt 2 ein: ")
    y2 = input("Bitte geben Sie den y-Wert von Punkt 2 ein: ")
    z2 = input("Bitte geben Sie den z-Wert von Punkt 2 ein: ")
    try:
        val = float(x1), float(y1), float(x2), float(y2), float(z1), float(z2)
    except ValueError:
        print("Mindestens eine Eingabe ist keine Zahl. ValueError")
    print(distance3(float(x1), float(y1), float(z1), float(x2), float(y2), float(z2)))


def reg_polygon():
    x = float(input("Geben Sie den start x-Wert ein: "))
    y = float(input("Geben Sie den start y-Wert ein: "))
    laenge = float(input("Geben Sie die Seitnlänge ein: "))
    ecken = int(input("Geben Sie die Anzahl von den Ecken ein:"))
    regular_polygon(x, y, laenge, ecken)
    

def polygon2():
    x1 = input("Geben Sie den x1 Wert ein: ")
    y1 = input("Geben Sie den y1 Wert ein: ")
    x2 = input("Geben Sie den x2 Wert ein: ")
    y2 = input("Geben Sie den y2 Wert ein: ")
    x3 = input("Geben Sie den x3 Wert ein: ")
    y3 = input("Geben Sie den y3 Wert ein: ")
    x4 = input("Geben Sie den x4 Wert ein: ")
    y4 = input("Geben Sie den y4 Wert ein: ")
    x5 = input("Geben Sie den x5 Wert ein: ")
    y5 = input("Geben Sie den y5 Wert ein: ")
    if x4 == '' and y4 == '':
        print(polygon(float(x1), float(y1), float(x2), float(y2), float(x3), float(y3)))
    elif x5 == '' and y5 == '' and x4 is not '' and y4 is not '':
        print(polygon(float(x1), float(y1), float(x2), float(y2), float(x3), float(y3), float(x4), float(y4)))
    elif x4 is not '' and y4 is not '' and x5 is not '' and y5 is not '':
        print(polygon(float(x1), float(y1), float(x2), float(y2), float(x3), float(y3), float(x4), float(y4), float(x5), float(y5)))


def stars_triangle(number):
    for i in range(1, number + 1):
        print(i * "*")


if eingabe == "1":
    eindimensional()
elif eingabe == "2":
    zweidimensional()
elif eingabe == "3":
    dreidimensional()
elif eingabe == "4":
    reg_polygon()
elif eingabe == "5":
    polygon2()
elif eingabe == "6":
    stars_triangle(int(input("Geben Sie eine Zahl ein: ")))
else:
    print("Falsche Eingabe!")
