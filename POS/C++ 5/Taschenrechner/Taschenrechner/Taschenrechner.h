#pragma once

#include <QtWidgets/QMainWindow>
#include "ui_Taschenrechner.h"
#include <QLineEdit>

class Taschenrechner : public QMainWindow
{
    Q_OBJECT

public:
    Taschenrechner(QWidget *parent = nullptr);
    ~Taschenrechner();

private:
    Ui::TaschenrechnerClass ui;
    QLineEdit *display{nullptr};
    bool justEvaluated{false};

private slots:
    void digitClicked();
    void opClicked();
    void clearEntry();
    void clearAll();
    void backspace();
    void evaluate();
    void parensClicked();
    void equalClicked();
};
