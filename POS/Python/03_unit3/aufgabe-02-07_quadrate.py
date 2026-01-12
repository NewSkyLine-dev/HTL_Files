"""
author: Fabian Oppermann
file_name: aufgabe-02-07_quadrate.py
"""

from turtle import *


pensize(9)
speed(0)

lt(20)


def rec(in_color, out_color):
    color(out_color, in_color)
    begin_fill()
    for i in range(4):
        fd(280)
        rt(90)
    end_fill()

rec("yellow", "darkblue")
rt(60)
rec("red", "lightblue")
rt(60)
rec("lightblue", "green")
rt(60)
rec("darkblue", "yellow")
rt(60)
rec("lightblue", "red")
rt(60)
rec("magenta", "green")

color("darkblue", "yellow")
begin_fill()
rt(60)
fd(280)
rt(90)
fd(280)
penup()
rt(135)
fd(395)
pendown()
end_fill()

hideturtle()

done()