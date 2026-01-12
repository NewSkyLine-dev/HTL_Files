"""
author: Oppermann Fabian
file_name: 17.py
"""


print("Bitte Seitenlänge eines Rechteckes eingeben:")
seitenlaenge = int(input())
print("Bitte Umfang eines Rechteckes eingeben:")
umfang = int(input())

flaeche = seitenlaenge * umfang
rest = seitenlaenge - umfang

print("Der Rest ist:", rest)
print("Die Fläche ist:", flaeche)