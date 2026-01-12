#include <iostream>
#include <chrono>

#include "game.h"
#include "conio.h"
#include "snake.h"

typedef std::chrono::high_resolution_clock Clock;

using namespace std;

int main(int, char **)
{
    Snake snake;
    int delay = START_SPEED;
    set_conio_terminal_mode();
    auto next = Clock::now();
    while (true)
    {
        if (Clock::now() > next)
        {
            int status = snake.update();
            if (status == -1)
            {
                // Game over
                snake.draw();
                snake.print_game_over();
                break;
            }
            if (status == 1)
            {
                delay -= SPEED_UP;
                if (delay < MAX_SPEED)
                {
                    delay = MAX_SPEED;
                }
            }
            next += chrono::milliseconds(delay);
            snake.draw();
        }
        if (kbhit())
        {
            char c = getch();
            switch (c)
            {
            case 'a':
                snake.set_direction(West);
                snake.draw();
                break;
            case 'w':
                snake.set_direction(North);
                snake.draw();
                break;
            case 'd':
                snake.set_direction(East);
                snake.draw();
                break;
            case 's':
                snake.set_direction(South);
                snake.draw();
                break;
            case 'q':
                return 0;
                break;
            }
        }
        sleep(0);
    }
}
