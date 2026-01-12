"""
author: Oppermann Fabian
file_name power8.py
"""

def convert(n, expo=0, nL=[]):
    if "8" in n or "9" in n:
        print("Error")
    else:
        while int(n) >= 8**expo:
            expo += 1
        expo -= 1
        for i in range(expo + 1):
            nL.append(int(n)//8**expo)
            n = int(n) % 8**expo
            expo -= 1
        return ''.join([str(elem) for elem in nL])

  
print(convert(input("Gebn Sie eine Zahl ein: ")))