#include "Tokenizer.h"
#include <QRegularExpressionMatch>

Tokenizer::Tokenizer() {
    initializePatterns();
}

void Tokenizer::initializePatterns() {
    // Order matters: more specific patterns should come first

    // Keywords
    patterns.append({QRegularExpression("\\bREPEAT\\b"), Token::Keyword, "REPEAT"});
    patterns.append({QRegularExpression("\\bMOVE\\b"), Token::Keyword, "MOVE"});
    patterns.append({QRegularExpression("\\bCOLLECT\\b"), Token::Keyword, "COLLECT"});
    patterns.append({QRegularExpression("\\bUNTIL\\b"), Token::Keyword, "UNTIL"});
    patterns.append({QRegularExpression("\\bIF\\b"), Token::Keyword, "IF"});

    // Condition keyword
    patterns.append({QRegularExpression("\\bIS-A\\b"), Token::Condition, "IS-A"});

    // Target types (OBSTACLE must come before single letters)
    patterns.append({QRegularExpression("\\bOBSTACLE\\b"), Token::Target, "OBSTACLE"});

    // Directions
    patterns.append({QRegularExpression("\\b(UP|DOWN|LEFT|RIGHT)\\b"), Token::Direction, ""});

    // Numbers
    patterns.append({QRegularExpression("\\b\\d+\\b"), Token::Number, ""});

    // Single letters (for IS-A A, IS-A B, etc.) - must come after other keywords
    patterns.append({QRegularExpression("\\b[A-Z]\\b"), Token::Letter, ""});

    // Braces
    patterns.append({QRegularExpression("[\\{\\}]"), Token::Brackets, ""});
}

QList<Token> Tokenizer::tokenize(const QString& input) {
    QList<Token> tokens;
    int line = 1;
    int column = 1;
    int position = 0;
    
    while (position < input.length()) {
        QChar currentChar = input[position];
        
        // Skip whitespace (except newlines for line tracking)
        if (currentChar.isSpace()) {
            if (currentChar == '\n') {
                line++;
                column = 1;
            } else {
                column++;
            }
            position++;
            continue;
        }
        
        // Try to match patterns
        bool matched = false;
        
        for (const auto& pattern : patterns) {
            QRegularExpressionMatch match = pattern.regex.match(input, position, QRegularExpression::NormalMatch, QRegularExpression::AnchorAtOffsetMatchOption);
            
            if (match.hasMatch() && match.capturedStart() == position) {
                QString value = match.captured(0);
                Token token(pattern.type, value, line, column);
                tokens.append(token);
                
                position += value.length();
                column += value.length();
                matched = true;
                break;
            }
        }
        
        if (!matched) {
            // Unknown character, create an Error token
            QString unknownChar = QString(input[position]);
            Token token(Token::Error, unknownChar, line, column);
            tokens.append(token);
            position++;
            column++;
        }
    }
    
    return tokens;
}
