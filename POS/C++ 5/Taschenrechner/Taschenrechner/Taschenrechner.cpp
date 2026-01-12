#include "Taschenrechner.h"

#define exprtk_disable_enhanced_features
#include "exprtk.hpp"

#include <QGridLayout>
#include <QPushButton>

Taschenrechner::Taschenrechner(QWidget *parent)
    : QMainWindow(parent)
{
	auto centralWidget = new QWidget(this);
	auto centralLayout = new QVBoxLayout(centralWidget);
	centralLayout->setSpacing(2);

	m_resultLabel = new QLabel("0");
	m_evalButton = new QPushButton("=");
	connect(m_evalButton, &QPushButton::clicked, this, &Taschenrechner::onEvalButtonClicked);

	centralLayout->addWidget(m_resultLabel);

	auto buttonGridWidget = new QWidget(this);
	auto buttonGridLayout = new QGridLayout(buttonGridWidget);

	std::array<char, 12> elements = {'7', '8', '9', '+', '4', '5', '6', '-', '1', '2', '3', '/' };

	for (int i = 0; i < 12; ++i) {
		auto button = new QPushButton(QString(elements[i]), this);
		connect(button, &QPushButton::clicked, this, &Taschenrechner::onCalcButtonClicked);
		button->setMinimumSize(40, 40);
		int row = i / 4;
		int col = i % 4;
		buttonGridLayout->addWidget(button, row, col);
	}

	centralLayout->addWidget(buttonGridWidget);
	
	auto commaButton = new QPushButton(",", this);
	connect(commaButton, &QPushButton::clicked, this, &Taschenrechner::onCalcButtonClicked);

	centralLayout->addWidget(commaButton);
	centralLayout->addWidget(m_evalButton);
	
	setCentralWidget(centralWidget);
}

void Taschenrechner::onEvalButtonClicked()
{
	QString expressionStr = m_resultLabel->text();
	expressionStr.replace(",", ".");
	typedef exprtk::symbol_table<double> symbol_table_t;
	typedef exprtk::expression<double>     expression_t;
	typedef exprtk::parser<double>             parser_t;
	expression_t   expression;
	parser_t parser;
	if (parser.compile(expressionStr.toStdString(), expression)) {
		double result = expression.value();
		m_resultLabel->setText(QString::number(result, 'g', 15).replace(".", ","));
	} else {
		m_resultLabel->setText("Error");
	}
}

void Taschenrechner::onCalcButtonClicked()
{
	auto button = qobject_cast<QPushButton*>(sender());
	if (button) {
		QString buttonText = button->text();
		QString currentText = m_resultLabel->text();
		if (currentText == "0" || "Error") {
			m_resultLabel->setText(buttonText);
		} else {
			m_resultLabel->setText(currentText + buttonText);
		}
	}
}

Taschenrechner::~Taschenrechner()
{}

