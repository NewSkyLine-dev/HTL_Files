"""
author: Oppermann Fabian
file_name: e.py
"""

n = int(input("Bitte geben Sie n ein: "))


def e(n):
    """
    Berechnet die Zahl e.
    >>> param n = Anzahl der Summanden
    >>> return e
    """
    summe = 0
    temp = 0
    while temp < n+1:
        summe += 1/factorial(temp)
        temp +=1
    return summe


def factorial(n):
    """
    Berechnet die Fakultät von n.
    >>> return = n!
    """
    if n == 0:
        return 1
    else:
        return n * factorial(n-1)


print(e(n))