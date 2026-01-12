"""
author: Oppermann Fabian
file_name: diagonal1.py
"""

from turtle import *


def quadrat(length):
    for i in range(4):
        rt(90)
        fd(length)


def diagonal(length):
    lt(45)
    fd(length)
    quadrat(length)


def main():
    length = float(input("Geben Sie die länge der Seiten ein:"))
    quadrat(length)
    diagonal(length=30)
    done()


main()