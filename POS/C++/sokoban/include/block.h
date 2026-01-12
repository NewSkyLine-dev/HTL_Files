#ifndef BLOCK_H
#define BLOCK_H

enum Block_Type
{
    WALL,
    BOX,
    TARGET,
    PLAYER,
    FLOOR,
    GOAL
};

struct Block
{
    int x;
    int y;
    Block_Type type;
};

#endif