#pragma once

#include <QtWidgets/QMainWindow>
#include "ui_Flugplaner.h"

class Flugplaner : public QMainWindow
{
    Q_OBJECT

public:
    Flugplaner(QWidget *parent = nullptr);
    ~Flugplaner();

private:
    Ui::FlugplanerClass ui;
};

