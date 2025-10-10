# Flugplaner - Implementierungsdokumentation

## Übersicht
Das Flugplaner-Programm ist eine Qt-basierte C++ Anwendung, die Flugverbindungen zwischen Flughäfen auf einer Weltkarte visualisiert und die optimale Route mit den wenigsten Zwischenstopps findet.

## Design Patterns

### Singleton Pattern (Databaser)
Die Datenbankklasse verwendet das Singleton-Pattern:
- **Einzige Instanz**: Nur eine Datenbankverbindung während der Programmausführung
- **Globaler Zugriff**: `Databaser::getInstance()` gibt Referenz zur Instanz zurück
- **Thread-safe**: Meyer's Singleton mit C++11 statischer lokaler Variable
- **Automatische Cleanup**: Destruktor wird beim Programmende aufgerufen
- Details siehe [SINGLETON_PATTERN.md](SINGLETON_PATTERN.md)

## Implementierte Funktionen

### 1. Datenbank-Integration (Singleton Pattern)
- **Databaser-Klasse**: Singleton-Pattern für einzige Datenbankverbindung
- `getInstance()`: Liefert Referenz zur einzigen Instanz
- Unterstützt generische Query-Methoden mit Template-Funktionen
- Lädt automatisch Daten für Airlines, Airports, Alliances und Routes

### 2. Model-Klassen (im Ordner `Models/`)

#### Airline.h
- Eigenschaften: id, name, alliance
- Konstruktor für QSqlQuery zur direkten Datenbankabfrage

#### Airport.h
- Eigenschaften: id, latitude (float), longitude (float), name, iata
- Koordinaten als float für präzise Berechnungen

#### Alliance.h
- Eigenschaften: id, name
- Repräsentiert Airline-Allianzen

#### Route.h
- Eigenschaften: airline, airport1, airport2
- Definiert Flugverbindungen zwischen zwei Flughäfen

### 3. Graph-Algorithmus (FlyGraph.h/.cpp)

#### Kernfunktionalität:
- **BuildGraph()**: Lädt alle Flughäfen, Routen und Airlines aus der Datenbank
- **FindShortestPath()**: Implementiert BFS-Algorithmus (Breadth-First Search)
  - Findet alle kürzesten Pfade zwischen zwei Flughäfen
  - Priorisiert Routen nach:
    1. Bevorzugte Fluglinie (höchste Priorität) - 100 Punkte
    2. Allianz der bevorzugten Fluglinie - 50 Punkte
    3. Andere Fluglinien - 0 Punkte
  - Unterstützt mehrere gleichwertige Routen und wählt die beste basierend auf Präferenzen

#### Datenstrukturen:
- **FlightSegment**: Einzelner Flugabschnitt (von-nach-Airline)
- **FlightPath**: Komplette Route mit mehreren Segmenten

### 4. Kartendarstellung (MapWidget.h/.cpp)

#### Custom QWidget für Visualisierung:
- **paintEvent()**: Zeichnet Weltkarte mit Flugrouten
- **latLongToPixel()**: Konvertiert geografische Koordinaten zu Pixelpositionen
  - Latitude: +90° (oben) bis -90° (unten)
  - Longitude: -180° (links) bis +180° (rechts)
  - Mittelpunkt: Äquator und Nullmeridian

#### Farbkodierung:
- **Rot**: Flüge der bevorzugten Fluglinie
- **Blau**: Flüge der Allianz
- **Grau**: Andere Fluglinien
- **Schwarz**: IATA-Codes der Flughäfen

### 5. Benutzeroberfläche (Flugplaner.h/.cpp)

#### GUI-Elemente:
1. **MapWidget**: Zeigt Weltkarte mit Flugrouten (linke Seite)
2. **ComboBox "Von"**: Auswahl Startflughafen (durchsuchbar)
3. **ComboBox "Nach"**: Auswahl Zielflughafen (durchsuchbar)
4. **ComboBox "Fluglinie"**: Optional - Bevorzugte Airline
5. **Suchen-Button**: Startet Routensuche
6. **Tabelle**: Zeigt Flugdetails (Von, Nach, Fluglinie, IATA-Codes)

