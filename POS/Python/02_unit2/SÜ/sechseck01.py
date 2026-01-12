"""
author: Fabian Oppermann
file_name: sechseck.py
"""

from turtle import *

penup()
backward(50)
pendown()

rt(90)
for i in range(6):
    forward(100)
    left(60)

done()