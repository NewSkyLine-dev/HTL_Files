#include <cstdlib>
#include <stdexcept>
#include <vector>
#include <sstream>
#include <algorithm>

#include "rpncalc.h"

void RPNCalc::clear()
{
    while (!stack_.empty())
    {
        stack_.pop();
    }
}

std::vector<double> RPNCalc::getStackAsVector() const
{
    std::vector<double> result;
    std::stack<double> temp = stack_;
    while (!temp.empty())
    {
        result.push_back(temp.top());
        temp.pop();
    }
    std::reverse(result.begin(), result.end());
    return result;
}

void RPNCalc::push(double v)
{
    stack_.push(v);
}

std::optional<double> RPNCalc::pop()
{
    if (stack_.empty())
        return std::nullopt;
    double v = stack_.top();
    stack_.pop();
    return v;
}

double RPNCalc::evaluateExpression(const std::string &expression)
{
    std::istringstream iss(expression);
    std::string token;

    while (iss >> token)
    {
        if (token == "+")
        {
            if (!add())
                throw std::runtime_error("Nicht genug Operanden für +");
        }
        else if (token == "-")
        {
            if (!sub())
                throw std::runtime_error("Nicht genug Operanden für -");
        }
        else if (token == "*" || token == "×")
        {
            if (!mul())
                throw std::runtime_error("Nicht genug Operanden für *");
        }
        else if (token == "/")
        {
            if (!div())
                throw std::runtime_error("Nicht genug Operanden für /");
        }
        else
        {
            // Try to parse as number
            try
            {
                std::string normalizedToken = token;
                std::replace(normalizedToken.begin(), normalizedToken.end(), ',', '.');
                double value = std::stod(normalizedToken);
                push(value);
            }
            catch (const std::exception &)
            {
                throw std::runtime_error("Ungültiges Token: " + token);
            }
        }
    }

    if (stack_.empty())
    {
        throw std::runtime_error("Keine Ergebnis auf dem Stack");
    }

    return stack_.top();
}

bool RPNCalc::add()
{
    return bindOp([](double a, double b)
                  { return a + b; });
}

bool RPNCalc::sub()
{
    return bindOp([](double a, double b)
                  { return a - b; });
}

bool RPNCalc::mul()
{
    return bindOp([](double a, double b)
                  { return a * b; });
}

bool RPNCalc::div(double eps)
{
    return bindOp([eps](double a, double b)
                  {
        if (std::abs(b) < eps) throw std::runtime_error("Division durch 0");
        return a / b; });
}

template <typename F>
bool RPNCalc::bindOp(F f)
{
    if (stack_.size() < 2)
        return false;
    double b = stack_.top();
    stack_.pop();
    double a = stack_.top();
    stack_.pop();
    double r = f(a, b);
    stack_.push(r);
    return true;
}
