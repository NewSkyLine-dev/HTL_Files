# Singleton Pattern Implementation - Databaser Class

## Übersicht

Die `Databaser`-Klasse wurde nach dem **Singleton Design Pattern** implementiert. Dies stellt sicher, dass nur eine einzige Datenbankverbindung während der gesamten Laufzeit der Anwendung existiert.

## Warum Singleton?

### Vorteile:
1. **Globaler Zugriffspunkt**: Alle Teile der Anwendung können auf dieselbe Datenbankinstanz zugreifen
2. **Ressourcenschonung**: Nur eine Datenbankverbindung wird geöffnet
3. **Thread-Safety**: Seit C++11 ist die statische lokale Variable thread-safe
4. **Automatische Cleanup**: Der Destruktor wird beim Programmende automatisch aufgerufen
5. **Keine globalen Variablen**: Vermeidet Probleme mit Initialisierungsreihenfolge

### Design-Entscheidungen:
- **Meyer's Singleton**: Verwendet statische lokale Variable in `getInstance()`
- **Privater Konstruktor**: Verhindert direkte Instanzierung
- **Gelöschter Copy-Konstruktor**: Verhindert Kopieren der Instanz
- **Gelöschter Assignment-Operator**: Verhindert Zuweisung

## Implementation

### Header (Databaser.h)

```cpp
class Databaser
{
private:
    // Private constructor - only accessible via getInstance()
    Databaser();
    ~Databaser();
    
    // Delete copy constructor and assignment operator
    Databaser(const Databaser&) = delete;
    Databaser& operator=(const Databaser&) = delete;

    QSqlDatabase db;
    bool connected;

public:
    // Singleton instance getter
    static Databaser& getInstance();

    bool connect(const QString& path);
    void disconnect();
    bool isConnected() const;

    template <typename T>
    T runQuery(const QString& query, const T& defaultValue = T());

    template <typename T>
    QList<T> runQueryList(const QString& query);
};
```

### Source (Databaser.cpp)

```cpp
Databaser::Databaser() : connected(false)
{
    // Private constructor - only accessible via getInstance()
}

Databaser& Databaser::getInstance()
{
    static Databaser instance; // Created once, destroyed at program end
    return instance;
}
```

## Verwendung

### In main.cpp

```cpp
int main(int argc, char *argv[])
{
    QApplication app(argc, argv);
    
    // Get singleton instance
    Databaser& db = Databaser::getInstance();
    
    // Connect to database
    if (!db.connect(dbPath)) {
        return -1;
    }
    
    // ... application code ...
    
    // Cleanup (optional - happens automatically)
    db.disconnect();
    
    return app.exec();
}
```

### In anderen Klassen

```cpp
void FlyGraph::BuildGraph()
{
    // Get singleton instance
    Databaser& db = Databaser::getInstance();
    
    // Use database
    QList<Airport> airports = db.runQueryList<Airport>("SELECT * FROM Airport;");
    
    // No cleanup needed - singleton manages its own lifetime
}
```

## Änderungen gegenüber alter Implementation

### Vorher (Statische Methoden):
```cpp
class Databaser {
public:
    static QSqlDatabase db;
    static bool connect(const QString& path);
    static QList<T> runQueryList(const QString& query);
};

// Verwendung:
Databaser::connect(path);
QList<Airport> airports = Databaser::runQueryList<Airport>("...");
```

### Nachher (Singleton):
```cpp
class Databaser {
private:
    Databaser();
    QSqlDatabase db;
public:
    static Databaser& getInstance();
    bool connect(const QString& path);
    QList<T> runQueryList(const QString& query);
};

// Verwendung:
Databaser& db = Databaser::getInstance();
db.connect(path);
QList<Airport> airports = db.runQueryList<Airport>("...");
```

## Vorteile der neuen Implementation

### 1. Bessere Kapselung
- Datenbankverbindung ist privates Member (nicht mehr static public)
- Zustand (`connected`) wird intern verwaltet

### 2. Objektorientierter
- Methoden arbeiten auf Instanz statt statisch
- Leichter zu testen und zu mocken

### 3. Flexibler
- Kann in Zukunft leichter erweitert werden (z.B. Connection Pooling)
- Ermöglicht Subclassing falls nötig

### 4. Sicherer
- Copy-Konstruktor und Assignment gelöscht (nicht nur privat)
- `const`-Korrektheit (`isConnected() const`)

## Thread-Safety

Die Implementation ist **thread-safe** dank C++11:

```cpp
static Databaser& getInstance()
{
    static Databaser instance; // Thread-safe initialization since C++11
    return instance;
}
```

**Garantie**: Die statische lokale Variable wird nur einmal initialisiert, auch wenn mehrere Threads gleichzeitig `getInstance()` aufrufen.

**Hinweis**: Die Datenbankoperationen selbst sind **nicht** thread-safe. Qt's SQLite-Driver erfordert, dass alle Datenbankoperationen vom gleichen Thread ausgeführt werden, der die Verbindung erstellt hat.

## Memory Management

### Automatische Cleanup
Der Singleton wird automatisch beim Programmende zerstört:

```cpp
Databaser::~Databaser()
{
    disconnect(); // Schließt Datenbankverbindung
}
```

### Keine manuellen delete-Aufrufe nötig
```cpp
// FALSCH - niemals nötig:
delete &Databaser::getInstance(); // Compile-Error!

// RICHTIG - automatisch:
// Destruktor wird beim Programmende aufgerufen
```

## Best Practices

### ✅ DO:
- Referenz zum Singleton speichern für mehrfache Verwendung
- `getInstance()` zu Beginn der Funktion aufrufen
- Verbindung in `main()` herstellen und testen

```cpp
void myFunction() {
    Databaser& db = Databaser::getInstance();
    db.runQuery(...);
    db.runQuery(...);
    db.runQuery(...);
}
```

### ❌ DON'T:
- Pointer zum Singleton speichern
- Singleton in mehreren Threads ohne Synchronisation verwenden
- Datenbankoperationen nach Programmende ausführen

```cpp
// FALSCH:
Databaser* db = &Databaser::getInstance(); // Unnötiger Pointer

// FALSCH:
new Databaser(); // Compile-Error - Konstruktor ist privat
```

## Testing

Für Unit-Tests kann die Singleton-Implementation problematisch sein. Mögliche Lösungen:

### Option 1: Dependency Injection Interface
```cpp
class IDatabaser {
public:
    virtual ~IDatabaser() = default;
    virtual QList<T> runQueryList(const QString& query) = 0;
};

class Databaser : public IDatabaser {
    // ... implementation
};

class MockDatabaser : public IDatabaser {
    // ... mock implementation for testing
};
```

### Option 2: Reset-Methode (nur für Tests)
```cpp
#ifdef UNIT_TEST
    void resetInstance() {
        disconnect();
        connected = false;
    }
#endif
```

## Zusammenfassung

Die Singleton-Implementation bietet:
- ✅ Einzige Datenbankverbindung
- ✅ Globaler, typsicherer Zugriff
- ✅ Automatisches Memory Management
- ✅ Thread-safe Initialisierung
- ✅ Bessere Kapselung als statische Methoden
- ✅ Objektorientiertes Design

Die Implementation folgt modernen C++-Best-Practices und ist eine robuste Lösung für die Datenbankverwaltung in der Qt-Anwendung.
