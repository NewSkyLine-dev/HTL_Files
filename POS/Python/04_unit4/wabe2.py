"""
author: Fabian Oppermann
file_name: wabe2.py
"""

from turtle import *

pensize(5)
speed(9)


def hexagon(in_color):
    color("red", in_color)
    begin_fill()
    for e in range(6):
      forward(100)
      left(60)
    end_fill()


for i in range (6):
    hexagon("orange")
    forward(100)
    right(60)


rt(120)
hexagon("yellow")
done()