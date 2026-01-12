#ifndef MYDATE_H
#define MYDATE_H

class MyDate
{
private:
    int year;
    int month;
    int day;
    static std::string str_months[12];
    static std::string str_days[7];
    static int days_in_month[12];
public:
    MyDate(int year, int month, int day);
    static bool is_leap_year(int year);
    static bool is_valid_date(int year, int month, int day);
    static int get_day_of_the_week(int year, int month, int day);
    void set_date(int year, int month, int day);
    void set_month(int month);
    void set_year(int year);
    void set_day(int day);
    int get_year();
    int get_month();
    int get_day();
    std::string to_string();
    void next_day();
    void next_month();
    void next_year();
    void previous_day();
    void previous_month();
    void previous_year();
};

#endif //MYDATE_H