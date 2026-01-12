#include "UPNRechner.h"

#include <QVBoxLayout>
#include <QLineEdit>
#include <QPushButton>
#include <QMessageBox>

UPNRechner::UPNRechner(QWidget *parent)
    : QMainWindow(parent)
{
	auto centralWidget = new QWidget(this);
	auto centralLayout = new QHBoxLayout(centralWidget);

	// Result Label
	m_resultLabel = new QLabel(centralWidget);

	// Input Box
	m_inputBox = new QLineEdit(centralWidget);
	connect(m_inputBox, &QLineEdit::returnPressed, this, &UPNRechner::onCalculate);
	
	// Calculate Button
	m_calculateButton = new QPushButton(centralWidget);
	m_calculateButton->setText("Calculate");
	connect(m_calculateButton, &QPushButton::clicked, this, &UPNRechner::onCalculate);

	centralLayout->addWidget(m_inputBox);
	centralLayout->addWidget(m_calculateButton);
	centralLayout->addWidget(m_resultLabel);

	setCentralWidget(centralWidget);
}

void UPNRechner::onCalculate()
{
	QString input = m_inputBox->text();
	if (input.isEmpty())
	{
		QMessageBox::warning(this, "Error", "Input is empty!");
		return;
	}

	if (!input.contains(' ') || input.length() <= 0) 
	{
		QMessageBox::warning(this, "Error", "Wrong Input!");
		return;
	}

	// Clear stack
	while (!m_upnStack.empty()) m_upnStack.pop();

	// Split input by spaces
	QStringList tokens = input.split(' ', Qt::SkipEmptyParts);

	// Fill stack
	for (const QString& token : tokens)
	{
		// Check if token is a number
		bool isNumber;
		int number = token.toInt(&isNumber);

		if (isNumber) 
		{
			m_upnStack.push(number);
		}
		else
		{
			if (m_upnStack.size() < 2)
			{
				QMessageBox::warning(this, "Error", "Not enough operands for operation!");
				return;
			}

			int a = m_upnStack.top(); m_upnStack.pop();
			int b = m_upnStack.top(); m_upnStack.pop();

			if (token == "+")
				m_upnStack.push(b + a);
			else if (token == "-")
				m_upnStack.push(b - a);
			else if (token == "*")
				m_upnStack.push(b * a);
			else if (token == "/")
			{
				if (a == 0)
				{
					QMessageBox::warning(this, "Error", "Division by zero!");
					return;
				}
				m_upnStack.push(b / a);
			}
			else
			{
				QMessageBox::warning(this, "Error", "Unknown operator: " + token);
				return;
			}
		}
	}

	if (m_upnStack.size() != 1)
	{
		QMessageBox::warning(this, "Error", "The input does not reduce to a single result!");
		return;
	}

	int result = m_upnStack.top();
	m_resultLabel->setText("Result: " + QString::number(result));
}

UPNRechner::~UPNRechner()
{}

