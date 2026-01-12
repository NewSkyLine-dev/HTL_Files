#include <iostream>

#include "part.h"

using namespace std;

std::string Part::unicode_letters[26] = {"🅐", "🅑", "🅒", "🅓", "🅔", "🅕", "🅖", "🅗", "🅘", "🅙", "🅚", "🅛", "🅜", "🅝", "🅞", "🅟", "🅠", "🅡", "🅢", "🅣", "🅤", "🅥", "🅦", "🅧", "🅨", "🅩"};

Part::Part(int x, int y, std::string d, int value)
{
    this->x = x;
    this->y = y;
    this->data = d;
    this->value = value;
    if (value < 0 || value > 25)
    {
        value = 0;
    }
}

void Part::eat()
{
    x = 0;
    y = 0;
    // this->data = unicode_letters[value];
}

Part::~Part()
{
    delete next;
}

void Part::draw()
{
    if (x != 0 && y != 0)
    {
        cout << "\033[" << y << ";" << x << "H" << data;
    }
    if (next)
    {
        next->draw();
    }
}

// Das Teil bewegt sich auf die übergebenen x/y-Koordinaten und gibt seine eigenen Koordinaten an das nächsten Teil weiter.
void Part::move(int x, int y)
{
    if (this->next && this->x)
        this->next->move(this->x, this->y);
    if (x != 0 && y != 0)
    {
        this->x = x;
        this->y = y;
    }
}

// Gibt true zurück wenn ein Teil die übergebenen x/y-Koordinaten hat.
bool Part::contains(int x, int y)
{
    if (x == this->x && y == this->y)
        return true;
    if (this->next)
        return this->next->contains(x, y);
    return false;
}

// Fügt einen neuen Teil in die verkettete Liste ein. Wie bei Freecell fügen wir im 1. Teil das neue Teil am Ende ein. (immer return this)
Part *Part::add(Part *p)
{
    if (p->value >= this->value)
    {
        if (this->next)
            this->next = this->next->add(p);
        else
            this->next = p;
        return this;
    }
    else
    {
        p->next = this;
        return p;
    }
}

// Gibt den Teil zurück, der die übergebenen x/y-Koordinaten hat.
Part *Part::get(int x, int y)
{
    if (x == this->x && y == this->y)
        return this;
    if (this->next)
        return this->next->get(x, y);
    return nullptr;
}

// Entfernt den Teil mit den übergebenen x/y-Koordinaten aus der verketteten List.
Part *Part::remove(int x, int y)
{
    Part *p;

    if (x == this->x && y == this->y)
    {
        p = this->next;
        this->next = nullptr;
        return p;
    }
    else
    {
        if (this->next)
            this->next = this->next->remove(x, y);
        return this;
    }
    return nullptr;
}

// Erweiterung
bool Part::contains_all_letters(int v)
{
    int va = v;

    if (v == this->value)
    {
        if (this->value == 25)
            return true;
        va = v + 1;
    }
    else if (v < this->value)
        return false;

    return this->next && this->next->contains_all_letters(va);
}

Part *Part::remove_and_move(int v)
{
    Part *p = nullptr;

    if (v == this->value)
    {
        p = this->next;
        this->next = nullptr;
        if (p)
            p->move(this->x, this->y);
        if (this)
        {
            this->~Part();
            delete p;
        }
        return p;
    }
    else
    {
        if (this->next)
            this->next = this->next->remove(this->x, this->y);
        return this;
    }
}