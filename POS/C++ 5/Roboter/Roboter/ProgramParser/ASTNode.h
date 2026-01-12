#pragma once

#include <QString>
#include <QList>
#include <memory>

// Forward declarations
class ASTNode;
class ProgramNode;
class StatementNode;
class RepeatNode;
class MoveNode;
class CollectNode;
class UntilNode;
class IfNode;
class ConditionNode;

// Visitor pattern for AST traversal
class ASTVisitor {
public:
    virtual ~ASTVisitor() = default;
    virtual void visit(ProgramNode* node) = 0;
    virtual void visit(RepeatNode* node) = 0;
    virtual void visit(MoveNode* node) = 0;
    virtual void visit(CollectNode* node) = 0;
    virtual void visit(UntilNode* node) = 0;
    virtual void visit(IfNode* node) = 0;
};

// Base AST Node
class ASTNode {
public:
    virtual ~ASTNode() = default;
    virtual void accept(ASTVisitor* visitor) = 0;
    virtual QString toString(int indent = 0) const = 0;
    
protected:
    QString getIndent(int level) const {
        return QString(level * 2, ' ');
    }
};

// Direction enum for MOVE commands
enum class Direction {
    UP,
    DOWN,
    LEFT,
    RIGHT
};

inline QString directionToString(Direction dir) {
    switch (dir) {
        case Direction::UP: return "UP";
        case Direction::DOWN: return "DOWN";
        case Direction::LEFT: return "LEFT";
        case Direction::RIGHT: return "RIGHT";
        default: return "UNKNOWN";
    }
}

inline Direction stringToDirection(const QString& str) {
    if (str == "UP") return Direction::UP;
    if (str == "DOWN") return Direction::DOWN;
    if (str == "LEFT") return Direction::LEFT;
    if (str == "RIGHT") return Direction::RIGHT;
    return Direction::UP; // default
}

// Statement Node (base for all statements)
class StatementNode : public ASTNode {
public:
    virtual ~StatementNode() = default;
};

// COLLECT statement
class CollectNode : public StatementNode {
public:
    void accept(ASTVisitor* visitor) override {
        visitor->visit(this);
    }
    
    QString toString(int indent = 0) const override {
        return getIndent(indent) + "COLLECT";
    }
};

// MOVE direction statement
class MoveNode : public StatementNode {
public:
    Direction direction;
    
    MoveNode(Direction dir) : direction(dir) {}
    
    void accept(ASTVisitor* visitor) override {
        visitor->visit(this);
    }
    
    QString toString(int indent = 0) const override {
        return getIndent(indent) + "MOVE " + directionToString(direction);
    }
};

// REPEAT count { statements } block
class RepeatNode : public StatementNode {
public:
    int count;
    QList<std::shared_ptr<StatementNode>> body;
    
    RepeatNode(int cnt) : count(cnt) {}
    
    void accept(ASTVisitor* visitor) override {
        visitor->visit(this);
    }
    
    void addStatement(std::shared_ptr<StatementNode> stmt) {
        body.append(stmt);
    }
    
    QString toString(int indent = 0) const override {
        QString result = getIndent(indent) + "REPEAT " + QString::number(count) + " {\n";
        for (const auto& stmt : body) {
            result += stmt->toString(indent + 1) + "\n";
        }
        result += getIndent(indent) + "}";
        return result;
    }
};

// Condition for UNTIL and IF (e.g., "DOWN IS-A OBSTACLE" or "UP IS-A A")
class ConditionNode {
public:
    Direction direction;    // The direction to check
    QString target;         // "OBSTACLE" or a letter like "A", "B"
    bool isObstacle;        // true if checking for obstacle

    ConditionNode(Direction dir, const QString& tgt, bool obstacle)
        : direction(dir), target(tgt), isObstacle(obstacle) {}

    QString toString() const {
        return directionToString(direction) + " IS-A " + target;
    }
};

// UNTIL condition { statements } loop
class UntilNode : public StatementNode {
public:
    std::shared_ptr<ConditionNode> condition;
    QList<std::shared_ptr<StatementNode>> body;

    UntilNode(std::shared_ptr<ConditionNode> cond) : condition(cond) {}

    void accept(ASTVisitor* visitor) override {
        visitor->visit(this);
    }

    void addStatement(std::shared_ptr<StatementNode> stmt) {
        body.append(stmt);
    }

    QString toString(int indent = 0) const override {
        QString result = getIndent(indent) + "UNTIL " + condition->toString() + " {\n";
        for (const auto& stmt : body) {
            result += stmt->toString(indent + 1) + "\n";
        }
        result += getIndent(indent) + "}";
        return result;
    }
};

// IF condition { statements } conditional
class IfNode : public StatementNode {
public:
    std::shared_ptr<ConditionNode> condition;
    QList<std::shared_ptr<StatementNode>> body;

    IfNode(std::shared_ptr<ConditionNode> cond) : condition(cond) {}

    void accept(ASTVisitor* visitor) override {
        visitor->visit(this);
    }

    void addStatement(std::shared_ptr<StatementNode> stmt) {
        body.append(stmt);
    }

    QString toString(int indent = 0) const override {
        QString result = getIndent(indent) + "IF " + condition->toString() + " {\n";
        for (const auto& stmt : body) {
            result += stmt->toString(indent + 1) + "\n";
        }
        result += getIndent(indent) + "}";
        return result;
    }
};

// Program Node (root of AST)
class ProgramNode : public ASTNode {
public:
    QList<std::shared_ptr<StatementNode>> statements;

    void accept(ASTVisitor* visitor) override {
        visitor->visit(this);
    }

    void addStatement(std::shared_ptr<StatementNode> stmt) {
        statements.append(stmt);
    }

    QString toString(int indent = 0) const override {
        QString result = getIndent(indent) + "Program {\n";
        for (const auto& stmt : statements) {
            result += stmt->toString(indent + 1) + "\n";
        }
        result += getIndent(indent) + "}";
        return result;
    }
};
