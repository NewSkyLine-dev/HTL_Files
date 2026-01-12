"""
autor: Oppermann Fabian
file_name: game.py
"""


import random as ra

userPoints = {}

def main():
    punkteMensch = 0
    PunkteComputer = 0
    userName = input("Geben Sie ihren Namen ein: ")
    while len(userName) > 5:
        print("Dein Nutzername darf maximal 5 buchstaben haben")
        userName = input("Geben Sie ihren Namen ein: ")
    maxPoints = int(input("Willkommen zu Stein, Papier Schere\n\nWiviele Punkte sind zu sieg Nötig"))

    while punkteMensch < maxPoints or PunkteComputer < maxPoints:
        # Begin der Logik von Schere Stein und Papier
        chooseComputer = ra.randint(1, 3)
        print(chooseComputer)
        try:
            chooseMensch = int(input("Wähle:  (1)Stein   (2)Papier   (3)Schere\t"))
        except Exception:
            chooseMensch = int(input("Wähle:  (1)Stein   (2)Papier   (3)Schere\t"))

        # Überprüft ob Auswhl über 3 ist
        if chooseMensch > 3:
            print("Du kannst nur einer dieser 3 möglichkeiten Wählen")
            try:
                chooseMensch = int(input("Wähle:  (1)Stein   (2)Papier   (3)Schere\t"))
            except Exception:
                chooseMensch = int(input("Wähle:  (1)Stein   (2)Papier   (3)Schere\t"))

        if chooseMensch == 1 and chooseComputer == 2:
            print(f"{userName}: Stein   Computer: Papier    => Computer gewinnt!")
            PunkteComputer += 1
        
        elif chooseMensch == 1 and chooseComputer == 3:
            print(f"{userName}: Stein    Computer: Schere    => Mensch gewinnt!")
            punkteMensch += 1

        elif chooseMensch == 2 and chooseComputer == 1:
            print(f"{userName}: Papier    Computer: Stein    => Mensch gewinnt!")
            punkteMensch += 1

        elif chooseMensch == 2 and chooseComputer == 3:
            print(f"{userName}: Papier    Computer: Schere    => Computer gewinnt!")
            PunkteComputer += 1

        elif chooseMensch == 3 and chooseComputer == 1:
            print(f"{userName}: Schere    Computer: Stein    => Computer gewinnt!")
            PunkteComputer += 1

        elif chooseMensch == 3 and chooseComputer == 2:
            print(f"{userName}: Schere    Computer: Papier    => Mensch Gewinnt!")
            punkteMensch += 1

        elif chooseMensch == chooseComputer:
            print("Unentschieden!")

        print(f"Punkte: {userName} {punkteMensch}    Computer {PunkteComputer}")

        if punkteMensch == maxPoints:
            print(f"{userName} hat gewonnen!")
            userPoints[userName] = punkteMensch
            break

        elif PunkteComputer == maxPoints:
            print("Computer hat gewonnen!")
            userPoints[userName] = punkteMensch
            break

    again = input("Wollen Sie nochmal Spielen? [j/n]")
    if again == "j":
        main()
    else:
        sortedDict = {k: v for k, v in sorted(userPoints.items(), key=lambda x: x[1], reverse=True)}
        for key, value in sortedDict.items():
            print(f"{key}:  {value}")

    
    # code für anzeigen der username

if __name__ == '__main__':
    main()