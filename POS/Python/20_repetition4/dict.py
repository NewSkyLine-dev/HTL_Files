def sum_values(d):
    return sum(d.values())
# print(sum_values({"Hallo": 4, "Test": 5, "Noch": 6}))


def mul_values(d):
    result = 1
    for key in d.keys():
        result *= d[key]
    return result
# print(mul_values({1: 2, 3: 4, 5: 6}))


def sub_values(d):
    result = 0
    for key, value in d.items():
        result -= value
    return result
# print(sub_values({"a": 1, "b": 2, "c": 3}))


def sort_values(d):
    return sorted(d.values())

# print(sort_values({'a':1, 'b':2, 'c':3}))


def sort_values2(d):
    return sorted(d.values(), key=lambda x: x[1])
# print(sort_values2({
#     "a": (1, 2),
#     "b": (3, 1),
#     "c": (2, 3)
# }))