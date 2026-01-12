def compare(a, b):
    if a < b:
        return -1
    elif a == b:
        return 0
    elif a > b:
        return 1


def slope(x1, y1, x2, y2):
    k = (y2 - y1) / (x2 - x1)
    return k


def intercept(x1, y1, x2, y2):
    k = slope(x1, y1, x2, y2)
    b = y1 - k * x1
    return b

