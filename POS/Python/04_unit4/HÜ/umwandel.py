def convertbinary(number, power):
    number2 = number
    for i in range(power + 1):
        result = number // 2**power
        rest = number % 2**power
        print(number, "/ 2^", power, "=", result, "R=", rest)
        number = rest
        power -= 1
    number3 = bin(number2)
    print(number3[2:])
    print("_____________________________________")


convertbinary(21, 4)
convertbinary(22, 4)
convertbinary(25, 4)
convertbinary(43, 5)
convertbinary(68, 6)
convertbinary(94, 6)