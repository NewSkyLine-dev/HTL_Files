def perimeter(a, b):
    return 2 * (a + b)

def area(a, b):
    return a * b

def class_(a):
    if a < 100:
        return "kleines Quadrat"
    elif a >= 100 and a < 200:
        return "großes Quadrat"
    elif a < 200:
        return "kleines Rechteck"
    else:
        return "großes Rechteck"

a = int(input("Geben Sie die erste Seitnlänge ein: "))
b = int(input("Geben Sie die zweite Seitnlänge ein: "))

print("Der Umfang ist:", perimeter(a, b))
print("Die Fläche ist:", area(a, b))
print("Die Größenklasse ist:", class_(area(a, b)))