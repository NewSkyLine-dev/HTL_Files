"""
author: Oppermann Fabian
file_name: triangle.py
"""


def main():
    cathetus = float(input("Bitte geben Sie die Kathete ein: "))
    hypotenuse = float(input("Bitte geben Sie die Hypotenuse ein: "))
    cathetus2 = hypotenuse ** 2 - cathetus ** 2
    cathetus2 = cathetus2 ** 0.5
    print("Die andere Kathete ist:", cathetus2)
    print("Der Umfang ist:", cathetus + cathetus2)
    print("Die Fläche ist:", cathetus * cathetus2 / 2)


if __name__ == '__main__':
    main()