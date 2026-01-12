#include "Roboter.h"
#include "robotmap.h"
#include <QVBoxLayout>
#include <QFileDialog>
#include <QDebug>
#include <QPushButton>
#include <QMessageBox>
#include <QThread>
#include <QCoreApplication>

Roboter::Roboter(QWidget *parent)
	: QMainWindow(parent), executionTimer(nullptr), isExecuting(false)
{
	// ui.setupUi(this);
	auto map = new RobotMap(this);
	robotMap = map;
	auto central_layout = new QHBoxLayout();
	auto central_widget = new QWidget(this);
	program_input = new QTextEdit(this);
	program_input->setMinimumHeight(this->size().height());

	central_layout->addWidget(map, 3);

	// Create right panel with program input and buttons
	auto right_panel = new QVBoxLayout();
	right_panel->addWidget(program_input);

	// Add execute button
	auto executeButton = new QPushButton("Execute Program", this);
	connect(executeButton, &QPushButton::clicked, this, &Roboter::executeProgram);
	right_panel->addWidget(executeButton);

	auto right_widget = new QWidget();
	right_widget->setLayout(right_panel);

	central_layout->addWidget(right_widget, 1);
	central_widget->setLayout(central_layout);
	this->setCentralWidget(central_widget);

	QMenu *fileMenu = menuBar()->addMenu(tr("&File"));
	QAction *openLevelAction = fileMenu->addAction(tr("O&pen Level"));
	QAction *openProgramAction = fileMenu->addAction(tr("Open &Program"));

	connect(openLevelAction, &QAction::triggered, this, [map, this]()
			{
		QString filename = QFileDialog::getOpenFileName(this, tr("Open Level"), "", tr("XML Files (*.xml)"));
		if (!filename.isEmpty()) {
			QList<RobotElement*> elements = level_parser.parse_level(filename.toStdString(), map);
			map->setElements(elements);
		} });

	connect(openProgramAction, &QAction::triggered, this, [this]()
			{
		QString filename = QFileDialog::getOpenFileName(this, tr("Open Program"), "", tr("Program Files (*.txt)"));
		if (!filename.isEmpty()) {
			QFile file(filename);
			if (file.open(QIODevice::ReadOnly | QIODevice::Text)) {
				program_content = file.readAll().toStdString();
				program_input->setText(QString::fromStdString(program_content));
				parseProgram();
			}
		} });
}

Roboter::~Roboter()
{
}

void Roboter::parseProgram()
{
	QString programText = QString::fromStdString(program_content);

	// Step 1: Tokenize
	QList<Token> tokens = tokenizer.tokenize(programText);

	qDebug() << "=== Tokenization Results ===";
	for (const Token &token : tokens)
	{
		qDebug() << "Line" << token.line
				 << "Col" << token.column
				 << token.typeToString()
				 << ":" << token.text;
	}

	// Step 2: Parse and build AST
	Parser parser(tokens);
	ast = parser.parse();

	// Check for parse errors
	if (parser.hasErrors())
	{
		qDebug() << "\n=== Parse Errors ===";
		for (const auto &error : parser.getErrors())
		{
			qDebug() << error.toString();
		}

		// Show error dialog
		QString errorMsg = "Parse Errors:\n";
		for (const auto &error : parser.getErrors())
		{
			errorMsg += error.toString() + "\n";
		}
		QMessageBox::critical(this, "Parse Error", errorMsg);
	}
	else
	{
		qDebug() << "\n=== AST Structure ===";
		qDebug() << ast->toString();
	}
}

void Roboter::executeProgram()
{
	// Parse the program first if needed
	if (!ast)
	{
		program_content = program_input->toPlainText().toStdString();
		parseProgram();
	}

	// Check if we have a valid AST
	if (!ast)
	{
		QMessageBox::warning(this, "Execution Error", "No valid program to execute");
		return;
	}

	// Get the elements from the map
	QList<RobotElement *> &elements = robotMap->getElements();

	if (elements.isEmpty())
	{
		QMessageBox::warning(this, "Execution Error", "No level loaded. Please load a level first.");
		return;
	}

	qDebug() << "\n=== Executing Program ===";

	// Prevent multiple simultaneous executions
	if (isExecuting)
	{
		return;
	}
	isExecuting = true;

	// Create interpreter and set up step callback for animation
	Interpreter interpreter(elements);
	interpreter.setStepCallback([this]()
								{
									updateMapDisplay();
									QThread::msleep(300);			   // 300ms delay between steps
									QCoreApplication::processEvents(); // Process UI events
								});

	interpreter.execute(ast);
	isExecuting = false;

	// Check for execution errors
	if (interpreter.hasErrors())
	{
		qDebug() << "\n=== Execution Errors ===";
		QString errorMsg = "Execution Errors:\n";
		for (const auto &error : interpreter.getErrors())
		{
			qDebug() << error.toString();
			errorMsg += error.toString() + "\n";
		}
		QMessageBox::critical(this, "Execution Error", errorMsg);
	}
	else
	{
		// Show success message with collected letters
		QString collectedLetters = interpreter.getCollectedLetters();
		QString message = "Program executed successfully!\n";
		if (!collectedLetters.isEmpty())
		{
			message += "Collected letters: " + collectedLetters;
		}
		else
		{
			message += "No letters collected.";
		}
		QMessageBox::information(this, "Execution Complete", message);
	}

	// Final map update
	updateMapDisplay();

	// Show execution log
	qDebug() << "\n=== Execution Log ===";
	for (const QString &logEntry : interpreter.getExecutionLog())
	{
		qDebug() << logEntry;
	}
}

void Roboter::updateMapDisplay()
{
	robotMap->update();
	robotMap->repaint();
}
