#include "expression.h"
#include "../Roboter.h"

#include <QMessageBox>

QList<QString> Expression::errors;

Expression::Expression()
{

}

void Expression::parse(QList<Token> &tokenlist)
{

}

bool Expression::run(Roboter *window)
{
    // lex(...)
    // parse(...)

    if (this->errors.size() > 0) {
        QMessageBox::critical(window, "Error", "There were errors during execution:\n" + this->errors.join("\n"));
        return false;
    }
    return true;
}
