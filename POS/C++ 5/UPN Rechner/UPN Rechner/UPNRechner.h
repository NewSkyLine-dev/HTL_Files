#pragma once

#include <QtWidgets/QMainWindow>
#include <QLabel>
#include <QPushButton>
#include <QLineEdit>
#include <stack>

#include "ui_UPNRechner.h"

class UPNRechner : public QMainWindow
{
    Q_OBJECT

public:
    UPNRechner(QWidget *parent = nullptr);
    ~UPNRechner();

private slots:
    void onCalculate();

private:
    std::stack<int> m_upnStack;
    // UI
	QPushButton* m_calculateButton;
	QLineEdit* m_inputBox;
	QLabel* m_resultLabel;
};

