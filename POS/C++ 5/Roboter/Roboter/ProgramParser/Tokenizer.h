#pragma once

#include "../Token/token.h"
#include <QString>
#include <QList>
#include <QRegularExpression>

class Tokenizer {
public:
    Tokenizer();
    
    // Tokenize the input program and return a list of tokens
    QList<Token> tokenize(const QString& input);

private:
    struct TokenPattern {
        QRegularExpression regex;
        Token::Type type;
        QString keyword;  // For specific keyword identification
    };

    QList<TokenPattern> patterns;
    
    void initializePatterns();
};
