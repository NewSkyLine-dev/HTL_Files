#include "sokoban.h"

int main()
{
    sf::RenderWindow window(sf::VideoMode(768, 640), "Sokoban");
    Sokoban sokoban(window);

    sokoban.start();

    while (window.isOpen())
    {
        sf::Event event;
        while (window.pollEvent(event))
        {
            if (event.type == sf::Event::Closed)
            {
                window.close();
            }
            if (event.type == sf::Event::KeyPressed)
            {
                sokoban.key_pressed(event.key.code);
            }
            if (event.type == sf::Event::KeyReleased)
            {
                sokoban.key_released(event.key.code);
            }
        }
        sokoban.draw();
    }
    return 0;
}