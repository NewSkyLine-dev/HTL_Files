# Calculator WPF - Interpreter Pattern

A WPF calculator application implementing the **Interpreter Pattern** for parsing and evaluating mathematical expressions.

## Features

- **Basic Operations**: `+`, `-`, `*`, `/`
- **Power/Exponentiation**: `^` (right-associative: `2^3^4 = 2^(3^4)`)
- **Parentheses**: `(`, `)` for grouping
- **Variables**: Single lowercase letters `a-z`
- **Unary Negation**: `-5`, `-(2+3)`
- **Decimal Numbers**: `3.14`, `0.5`

## Architecture

The project follows the **Interpreter Pattern** with a clear separation of concerns:

### Token Layer (`/Token`)
- **TokenType.cs**: Enum defining all token types
- **Token.cs**: Token class with type, text, position, and optional numeric value
- **Tokenizer.cs**: Lexer that converts input string to tokens

### AST Layer (`/AST`)
- **Expression.cs**: Abstract base class (Interpreter Pattern's Abstract Expression)
- **NumberExpression.cs**: Terminal expression for numeric literals
- **VariableExpression.cs**: Terminal expression for variable references
- **BinaryExpression.cs**: Non-terminal expression for binary operations (+, -, *, /, ^)
- **UnaryExpression.cs**: Non-terminal expression for unary operations (negation)
- **EvaluationContext.cs**: Context class holding variable values

### Parser Layer (`/Parser`)
- **ParseException.cs**: Exception class for parse errors
- **Parser.cs**: Recursive descent parser building the AST

## ABNF Grammar

```abnf
expression = term *( ("+" / "-") term )
term       = power *( ("*" / "/") power )
power      = unary ["^" power]           ; right-associative
unary      = ["-"] factor
factor     = number / variable / "(" expression ")"
number     = 1*DIGIT ["." 1*DIGIT]
variable   = %x61-7A                      ; a-z
```

## Operator Precedence (lowest to highest)

1. `+`, `-` (Addition, Subtraction)
2. `*`, `/` (Multiplication, Division)
3. `^` (Exponentiation) - right-associative
4. `-` (Unary negation)

## Interpreter Pattern

The Interpreter Pattern is implemented as follows:

```
Expression (Abstract)
├── NumberExpression (Terminal)
├── VariableExpression (Terminal)  
├── BinaryExpression (Non-Terminal)
│   ├── Left: Expression
│   └── Right: Expression
└── UnaryExpression (Non-Terminal)
    └── Operand: Expression
```

Each expression implements the `Interpret(EvaluationContext context)` method:
- **Terminal expressions** return values directly
- **Non-terminal expressions** delegate to their children and combine results

## Usage

1. Enter an expression in the text box (e.g., `2 + 3 * 4`)
2. Click `=` or press Enter to calculate
3. Set variables using the variable panel (e.g., `x = 10`)
4. Use variables in expressions (e.g., `x^2 + 2*x + 1`)
5. Expand the AST panel to see the expression tree

## Examples

| Expression | Result |
|------------|--------|
| `2 + 3 * 4` | 14 |
| `(2 + 3) * 4` | 20 |
| `2^3^2` | 512 (= 2^9) |
| `x^2` (x=5) | 25 |
| `-(-5)` | 5 |
| `3.14 * 2` | 6.28 |

## Building

```bash
dotnet build
dotnet run
```

## Requirements

- .NET 8.0
- Windows (WPF)
