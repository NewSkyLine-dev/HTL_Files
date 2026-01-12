"""
author: Oppermann Fabian
file_name: rate.py
"""

import random

name = input("Hallo! Wie lautet dein Name? ")
print(f"""Hallo {name}!
Ich denke mir jetzt eine Zahl zwischen 1 und 20 aus.\n""")

n = random.randint(1, 20)
eingabe = int(input("Wie lautet dein Tipp? "))
versuche = 1


while eingabe != n:
    if eingabe > n:
        print("Deine Zahl ist zu hoch!")
    else:
        print("Eingabe ist zu niedrig!")
    versuche += 1
    eingabe = int(input("Wie lautet dein Tipp? "))

print(f"\nGut geraten, {name}! Du hast meine Zahl in {versuche} Versuche erraten!")