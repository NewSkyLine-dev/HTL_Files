#ifndef MYDATETIME_H
#define MYDATETIME_H

class MyDateTime
{
private:
    int year;
    int month;
    int day;
    int hour; 
    int minute; 
    static std::string str_months[12];
    static std::string str_days[7];
    static int days_in_month[12];
public:
    MyDateTime(int year, int month, int day, int hour, int minute);
    static bool is_leap_year(int year);
    static bool is_valid_date(int year, int month, int day);
    static bool is_valid_time(int hour, int minute); //TODO
    static int get_day_of_the_week(int year, int month, int day);
    void set_date(int year, int month, int day);
    void set_time(int hour, int minute); //TODO
    void set_month(int month);
    void set_year(int year);
    void set_day(int day);
    void set_hour(int hour); //TODO
    void set_minute(int minute); //TODO
    int get_year();
    int get_month();
    int get_day();
    int get_hour(); //TODO
    int get_minute(); //TODO
    std::string to_string();
    void next_day();
    void next_month();
    void next_year();
    void next_hour(); //TODO
    void next_minute(); //TODO
    void previous_day();
    void previous_month();
    void previous_year();
    void previous_hour(); //TODO
    void previous_minute(); //TODO
    void add_years(int year); //TODO
    void add_months(int months); //TODO
    void add_days(int days); //TODO
    void add_hours(int hours); //TODO
    void add_minutes(int minutes); //TODO
};

#endif // MYDATETIME_H