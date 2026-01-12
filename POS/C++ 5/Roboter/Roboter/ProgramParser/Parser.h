#pragma once

#include "ASTNode.h"
#include "../Token/token.h"
#include <QList>
#include <QString>
#include <memory>

class ParseException {
public:
    QString message;
    int line;
    int column;

    ParseException(const QString& msg, int ln = 0, int col = 0)
        : message(msg), line(ln), column(col) {}

    QString toString() const {
        return QString("Parse Error at line %1, column %2: %3")
            .arg(line).arg(column).arg(message);
    }
};

class Parser {
public:
    Parser(const QList<Token>& tokens);

    // Parse the token list and return the root AST node
    std::shared_ptr<ProgramNode> parse();

    // Get parse errors
    QList<ParseException> getErrors() const { return errors; }
    bool hasErrors() const { return !errors.empty(); }

private:
    QList<Token> tokens;
    int currentIndex;
    QList<ParseException> errors;

    // Token navigation
    Token& current();
    Token& peek(int offset = 1);
    bool isAtEnd() const;
    void advance();
    bool check(Token::Type type) const;
    bool match(Token::Type type);
    void expect(Token::Type type, const QString& message);

    // Parsing methods
    std::shared_ptr<StatementNode> parseStatement();
    std::shared_ptr<RepeatNode> parseRepeat();
    std::shared_ptr<MoveNode> parseMove();
    std::shared_ptr<CollectNode> parseCollect();
    std::shared_ptr<UntilNode> parseUntil();
    std::shared_ptr<IfNode> parseIf();
    std::shared_ptr<ConditionNode> parseCondition();

    // Error handling
    void addError(const QString& message);
    void synchronize();
};
