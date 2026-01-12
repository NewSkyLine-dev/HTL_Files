"""
author: Oppermann Fabian
file_name sumdigits.py
"""

def quersumme(n):
    val = 0
    indx = 1
    if n[0] == "-":
        for i in n[1:]:
            val += int(n[indx])
            indx += 1 
        return val * -1
    elif "." in n:
        for i in n:
            if i == ".":
                pass
            else:
                val += int(i)
        return val
    else:
        for i in n:
            val += int(i)
        return val 

print(quersumme(input("Geben Sie eine Zahl ein: ")))