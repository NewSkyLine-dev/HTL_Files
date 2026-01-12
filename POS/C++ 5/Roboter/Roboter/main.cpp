#include "Roboter.h"
#include <QtWidgets/QApplication>

int main(int argc, char *argv[])
{
    QApplication app(argc, argv);
    Roboter window;
    window.show();
    return app.exec();
}
