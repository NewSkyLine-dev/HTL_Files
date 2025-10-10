#pragma once

#include <QtWidgets/QMainWindow>
#include "ui_QtWidgetsApp.h"

class QtWidgetsApp : public QMainWindow
{
    Q_OBJECT

public:
    QtWidgetsApp(QWidget *parent = nullptr);
    ~QtWidgetsApp();

private:
    Ui::QtWidgetsAppClass ui;
};

