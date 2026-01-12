#pragma once

#include "tinyxml2.h"
#include "robotelement.h"
#include <QList>
#include "robotmap.h"

class level_parser
{
public:
	QList<RobotElement*> parse_level(std::string filename, RobotMap* map);
};

