"""
author: Oppermann Fabian
file_name: union.py
"""

def union(a, b):
    return a + [x for x in b if x not in a]


def intersection(a, b):
    return [x for x in a if x in b]


def diff_set(a, b):
    return {x for x in a if x not in b}


# print(diff_set({1, 2, 3, 4}, {2, 3, 5}))
# print(intersection([1, 2, 3], [2, 3, 4]))
# print(union([1, 2, 3], [2, 3, 4]))
