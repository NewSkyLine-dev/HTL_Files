"""
author: Fabian Oppermann
file_name: sort.py
"""

def sort(a,b,c):
    if a > b:
        a,b = b,a
    if a > c:
        a,c = c,a
    if b > c:
        b,c = c,b
    return a,b,c

def main():
    a = int(input("Geben Sie die erste Nummer ein: "))
    b = int(input("Geben Sie die zweite Nummer ein: "))
    c = int(input("Geben Sie die dritte Nummer ein: "))
    print(sort(a,b,c))

main()