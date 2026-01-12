#include <fstream>
#include <iostream>
#include <stdexcept>
// Platz für Includes

#include "sokoban.h"

Sokoban::Sokoban(sf::RenderWindow &_window) : window(_window)
{
    if (!texture.loadFromFile("../data/Sokoban.png"))
        throw std::runtime_error("Could not load resource file");

    sprite_wall.setTexture(texture);
    sprite_wall.setTextureRect(sf::IntRect(128, 0, 64, 64));
    sprite_box.setTexture(texture);
    sprite_box.setTextureRect(sf::IntRect(0, 0, 64, 64));
    sprite_target.setTexture(texture);
    sprite_target.setTextureRect(sf::IntRect(64, 64, 64, 64));
    sprite_floor.setTexture(texture);
    sprite_floor.setTextureRect(sf::IntRect(0, 64, 64, 64));
    sprite_goal.setTexture(texture);
    sprite_goal.setTextureRect(sf::IntRect(64, 0, 64, 64));
    sprite_player[Direction::LEFT].setTexture(texture);
    sprite_player[Direction::LEFT].setTextureRect(sf::IntRect(64, 192, 64, 64));
    sprite_player[Direction::RIGHT].setTexture(texture);
    sprite_player[Direction::RIGHT].setTextureRect(sf::IntRect(0, 128, 64, 64));
    sprite_player[Direction::UP].setTexture(texture);
    sprite_player[Direction::UP].setTextureRect(sf::IntRect(0, 192, 64, 64));
    sprite_player[Direction::DOWN].setTexture(texture);
    sprite_player[Direction::DOWN].setTextureRect(sf::IntRect(64, 128, 64, 64));

    if (!font.loadFromFile("../data/GistLight.otf"))
        throw std::runtime_error("Could not load font file");

    text.setFont(font);
    text.setFillColor(sf::Color::Red);
    text.setCharacterSize(30);
}

void Sokoban::draw_block(Block &b)
{
    sf::Sprite block;
    switch (b.type)
    {
    case Block_Type::BOX:
        block = sprite_box;
        break;
    case Block_Type::WALL:
        block = sprite_wall;
        break;
    case Block_Type::TARGET:
        block = sprite_target;
        break;
    case Block_Type::FLOOR:
        block = sprite_floor;
        break;
    case Block_Type::GOAL:
        block = sprite_goal;
        break;
    case Block_Type::PLAYER:
        block = sprite_player[player_dir];
        break;
    }
    int pos_x = b.x * 64 - (player.x > 8 ? (player.x - 9) * 64 : -64);
    int pos_y = b.y * 64 - (player.y > 6 ? (player.y - 7) * 64 : -64);
    if (pos_y > 0 && pos_x > 0)
    {
        block.setPosition(pos_x, pos_y);
        window.draw(block);
    }
}

void Sokoban::key_pressed(sf::Keyboard::Key k)
{
    switch (k)
    {
    case sf::Keyboard::Left:

        break;
    case sf::Keyboard::Right:

        break;
    case sf::Keyboard::Up:

        break;
    case sf::Keyboard::Down:

        break;
    }
}

void Sokoban::key_released(sf::Keyboard::Key k)
{
    if (is_level_finished())
    {
        load_level();
        return;
    }
    switch (k)
    {
    case sf::Keyboard::Left:
        move_player(Direction::LEFT);
        break;
    case sf::Keyboard::Right:
        move_player(Direction::RIGHT);
        break;
    case sf::Keyboard::Up:
        move_player(Direction::UP);
        break;
    case sf::Keyboard::Down:
        move_player(Direction::DOWN);
        break;
    case sf::Keyboard::A:
        move_player(Direction::LEFT);
        break;
    case sf::Keyboard::D:
        move_player(Direction::RIGHT);
        break;
    case sf::Keyboard::W:
        move_player(Direction::UP);
        break;
    case sf::Keyboard::S:
        move_player(Direction::DOWN);
        break;
    }
}

