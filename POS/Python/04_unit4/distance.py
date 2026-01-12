"""
author: Fabian Oppermann
file_name: distance.py
"""

x = int(input("Geben Sie den erste Punkt ein: "))
y = int(input("Geben Sie den zweiten Punkt ein: "))


def distance(x, y):
    if x >= y:
        result = x - y
    else:
        result = y - x
    return result


print("----------------------------------------------")
print("Ohne abs Funktion: " + str(distance(x, y)))
print("Mit abs Funktion: " + str(abs(x-y)))
print("----------------------------------------------")