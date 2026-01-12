def main():
    """
    Hauptprogramm
    """
    # Schüler einlesen
    schueler = {}
    print("Bitte die Schüler eingeben:")
    while True:
        try:
            name = input("Schüler: ")
            if name in schueler:
                print("Schüler bereits vorhanden!")
            else:
                schueler[name] = {}
        except EOFError:
            break

    # Gegenstände und Punkte einlesen
    gegenstaende = {}
    print("Bitte die Gegenstände und die jeweiligen Punkte eingeben:")
    while True:
        try:
            gegenstand = input("Gegenstand: ")
            if gegenstand in gegenstaende:
                print("Gegenstand bereits vorhanden!")
            else:
                gegenstaende[gegenstand] = {}
                for name in schueler:
                    try:
                        punkte = int(input("Schüler {}: ".format(name)))
                        gegenstaende[gegenstand][name] = punkte
                    except ValueError:
                        print("Keine gültige Punktezahl!")
        except EOFError:
            break


    # Auswertungen
    print("Auswertungen")
    print("============")
    for gegenstand in gegenstaende:
        print("Gegenstand {}".format(gegenstand))
        print("-" * len(gegenstand))
        # Rangliste erstellen
        rangliste = []
        for name in schueler:
            punkte = gegenstaende[gegenstand][name]
            rangliste.append((name, punkte))
        rangliste.sort(key=lambda x: x[1], reverse=True)
        # Rangliste ausgeben
        for i, (name, punkte) in enumerate(rangliste):
            rang = i + 1
            note = noten(punkte)
            print(f"Schüler: {name}      Rang: {rang}        Punkte: {punkte}      Note:{note}")
        # Schnitt berechnen
        schnitt = sum(punkte for _, punkte in rangliste) / len(rangliste)
        print("{} Schüler Schnitt: {:.1f}".format(len(rangliste), schnitt))

    # Reihung der Gegenstände basierend auf den Schnitt der Punkte
    print("Reihung der Gegenstände basierend auf den Schnitt der Punkte")
    print("============================================================")
    schnitte = []
    for gegenstand in gegenstaende:
        rangliste = []
        for name in schueler:
            punkte = gegenstaende[gegenstand][name]
            rangliste.append((name, punkte))
        rangliste.sort(key=lambda x: x[1], reverse=True)
        schnitt = sum(punkte for _, punkte in rangliste) / len(rangliste)
        schnitte.append((gegenstand, schnitt))
    schnitte.sort(key=lambda x: x[1], reverse=True)
    for i, (gegenstand, schnitt) in enumerate(schnitte):
        print("{}. {}: {:.1f}".format(i + 1, gegenstand, schnitt))


def noten(punkte):
    """
    Berechnet die Note auselönd der Punkte
    """
    if punkte >= 90:
        return 1
    elif punkte >= 80:
        return 2
    elif punkte >= 70:
        return 3
    elif punkte >= 60:
        return 4
    else:
        return 5


if __name__ == "__main__":
    main()