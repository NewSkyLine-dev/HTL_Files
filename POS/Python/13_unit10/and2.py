"""
author: Oppermann Fabian
file_name: and2.py
"""

print(f"""
a | b | a and b
--+---+--------""")
for (a, b) in ((False, False), (False, True), (True, False), (True, True)):
    print(int(a), "|", int(b), "|   ", int(a) and int(b))

print("\n################################################")