#### Funktionsweise:
- Flughäfen werden beim Start aus Datenbank geladen
- Durchsuchbare ComboBoxen mit Format: "IATA - Name (ID)"
- Validierung der Eingaben vor der Suche
- Automatische Aktualisierung von Karte und Tabelle nach erfolgreicher Suche

## Memory Management

### Beachtete Punkte:
1. **Singleton Pattern (Databaser)**: 
   - Einzige Instanz wird automatisch beim Programmende zerstört
   - Keine manuellen delete-Aufrufe nötig
   - Meyer's Singleton: `static Databaser instance` in `getInstance()`

2. **Qt Parent-Child System**: Alle Widgets werden mit Parent erstellt
   - Qt löscht automatisch Child-Widgets beim Zerstören des Parents
   - Beispiel: `new QComboBox(this)` - wird automatisch aufgeräumt

3. **Temporäre Objekte**: 
   - QPixmap für Bildanzeige wird nach Gebrauch gelöscht
   - Datenbankabfragen liefern Kopien, keine Pointer

4. **Smart Pointers**: Nicht erforderlich, da Qt's Ownership-System und Singleton-Pattern verwendet werden

## Kompilierung

### Projekt-Konfiguration:
- **Qt-Module**: core, gui, widgets, sql
- **Neue Dateien**:
  - MapWidget.h / MapWidget.cpp (als QtMoc compiliert)
  - FlyGraph.h / FlyGraph.cpp
  - Aktualisierte Model-Klassen

### Build-Prozess:
1. Projekt in Visual Studio öffnen
2. Debug oder Release Configuration wählen
3. Build starten (F7)
4. Resources werden automatisch ins Output-Verzeichnis kopiert

## Verwendung

1. **Programm starten**: Datenbank wird automatisch verbunden
2. **Startflughafen wählen**: In "Von"-ComboBox
3. **Zielflughafen wählen**: In "Nach"-ComboBox
4. **Optional Airline wählen**: Für Präferenz bei Routenauswahl
5. **"Suchen" klicken**: Route wird berechnet und angezeigt
6. **Ergebnis**: 
   - Farbige Linien auf Weltkarte
   - IATA-Codes an Flughäfen
   - Detaillierte Fluginformationen in Tabelle

## Technische Details

### Koordinatenumrechnung:
```cpp
// Latitude: +90 (top) to -90 (bottom)
y = (90 - latitude) / 180 * height

// Longitude: -180 (left) to +180 (right)
x = (longitude + 180) / 360 * width
```

### BFS-Algorithmus:
- Verwendet QQueue für Breitensuche
- QMap für Distanzen und Vorgänger
- Rekursiver Pfadaufbau mit Scoring-Funktion

### Datenbank-Queries:
- Singleton-basierte Methoden in Databaser-Klasse
- `Databaser::getInstance()` für Zugriff auf Instanz
- Template-basierte Query-Methoden
- Konstruktoren in Model-Klassen nehmen QSqlQuery entgegen
- Automatische Typkonvertierung (toInt(), toFloat(), toString())

## Mögliche Erweiterungen

1. **Suchfunktion**: Filtern der ComboBoxen nach IATA oder Name
2. **Mehrere Routen**: Anzeige alternativer Verbindungen
3. **Flugdauer**: Berechnung basierend auf Distanz
4. **Interaktivität**: Klick auf Flughafen für Informationen
5. **Exportfunktion**: Speichern der Route als PDF/Bild

## Fehlerbehebung

### Häufige Probleme:
1. **Datenbank nicht gefunden**: Prüfen Sie den Pfad in main.cpp
2. **Keine Route gefunden**: Möglicherweise keine Verbindung in DB
3. **Karte wird nicht angezeigt**: Earthmap.jpg muss in resources/ liegen
4. **Kompilierungsfehler**: Qt-Module (sql) in Projektkonfiguration prüfen

## Autoren und Lizenz

Erstellt als Schulprojekt für HTL POS (Programmieren und Softwareentwicklung).
