"""
author: Oppermann Fabian
file_name honor8.py
"""

def convToDec(n, val=0, index=0):
    temp = 0
    if "8" in n or "9" in n:
        print("Error")
    else:
        for numbers in list(n):
            val += int(numbers) + temp
            temp = val * 8

        print(val)

convToDec(input("Geben Sie eine Zahl ein: "))