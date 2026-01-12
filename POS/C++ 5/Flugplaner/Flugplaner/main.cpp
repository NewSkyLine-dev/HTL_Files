#include "stdafx.h"
#include "Flugplaner.h"
#include <QtWidgets/QApplication>
#include <Models/alliance.h>

void setupDbConnection()
{
	qx::QxSqlDatabase::getSingleton()->setDriverName("QSQLITE");
	qx::QxSqlDatabase::getSingleton()->setDatabaseName("./AirlineRoutes.db");
	qx::QxSqlDatabase::getSingleton()->setHostName("localhost");
	qx::QxSqlDatabase::getSingleton()->setUserName("");
	qx::QxSqlDatabase::getSingleton()->setPassword("");
}

int main(int argc, char *argv[])
{
    QApplication app(argc, argv);

    setupDbConnection();

    Flugplaner window;
    window.show();
    return app.exec();
}
