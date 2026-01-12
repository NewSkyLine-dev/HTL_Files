# 2. CMake-Project
Vervollständigen Sie die Funktionen ```int random_card_value()``` in der Datei **pa.cpp** so, dass eine Zufallszahl zwischen 1 und 13 zuruück gegeben wird. Fügen Sie in der main Funktion den notwendigen Code hinzu, damit die Zufallszahl korrekt initializiert werden. 

# 3. Karten Ausgeben 
Vervollstndigen Sie die Funktion `void print_card(int card)`. 
Diese Funktion bekommt einen Kartwert von 1 bis 13 und soll die Karte auf cout ausgeben. Ein Wert 1 soll A ausgeben, 2-10 werden als Zahlen ausgegeben, 11 als J, 12 als Q und 13 als K. 

# 4. Benutzereingabe
Erzeugen Sie mit der Funktion aus Aufgabe 2 einen Kartenwert in der main-Funktion und geben Sie ihm mit der Funktion aus Aufgabe 3 aus. Fragen Sie den Benutzer ob der Nächste Kartenwert größer, kleiner oder gleich sein wird. Wiederholen Sie die Ausgabe und Abfrage so lange, wie der Benutzer richtig rät. Zählen Sie die Anzahl der richtigen Versuche mit, 

# 5. Kartenfarben
Die Funktion ```void print_card(Card &card)``` soll die Karte im Strukt mit dem Symbol und Kartenwert ausgeben. Herz- und Karo-Karten sollen in Rot und Pik- und Kreuz-Karten in Blau ausgegeben werden. 

# 6. Konstruktor 
Erstellen Sie einen Konstruktor für das Strukt Card. Der Konstruktor soll keinen Parameter haben und die Farben und den Wert zufällig bestimmen. 

# 7. Wertvollste Karte
In der Funktion ```void most_valuable_card(Card cards[], intnumber_cards)``` soll die wertvollste Karte des Arrays ausgegeben werden. Eine Zwei ist die niedrigste Karte. Ein Ass ist mehr wert als ein König. 