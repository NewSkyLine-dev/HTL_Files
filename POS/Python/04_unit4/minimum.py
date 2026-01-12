"""
author: Fabian Oppermann
file_name: minimum.py
"""
def minimum(a, b, c):
    if a < b and a < c:
        return a
    elif b < a and b < c:
        return b
    else:
        return c

def main():
    a = int(input("Enter a number: "))
    b = int(input("Enter a number: "))
    c = int(input("Enter a number: "))
    print("The smallest number is", minimum(a, b, c))

main()