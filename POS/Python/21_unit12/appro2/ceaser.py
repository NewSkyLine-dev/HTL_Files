"""
author: Oppermann Fabian
file_name: ceaser.py
"""


def createCeaser():
    alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    caeserAlphabet = "DEFGHIJKLMNOPQRSTUVWXYZABC"
    alphabetSum = {}

    # Macht ein Dictionary mit dem Alphabet und dem caeserAlphabet
    # key ist original Alphabet
    for letter1, letter2 in zip(alphabet, caeserAlphabet):
        alphabetSum[letter1] = letter2

    return alphabetSum


def encrypt_msg(s):
    normalSum = {}
    encrypt = ""
    for k, v in createCeaser().items():
        normalSum[v] = k
    
    for letters in s:
        if letters == " ":
            encrypt += " "
        else:
            encrypt += normalSum[letters]
    
    return encrypt

def main():
    crypt = ""
    eingabe = input("Text eingabene: ").upper()

    # Erkennt welcher buchstabe es ist und ersetzt ihn mit dem caeserAlphabet
    for letters in eingabe:
        if letters == " ":
            crypt += " "
        else:
            crypt += createCeaser()[letters]

    return "".join(f"{crypt} -> {encrypt_msg(crypt)}")
        

print(main())