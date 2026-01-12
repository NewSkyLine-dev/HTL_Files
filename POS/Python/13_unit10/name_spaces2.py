"""
author: Oppermann Fabian
file_name: name_spaces2.py
"""

# name = input("Gib einen Namen ein: ")


s = 'Fabian Oppermann'
def name_spaces(s):
    for x, s2 in enumerate(s):
        if x % 2 == 0:
            print(s2, end="")
        else:
            print(s2, end=" ")
        
name_spaces(s)
