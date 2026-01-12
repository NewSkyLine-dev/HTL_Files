def power(x, y):
    if x == 1:
        return 1
    elif x == -1 and y % 2 == 0:
        return 1
    elif x == -1 and y % 2 != 0:
        return -1
    elif y == 0:
        return 1
    elif y == 1:
        return x
    elif y > 1:
        return x * power(x, y - 1)
    elif y < 0 or x == 0:
        return "Invalid input"


print(power(float(input("Geben Sie die Zahl x ein:")), float(input("Geben Sie die Zahl y ein:"))))


def sqrt(x0):
    a = x0
    x1 = (x0 + a / x0)/2
    x2 = (x1 + a / x1)/2
    x3 = (x2 + a / x2)/2
    return x3


sqrt(float(input("Geben Sie die Zahl x ein für sqrt:"))),


def minimum(a, b, c):
    if a < b and a < c:
        return a
    elif b < a and b < c:
        return b
    else:
        return c


def main():
    a = int(input("Bitte gib die erste Zahl ein: "))
    b = int(input("Bitte gib die zweite Zahl ein: "))
    c = int(input("Bitte gib die dritte Zahl ein: "))
    print("Das Minimum der drei Zahlen ist", minimum(a, b, c))


main()