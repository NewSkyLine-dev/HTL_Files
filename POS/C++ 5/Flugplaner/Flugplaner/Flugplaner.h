#pragma once

#include <QtWidgets/QMainWindow>
#include <QComboBox>
#include <QTableWidget>
#include "ui_Flugplaner.h"
#include "MapWidget.h"

class Flugplaner : public QMainWindow
{
    Q_OBJECT

public:
    Flugplaner(QWidget *parent = nullptr);
    ~Flugplaner();

private slots:
    void on_searchButton_clicked();
    void onFromAirportChanged(int index);
    void onToAirportChanged(int index);
    void onAirlineChanged(int index);

private:
    Ui::FlugplanerClass ui;
    
    QComboBox* fromCombo;
    QComboBox* toCombo;
    QComboBox* airlineCombo;
    QTableWidget* flightTable;
    MapWidget* mapWidget;
    
    int selectedFromAirportId;
    int selectedToAirportId;
    int selectedAirlineId;

    void setupUI();
    void loadAirports();
    void loadAirlines();
    void updateFlightTable(const FlightPath& path);
};

