#pragma once

struct Roman_Digit
{
    std::string digit;
    int value;
    int next;
};

class Roman_Number
{
private:
    static Roman_Digit single_digits[];
    static Roman_Digit double_digits[];
    int value = 0;
    std::string number = "";

public:
    static int roman_to_dec(std::string roman);
    static std::string dec_to_roman(int dec_value, bool multiplication);
    Roman_Number(int value, bool multiplication);
    Roman_Number(std::string number);
    std::string get_number();
    int get_value();
};