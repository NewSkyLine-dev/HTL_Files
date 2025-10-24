#pragma once

#include <QString>
#include <QSqlDatabase>
#include <QxOrm.h>

class Databaser
{
private:
    Databaser();
    ~Databaser();

    Databaser(const Databaser &) = delete;
    Databaser &operator=(const Databaser &) = delete;

    QSqlDatabase db;
    bool connected;

public:
    static Databaser &getInstance();

    bool connect(const QString &path);
    void disconnect();
    bool isConnected() const;
};