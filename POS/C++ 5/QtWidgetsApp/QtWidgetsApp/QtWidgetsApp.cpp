#include "QtWidgetsApp.h"

#include "MyLib.h"

QtWidgetsApp::QtWidgetsApp(QWidget *parent)
    : QMainWindow(parent)
{
    ui.setupUi(this);
    int sum = MyLib::add(1, 2);
    setWindowTitle(QString("Summe: %1").arg(sum));
}

QtWidgetsApp::~QtWidgetsApp()
{}

