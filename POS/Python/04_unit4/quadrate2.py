"""
author: Fabian Oppermann
file_name: quadrate2.py
"""

from turtle import *


def draw_square():
    for i in range(4):
        forward(100)
        right(90)


def draw_art():
    color("black")
    speed(2)
    for i in range(6):
        draw_square()
        right(20)
    exitonclick()


draw_art()