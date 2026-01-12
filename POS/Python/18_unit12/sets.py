def union(a, b):
    c = set()
    for i in a:
        c.add(i)
    for i in b:
        c.add(i)
    return c


def minus(a, b):
    for i in b:
        a.discard(i)
    return a


def concat(a, b):
    for i in b:
        a.append(i)
    return a


def insert(a, b, i):
    a.insert(i, b[0])
    for j in range(1, len(b)):
        a.insert(i + j, b[j])
    return a


def sort(a):
    a.sort()
    return a


def sort_descending(a):
    return sorted(a, reverse=True)


def minimum(seq):
    min = seq[0]
    for i in seq:
        if i < min:
            min = i
    return min


def maximum(seq):
    max_y = seq[0][1]
    max_x = seq[0][0]
    for x, y in seq:
        if y > max_y:
            max_y = y
            max_x = x
    return max_x, max_y


def minimum(liste):
    if len(liste) == 0:
        return "Es wurden keine Zahlen eingegeben!"
    else:
        min = liste[0]
        for i in liste:
            if i < min:
                min = i
        return min


def num9():
    liste = []
    while True:
        try:
            eingabe = int(input("Bitte geben Sie eine Zahl ein: "))
            liste.append(eingabe)
        except ValueError:
            print("Bitte geben Sie eine Zahl ein!")
        except EOFError:
            break
    print
    (minimum(liste))

def find(seq, num):
    for i in range(len(seq)):
        if seq[i] == num:
            return i
    return -1