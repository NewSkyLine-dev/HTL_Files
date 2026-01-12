#include "Taschenrechner.h"
#include <QtWidgets/QApplication>

int main(int argc, char *argv[])
{
    QApplication app(argc, argv);
    Taschenrechner window;
    window.show();
    return app.exec();
}
