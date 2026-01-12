"""
author: Fabian Oppermann
file_name: wabe.py
"""

from turtle import *

speed(9)
def hexagon(in_color):
    color("brown", in_color)
    begin_fill()
    for _ in range(6):
      forward(100)
      left(60)
    end_fill()

for _ in range (6):
    hexagon("orange")
    forward(100)
    right(60)

rt(120)
hexagon("yellow")

done()