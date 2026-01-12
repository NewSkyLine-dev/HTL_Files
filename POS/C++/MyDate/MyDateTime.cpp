#include <iostream>
#include "MyDateTime.h"

using namespace std;

MyDateTime::MyDateTime(int year, int month, int day, int hour, int minute)
{
    // Date
    is_valid_date(year, month, day);
    set_date(year, month, day);

    // Time
    is_valid_time(hour, minute);
    set_time(hour, minute);
}

#pragma mark MyDate
#pragma region MyDate

string MyDateTime::str_months[12] = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
string MyDateTime::str_days[7] = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"};
int MyDateTime::days_in_month[12] = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};

bool MyDateTime::is_leap_year(int year)
{
    if (year % 4 == 0 && year % 100 != 0 || year % 400 == 0)
        return true;
    return false;
}

bool MyDateTime::is_valid_date(int year, int month, int day)
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

int MyDateTime::get_day_of_the_week(int year, int month, int day)
{
    static int t[] = {0, 3, 2, 5, 0, 3, 5, 1, 4, 6, 2, 4};
    year -= month < 3;
    return (year + year / 4 - year / 100 + year / 400 + t[month - 1] + day) % 7;
}

void MyDateTime::set_date(int year, int month, int day)
{
    set_year(year);
    set_month(month);
    set_day(day);
}

void MyDateTime::set_month(int month)
{
    this->month = month;
}

void MyDateTime::set_year(int year)
{
    this->year = year;
}

void MyDateTime::set_day(int day)
{
    this->day = day;
}

int MyDateTime::get_year()
{
    return year;
}

int MyDateTime::get_month()
{
    return month;
}

int MyDateTime::get_day()
{
    return day;
}

string MyDateTime::to_string()
{
    string current_day_of_week = str_days[MyDateTime::get_day_of_the_week(year, month, day)];
    string current_month = str_months[month - 1];
    string current_year = std::to_string(year);
    return std::to_string(day) + " " + current_day_of_week + " " + current_month + " " + current_year + " " + std::to_string(hour) + ":" + std::to_string(minute);
}

void MyDateTime::next_day()
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

void MyDateTime::next_month()
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

void MyDateTime::next_year()
{
    if (is_leap_year(year))
    {
        day--;
    }
    year++;
}

void MyDateTime::previous_day()
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

void MyDateTime::previous_year()
{
    year--;
}

void MyDateTime::previous_month()
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

#pragma endregion MyDate

#pragma mark MyDateTime
#pragma region MyDateTime

bool MyDateTime::is_valid_time(int hour, int minute)
{
    if (hour < 0 || hour > 24)
    {
        throw std::string("Falsche Stunde!");
        return false;
    }
    else if (minute < 0 || minute > 60)
    {
        throw std::string("Falsche Stunde!");
        return false;
    }
    return true;
}

void MyDateTime::set_time(int hour, int minute)
{
    set_hour(hour);
    set_minute(minute);
}

void MyDateTime::set_hour(int hour)
{
    this->hour = hour;
}

void MyDateTime::set_minute(int minute)
{
    this->minute = minute;
}

int MyDateTime::get_hour()
{
    return hour;
}

int MyDateTime::get_minute()
{
    return hour;
}

void MyDateTime::next_hour()
{
    if (hour == 23)
    {
        hour = 0;
        day++;
    }
    else
    {
        hour++;
    }
}

void MyDateTime::next_minute()
{
    if (minute == 59)
    {
        hour++;
        minute = 0;
    }
    else
    {
        minute++;
    }
}

void MyDateTime::previous_hour()
{
    if (hour == 0)
    {
        hour = 23;
        previous_day();
    }
    else
    {
        hour--;
    }
}

void MyDateTime::previous_minute()
{
    if (minute == 0)
    {
        minute = 59;
        hour--;
    }
    else
    {
        minute--;
    }
}

void MyDateTime::add_years(int year)
{
    if (year < 0)
    {
        this->year -= year;
    }
    else
    {
        this->year += year;
    }
}

void MyDateTime::add_months(int months)
{
    if (months >= 0)
    {
        if (month + months <= 12)
        {
            month += months;
        }
        else
        {
            for (int i = 0; i < months; i++)
            {
                next_month();
            }
        }
    }
    else
    {
        months *= -1;
        if (this->month - months >= 1)
        {
            this->month -= months;
        }
        else
        {
            for (int i = 0; i < months; i++)
            {
                previous_month();
            }
        }
    }
}

void MyDateTime::add_days(int days)
{
    if (days >= 0)
    {
        if (day + days <= 12)
        {
            day += days;
        }
        else
        {
            for (int i = 0; i < days; i++)
            {
                next_day();
            }
        }
    }
    else
    {
        days *= -1;
        if (this->day - days >= 1)
        {
            this->day -= days;
        }
        else
        {
            for (int i = 0; i < days; i++)
            {
                previous_day();
            }
        }
    }
}

void MyDateTime::add_hours(int hours)
{
    if (hours >= 0)
    {
        if (hour + hours <= 24)
        {
            hour += hours;
        }
        else
        {
            for (int i = 0; i < hours; i++)
            {
                next_hour();
            }
        }
    }
    else
    {
        hours *= -1;
        if (this->hour - hours >= 1)
        {
            this->hour -= hours;
        }
        else
        {
            for (int i = 0; i < hours; i++)
            {
                previous_hour();
            }
        }
    }
}

void MyDateTime::add_minutes(int minutes)
{
    if (minute >= 0)
    {
        if (minute + minutes <= 60)
        {
            minute += minutes;
        }
        else
        {
            for (int i = 0; i < minutes; i++)
            {
                next_minute();
            }
        }
    }
    else
    {
        minutes *= -1;
        if (this->minute - minute >= 0)
        {
            this->minute -= minute;
        }
        else
        {
            for (int i = 0; i < minute; i++)
            {
                previous_minute();
            }
        }
    }
}

#pragma endregion MyDate