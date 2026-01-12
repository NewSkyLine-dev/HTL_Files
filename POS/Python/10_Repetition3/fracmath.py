from fractions import add_fractions, sub_fractions, mul_fractions, div_fractions

ein = input("Welche Methode: ")
n1 = int(input("Zahl 1: "))
d1 = int(input("Zahl 2: "))
n2 = int(input("Zahl 3: "))
d2 = int(input("Zahl 4: "))

if ein == "add":
    print(add_fractions(n1, d1, n2, d2))
elif ein == "sub":
    print(sub_fractions(n1, d1, n2, d2))
elif ein == "mul":
    print(mul_fractions(n1, d1, n2, d2))
elif ein == "div":
    print(div_fractions(n1, d1, n2, d2))