#include "Flugplaner.h"
#include <QtWidgets/QApplication>
#include <QSqlDatabase>
#include <QDir>
#include <QMessageBox>
#include "Databaser.h"

int main(int argc, char *argv[])
{
    QApplication app(argc, argv);
    
    // Get Databaser singleton instance and connect to database
    Databaser& db = Databaser::getInstance();
    
    QString dbPath = QDir::currentPath() + "/resources/AirlineRoutes.db";
    if (!db.connect(dbPath)) {
        QMessageBox::critical(nullptr, "Database Error", 
            "Failed to connect to the database.\nPath: " + dbPath);
        return -1;
    }
    
    qInfo() << "Database connection established with" << dbPath;

    Flugplaner window;
    window.show();
    
    int result = app.exec();
    
    // Cleanup: disconnect database
    db.disconnect();
    
    return result;
}
