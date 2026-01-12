#include "XMLExplorer.h"
#include <QtWidgets/QApplication>

int main(int argc, char *argv[])
{
    QApplication app(argc, argv);
    XMLExplorer window;
    window.show();
    return app.exec();
}
