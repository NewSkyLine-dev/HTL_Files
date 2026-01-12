#pragma once

#include <QtWidgets/QMainWindow>
#include <QLineEdit>
#include <QChartView>
#include <QChart>
#include <QLineSeries>

#include "ui_Formel1.h"

class Formel1 : public QMainWindow
{
    Q_OBJECT

public:
    Formel1(QWidget *parent = nullptr);
    ~Formel1();
	void initChart();

private slots:
    void onDriverSelected();

private:
    QLineEdit* m_nameInput;
    QComboBox* m_searchResults;
    QChartView* m_chartView;
    QChart* m_chart;
    QLineSeries* m_series;

    // Driver Info Labels (displayed)
    QLabel* nameUnderLabel;
	QLabel* surnameUnderLabel;
    QLabel* birthDateUnderLabel;
	QLabel* nationalityUnderLabel;
	QLabel* countDrivesUnderLabel;
	QLabel* teamUnderLabel;
};

