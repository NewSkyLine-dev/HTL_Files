#pragma once

#include <QtWidgets/QMainWindow>
#include "ui_Taschenrechner.h"

#include <QLabel>
#include <QPushButton>

class Taschenrechner : public QMainWindow
{
    Q_OBJECT

public:
    Taschenrechner(QWidget *parent = nullptr);
    ~Taschenrechner();

private slots:
    void onCalcButtonClicked();
	void onEvalButtonClicked();

private:
    QLabel* m_resultLabel;
    QPushButton* m_evalButton;
};

