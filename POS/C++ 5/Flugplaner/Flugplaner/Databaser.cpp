#include "Databaser.h"
#include <QSqlDatabase>
#include <QSqlError>
#include <QDebug>

// Singleton constructor
Databaser::Databaser() : connected(false)
{
}

Databaser::~Databaser()
{
    disconnect();
}

// Get singleton instance
Databaser &Databaser::getInstance()
{
    static Databaser instance;
    return instance;
}

bool Databaser::connect(const QString &path)
{
    // Remove existing connection if present
    if (QSqlDatabase::contains(QSqlDatabase::defaultConnection))
    {
        QSqlDatabase::removeDatabase(QSqlDatabase::defaultConnection);
    }

    db = QSqlDatabase::addDatabase("QSQLITE");
    db.setDatabaseName(path);

    if (!db.open())
    {
        qCritical() << "Database connection failed:" << db.lastError().text();
        qCritical() << "Database path:" << path;
        connected = false;
        return false;
    }

    connected = true;
    qInfo() << "Database opened successfully:" << path;
    return true;
}

void Databaser::disconnect()
{
    if (connected && db.isOpen())
    {
        db.close();
    }
    if (QSqlDatabase::contains(QSqlDatabase::defaultConnection))
    {
        QSqlDatabase::removeDatabase(QSqlDatabase::defaultConnection);
    }
    connected = false;
}

bool Databaser::isConnected() const
{
    return connected && db.isOpen();
}