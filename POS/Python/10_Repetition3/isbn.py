def isbn1_checknumber(isbn):
    indxLen = (len(isbn) + 1) - len(isbn) 
    val = 0
    for numbers in isbn[:len(isbn) -1]:
        val += indxLen * int(numbers)
        indxLen += 1
    if val % 11 == 1:
        return True
    return False


print(isbn1_checknumber("3897212161"))