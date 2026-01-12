"""
author: Oppermann Fabian
file_name: quader.py
"""

def main():
    print("Bitte geben Sie die Raumdiagonale ein:")
    r = float(input())
    print("Bitte geben Sie die erste Seite ein:")
    s1 = float(input())
    print("Bitte geben Sie die zweite Seite ein:")
    s2 = float(input())
    s3 = (r ** 2 + s1 ** 2 + s2 ** 2) ** 0.5
    print("Die dritte Seite beträgt:", s3)
    print("Das Volumen beträgt:", s1 * s2 * s3 / 6)


if __name__ == '__main__':
    main()