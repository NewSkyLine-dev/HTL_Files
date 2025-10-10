#pragma once

#include <QString>

#include "calculator_global.h"

class CALCULATOR_EXPORT Calculator
{
public:
    Calculator();

    QString eval(const QString& expression);
};
