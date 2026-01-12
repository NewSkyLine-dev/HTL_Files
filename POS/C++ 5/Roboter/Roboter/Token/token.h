#ifndef TOKEN_H
#define TOKEN_H

#include <QString>

class Token
{
public:
    enum Type { 
        Keyword,      // REPEAT, MOVE, COLLECT, UNTIL, IF
        Letter,       // General letters (A, B, C, etc.)
        Number,       // Numeric values
        Brackets,     // { }
        Direction,    // UP, DOWN, LEFT, RIGHT
        Condition,    // IS-A
        Target,       // OBSTACLE
        Newline,      // Line breaks
        Error         // Unknown tokens
    };

    QString text = "Missing Token";
    Type type = Error;
    int line = 1;
    int column = 1;

    // Constructor
    Token(Type t = Error, const QString& txt = "Missing Token", int ln = 1, int col = 1)
        : type(t), text(txt), line(ln), column(col) {}

    // Helper method to convert type to string for debugging
    QString typeToString() const {
        switch (type) {
            case Keyword: return "Keyword";
            case Letter: return "Letter";
            case Number: return "Number";
            case Brackets: return "Brackets";
            case Direction: return "Direction";
            case Condition: return "Condition";
            case Target: return "Target";
            case Newline: return "Newline";
            case Error: return "Error";
            default: return "Unknown";
        }
    }
};

#endif // TOKEN_H
