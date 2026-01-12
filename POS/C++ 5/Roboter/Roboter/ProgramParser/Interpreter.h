#pragma once

#include "ASTNode.h"
#include "../include/robotelement.h"
#include <QList>
#include <QString>
#include <QDebug>
#include <memory>
#include <functional>

class InterpreterException
{
public:
    QString message;

    InterpreterException(const QString &msg) : message(msg) {}

    QString toString() const
    {
        return QString("Interpreter Error: %1").arg(message);
    }
};

class Interpreter : public ASTVisitor
{
public:
    Interpreter(QList<RobotElement *> &elements);

    // Set callback to be called after each step
    void setStepCallback(std::function<void()> callback) { stepCallback = callback; }

    // Execute the program AST
    void execute(std::shared_ptr<ProgramNode> program);

    // ASTVisitor interface implementation
    void visit(ProgramNode *node) override;
    void visit(RepeatNode *node) override;
    void visit(MoveNode *node) override;
    void visit(CollectNode *node) override;
    void visit(UntilNode *node) override;
    void visit(IfNode *node) override;

    // Get execution results
    QString getCollectedLetters() const { return collectedLetters; }
    QList<QString> getExecutionLog() const { return executionLog; }
    bool hasErrors() const { return !errors.empty(); }
    QList<InterpreterException> getErrors() const { return errors; }

private:
    QList<RobotElement *> &elements;
    RobotElement *robot;
    QString collectedLetters;
    QList<QString> executionLog;
    QList<InterpreterException> errors;
    std::function<void()> stepCallback;

    // Helper methods
    void findRobot();
    bool canMoveTo(int x, int y);
    void moveRobot(Direction direction);
    void collectLetter();
    void log(const QString &message);
    void addError(const QString &message);
    RobotElement *getElementAt(int x, int y, RobotElement::Type type);
    bool checkCondition(std::shared_ptr<ConditionNode> condition);
    bool hasObstacleAt(int x, int y);
    bool hasLetterAt(int x, int y, const QString &letter);
};
