#include "QtWidgetsApp.h"
#include <QtWidgets/QApplication>

int main(int argc, char *argv[])
{
    QApplication app(argc, argv);
    QtWidgetsApp window;
    window.show();
    return app.exec();
}
