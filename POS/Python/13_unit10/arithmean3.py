"""
author: Oppermann Fabian
file_name: arithmean3.py
"""

summ = 0
count = 0

while True:
    s = input("Enter a number: ")
    if s == "q":
        break
    sum += float(s)
    count += 1

print("The arithmetical mean is", summ / count)
