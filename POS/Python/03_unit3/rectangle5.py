"""
author: Fabian Oppermann
file_name: rectange5.py
"""

import math

def main():
    perimeter = eval(input("Enter the perimeter: "))
    area = eval(input("Enter the area: "))

    length = math.sqrt(area * 4)
    width = math.sqrt(area * perimeter / 16)

    print()
    print("The length is", length, "and the width is", width)

main()