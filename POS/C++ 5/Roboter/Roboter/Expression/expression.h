#ifndef EXPRESSION_H
#define EXPRESSION_H

#include <QList>
#include "../Token/token.h"

class Roboter;
class Expression
{
public:
    Expression();
    virtual void parse(QList<Token> &tokenlist);
    virtual bool run(Roboter *window);
    static QList<QString> errors;
};

#endif // EXPRESSION_H
