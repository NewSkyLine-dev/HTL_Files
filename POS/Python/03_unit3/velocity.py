"""
author: Fabian Oppermann
file_name: velocity.py
"""

def main():
    s = float(input("Enter the distance traveled in km: "))
    t = float(input("Enter the time traveled in h: "))
    v = s/t
    print("The velocity is", v, "km/h")


main()