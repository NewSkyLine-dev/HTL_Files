# Flugplaner

Ein Qt-basiertes C++ Programm zur Visualisierung und Berechnung optimaler Flugverbindungen auf einer Weltkarte.

## Features

✈️ **Routensuche** - Findet die kürzeste Verbindung zwischen zwei Flughäfen
🗺️ **Weltkarten-Visualisierung** - Zeigt Flugrouten auf einer interaktiven Karte
🎨 **Farbkodierung** - Unterschiedliche Farben für bevorzugte Airlines, Allianzen und andere Flüge
📊 **Detaillierte Ansicht** - Tabellarische Darstellung aller Flugabschnitte
🔍 **Durchsuchbar** - Schnelle Suche nach Flughäfen per IATA-Code oder Name

## Screenshots

Das Programm zeigt:
- Links: Weltkarte mit eingezeichneten Flugrouten
- Rechts: Auswahl der Flughäfen und Details der Route

## Technologie

- **Sprache**: C++17
- **Framework**: Qt 6.9.2
- **Datenbank**: SQLite
- **Build-System**: Visual Studio 2022 mit Qt VS Tools
- **Design Patterns**: Singleton (Databaser), Observer (Qt Signals/Slots)

## Installation

1. Visual Studio 2022 mit C++ installieren
2. Qt 6.9.2 installieren (mit SQL-Modul)
3. Qt VS Tools Extension installieren
4. Projekt öffnen: `Flugplaner.sln`
5. Build und Run (F5)

## Verwendung

1. **Startflughafen wählen**: Wählen Sie den Abflughafen aus der Dropdown-Liste
2. **Zielflughafen wählen**: Wählen Sie den Zielflughafen
3. **Optional: Airline wählen**: Bevorzugen Sie eine bestimmte Fluglinie
4. **Suchen**: Klicken Sie auf "Suchen" für die beste Route

### Farbcode der Routen:
- 🔴 **Rot**: Flüge mit der bevorzugten Fluglinie
- 🔵 **Blau**: Flüge innerhalb der gleichen Allianz
- ⚫ **Grau**: Flüge mit anderen Fluglinien

## Projektstruktur

```
Flugplaner/
├── Flugplaner.h/.cpp       # Hauptfenster und GUI-Logik
├── MapWidget.h/.cpp        # Custom Widget für Kartendarstellung
├── Databaser.h/.cpp        # Datenbank-Schnittstelle
├── Models/                 # Datenmodelle
│   ├── Airline.h
│   ├── Airport.h
│   ├── Alliance.h
│   └── Route.h
├── Utilities/
│   └── FlyGraph.h/.cpp     # Routing-Algorithmus (BFS)
└── resources/
    ├── AirlineRoutes.db    # SQLite Datenbank
    └── Earthmap.jpg        # Weltkartenbild
```

## Algorithmus

Das Programm verwendet einen **Breadth-First Search (BFS)** Algorithmus um:
1. Alle kürzesten Pfade zu finden
2. Multiple Routen mit gleicher Länge zu identifizieren
3. Die beste Route basierend auf Airline-Präferenzen zu wählen

### Priorisierung:
- Bevorzugte Airline: +100 Punkte
- Gleiche Allianz: +50 Punkte
- Andere Airlines: 0 Punkte

## Datenbank Schema

### Airport
- `id` - Eindeutige ID
- `latitude` - Geographische Breite (-90 bis +90)
- `longitude` - Geographische Länge (-180 bis +180)
- `name` - Flughafenname
- `iata` - IATA-Code (z.B. VIE, JFK)

### Airline
- `id` - Eindeutige ID
- `name` - Name der Fluglinie
- `alliance` - Allianz-ID

### Route
- `airline` - Airline-ID
- `airport1` - Start-Flughafen-ID
- `airport2` - Ziel-Flughafen-ID

### Alliance
- `id` - Eindeutige ID
- `name` - Allianzname (z.B. Star Alliance, SkyTeam)

## Memory Management

Das Programm nutzt moderne C++ Best Practices:
- **Singleton Pattern**: Databaser-Klasse für einzige Datenbankverbindung
- **Qt Parent-Child Ownership**: Alle GUI-Widgets werden automatisch vom Parent gelöscht
- **Keine manuellen delete**: Automatische Cleanup durch Qt und Singleton-Pattern
- **Datenbank-Queries**: Liefern Werte, keine Pointer

Details zum Singleton-Pattern: [SINGLETON_PATTERN.md](SINGLETON_PATTERN.md)

## Weitere Dokumentation

- [IMPLEMENTATION.md](IMPLEMENTATION.md) - Detaillierte technische Dokumentation
- [SINGLETON_PATTERN.md](SINGLETON_PATTERN.md) - Design Pattern Dokumentation für Databaser

## Lizenz

Schulprojekt für HTL - Höhere Technische Lehranstalt
