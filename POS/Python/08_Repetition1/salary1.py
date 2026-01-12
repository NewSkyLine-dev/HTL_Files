"""
author: Oppermann Fabian
file_name: slaray1.py
"""

# Variablen definieren
x = 1800
k = 20
d = 0

# Nutzer nach Jahren fragen
jahre = int(input("Wie viele Jahre sind Sie schon bei uns?"))

# Berechnung
y = x + (k * jahre) + d

# Ergebnis ausgeben
print("Ihr Gehalt in diesem Jahr wird sein:", y, "Euro")