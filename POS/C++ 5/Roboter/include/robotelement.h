#ifndef ROBOTELEMENT_H
#define ROBOTELEMENT_H

#include "RobotLibrary_global.h"
#include <QString>

class ROBOTLIBRARY_EXPORT RobotElement
{
public:
    enum Type { Obstacle, Wall, Letter, Robot };
    RobotElement();
    int x = 0;
    int y = 0;
    QString letter;
    Type type = Wall;
};

#endif // ROBOTELEMENT_H
