"""
author: Fabian Oppermann
file_name: dreieck_arbeit.py
"""



from turtle import *

startseite = 65
aenderung = 35

pensize(10)
right(90)

seite = startseite

def rec(in_color, out_color):
    color(in_color, out_color)
    begin_fill()
    for i in range(3):
        fd(seite)
        lt(120)
    lt(120)
    end_fill()


rec("red", "cyan")

seite = seite + aenderung

rec("green", "magenta")

seite = seite + aenderung

rec("blue", "yellow")

done()