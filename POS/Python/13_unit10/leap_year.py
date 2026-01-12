"""
author: Oppermann Fabian
file_name: leap_year.py
"""

def check_leap_year(year):
    if year % 4 == 0 and year % 100 != 0 or year % 400 == 0:
        return True
    else:
        return False

year = int(input("Gib eine Jahreszahl ein: "))

if check_leap_year(year):
    print("Es handelt sich um ein Schaltjahr")
else:
    print("Es handelt sich nicht um ein Schaltjahr")
