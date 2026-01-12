# MyDate
## Aufgabe
In der Klasse ```MyDate```, wird eine Datum gespeichert. Erstellen Sie ein neues CMake-Projekt für die Übung. Die Klasse soll in den beiden Dateien ```mydate.h``` und ```mydate.cpp``` ausprogrammiert werden. Stellen Sie mit ```#ifndef``` sicher, dass die Header-Datei auch ohne Fehler mehrfach inkludiert werden kann.

In der Klasse werden keine Daten vom Benutzer eingelesen oder auf die Konsole ausgegeben.

## Variablen:
```cpp
year (int): Das Jahr.
month (int): Das Monat von 1 (Jan) bis 12 (Dec).
day (int): Der Tag von 1 bis 28|29|30|31, wobei der letzte Tag vom Monat und ob das Jahr ein Schaltjahr ist abhängt Feb (28|29).
str_months (std::string[]): Die Monatsnamen
str_days (std::string[]): Die Wochentagsnamen
day_in_months (int[]): Die Anzahl der Tage in allen 12 Monaten
Konstruktor:
MyDate(int year, int month, int day): Legt ein neues Datum mit Jahr, Monat und Tag fest
Funktionen:
is_leap_year(int year): Prüft ob das Jahr ein Schaltjahr ist. Ein Schaltjahr ist durch 4 teilbar aber nicht durch 100, oder es ist durch 400 teilbar.
is_valid_date(int year, int month, int day): Prüft ob es ein gültiges Datum ist.
get_day_of_week(int year, int month, int day): Ermittelt den Wochentag zu einem Datum. (Wochentagsberechnung)
set_date(int year, int month, int day): Setzt Jahr, Monat und Tag.
set_year(int year): Setzt das Jahr.
set_month(int month): Setzt den Monat.
set_day(int day): Setzt den Tag.
get_year(): Gibt das Jahr zurück.
get_month(): Gibt den Monat zurück.
get_day(): Gibt den Tag zurück.
to_string(): String im Format "xxxday d mmm yyyy", z.B., "Tuesday 14 Feb 2012". (Zahlen in string umwandeln)
next_day(): Auf den nächsten Tag weiterschalten (31 Dec 2000 wird zu 1 Jan 2001).
next_month(): Auf den nächsten Monat weiterschalten (31 Oct 2012 wird zu 30 Nov 2012).
next_year(): Auf das nächste Jahr weiterschalten (29 Feb 2012 wird zu 28 Feb 2013).
previous_day(): Einen Tag zurück wechseln.
previous_month(): Ein Monat zurück wechseln.
previous_year(): Ein Jahr zurück wechseln.
```

## Fehlerbehandlung
Wenn bei einer Funktion Werte übergeben werden die kein gültiges Datum ergeben (außer bei is_valid_date) soll eine Fehlermeldung mit einer Exception ausgelöst werden. Der Code dazu ist sehr einfach und gibt eine kurze Fehlermeldung mit, die dann auf der Konsole erscheint wenn der Fehler ausgelöst wurde. In dem Beispiel wird z.B. der Fehler, dass die Division durch 0 nicht möglich ist mit einer Exception angezeigt.
```cpp
double division(int a, int b) {
   if( b == 0 ) {
      throw "Division by zero condition!";
   }
   return (a/b);
}
```
## Testen
Alle Funktionen der Klasse sollen in der main-Funktion getestet werden. Es sollen auch die selteneren Fälle getestet werden. Ist z.B. beim 31.12.1999 der nächste Tag korrekt der 1.1.2000?

In der main-Funktion können auch Daten von cin eingelesen werden und es sollen die verschiedenen Ergebnisse der Funktionen auf die Konsole (cout) ausgegeben werden um zu Testen ob diese korrekt funktionieren.
Zielsetzung
Klassen
Klassen-Funktionen
UML-Klassendiagramm
Erweiterung
Die Klasse soll von MyDate in MyDateTime kopiert werden (neue .h und .cpp Datei, neuer Name) und zusätzlich zum Datum auch noch die Uhrzeit (Stunde, Minute) speichern.
Für die Uhrzeit sollen zusätzliche Funktionen is_valid_time, set_time, set_hour, set_minute, get_hour, get_minute, next_hour, next_minute, previous_hour und previous_minute zur Verfügung stehen. Die Funktionalität enspricht der vorhandenen Funktionen jedoch für die Uhrzeit statt für das Datum.
Die Funktionen add_years, add_months, add_days, add_hours und add_minutes sollen jeweils eine positive oder negative Integer-Zahl als Paramter akzeptieren und die entsprechende Anzahl an Jahren,... zum Datum / Uhrzeit dazu zählen bzw. abziehen.