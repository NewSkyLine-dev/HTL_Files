#ifndef RPNCALC_H
#define RPNCALC_H

#include <optional>
#include <stack>
#include <vector>
#include <string>

class RPNCalc
{
public:
    void clear();
    std::vector<double> getStackAsVector() const;
    void push(double v);
    std::optional<double> pop();
    bool add();
    bool sub();
    bool mul();
    bool div(double eps = 1e-18);
    double evaluateExpression(const std::string &expression);

private:
    std::stack<double> stack_;

    template <typename F>
    bool bindOp(F f);
};

#endif // RPNCALC_H
