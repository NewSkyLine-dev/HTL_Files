from turtle import *

def sector(radius, angle):
    fd(radius)
    lt(90)
    circle(radius, angle)
    lt(90)
    fd(radius)
    lt(180 - angle)


for i in range(3):
    sector(100, 50)
    lt(90)
