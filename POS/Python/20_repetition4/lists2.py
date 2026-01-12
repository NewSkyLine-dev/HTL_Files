def length(a):
    i = 0
    for j in a:
        i += 1
    return i
# print(length("abc")) 

def count_numbers(a):
    count = 0
    for i in a:
        if type(i) == list:
            count += count_numbers(i)
        else:
            count += 1
    return count
# print(count_numbers([1, [2, 3, 4], [5], 6]))

def max_numbers(a):
    max_number = 0
    for i in a:
        if type(i) == list:
            max_number = max(max_number, max_numbers(i))
        else:
            max_number = max(max_number, i)
    return max_number
# print(max_numbers([2, 5, 1]))

def add(a, b): 
    sumList = []
    for i in range(len(a)):
        sumList.append(a[i] + b[i])
    return sumList
# print(add([1, 2, 3], [1, 2, 3]))


def mul(a, b):
    if len(a) > len(b):
        return [a[i] * b[i] for i in range(len(b))]
    else:
        return [a[i] * b[i] for i in range(len(a))]

# print(mul([1, 2, 3, 4, 5], [2] * 3))
# print(mul([1, 2, 3], [2] * 6))

def sub(a, b):
    for i in range(len(a)):
        a[i] = a[i] - b[i]
    return a
# a = [1, 2, 3]
# b = list(range(5))
# print(sub(a, b))
# print(a)
# print(b)

def filter_greater(a, number):
    return [i for i in a if i > number]
# print(filter_greater([5, 2, 3, 8, 4, 1], 3))