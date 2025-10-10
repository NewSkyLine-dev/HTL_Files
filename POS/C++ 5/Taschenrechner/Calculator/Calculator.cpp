#include <QString>

#include "Calculator.h"

#define exprtk_disable_enhanced_features
#include "exprtk.hpp"

Calculator::Calculator()
{
}

QString Calculator::eval(const QString &expression)
{
	exprtk::expression<double> expression_tk;
	exprtk::parser<double> parser;
	exprtk::symbol_table<double> symbol_table;
	expression_tk.register_symbol_table(symbol_table);
	std::string expr_str = expression.toStdString();
	if (!parser.compile(expr_str, expression_tk))
	{
		return "Error: " + QString::fromStdString(parser.error());
	}
	double result = expression_tk.value();
	return QString::number(result);
}
