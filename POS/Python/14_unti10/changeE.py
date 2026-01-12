"""
author: Oppermann Fabian
file_name: changeE.py
"""


n = input("Wort: ")

def changeE(s):
    result = ""
    for c in s:
        if c == "e":
            result += "E"
        else:
            result += c
    return result

print(changeE(n))