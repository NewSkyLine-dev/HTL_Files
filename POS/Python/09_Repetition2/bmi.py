def bmi(weight, height):
    return weight / (height ** 2)

if bmi(55, 1.82) >= 15.5 and bmi(55, 1.82) <= 26:
    print("Normalgewicht")
elif bmi(55, 1.82) > 26.5:
    print("Übergewicht")
elif bmi(55, 1.82) < 15:
    print("Untergewicht")