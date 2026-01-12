"""
author: Oppermann Fabian
file_name: linfunc.py
"""

frage = input("(A) Funktion f1 für die Zahlen von 0 bis 10\n(B) Funktion f2 für die Zahlen von -5 bis 5\n(C) Funktion f3 für die Zahlen von -7 bis 7\n")

if frage == "A" or "a":
    def f1(x):
        return 2 * x
    for i in range(11):
        print(f"f(x) = {f1(i)}")

elif frage == "B" or "b":
    def f2(x):
        return 3 * x
    for i in range(-5, 5):
        print(f"f(x) = {f2(i)}")

elif frage == "C" or "c":
    def f3(x):
        return 2 * x + 1
    for i in range(-7, 7):
        print(f"f(x) = {f3(i)}")