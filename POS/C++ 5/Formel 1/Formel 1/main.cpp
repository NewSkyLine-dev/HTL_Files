#include "stdafx.h"
#include "Formel1.h"
#include <QtWidgets/QApplication>

int main(int argc, char *argv[])
{
    QApplication app(argc, argv);

	qx::QxSqlDatabase::getSingleton()->setDriverName("QSQLITE");
	qx::QxSqlDatabase::getSingleton()->setDatabaseName("./resources/Formula1.sqlite");
	qx::QxSqlDatabase::getSingleton()->setHostName("localhost");
	qx::QxSqlDatabase::getSingleton()->setUserName("");
	qx::QxSqlDatabase::getSingleton()->setPassword("");

    Formel1 window;
    window.show();
    return app.exec();
}
