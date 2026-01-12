import math


def sqrt(n):
    # Ergebnis
    result = 1
    # Zwischenspeicher
    temp = 0
    # Schleife
    for i in range(n):
        temp = (result + n / result)/2
        result = temp
    return result


n = int(input("Bitte eine Zahl eingeben: "))

print("Die annähernde Quadratwurzel von", n, "ist", sqrt(n))
print("Die Quadratwurzel von", n, "ist", math.sqrt(n))
print("Der Fehler beträgt", abs(math.sqrt(n) - sqrt(n)))