from logging import exception
from utils import slope, intercept

try:
    x1 = float(input("x1: "))
    y1 = float(input("y1: "))
    x2 = float(input("x2: "))
    y2 = float(input("y2: "))
    print("Geradengleichung: y =", slope(x1, y1, x2, y2), "x +", intercept(x1, y1, x2, y2))
except Exception:
    print("Error")