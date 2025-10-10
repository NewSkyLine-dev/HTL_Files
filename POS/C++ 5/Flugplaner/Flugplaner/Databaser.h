#pragma once

#include <QString>
#include <QSqlDatabase>
#include <QSqlQuery>
#include <QSqlError>
#include <QVariant>
#include <QDebug>

class Databaser
{
private:
    Databaser();
    ~Databaser();
    
    Databaser(const Databaser&) = delete;
    Databaser& operator=(const Databaser&) = delete;

    QSqlDatabase db;
    bool connected;

public:
    static Databaser& getInstance();

    bool connect(const QString& path);
    void disconnect();
    bool isConnected() const;

    template <typename T>
    T runQuery(const QString& query, const T& defaultValue = T());

    template <typename T>
    QList<T> runQueryList(const QString& query);
};

template<typename T>
inline T Databaser::runQuery(const QString& query, const T& defaultValue)
{
    if (!connected || !db.isOpen()) {
        qWarning() << "Database not connected!";
        return defaultValue;
    }

    T result = defaultValue;
    QSqlQuery sqlQuery(db);

    if (!sqlQuery.exec(query)) {
        qCritical() << "Query execution failed:" << sqlQuery.lastError().text();
        qCritical() << "Query:" << query;
        return defaultValue;
    }

    if (sqlQuery.next()) {
        result = sqlQuery.value(0).template value<T>();
    }
    else {
        qWarning() << "No results returned for query:" << query;
    }

    return result;
}

template<typename T>
inline QList<T> Databaser::runQueryList(const QString& query)
{
    QList<T> results;

    if (!connected || !db.isOpen()) {
        qWarning() << "Database not connected!";
        return results;
    }

    QSqlQuery sqlQuery(db);

    if (!sqlQuery.exec(query)) {
        qCritical() << "Query execution failed:" << sqlQuery.lastError().text();
        qCritical() << "Query:" << query;
        return results;
    }

    while (sqlQuery.next()) {
        // This assumes T has a constructor that takes QSqlQuery
        results.append(T(sqlQuery));
    }

    return results;
}