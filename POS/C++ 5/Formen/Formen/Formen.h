#pragma once

#include <QtWidgets/QMainWindow>
#include "ui_Formen.h"

class Formen : public QMainWindow
{
    Q_OBJECT

public:
    Formen(QWidget *parent = nullptr);
    ~Formen();

private:
    Ui::FormenClass ui;
};

