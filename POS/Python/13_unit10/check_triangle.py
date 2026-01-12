"""
author: Oppermann Fabian
file_name: check_triangle.py
"""



a = int(input("a: "))
b = int(input("b: "))
c = int(input("c: "))

if a + b > c and a + c > b and b + c > a:
    print("Das ist ein gültiges Dreieck")
else:
    print("Das ist kein gültiges Dreieck")
