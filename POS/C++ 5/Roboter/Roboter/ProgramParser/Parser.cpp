#include "Parser.h"
#include <QDebug>

Parser::Parser(const QList<Token>& tokens)
    : tokens(tokens), currentIndex(0) {
}

std::shared_ptr<ProgramNode> Parser::parse() {
    auto program = std::make_shared<ProgramNode>();
    errors.clear();
    
    while (!isAtEnd()) {
        try {
            auto stmt = parseStatement();
            if (stmt) {
                program->addStatement(stmt);
            }
        } catch (const ParseException& e) {
            errors.append(e);
            synchronize();
        }
    }
    
    return program;
}

Token& Parser::current() {
    if (currentIndex >= tokens.size()) {
        static Token eofToken(Token::Error, "EOF", 0, 0);
        return eofToken;
    }
    return tokens[currentIndex];
}

Token& Parser::peek(int offset) {
    int index = currentIndex + offset;
    if (index >= tokens.size()) {
        static Token eofToken(Token::Error, "EOF", 0, 0);
        return eofToken;
    }
    return tokens[index];
}

bool Parser::isAtEnd() const {
    return currentIndex >= tokens.size();
}

void Parser::advance() {
    if (!isAtEnd()) {
        currentIndex++;
    }
}

bool Parser::check(Token::Type type) const {
    if (isAtEnd()) return false;
    return tokens[currentIndex].type == type;
}

bool Parser::match(Token::Type type) {
    if (check(type)) {
        advance();
        return true;
    }
    return false;
}

void Parser::expect(Token::Type type, const QString& message) {
    if (!check(type)) {
        addError(message);
        throw ParseException(message, current().line, current().column);
    }
    advance();
}

void Parser::addError(const QString& message) {
    errors.append(ParseException(message, current().line, current().column));
}

void Parser::synchronize() {
    advance();

    while (!isAtEnd()) {
        // Try to find the next statement start
        if (check(Token::Keyword)) {
            QString keyword = current().text;
            if (keyword == "REPEAT" || keyword == "MOVE" || keyword == "COLLECT" ||
                keyword == "UNTIL" || keyword == "IF") {
                return;
            }
        }
        advance();
    }
}

std::shared_ptr<StatementNode> Parser::parseStatement() {
    if (!check(Token::Keyword)) {
        addError("Expected a statement keyword (REPEAT, MOVE, COLLECT, UNTIL, or IF)");
        throw ParseException("Expected statement", current().line, current().column);
    }

    QString keyword = current().text;

    if (keyword == "REPEAT") {
        return parseRepeat();
    } else if (keyword == "MOVE") {
        return parseMove();
    } else if (keyword == "COLLECT") {
        return parseCollect();
    } else if (keyword == "UNTIL") {
        return parseUntil();
    } else if (keyword == "IF") {
        return parseIf();
    }

    addError("Unknown keyword: " + keyword);
    throw ParseException("Unknown keyword", current().line, current().column);
}

std::shared_ptr<RepeatNode> Parser::parseRepeat() {
    expect(Token::Keyword, "Expected 'REPEAT'");
    
    if (!check(Token::Number)) {
        addError("Expected number after REPEAT");
        throw ParseException("Expected number", current().line, current().column);
    }
    
    int count = current().text.toInt();
    advance();
    
    expect(Token::Brackets, "Expected '{' after REPEAT count");
    
    auto repeatNode = std::make_shared<RepeatNode>(count);
    
    // Parse statements inside the block
    while (!check(Token::Brackets) && !isAtEnd()) {
        auto stmt = parseStatement();
        if (stmt) {
            repeatNode->addStatement(stmt);
        }
    }
    
    if (!check(Token::Brackets) || current().text != "}") {
        addError("Expected '}' to close REPEAT block");
        throw ParseException("Expected '}'", current().line, current().column);
    }
    advance();
    
    return repeatNode;
}

std::shared_ptr<MoveNode> Parser::parseMove() {
    expect(Token::Keyword, "Expected 'MOVE'");
    
    if (!check(Token::Direction)) {
        addError("Expected direction (UP, DOWN, LEFT, RIGHT) after MOVE");
        throw ParseException("Expected direction", current().line, current().column);
    }
    
    Direction dir = stringToDirection(current().text);
    advance();
    
    return std::make_shared<MoveNode>(dir);
}

std::shared_ptr<CollectNode> Parser::parseCollect() {
    expect(Token::Keyword, "Expected 'COLLECT'");
    return std::make_shared<CollectNode>();
}

std::shared_ptr<ConditionNode> Parser::parseCondition() {
    // Parse: DIRECTION IS-A TARGET
    // e.g., "DOWN IS-A OBSTACLE" or "UP IS-A A"

    if (!check(Token::Direction)) {
        addError("Expected direction (UP, DOWN, LEFT, RIGHT) in condition");
        throw ParseException("Expected direction", current().line, current().column);
    }

    Direction dir = stringToDirection(current().text);
    advance();

    if (!check(Token::Condition) || current().text != "IS-A") {
        addError("Expected 'IS-A' after direction in condition");
        throw ParseException("Expected IS-A", current().line, current().column);
    }
    advance();

    // Check for OBSTACLE or a Letter
    QString target;
    bool isObstacle = false;

    if (check(Token::Target) && current().text == "OBSTACLE") {
        target = "OBSTACLE";
        isObstacle = true;
        advance();
    } else if (check(Token::Letter)) {
        target = current().text;
        isObstacle = false;
        advance();
    } else {
        addError("Expected 'OBSTACLE' or a letter (A, B, etc.) after IS-A");
        throw ParseException("Expected target", current().line, current().column);
    }

    return std::make_shared<ConditionNode>(dir, target, isObstacle);
}

std::shared_ptr<UntilNode> Parser::parseUntil() {
    expect(Token::Keyword, "Expected 'UNTIL'");

    // Parse condition
    auto condition = parseCondition();

    // Expect opening brace
    if (!check(Token::Brackets) || current().text != "{") {
        addError("Expected '{' after UNTIL condition");
        throw ParseException("Expected '{'", current().line, current().column);
    }
    advance();

    auto untilNode = std::make_shared<UntilNode>(condition);

    // Parse statements inside the block
    while (!isAtEnd() && !(check(Token::Brackets) && current().text == "}")) {
        auto stmt = parseStatement();
        if (stmt) {
            untilNode->addStatement(stmt);
        }
    }

    if (!check(Token::Brackets) || current().text != "}") {
        addError("Expected '}' to close UNTIL block");
        throw ParseException("Expected '}'", current().line, current().column);
    }
    advance();

    return untilNode;
}

std::shared_ptr<IfNode> Parser::parseIf() {
    expect(Token::Keyword, "Expected 'IF'");

    // Parse condition
    auto condition = parseCondition();

    // Expect opening brace
    if (!check(Token::Brackets) || current().text != "{") {
        addError("Expected '{' after IF condition");
        throw ParseException("Expected '{'", current().line, current().column);
    }
    advance();

    auto ifNode = std::make_shared<IfNode>(condition);

    // Parse statements inside the block
    while (!isAtEnd() && !(check(Token::Brackets) && current().text == "}")) {
        auto stmt = parseStatement();
        if (stmt) {
            ifNode->addStatement(stmt);
        }
    }

    if (!check(Token::Brackets) || current().text != "}") {
        addError("Expected '}' to close IF block");
        throw ParseException("Expected '}'", current().line, current().column);
    }
    advance();

    return ifNode;
}
