#include "Formen.h"
#include <QtWidgets/QApplication>

int main(int argc, char *argv[])
{
    QApplication app(argc, argv);
    Formen window;
    window.show();
    return app.exec();
}
