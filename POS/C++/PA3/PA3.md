# PA3

## 1. UML-Klassendiagramm

Entpacken Sie das Angabeprojekt nach / abgabe und öffnen Sie es. Erstellen Sie die Klasse Letters entsprechend dem UML-Klassendiagramm auf der Rückseite in den .h / .cpp Dateien. Die Funktionen leer lassen bzw. nur ein return einfügen (damit ohne Fehler kompiliert wird).

## 2. Rekursion

In der statischen Funktion only _letters soll rekursiv überprüft werden, ob in dem Wort nur die Buchstaben A-Z bzw. a-z vorkommen. Für die Überprüfung, ob ein einzelner Charakter ein Buchstabe ist, soll die Funktion isalpha verwendet werden. Ein leerer String enthält nur Buchstaben (gibt true zurück).

## 3. Buchstaben zählen

Die Funktion count_letters der Klasse soll für jeden Buchstaben in dem übergebenen String den passenden Wert im Array erhöhen. Den Index für das Array erhält man, indem man von Großbuchstaben ‚A' abzieht und von Kleinbuchstaben ,a'. Ergibt Index 0 für A oder a bis Index 25 für Z oder z. Mit den Funktionen toupper bzw. tolower kann zwischen Groß- und Kleinbuchstaben gewechselt werden.

## 4. String-Verarbeitung

In der main-Funktion sollen vom Benutzer so lange Wörter abgefragt werden, bis er „Ende" eingibt. Das Wort soll mit der Funktion aus Aufgabe 2 überprüft werden, ob es nur Buchstaben sind. Enthält das Wort ungültige Elemente wird eine Fehlermeldung ausgegeben und das Wort nicht weiterverarbeitet. Für gültige Wörter wird die Funktion count_letters der Buchstaben- Klasse aufgerufen.

## 5. Liste ausgeben

Nachdem der Benutzer in der main-Funktion Ende eingegeben hat, soll das Ergebnis mit der print Funktion der Klasse ausgegeben werden. Geben Sie in der Funktion für jeden Buchstaben den Buchstaben selbst und die Anzahl aus. Siehe Beispielausgabe.

## 6. Rekursive Summe

Die Funktion sum soll rekursiv die Summe aller Buchstaben zurückgegeben. Rufen Sie die Funktion in der Main-Funktion nach der Ausgabe der Buchstaben auf, um die Summe aller Buchstaben auszugeben.

## 7. Rekursives Ausgeben

Die Funktion print_recursiv soll die Buchstaben wie in Aufgabe 5 ausgeben, jedoch soll keine Schleife verwendet werden, sondern die Funktion soll rekursiv sein.

```cxx
+-----------------------------------------------+
|                    Letters                    |
+-----------------------------------------------+
| - letters: int[26]                            |
+-----------------------------------------------+
| + only_letters(word: std::string): bool static|
| + count_letters(word: std::string): void      |
| + print(): void                               |
| + sum(index: int): int                        |
| + print_recursiv(letter: char): void          |
+-----------------------------------------------+
```
