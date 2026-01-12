#include <iostream>
#include "MyDate.h"

using namespace std;

string MyDate::str_months[12] = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
string MyDate::str_days[7] = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"};
int MyDate::days_in_month[12] = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};

MyDate::MyDate(int year, int month, int day)
{
    is_valid_date(year, month, day);
    set_date(year, month, day);
}

bool MyDate::is_leap_year(int year)
{
    if (year % 4 == 0 && year % 100 != 0 || year % 400 == 0)
        return true;
    return false;
}

bool MyDate::is_valid_date(int year, int month, int day)
{
    if (is_leap_year(year))
    {
        if (day < 0 || day > 29 && month == 2)
        {
            throw std::string("Falscher Tag");
            return false;
        }
    }
    else if (month < 0 || month > 12)
    {
        throw std::string("Falscher Monat");
        return false;
    }
    else if (day < 0 || day > days_in_month[month - 1])
    {
        throw std::string("Falscher Tag");
        return false;
    }
    else if (year < 0)
    {
        throw std::string("Falsches Jahr");
        return false;
    }
    return true;
}

int MyDate::get_day_of_the_week(int year, int month, int day)
{
    static int t[] = {0, 3, 2, 5, 0, 3, 5, 1, 4, 6, 2, 4};
    year -= month < 3;
    return (year + year / 4 - year / 100 + year / 400 + t[month - 1] + day) % 7;
}

void MyDate::set_date(int year, int month, int day)
{
    set_year(year);
    set_month(month);
    set_day(day);
}

void MyDate::set_month(int month)
{
    this->month = month;
}

void MyDate::set_year(int year)
{
    this->year = year;
}

void MyDate::set_day(int day)
{
    this->day = day;
}

int MyDate::get_year()
{
    return year;
}

int MyDate::get_month()
{
    return month;
}

int MyDate::get_day()
{
    return day;
}

string MyDate::to_string()
{
    string current_day_of_week = str_days[MyDate::get_day_of_the_week(year, month, day)];
    string current_month = str_months[month - 1];
    string current_year = std::to_string(year);
    return std::to_string(day) + " " + current_day_of_week + " " + current_month + " " + current_year;
}

void MyDate::next_day()
{
    if (day == days_in_month[month - 1] && month != 12)
    {
        day = 1;
        month++;
    }
    else if (day == days_in_month[11])
    {
        month = 1;
        day = 1;
        year++;
    }
    else
    {
        day++;
        ;
    }
}

void MyDate::next_month()
{
    if (month == 12)
    {
        month = 1;
    }
    else if (is_leap_year(year))
    {
        days_in_month[1] = 29;
        month++;
        day--;
    }
    else if (day == days_in_month[month - 1])
    {
        if (month == 12)
        {
            month = 1;
            year++;
            day = days_in_month[month - 1];
        }
        else
        {
            month++;
            day = days_in_month[month - 1];
        }
    }
    else
    {
        if (day > days_in_month[month])
        {
            day = 30;
        }
        month++;
    }
    days_in_month[1] = 28;
}

void MyDate::next_year()
{
    if (is_leap_year(year))
    {
        day--;
    }
    year++;
}

void MyDate::previous_day()
{
    if (day == 1 && month != 3)
    {
        day = days_in_month[month - 1];
        month--;
    }
    else if (is_leap_year(year) && month == 3 && day == 1)
    {
        day = 29;
        month--;
    }
    else if (!is_leap_year(year) && month == 3 && day == 1)
    {
        day = 28;
        month--;
    }
    else
    {
        day--;
    }
}

void MyDate::previous_year()
{
    year--;
}

void MyDate::previous_month()
{
    if (month == 1)
    {
        month = 12;
        year--;
    }
    else
    {
        month--;
    }
}