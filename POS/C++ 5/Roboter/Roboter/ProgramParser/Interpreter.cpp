#include "Interpreter.h"

Interpreter::Interpreter(QList<RobotElement *> &elements)
    : elements(elements), robot(nullptr)
{
}

void Interpreter::execute(std::shared_ptr<ProgramNode> program)
{
    errors.clear();
    executionLog.clear();
    collectedLetters.clear();

    // Find the robot in the element list
    findRobot();

    if (!robot)
    {
        addError("No robot found on the map");
        return;
    }

    log(QString("Starting execution. Robot at position (%1, %2)").arg(robot->x).arg(robot->y));

    try
    {
        // Start visiting the AST
        program->accept(this);
        log("Program execution completed successfully");
    }
    catch (const InterpreterException &e)
    {
        errors.append(e);
        log("Program execution failed: " + e.message);
    }

    log(QString("Final robot position: (%1, %2)").arg(robot->x).arg(robot->y));
    log(QString("Collected letters: %1").arg(collectedLetters.isEmpty() ? "(none)" : collectedLetters));
}

void Interpreter::visit(ProgramNode *node)
{
    log("Executing program");
    for (const auto &statement : node->statements)
    {
        statement->accept(this);
    }
}

void Interpreter::visit(RepeatNode *node)
{
    log(QString("Repeat %1 times {").arg(node->count));

    for (int i = 0; i < node->count; i++)
    {
        log(QString("  Iteration %1 of %2").arg(i + 1).arg(node->count));
        for (const auto &statement : node->body)
        {
            statement->accept(this);
        }
    }

    log("}");
}

void Interpreter::visit(MoveNode *node)
{
    QString dirStr = directionToString(node->direction);
    log(QString("Move %1").arg(dirStr));

    try
    {
        moveRobot(node->direction);
        log(QString("  -> Robot moved to (%1, %2)").arg(robot->x).arg(robot->y));
    }
    catch (const InterpreterException &e)
    {
        addError(e.message);
        throw e;
    }
}

void Interpreter::visit(CollectNode *node)
{
    log("Collect");

    try
    {
        collectLetter();
    }
    catch (const InterpreterException &e)
    {
        addError(e.message);
        throw e;
    }
}

void Interpreter::findRobot()
{
    robot = nullptr;
    for (RobotElement *element : elements)
    {
        if (element->type == RobotElement::Robot)
        {
            robot = element;
            break;
        }
    }
}

bool Interpreter::canMoveTo(int x, int y)
{
    // Check for walls and obstacles
    for (RobotElement *element : elements)
    {
        if (element->x == x && element->y == y)
        {
            if (element->type == RobotElement::Wall ||
                element->type == RobotElement::Obstacle)
            {
                return false;
            }
        }
    }
    return true;
}

void Interpreter::moveRobot(Direction direction)
{
    if (!robot)
    {
        throw InterpreterException("Cannot move: robot not found");
    }

    int newX = robot->x;
    int newY = robot->y;

    switch (direction)
    {
    case Direction::UP:
        newY--;
        break;
    case Direction::DOWN:
        newY++;
        break;
    case Direction::LEFT:
        newX--;
        break;
    case Direction::RIGHT:
        newX++;
        break;
    }

    if (!canMoveTo(newX, newY))
    {
        throw InterpreterException(
            QString("Cannot move %1: obstacle or wall at position (%2, %3)")
                .arg(directionToString(direction))
                .arg(newX)
                .arg(newY));
    }

    // Move the robot
    robot->x = newX;
    robot->y = newY;

    // Trigger callback for UI update
    if (stepCallback)
    {
        stepCallback();
    }
}

void Interpreter::collectLetter()
{
    if (!robot)
    {
        throw InterpreterException("Cannot collect: robot not found");
    }

    // Find letter at robot's position
    RobotElement *letter = getElementAt(robot->x, robot->y, RobotElement::Letter);

    if (!letter)
    {
        throw InterpreterException(
            QString("No letter to collect at position (%1, %2)")
                .arg(robot->x)
                .arg(robot->y));
    }

    collectedLetters += letter->letter;
    log(QString("  -> Collected letter '%1'").arg(letter->letter));

    // Remove the letter from the map
    elements.removeOne(letter);
    delete letter;

    // Trigger callback for UI update
    if (stepCallback)
    {
        stepCallback();
    }
}

void Interpreter::log(const QString &message)
{
    executionLog.append(message);
    qDebug() << message;
}

void Interpreter::addError(const QString &message)
{
    errors.append(InterpreterException(message));
}

RobotElement *Interpreter::getElementAt(int x, int y, RobotElement::Type type)
{
    for (RobotElement *element : elements)
    {
        if (element->x == x && element->y == y && element->type == type)
        {
            return element;
        }
    }
    return nullptr;
}

bool Interpreter::hasObstacleAt(int x, int y)
{
    for (RobotElement *element : elements)
    {
        if (element->x == x && element->y == y)
        {
            if (element->type == RobotElement::Wall ||
                element->type == RobotElement::Obstacle)
            {
                return true;
            }
        }
    }
    return false;
}

bool Interpreter::hasLetterAt(int x, int y, const QString &letter)
{
    for (RobotElement *element : elements)
    {
        if (element->x == x && element->y == y &&
            element->type == RobotElement::Letter &&
            element->letter == letter)
        {
            return true;
        }
    }
    return false;
}

bool Interpreter::checkCondition(std::shared_ptr<ConditionNode> condition)
{
    if (!robot)
    {
        throw InterpreterException("Cannot check condition: robot not found");
    }

    // Calculate position to check based on direction
    int checkX = robot->x;
    int checkY = robot->y;

    switch (condition->direction)
    {
    case Direction::UP:
        checkY--;
        break;
    case Direction::DOWN:
        checkY++;
        break;
    case Direction::LEFT:
        checkX--;
        break;
    case Direction::RIGHT:
        checkX++;
        break;
    }

    // Check the condition
    if (condition->isObstacle)
    {
        return hasObstacleAt(checkX, checkY);
    }
    else
    {
        return hasLetterAt(checkX, checkY, condition->target);
    }
}

void Interpreter::visit(UntilNode *node)
{
    log(QString("UNTIL %1 {").arg(node->condition->toString()));

    int iteration = 0;
    const int maxIterations = 1000; // Safety limit

    // Execute body while condition is NOT true
    while (!checkCondition(node->condition))
    {
        iteration++;
        if (iteration > maxIterations)
        {
            throw InterpreterException("UNTIL loop exceeded maximum iterations (possible infinite loop)");
        }

        log(QString("  UNTIL iteration %1").arg(iteration));
        for (const auto &statement : node->body)
        {
            statement->accept(this);
        }
    }

    log(QString("} (UNTIL condition met after %1 iterations)").arg(iteration));
}

void Interpreter::visit(IfNode *node)
{
    log(QString("IF %1").arg(node->condition->toString()));

    if (checkCondition(node->condition))
    {
        log("  -> Condition is TRUE, executing body");
        for (const auto &statement : node->body)
        {
            statement->accept(this);
        }
    }
    else
    {
        log("  -> Condition is FALSE, skipping body");
    }
}
