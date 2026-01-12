# RoboterWPF

A C#/WPF port of the Qt-based Robot programming language application, implementing the **Interpreter Design Pattern**.

## Overview

This application allows you to control a robot on a grid map using a simple programming language. The robot can move in four directions (UP, DOWN, LEFT, RIGHT), collect letters, and respond to conditions about its environment.

## Design Pattern: Interpreter Pattern

This application implements the classic **Interpreter Pattern** from the Gang of Four (GoF) design patterns. The pattern is used to define a grammar for a simple language and interpret sentences in that language.

### How the Interpreter Pattern is Implemented

```
┌─────────────────────────────────────────────────────────────────┐
│                        Client (MainWindow)                       │
│                               │                                  │
│                               ▼                                  │
│                    ┌──────────────────┐                         │
│                    │ InterpreterContext │  (Holds execution state)│
│                    └──────────────────┘                         │
│                               │                                  │
│                               ▼                                  │
│           ┌───────────────────────────────────────┐             │
│           │     AbstractExpression (Expression)    │             │
│           │         + Interpret(context)           │             │
│           └───────────────────────────────────────┘             │
│                      ▲                 ▲                        │
│          ┌───────────┘                 └───────────┐            │
│          │                                         │            │
│  ┌───────────────────┐               ┌─────────────────────┐   │
│  │ TerminalExpression │               │NonTerminalExpression│   │
│  │  - MoveExpression  │               │ - ProgramExpression │   │
│  │  - CollectExpression│              │ - RepeatExpression  │   │
│  │  - ConditionExpr.  │               │ - UntilExpression   │   │
│  └───────────────────┘               │ - IfExpression      │   │
│                                       └─────────────────────┘   │
└─────────────────────────────────────────────────────────────────┘
```

### Key Components

1. **AbstractExpression (`Expression`)**: Base class with abstract `Interpret(context)` method
2. **TerminalExpression**: Expressions that interpret themselves directly
   - `MoveExpression`: Executes robot movement
   - `CollectExpression`: Collects a letter at robot's position
   - `ConditionExpression`: Evaluates a boolean condition
3. **NonTerminalExpression**: Expressions that contain child expressions (Composite pattern)
   - `ProgramExpression`: Root node, contains all top-level statements
   - `RepeatExpression`: Loop that forwards interpret to children N times
   - `UntilExpression`: Loop that forwards interpret while condition is false
   - `IfExpression`: Conditional that forwards interpret only if condition is true
4. **Context (`InterpreterContext`)**: Holds the execution state (robot position, collected letters, map elements)

### How Interpretation Works

```csharp
// Client creates context and calls Interpret() on the AST root
var context = new InterpreterContext(elements);
ast.Interpret(context);  // AST interprets itself!

// Inside ProgramExpression.Interpret():
public override void Interpret(InterpreterContext context)
{
    // Non-terminal: forwards to all child expressions
    foreach (var statement in Statements)
    {
        statement.Interpret(context);
    }
}

// Inside MoveExpression.Interpret():
public override void Interpret(InterpreterContext context)
{
    // Terminal: directly performs the action
    context.MoveRobot(Direction);
}
```

## Project Structure

```
RoboterWPF/
├── App.xaml                    # Application entry point
├── App.xaml.cs
├── MainWindow.xaml             # Main window UI (Client)
├── MainWindow.xaml.cs
├── RoboterWPF.csproj           # Project file
├── Controls/
│   ├── RobotMap.xaml           # Custom control for map rendering
│   └── RobotMap.xaml.cs
├── Models/
│   ├── RobotElement.cs         # Model for map elements
│   └── LevelParser.cs          # XML level file parser
├── Parser/
│   ├── ASTNode.cs              # Expression classes (Interpreter Pattern)
│   ├── InterpreterContext.cs   # Context holding execution state
│   └── Parser.cs               # Parser that builds AST from tokens
├── Token/
│   ├── Token.cs                # Token class definition
│   └── Tokenizer.cs            # Lexer that converts text to tokens
└── data/                       # Sample level and program files
```

## Language Syntax (Grammar)

```
program     → statement*
statement   → move | collect | repeat | until | if
move        → "MOVE" direction
collect     → "COLLECT"
repeat      → "REPEAT" number "{" statement* "}"
until       → "UNTIL" condition "{" statement* "}"
if          → "IF" condition "{" statement* "}"
condition   → direction "IS-A" target
direction   → "UP" | "DOWN" | "LEFT" | "RIGHT"
target      → "OBSTACLE" | letter
```

### Example Programs

**Simple movement and collection:**
```
REPEAT 3 {
    MOVE RIGHT
}
MOVE DOWN
COLLECT
```

**Using conditions:**
```
UNTIL DOWN IS-A OBSTACLE {
    MOVE DOWN
}
IF RIGHT IS-A A {
    MOVE RIGHT
    COLLECT
}
```

## Level File Format

Levels are defined in XML format:

```xml
<?xml version="1.0" encoding="utf-8"?>
<XML_Field>
  <Width>10</Width>
  <Height>10</Height>
  <Fields>
    <XML_Cell>
      <X>1</X>
      <Y>1</Y>
      <Type>robot</Type>
    </XML_Cell>
    <XML_Cell>
      <X>5</X>
      <Y>5</Y>
      <Type>A</Type>
    </XML_Cell>
    <XML_Cell>
      <X>3</X>
      <Y>3</Y>
      <Type>stone</Type>
    </XML_Cell>
  </Fields>
</XML_Field>
```

### Element Types
- `robot` - The robot's starting position
- `stone` - An obstacle the robot cannot pass through
- Single letters (A-Z) - Collectible items

## Building and Running

### Requirements
- .NET 8.0 SDK
- Windows (WPF is Windows-only)

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run
```

## Usage

1. **Load a Level**: File → Open Level, then select an XML level file
2. **Load or Write a Program**: Either load a program file (File → Open Program) or type directly in the text editor
3. **Execute**: Click "Execute Program" to run the program
4. The robot will animate through each step with a 300ms delay
5. A dialog will show the result, including any collected letters

## Interpreter Pattern Benefits

1. **Easy to extend**: Adding new commands only requires creating a new Expression subclass
2. **Grammar in code**: The class hierarchy directly mirrors the language grammar
3. **Self-interpreting AST**: Each node knows how to execute itself
4. **Separation of concerns**: Context holds state, Expressions hold behavior

## Original Project

This is a C#/WPF port of a Qt/C++ application. The original used:
- Qt Widgets for UI
- TinyXML2 for XML parsing
- QRegularExpression for tokenization
- Visitor Pattern for interpretation

The C# version uses:
- WPF for UI
- System.Xml.Linq for XML parsing
- System.Text.RegularExpressions for tokenization
- **Interpreter Pattern** for interpretation (each AST node interprets itself)