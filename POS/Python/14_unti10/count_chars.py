"""
author: Oppermann Fabian
file_name: count_chars.py
"""


def count_chars(s, c):
    len = 0
    for i in s:
        if c in i:
            len += 1
    return len


print(count_chars("abcabcabc" ,"a"))