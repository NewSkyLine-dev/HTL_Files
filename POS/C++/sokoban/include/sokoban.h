#ifndef DANCE_H
#define DANCE_H

#include <list>
#include <string>
// Hier kommen die notwendigen Includes hin
#include <vector>

#include "SFML/Graphics.hpp"
#include "block.h"

enum Direction
{
    LEFT,
    RIGHT,
    UP,
    DOWN
};

class Sokoban
{
  private:
    sf::RenderWindow &window;
    sf::Texture texture;
    sf::Sprite sprite_wall;
    sf::Sprite sprite_box;
    sf::Sprite sprite_goal;
    sf::Sprite sprite_floor;
    sf::Sprite sprite_target;
    sf::Sprite sprite_player[4];
    sf::Font font;
    sf::Text text;
    Block player{.type = Block_Type::PLAYER};
    Direction player_dir = Direction::RIGHT;

    std::string title = "Title here";
    std::string current_pack = "Boxxle1";
    int current_level = 0;

    // Variablen für die Blöcke und Levels anlegen
    std::vector<std::string> levels;
    std::vector<Block> blocks;

    void draw_block(Block &b);
    size_t find_first_xml_tag(std::string xml, std::string tag, size_t offset, bool after_tag = false);
    void load_level();
    bool move_box(int x, int y, Direction dir);
    bool is_box(int x, int y);
    bool has_target(int x, int y);
    bool can_move(int x, int y);
    bool is_level_finished();
    void move_player(Direction dir);
    void reset_level();
    void switch_pack();
    void load_progress();
    void save_progress();

  public:
    explicit Sokoban(sf::RenderWindow &_window);
    void start();
    void draw();
    void key_pressed(sf::Keyboard::Key k);
    void key_released(sf::Keyboard::Key k);
};

#endif