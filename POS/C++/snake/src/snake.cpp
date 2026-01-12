#include <iostream>
#include <cstdlib>
#include <ctime>

#include "game.h"
#include "snake.h"
#include "conio.h"

using namespace std;

Snake::Snake()
{
    w = get_terminal_width();
    h = get_terminal_height();
    x = w / 2;
    y = h / 2;
    srand(time(nullptr));
    border = new Part(1, 1, "▩");
    border->add(new Part(1, h, "▩"));
    for (int i = 2; i <= w; i++)
    {
        border->add(new Part(i, 1, "▩"));
        border->add(new Part(i, h, "▩"));
    }
    for (int i = 2; i < h; i++)
    {
        border->add(new Part(1, i, "▩"));
        border->add(new Part(w, i, "▩"));
    }
    int v = rand() % 26;
    food = new Part(rand() % (w - 2) + 2, rand() % (h - 2) + 2, string(1, 'A' + v), v);
    for (int i = 0; i < FOOD_AMOUNT; i++)
    {
        int fx = rand() % (w - 2) + 2;
        int fy = rand() % (h - 2) + 2;
        if (food->contains(fx, fy))
        {
            i--;
        }
        else
        {
            v = rand() % 26;
            food = food->add(new Part(fx, fy, string(1, 'A' + v), v));
        }
    }
}

Snake::~Snake()
{
    delete border;
    delete body;
    delete food;
}

int Snake::update()
{
    if (body)
    {
        body->move(x, y);
    }
    switch (dir)
    {
    case South:
        y++;
        break;
    case North:
        y--;
        break;
    case East:
        x++;
        break;
    case West:
        x--;
        break;
    }
    if (border->contains(x, y))
    {
        return -1;
    }
    if (body && body->contains(x, y))
    {
        return -1;
    }
    if (food && food->contains(x, y))
    {
        Part *p = food->get(x, y);
        food = food->remove(x, y);
        p->eat();
        // Add a point, because snake ate a letter
        points++;

        if (body)
        {
            body = body->add(p);
        }
        else
        {
            body = p;
        }
        if (body->contains_all_letters(0))
        {
            this->points += 25;
            for (int i = 0; i < 25; ++i)
                body = body->remove_and_move(i);
        }
        for (int i = 0; i < 150; i++)
        {
            int fx = rand() % (w - 2) + 2;
            int fy = rand() % (h - 2) + 2;
            if (!food->contains(fx, fy) && !body->contains(fx, fy))
            {
                int v = rand() % 26;
                food = food->add(new Part(fx, fy, string(1, 'A' + v), v));
                break;
            }
        }
        return 1;
    }
    return 0;
}

void Snake::draw()
{
    cout << "\033[2J";
    border->draw();
    if (food)
    {
        food->draw();
    }
    if (body)
    {
        body->draw();
    }
    switch (dir)
    {
    case South:
        cout << "\033[" << y << ";" << x << "H▽";
        break;
    case North:
        cout << "\033[" << y << ";" << x << "H△";
        break;
    case East:
        cout << "\033[" << y << ";" << x << "H▷";
        break;
    case West:
        cout << "\033[" << y << ";" << x << "H◁";
        break;
    }
    cout << "\033[" << h << ";" << w << "H";
    cout.flush();
}

void Snake::print_game_over()
{
    std::cout << "\033[" << get_terminal_height() / 2 << ";" << get_terminal_width() / 2
              << "HGame Over! Points: " << this->points << std::endl;
    std::cout << "\033[999;1H";
}