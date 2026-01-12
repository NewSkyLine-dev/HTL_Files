"""
author: Fabian Oppermann
file_name: draw_linfunc.py
"""


from turtle import *


speed(0)
cm = 3.779528 * 10 # 3.779528 is a pixel in mm
prompt = "Werte"
k = float(textinput("Geben Sie k ein: ", prompt))
d = float(textinput("Geben Sie d ein: ", prompt))


def draw_axes():
    for i in range(-10, 11):
        rt(90)
        fd(cm / 2)
        rt(180)
        fd(cm)
        write(str(i))
        rt(180)
        fd(cm / 2)
        lt(90)
        if i < 10:
            fd(cm)


def axes(): 
    penup()
    goto(-302 - cm * 2, 0)
    pendown()
    draw_axes()
    penup()
    goto(0, 0)
    goto(0, -302 - cm * 2)
    lt(90)
    pendown()
    draw_axes()


def draw_linfunc():
    penup()
    goto(0, cm * d )
    pendown()
    goto(-(d / k) * cm , 0)


def main(): 
    axes()
    draw_linfunc()
    done()


main()