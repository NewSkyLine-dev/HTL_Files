"""
author: Fabian Oppermann
file_name: distance.py
"""

def main():
    t = float(input("Enter the time t in h: "))
    g = 9.81
    s = 0.5 * g * t**2
    print("The distance s is: ", s, "km")

main()