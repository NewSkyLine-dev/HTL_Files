"""
author: Fabian Oppermann
file_name: salary.py
"""


#1)
def salary(x, k, d):
    for i in range(5):
        x += k
        k += d
        print("Year {}: {}".format(i + 1, x))


salary(int(input("Geben Sie den Anfangsgehalt ein: ")), 20, 20)


print("----------------------------------------------------------------")

#2)
def salary(x, k, d):
    for i in range(5):
        x += x * k / 100
        k += d
        print("Year {}: {}".format(i + 1, x))


salary(int(input("Geben Sie den Anfangsgehalt ein: ")), 3, 3)

