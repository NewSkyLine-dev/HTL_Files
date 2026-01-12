"""
author: Oppermann Fabian ©
file_name: caeser.py 
"""

msg = input("Satz oder Wort: ")

"""
# Erste Version mit Dictionarys!
alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz "
caeser = "DEFGHIJKLMNOPQRSTUVWXYZABCdefghijklmnopqrstuvwxyzabc "
caDict = {}


for k1, k2 in zip(alphabet, caeser):
    caDict[k1] = k2


def encrypt1(msg):
    Nachricht = ""
    for word in msg:
        Nachricht += caDict[word]
    return Nachricht


def Invert(Dict):
    InvDic = {}
    for k, v in Dict.items():
        InvDic[v] = k
    return InvDic
        
        
def encrypt2():
    Nachricht = ""
    for word in encrypt1(msg):
        Nachricht += Invert(caDict)[word]
    return Nachricht


print(encrypt1(msg))
print(encrypt2())
"""


# Zweite Version mit Ascii Tabelle
def encrypt2(msg, k):
    Nachricht = ""
    for word in msg:
        if word.isupper():
            if ord(word) + 3 >= 91:
                Nachricht += chr((ord(word) + k) - 26)
            elif word == " ":
                Nachricht += " "
            else:
                Nachricht += chr(ord(word) + k)
        else:
            if ord(word) + 3 >= 122:
                Nachricht += chr((ord(word) + k) - 26)
            elif word == " ":
                Nachricht += " "
            else:
                Nachricht += chr(ord(word) + k)           
    return Nachricht
        

def decrypt2(msg, k):
    Nachricht = ""
    for word in msg:
        if word.isupper():
            if ord(word) - k + 26 >= 91:
                Nachricht += chr((ord(word) - k))
            elif word == " ":
                Nachricht += " "
            else:
                Nachricht += chr(ord(word)+ 26 - k)
        else:
            if ord(word) - k >= 97:
                Nachricht += chr(ord(word) - 3)
            elif word == " ":
                Nachricht += " "
            else:
                Nachricht += chr(ord(word) - 3 + 26)
    return Nachricht


print(encrypt2(msg, 3))
print(decrypt2(encrypt2(msg, 3), 3))
