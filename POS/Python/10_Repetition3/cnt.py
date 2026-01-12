def cnt_ones(b):
    i = 0
    for ones in b:
        if ones == "1":
            i += 1
    print(i)


cnt_ones("1010100")