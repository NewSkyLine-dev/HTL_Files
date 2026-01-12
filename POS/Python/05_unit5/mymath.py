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


def sqrt(x0):
    a = x0
    x1 = (x0 + a / x0)/2
    x2 = (x1 + a / x1)/2
    x3 = (x2 + a / x2)/2
    return x3


def minimum(a, b, c):
    if a < b and a < c:
        return a
    elif b < a and b < c:
        return b
    else:
        return c

def grading(points):
    if points < 51:
        print("Sie haben nicht ein nicht Genügend")
    elif points < 63:
        print("Sie haben ein Genügend")
    elif points < 79:
        print("Sie haben ein Befriedigend")
    elif points < 91:
        print("Sie haben ein Gut")
    else:
        print("Sie haben ein Sehr Gut")

