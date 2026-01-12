#pragma once

/*
 * Robot Language Tokenizer - Usage Guide
 * 
 * The tokenizer breaks down robot programs into tokens using Qt6 QRegularExpression.
 * 
 * SUPPORTED TOKENS:
 * - Keyword: REPEAT, MOVE, COLLECT
 * - Direction: UP, DOWN, LEFT, RIGHT
 * - Number: Any integer value
 * - Brackets: { and }
 * 
 * EXAMPLE USAGE:
 * 
 * 1. Create a Tokenizer instance:
 *    Tokenizer tokenizer;
 * 
 * 2. Tokenize your program:
 *    QString program = "REPEAT 2 {\n    MOVE RIGHT\n}\nCOLLECT";
 *    QList<Token> tokens = tokenizer.tokenize(program);
 * 
 * 3. Process the tokens:
 *    for (const Token& token : tokens) {
 *        qDebug() << token.typeToString() << ":" << token.text;
 *    }
 * 
 * EXAMPLE OUTPUT for "REPEAT 2 { MOVE RIGHT }":
 * Keyword : REPEAT
 * Number : 2
 * Brackets : {
 * Keyword : MOVE
 * Direction : RIGHT
 * Brackets : }
 * 
 * INTEGRATION:
 * The tokenizer is already integrated into Roboter.cpp.
 * When you load a program file via "Open Program", it automatically
 * tokenizes the content and prints the results to qDebug().
 * 
 * NEXT STEPS:
 * 1. Parser: Build an AST (Abstract Syntax Tree) from the tokens
 * 2. Interpreter: Execute the parsed commands on the robot
 * 3. Error handling: Add semantic validation (e.g., matching braces)
 */
