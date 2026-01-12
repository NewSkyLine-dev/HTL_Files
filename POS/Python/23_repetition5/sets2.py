"""
author: Oppermann Fabian
file_name: sets2.py
"""


def cross_product(a, b):
    return {(x, y) for x in a for y in b}


# print(cross_product({1, 2, 3}, {4, 5}))


def output_mul_table(n):
    for i in range(1, n+1):
        for j in range(1, n+1):
            print(i*j, end=" ")
        print()


# output_mul_table(8)


def in_set(e, a): # e = list | a = list
    indxE = 0
    indxA = 0
    for i in range(len(e)):
        if e[indxE] == a[indxA]:
            return True
        indxA += 1
        
        
# print(in_set([5, 1, 22, 25], [1, 6, 5, -1, 10]))


def reverse_list(lst):
    print(",".join(str(x) for x in lst[::-1]))
    
    
# reverse_list([1, "a", 2.5])


def to_str(lst):
    return str(",".join(str(x) for x in lst))


# print(to_str([1, "a", 2.5]))


def reverse_number(n):
    return int(str(n)[::-1])


# print(reverse_number(123))


def is_palindrome(n):
    if n == reverse_number(n):
        return True
    else:
        return False
    
    
# print(is_palindrome(12321))


def add(lst):
    res = 0
    for i in lst:
        for j in i:
            res += j
    return res


# print(add([[1, 2, 3], [4, 5, 6], [7, 8, 9]]))


def minmax(lst):
    min = lst[0][0]
    max = lst[0][0]
    for i in range(len(lst)):
        for j in range(len(lst[i])):
            if lst[i][j] < min:
                min = lst[i][j]
            if lst[i][j] > max:
                max = lst[i][j]
    return (min, max)


# print(minmax([[4, 5, 6], [7, 8, 9], [1, 2, 3]]))


def change_case(s):
    return s.swapcase()


# print(change_case("aABb1 _Cc"))
