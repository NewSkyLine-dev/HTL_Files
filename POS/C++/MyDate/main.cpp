#include <iostream>
#include "MyDateTime.h"

using namespace std;

int main(int, char **)
{
    try
    {
        MyDateTime m(2023, 1, 1, 1, 1);
        m.add_hours(-3);
        cout << m.to_string() << endl;
    }
    catch (const string &e)
    {
        cerr << e << endl;
    }

    return 0;
}
