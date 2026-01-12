#ifndef CARD_H
#define CARD_H

enum Suits
{
    Spade,
    Club,
    Heart,
    Diamond
};

class Card
{
private:
    Suits suite;
    int value;
    Card *next = nullptr;
    // Erweiterung
    Card *prev = nullptr;

public:
    Card() {}
    Card(Suits suite, int value);
    void print(int x, int y);
    inline Suits get_suite() { return suite; }
    inline int get_value() { return value; }
    Card *remove_last_card();
    Card *get_last_card();
    void add_card(Card *card);

    // Erweiterung
    ~Card();
};

#endif