"""
author: Oppermann Fabian
file_name: logicalArith.py
"""


def is_subset(a, b): 
    if {x for x in b if x in a}:
        return True
    else:
        return False


def union(a, b):
    # return a + [x for x in b if x not in a] |  Währe die Möglichkeit auch OK? 
    c = set()
    for e in a:
        c.add(e)
    for e in b:
        c.add(e)
    return c


def intersect(a, b):
    return {x for x in a if x in b}


def diff_set(a, b):
    return {x for x in a if x not in b}


# print(is_subset({1, 2, 5}, {1, 2, 3, 4, 5}))
# print(union({1, 2, 3}, {1, 2, 3, 4}))
# print(intersect({1, 2, 3}, {2, 3, 4}))
# print(diff_set({1, 2, 3, 4}, {2, 3, 5}))