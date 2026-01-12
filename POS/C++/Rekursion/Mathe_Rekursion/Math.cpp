#include "Math.h"

double Math::power(double x, int n)
{
    if (x != 0)
        return (x * power(x, n - 1));
    else
        return 1;
}

long Math::fact(int n)
{
    if (n > 1)
        return n * fact(n - 1);
    else
        return 1;
}

int Math::ggt(int a, int b)
{
    if (b == 0)
        return a;
    else
        return ggt(b, a % b);
}

int Math::fib(int n)
{
    if (n < 2)
        return n;
    else
        return fib(n - 1) + fib(n - 2);
}
