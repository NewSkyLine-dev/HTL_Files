#include "Taschenrechner.h"

#include <QApplication>
#include <QWidget>
#include <QGridLayout>
#include <QPushButton>
#include <QLineEdit>
#include <QFont>
#include <QHBoxLayout>
#include <QVBoxLayout>
#include <QSizePolicy>
#include <QComboBox>

#define exprtk_disable_enhanced_features
#include "exprtk.hpp"

Taschenrechner::Taschenrechner(QWidget *parent)
    : QMainWindow(parent)
{
	setWindowTitle("Taschenrechner");
	auto* central = new QWidget;
	auto* root = new QVBoxLayout(central);
	root->setContentsMargins(12, 12, 12, 12);
	root->setSpacing(10);
	setCentralWidget(central);

	display = new QLineEdit(central);
	display->setReadOnly(true);
	display->setAlignment(Qt::AlignRight);
	QFont f = display->font();
	f.setPointSize(22);
	display->setFont(f);
	display->setMinimumHeight(50);
	display->setText("0");
	display->setStyleSheet("QLineEdit{ padding:10px; }");
	root->addWidget(display);

	auto* grid = new QGridLayout();
	grid->setSpacing(8);
	root->addLayout(grid);

	auto makeBtn = [&](const QString& text,
		void (Taschenrechner::* member)()) {
			auto* b = new QPushButton(text, central);
			b->setSizePolicy(QSizePolicy::Expanding, QSizePolicy::Expanding);
			QFont bf = b->font();
			bf.setPointSize(16);
			b->setFont(bf);
			connect(b, &QPushButton::clicked, this, member);
			return b;
	};

	auto makeComboBox = [&](const QStringList& items, void (Taschenrechner::* member)()) {
		auto* cb = new QComboBox(central);
		cb->addItems(items);
		cb->setSizePolicy(QSizePolicy::Expanding, QSizePolicy::Expanding);
		cb->setStyleSheet("QComboBox{ border:none; }");
		QFont bf = cb->font();
		bf.setPointSize(16);
		cb->setFont(bf);
		connect(cb, &QComboBox::activated, this, member);
		return cb;
	};

	// Top row: CE, C, ⌫, *
	grid->addWidget(makeBtn("CE", &Taschenrechner::clearEntry), 0, 0);
	grid->addWidget(makeBtn("C", &Taschenrechner::clearAll), 0, 1);
	grid->addWidget(makeBtn("⌫", &Taschenrechner::backspace), 0, 2);
	grid->addWidget(makeBtn("/", &Taschenrechner::opClicked), 0, 3);


	// Row 2: 7 8 9 *
	grid->addWidget(makeBtn("7", &Taschenrechner::digitClicked), 1, 0);
	grid->addWidget(makeBtn("8", &Taschenrechner::digitClicked), 1, 1);
	grid->addWidget(makeBtn("9", &Taschenrechner::digitClicked), 1, 2);
	grid->addWidget(makeBtn("*", &Taschenrechner::opClicked), 1, 3);


	// Row 3: 4 5 6 −
	grid->addWidget(makeBtn("4", &Taschenrechner::digitClicked), 2, 0);
	grid->addWidget(makeBtn("5", &Taschenrechner::digitClicked), 2, 1);
	grid->addWidget(makeBtn("6", &Taschenrechner::digitClicked), 2, 2);
	grid->addWidget(makeBtn("-", &Taschenrechner::opClicked), 2, 3);


	// Row 4: 1 2 3 +
	grid->addWidget(makeBtn("1", &Taschenrechner::digitClicked), 3, 0);
	grid->addWidget(makeBtn("2", &Taschenrechner::digitClicked), 3, 1);
	grid->addWidget(makeBtn("3", &Taschenrechner::digitClicked), 3, 2);
	grid->addWidget(makeBtn("+", &Taschenrechner::opClicked), 3, 3);


	// Row 5: 0 . () =
	grid->addWidget(makeBtn("0", &Taschenrechner::digitClicked), 4, 0);
	grid->addWidget(makeBtn(".", &Taschenrechner::opClicked), 4, 1);
	grid->addWidget(makeComboBox(QStringList{ "(", ")" }, &Taschenrechner::parensClicked), 4, 2);
	auto* eq = makeBtn("=", &Taschenrechner::equalClicked);
	grid->addWidget(eq, 4, 3);

	resize(320, 440);
	setMinimumSize(280, 380);
	setStyleSheet(R"(QPushButton{ padding:10px; border-radius:10px; }
					QPushButton#equals{ font-weight:bold; }
					QWidget{ background:#f5f6f7; } )");
}

Taschenrechner::~Taschenrechner()
{}

void Taschenrechner::digitClicked()
{
	auto* btn = qobject_cast<QPushButton*>(sender());
	if (!btn) return;
	if (display->text() == "0" || justEvaluated) {
		display->setText(btn->text());
		justEvaluated = false;
	}
	else {
		display->setText(display->text() + btn->text());
	}
}

void Taschenrechner::opClicked()
{
	auto* btn = qobject_cast<QPushButton*>(sender());
	if (!btn) return;

	QString t = btn->text();
	if (display->text().endsWith(" ")) {
		QString s = display->text();
		int i = s.lastIndexOf(' ');
		if (i >= 0) s = s.left(i) + t;
		display->setText(s);
	}
	else {
		display->setText(display->text() + t);
	}
	justEvaluated = false;
}

void Taschenrechner::clearEntry()
{
	display->setText("0");
	justEvaluated = false;
}

void Taschenrechner::clearAll()
{
	display->setText("0");
	justEvaluated = false;
}

void Taschenrechner::backspace()
{
	auto* btn = qobject_cast<QPushButton*>(sender());
	if (!btn) return;
	QString t = display->text();
	if (t.length() <= 1) {
		display->setText("0");
	}
	else {
		t.chop(1);
		display->setText(t);
	}
}

void Taschenrechner::evaluate()
{
	exprtk::expression<double> expression;
	exprtk::parser<double> parser;
	QString expr = display->text();
	if (parser.compile(expr.toStdString(), expression)) {
		double result = expression.value();
		display->setText(QString::number(result));
	}
	else {
		display->setText("Error");
	}
}

void Taschenrechner::parensClicked()
{
	auto* cb = qobject_cast<QComboBox*>(sender());
	if (!cb) return;
	QString t = cb->currentText();
	if (display->text() == "0" || justEvaluated) {
		display->setText(t);
		justEvaluated = false;
	}
	else {
		display->setText(display->text() + t);
	}
}

void Taschenrechner::equalClicked()
{
	evaluate();
	justEvaluated = true;
}

