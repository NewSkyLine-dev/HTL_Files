def gruss():
    meinName = str(input("Geben Sie Ihren Name ein: "))
    Ort = str(input("Geben Sie einen Ort ein: "))
    tätigkeit = str(input("Geben Sie eine Tätigkeit ein: "))
    gefallen = str(input("Geben Sie ein wie Ihnen dieser Ort gefällt: "))
    gefallen2 = str(input("Geben Sie ein wie Ihnen dieser Ort gefällt: "))
    andereName = str(input("Geben sie einen andere Name ein: "))

    print("Hallo {}!\nHier in {} gefällt es mir {}.\nGestern waren wir {}. Das war {}!\nBis bald,\n{}".format(meinName, Ort, gefallen, tätigkeit, gefallen2 ,andereName))


gruss()