a = int(input("Seite a: "))
b = int(input("Seite b: "))
c = int(input("Seite c: "))

s = (a + b + c) / 2

print("Flächeninhalt:", (s * (s - a) * (s - b) * (s - c)))