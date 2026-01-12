"""
author: Oppermann Fabian
file_name: slaray2.py
"""

x = 1800
k = 20
d = 0
print(x)


wunsch = int(input("Geben Sie Ihren Wunschgehalt ein: "))

while x < wunsch:
    x += k
    d += 1
print(f"Sie müssen noch {d} Jahre bei uns arbeiten")

# Berechnung des Bruttogehalts
brutto = 1800

# Berechnung des Sozialversicherungsbeitrags
sv = brutto * 0.2

# Berechnung des Einkommenssteuerbetrags
et = brutto * 0.3

# Berechnung des Netto
netto = brutto - sv - et

# Ausgabe
print("Bruttogehalt:", brutto)
print("Sozialversicherungsbeitrag:", sv)
print("Einkommenssteuer:", et)
print("Netto:", netto)