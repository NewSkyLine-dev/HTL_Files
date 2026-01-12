#include "stdafx.h"
#include "Flugplaner.h"
#include "Models/alliance.h"
#include "Models/Airline.h"

#include "QxOrm.h"

Flugplaner::Flugplaner(QWidget *parent)
    : QMainWindow(parent)
{
    ui.setupUi(this);

    alliance_list data;

    // Mit allen Relationen laden
    auto err = qx::dao::fetch_all_with_relation("*", data);

    // KORREKTUR: isValid() == true bedeutet FEHLER in QxOrm!
    if (err.isValid())
    {
        qDebug() << "Err: " << err.databaseText();
        qDebug() << "Driver error: " << err.driverText();
        qDebug() << "Native error code: " << err.nativeErrorCode();
        qDebug() << "Database error: " << err.text();
        
        QMessageBox::critical(this, "Error", "Failed to fetch alliances: " + err.text());
        return;
    }

    // TableView erstellen
    QTableView* tableView = new QTableView(this);
    
    // Model erstellen und konfigurieren
    QStandardItemModel* model = new QStandardItemModel(this);
    model->setColumnCount(2);
    model->setHeaderData(0, Qt::Horizontal, "Alliance ID");
    model->setHeaderData(1, Qt::Horizontal, "Alliance Name");
    
    // Daten in das Model laden
    for (const auto& alliancePtr : data)
    {
        QList<QStandardItem*> rowItems;
        rowItems << new QStandardItem(QString::number(alliancePtr->id));
        rowItems << new QStandardItem(alliancePtr->name);
        model->appendRow(rowItems);
    }
    
    tableView->setModel(model);
    
    // WICHTIG: TableView als Central Widget setzen (nicht zu Layout hinzufügen)
    setCentralWidget(tableView);
}

Flugplaner::~Flugplaner()
{}

