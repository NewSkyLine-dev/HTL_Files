#include <iostream>
#include <string>
#include "roman_number.h"

using namespace std;

Roman_Digit Roman_Number::single_digits[] = {"M", 1000, 1000, "D", 500, 100, "C", 100, 100, "L", 50, 10, "X", 10, 10, "V", 5, 1, "I", 1, 1};
Roman_Digit Roman_Number::double_digits[] = {"CM", 900, 90, "CD", 400, 90, "XC", 90, 9, "XL", 40, 9, "IX", 9, 0, "IV", 4, 0, "", 0, 0};

string Roman_Number::dec_to_roman(int dec_value, bool multiplication = false)
{
    if (dec_value <= 0)
    {
        throw std::string("Decimal value must be greater than 0");
    }

    std::string result;
    int remaining_decimal = dec_value;

    while (remaining_decimal > 0)
    {
        bool found = false;

        // Search for the largest Roman digit (from both arrays) whose value is less than or equal to the remaining decimal value.
        for (auto const &rd : double_digits)
        {
            if (rd.value == remaining_decimal)
            {
                result += rd.digit;
                remaining_decimal -= rd.value;
                found = true;
                break;
            }
        }
        if (!found)
        {
            for (auto const &rd : single_digits)
            {
                if (rd.value <= remaining_decimal)
                {
                    result += rd.digit;
                    remaining_decimal -= rd.value;
                    break;
                }
            }
        }
    }
    if (multiplication)
    {
        if (dec_value >= 1000)
        { 
            /*
            result = CCXII
            end_result = ""
            C is two times
            make substring where C ends:
            if (result[current+1] != result[current])
                make substring where result isnt the same
                for (how often C was in there)
                    end_result += result[current]
            */
            string end_result = "";
            int current = 1;
            while (!result.empty())
            {
                if (result[current-1] == result[current])
                {
                    current++;
                }
                else
                {
                    end_result += dec_to_roman(current, false) + " * " + result[0] + ". ";
                    result = result.substr(current);
                    current = 1;
                }
            }
            return end_result;
        }
        else 
            return result;
    }
    return result;
}

int Roman_Number::roman_to_dec(string roman)
{
    int decimal = 0;
    int next_value = 1000;
    std::string original = roman;
    while (!roman.empty())
    {
        std::string digit = roman.substr(0, 2);
        bool found = false;

        for (auto const &rd : double_digits)
        {
            if (rd.digit == digit)
            {
                if (rd.value > next_value)
                    throw std::string("Falsche römische Zahl");

                decimal += rd.value;
                roman.erase(0, 2);
                next_value = rd.next;
                found = true;
                break;
            }
        }
        if (!found)
        {
            digit = roman.substr(0, 1);
            for (auto const &rd : single_digits)
            {
                if (rd.digit == digit)
                {
                    if (rd.value > next_value)
                        throw std::string("Falsche römische Zahl");

                    decimal += rd.value;
                    roman.erase(0, 1);
                    next_value = rd.next;
                    found = true;
                    break;
                }
            }
        }
        if (!found)
            throw std::string("Falsche römische Zahl");
    }
    return decimal;
}

Roman_Number::Roman_Number(int value, bool multiplication)
{
    try
    {
        this->value = value;
        this->number = dec_to_roman(value, multiplication);
    }
    catch (const string &e)
    {
        cerr << "Nummer kann nicht unter 0 sein" << endl;
        exit(1);
    }
}

Roman_Number::Roman_Number(string number)
{
    try
    {
        this->number = number;
        this->value = roman_to_dec(number);
    }
    catch (const string &e)
    {
        cerr << "Falsche Römische Zahl" << endl;
        exit(1);
    }
}

string Roman_Number::get_number()
{
    return this->number;
}

int Roman_Number::get_value()
{
    return this->value;
}
