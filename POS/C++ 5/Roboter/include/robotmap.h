#ifndef ROBOTMAP_H
#define ROBOTMAP_H

#include "RobotLibrary_global.h"
#include "robotelement.h"
#include <QWidget>

class RobotMap : public QWidget
{
    Q_OBJECT
private:
    int w = 10;
    int h = 10;
    QPixmap wall;
    QPixmap obstacle;
    QPixmap robot;
    QList<RobotElement*> elements;

public:
    explicit RobotMap(QWidget *parent = nullptr);
    void paintEvent(QPaintEvent *event);
    void setSize(int x, int y);
    void setElements(QList<RobotElement*> list);
    void clearLevel();

    // Getter for elements (needed for interpreter)
    QList<RobotElement*>& getElements() { return elements; }

signals:

};

#endif // ROBOTMAP_H
