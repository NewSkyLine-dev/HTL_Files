"""
author: Fabian Oppermann
file_name: rectangles.py
"""
side1 = int(input("Enter the length of the first side of the rectangle: "))
side2 = int(input("Enter the length of the second side of the rectangle: "))

if side1 == side2:
    if side1 * side2 < 100:
        print("Das Quadrat ist ein kleines Quadrat.")
    elif side1 * side2 >= 100:
        print("Das Quadrat ist ein großes Quadrat.")
else:
    if side1 * side2 < 200:
        print("Das Rechteck ist ein kleines Rechteck.")
    elif side1 * side2 >= 200:
        print("Das Rechteck ist ein großes Rechteck.")