void Sokoban::move_player(Direction dir)
{
    int new_x = player.x;
    int new_y = player.y;
    switch (dir)
    {
    case Direction::LEFT:
        new_x--;
        break;
    case Direction::RIGHT:
        new_x++;
        break;
    case Direction::DOWN:
        new_y++;
        break;
    case Direction::UP:
        new_y--;
        break;
    }
    if (can_move(new_x, new_y) || (is_box(new_x, new_y) && move_box(new_x, new_y, dir)))
    {
        // Move the player
        player.x = new_x;
        player.y = new_y;
        player_dir = dir;
    }
}

bool Sokoban::move_box(int x, int y, Direction dir)
{
    /* Falls die Box in diese Richtung verschiebbar ist, die Box verschieben und true zurück geben*/
    int new_x = x;
    int new_y = y;
    switch (dir)
    {
    case Direction::LEFT:
        new_x--;
        break;
    case Direction::RIGHT:
        new_x++;
        break;
    case Direction::DOWN:
        new_y++;
        break;
    case Direction::UP:
        new_y--;
        break;
    }
    if (can_move(new_x, new_y))
    {
        levels_arr[current_level][y][x] = ' ';
        levels_arr[current_level][new_y][new_x] = '$';
        return true;
    }
    return false;
}

bool Sokoban::is_box(int x, int y)
{
    /* Prüfen ob an dieser Position eine Box ist*/
    if (levels_arr[current_level][y][x] == '$' || levels_arr[current_level][y][x] == '*')
    {
        return true;
    }
    return false;
}

bool Sokoban::has_target(int x, int y)
{
    /* Prüfen ob an dieser Position eine Target ist*/
    if (levels_arr[current_level][y][x] == '.' || levels_arr[current_level][y][x] == '*')
    {
        return true;
    }
    return false;
}

bool Sokoban::can_move(int x, int y)
{
    /*Prüfen ob diese Position betretbar ist (Kein Wall und keine Box)*/
    if (levels_arr[current_level][y][x] == '#' || levels_arr[current_level][y][x] == '$')
    {
        return false;
    }
    return true;
}

/**
 * @param # fügt einen Wand-Block ein
 * @param @ fügt einen Boden-Block ein und setzt die Postion des player-Blocks auf diese Position
 * @param $ fügt einen Boden-Block und einen Kisten-Block ein
 * @param . fügt einen Ziel-Block ein
 * @param * fügt einen Ziel-Block und einem Kisten-Block ein
 * @param + fügt einen Ziel-Block ein und setzt die Postion des player-Blocks auf diese Position eine Leerstelle fügt
 * einen Boden-Block ein
 */
void Sokoban::draw()
{
    window.clear();

    text.setString("Title: " + title);
    text.setPosition(50, 10);
    window.draw(text);
    text.setString("Level: " + std::to_string(current_level));
    text.setPosition(400, 10);
    window.draw(text);

    // Check if the current level has changed
    static int previous_level = -1; // Keep track of the previous level
    if (previous_level != current_level)
    {
        // Block storage to store the blocks
        std::vector<std::vector<Block>> block_storage;

        for (int y = 0; y < levels_arr[current_level].size(); y++)
        {
            const std::string &level_row = levels_arr[current_level][y];

            std::vector<Block> row_blocks;

            // Iterate over the characters in the row
            for (int x = 0; x < level_row.size(); x++)
            {
                char character = level_row[x];

                // Create a new block
                Block block;
                block.x = x;
                block.y = y;

                switch (character)
                {
                case '#':
                    block.type = Block_Type::WALL;
                    break;
                case '@':
                    block.type = Block_Type::FLOOR;
                    player.x = x;
                    player.y = y;
                    break;
                case '$':
                    block.type = Block_Type::BOX;
                    break;
                case '.':
                    block.type = Block_Type::TARGET;
                    break;
                case '*':
                    block.type = Block_Type::GOAL;
                    break;
                case '+':
                    block.type = Block_Type::GOAL;
                    player.x = x;
                    player.y = y;
                    break;
                case ' ':
                    block.type = Block_Type::FLOOR;
                    break;
                default:
                    block.type = Block_Type::FLOOR;
                    break;
                }

                row_blocks.push_back(block);

                // Draw the block
                draw_block(block);
            }
            block_storage.push_back(row_blocks);
        }

        // Update the previous level
        previous_level = current_level;

        // Store the block storage for the current level
        level_blocks = std::move(block_storage);
    }
    else
    {
        // Redraw the stored blocks for the current level
        for (const auto &row_blocks : level_blocks)
        {
            for (auto block : row_blocks)
            {
                // Draw the block
                draw_block(block);
            }
        }
    }

    // Draw the player block
    draw_block(player);

    window.display();
}

