"""
author: Oppermann Fabian
file_name: smuecheck.py
"""

points = int(input("Bitte gib die Punkte ein: "))

if points == 0:
    print(points, "Punkte sind absolut zu wenig!")
elif points == 1:
    print(points, "Punkte sind viel zu wenig!")
elif points == 2:
    print(points, "Punkte sind zu wenig!")
elif points == 3:
    print(points, "Punkte sind gerade genug!")
elif points == 4:
    print(points, "Punkte sind nicht schlecht!")
elif points == 5:
    print(points, "Punkte sind sehr gut!")
else:
    print("Falsche Eingabe!")