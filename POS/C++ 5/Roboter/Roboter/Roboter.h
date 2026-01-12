#pragma once

#include <QtWidgets/QMainWindow>
#include "ui_Roboter.h"
#include "XML/level_parser.h"
#include "ProgramParser/Tokenizer.h"
#include "ProgramParser/Parser.h"
#include "ProgramParser/ASTNode.h"
#include "ProgramParser/Interpreter.h"
#include <QLineEdit>
#include <memory>
#include <QTextEdit>
#include <QTimer>

class RobotMap;

class Roboter : public QMainWindow
{
    Q_OBJECT

public:
    Roboter(QWidget *parent = nullptr);
    ~Roboter();

private:
    Ui::RoboterClass ui;
    level_parser level_parser;
    std::string program_content;
    QTextEdit *program_input;
    Tokenizer tokenizer;
    std::shared_ptr<ProgramNode> ast;
    RobotMap *robotMap;
    QTimer *executionTimer;
    bool isExecuting;

    void parseProgram();
    void executeProgram();
    void updateMapDisplay();
};