void Sokoban::start()
{
    // Den Titel und die Levels aus der Datei ../data/Boxxle1.slc wie in der Angabe beschrieben laden
    std::ifstream level_file("../data/Boxxle1.slc");
    if (!level_file.is_open())
    {
        throw std::runtime_error("Could not open level file");
    }
    std::string data = "";
    std::string line = "";
    while (std::getline(level_file, line))
    {
        data += line;
    }
    level_file.close();
    size_t title_start = find_first_xml_tag(data, "Title", 0, false);
    size_t title_end = find_first_xml_tag(data, "Title", title_start, true);
    this->title = data.substr(title_start, title_end - title_start);

    // Level data
    size_t offset = 0;
    while (true)
    {
        auto end_offset = data.find("<Level ", offset);
        offset = data.find('>', end_offset);
        if (offset == std::string::npos)
        {
            break;
        }

        size_t end_tag_offset = data.find("</Level>", offset);
        std::string tag_dbg = data.substr(end_offset, offset - end_offset);
        size_t height_start = tag_dbg.find("Height=\"") + 8; // Add 8 to skip "Height=\""
        size_t height_end = tag_dbg.find("\"", height_start);

        std::string height_str = tag_dbg.substr(height_start, height_end - height_start);
        int height = std::stoi(height_str);

        if (end_tag_offset == std::string::npos)
        {
            break;
        }

        std::string level_data = data.substr(offset + 1, end_tag_offset - offset + 8);
        std::vector<std::string> layout;

        size_t layout_start = level_data.find('>') + 1;
        size_t layout_end = level_data.find("</L>", layout_start);
        for (int i = 0; i < height; i++)
        {
            std::string layout_line = std::string(level_data.substr(layout_start, layout_end - layout_start));
            layout.push_back(layout_line);
            layout_start = level_data.find("<L>", layout_end) + 3;
            layout_end = level_data.find("</L>", layout_start);
        }

        this->levels_arr.push_back(std::move(layout));
        offset = end_tag_offset + 8;
    }
}

/**
 * @brief Die Funktion ermittelt die Position des ersten passenden XML-Tags aus dem String
 *
 * @param xml Komplette XML Datei als String
 * @param tag Gesuchter Tag nach dem `<` "<Tag>"
 * @param offset Offset ab dem gesucht werden soll
 * @param after_tag Es gibt ein `</Tag>`
 * @return size_t Index des ersten Zeichens nach dem Tag
 */
size_t Sokoban::find_first_xml_tag(std::string xml, std::string tag, size_t offset, bool after_tag)
{
    if (!after_tag)
    {
        size_t starting_tag = xml.find("<" + tag + ">", offset);
        if (starting_tag == std::string::npos)
        {
            starting_tag = xml.find("<" + tag + " ", offset);
        }
        size_t second_tag_index = xml.find(">", starting_tag);
        std::string tag_content = xml.substr(starting_tag, second_tag_index - starting_tag);
        return second_tag_index + 1;
    }
    else
    {
        return xml.find("</" + tag + ">", offset);
    }
}

void Sokoban::load_level()
{
    current_level = 0;
    draw();
}

bool Sokoban::is_level_finished()
{
    // Prüfen ob an der Position jedes Targets eine Box ist
    /*Die Funktion is_level_finished soll true zurück geben, wenn an der Position jedes Ziels auch eine Kiste ist.*/
    for (int y = 0; y < levels_arr[current_level].size(); y++)
    {
        const std::string &level_row = levels_arr[current_level][y];
        for (int x = 0; x < level_row.size(); x++)
        {
            char character = level_row[x];
            if (character == '.' && !is_box(x, y))
            {
                return false;
            }
        }
    }
    return true;
}