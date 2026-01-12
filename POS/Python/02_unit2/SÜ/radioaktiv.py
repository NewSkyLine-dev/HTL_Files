"""
author: Fabian Oppermann
file_name: radioaktiv.py
"""

from turtle import *

speed(9)
penup()

for i in range(2):
    fd(200)
    lt(90)
pendown()

pensize(5)
fillcolor("yellow")

# Quadrat zeichnen
begin_fill()
for i in range(4):
    fd(400)
    lt(90)
end_fill()

# zurück zum Ursprung
penup()
fd(200)
lt(90)
fd(200)
pendown()

# drei schwarze Sektoren zeichnen
fillcolor("black")


for i in range(3):
    begin_fill()
    fd(160)
    lt(90)
    circle(160,60)
    lt(90)
    fd(160)
    lt(120)
    end_fill()

    lt(120)

# Zentraler Kreis
penup()
fd(50)
lt(90)
pencolor("yellow")
pendown()

begin_fill()
circle(50)
end_fill()

hideturtle()