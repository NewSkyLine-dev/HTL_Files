"""
author: Fabian Oppermann
file_name: grading.py
"""
score = int(input("Enter your score: "))

if score <= 50:
    print("Nicht genügend")
elif score <= 62:
    print("Genügend")
elif score <= 78:
    print("Befriedigend")
elif score <= 90:
    print("Gut")
else:
    print("Sehr gut")