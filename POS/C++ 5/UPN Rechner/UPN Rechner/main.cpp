#include "UPNRechner.h"
#include <QtWidgets/QApplication>

int main(int argc, char *argv[])
{
    QApplication app(argc, argv);
    UPNRechner window;
    window.show();
    return app.exec();
}
