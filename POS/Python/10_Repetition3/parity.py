def add_parity(b):
    parity = 0
    for i in b:
        if i == "1":
            parity += 1
    if parity % 2 == 0:
        b += "0"
    else:
        b += "1"
    return b


def check(b):
    parity = 0
    for i in b:
        if i == "1":
            parity += 1
    if parity % 2 == 0:
        return True
    else:
        return False

print(check(add_parity("01010101")))