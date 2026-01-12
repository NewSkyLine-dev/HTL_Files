#include "stdafx.h"
#include "PA1.h"
#include <QtWidgets/QApplication>

void setupDbConnection()
{
	QString dir = QCoreApplication::applicationDirPath() + "./PA1.db";
	qx::QxSqlDatabase::getSingleton()->setDriverName("QSQLITE");
	qx::QxSqlDatabase::getSingleton()->setDatabaseName(dir);
	qx::QxSqlDatabase::getSingleton()->setHostName("localhost");
	qx::QxSqlDatabase::getSingleton()->setUserName("");
	qx::QxSqlDatabase::getSingleton()->setPassword("");
}

int main(int argc, char *argv[])
{
    QApplication app(argc, argv);

	setupDbConnection();

    PA1 window;
    window.show();
    return app.exec();
}
