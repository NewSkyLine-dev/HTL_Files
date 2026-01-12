# 1. CMake-Projekt

Starten Sie VSCodium und installieren Sie falls notwendig die beiden Erweiterungen. Erstellen Sie im Ordner /abgabe einen Ordner PA2_Nachname_Vorname. Erstellen Sie ein CMake-Proiekt darin mit den Dateien main.cpp, pa.h und pa.cpp. Stellen Sie mit #ifndef sicher, dass die Header-Datei nicht doppelt geladen werden kann. Kopieren Sie die Datei angabedaten.h ins Projekt.

# 2. Strukt

Erstellen Sie in der Datei pa.h ein Struct Entry das dem UML-Klassendiagramm auf der Rückseite entspricht (Reihenfolge der Variablen einhalten).

# 3. Klasse

Erstellen Sie die Definition der Klasse Names (in der pa.h
Datei) entsprechend dem UML-Klassendiagramm (unter dem Strukt)
Implementieren Sie den Konstruktor in der Datei pa.cpp. Die bool-Variable wird im Konstruktor nur gespeichert. ANGABEANZAHL und ANGABEDATEN sind in der Datei angabedaten.h definiert und können nach dem Inkludieren der Datei im Code verwendet werden. (siehe Rückseite)

# 4. Anfangsbuchstaben

Implementieren Sie die statische Funktion stringStartsWith die prüft, ob der String mit dem zweiten übergebenen String startet.

# 5. Namen ausgeben

Implementieren Sie die Funktion printNames der Klasse in der pa.cpp-Datei. Die Funktion gibt die Daten des Namens auf die Konsole aus, wenn der Name vorhanden ist. Wenn die bool-Variable useBegin true ist, werden auch Namen ausgegeben, die mit dem Suchbegriff beginnen (Verwenden Sie die Funktion aus
Aufgabe 4)
Ist kein Name vorhanden werfen Sie einen String als Exception.

# 6. Benutzereingabe

In der Main-Funktion (main.cpp) soll der Benutzer zuerst wählen, ob er auch Namen suchen will, die mit dem Suchbegriff starten oder nur nach vollständigen Namen (für den Konstruktor). Danach soll der Benutzer so lange nach Namen suchen können, bis kein Name gefunden wird (mit der Funktion aus Aufgabe 5).
Die Exception soll abgefangen und nicht ausgegeben werden.

# 7. Sortiert ausgeben

Implementieren Sie die Funktion printNamesSorted der Klasse in der pa.cpp-Datei.
Die Funktion soll die Namen wie in Aufgabe 5 ausgeben, jedoch nach Häufigkeit sortiert. Speichern Sie dazu ab, welche Namen bereits ausgegeben wurden. Suchen
Sie dann den häufigsten noch nicht ausgegeben Namen und geben Sie ihn aus.
Wiederholen Sie das so lange bis kein noch nicht ausgegeben Name mehr gefunden wird. Alternative Lösungen sind ebenfalls zulässig. Verwenden Sie dann diese Funktion in der main-Funktion statt der Funktion aus Aufgabe 5.
