#include "level_parser.h"

#include <iostream>

static bool isLetter(const std::string& str) {
	return str.length() == 1 && std::isalpha(static_cast<unsigned char>(str[0]));
}

QList<RobotElement*> level_parser::parse_level(std::string filename, RobotMap* map)
{
	QList<RobotElement*> elements;
	tinyxml2::XMLDocument doc;
	if (doc.LoadFile(filename.c_str()) != tinyxml2::XML_SUCCESS) {
		return elements;
	}
	tinyxml2::XMLElement* root = doc.RootElement();
	if (!root) {
		return elements;
	}

	for (tinyxml2::XMLElement* elem = root->FirstChildElement(); 
		elem != nullptr; 
		elem = elem->NextSiblingElement()) {
		std::string name = elem->Name();

		if (name == "Width") {
			int width = elem->IntText(10);
			elem = elem->NextSiblingElement();

			int height = elem->IntText(10);
			map->setSize(width, height);
		}

		if (name == "Fields") {
			for (tinyxml2::XMLElement* field = elem->FirstChildElement();
				field != nullptr;
				field = field->NextSiblingElement()) 
			{
				RobotElement* robot_element = new RobotElement();
				robot_element->x = field->FirstChildElement("X")->IntText();
				robot_element->y = field->FirstChildElement("Y")->IntText();
				std::string type = field->FirstChildElement("Type")->GetText();
				std::transform(type.begin(), type.end(), type.begin(),
					[](unsigned char c) { return std::tolower(c); });

				if (type == "stone") {
					robot_element->type = RobotElement::Obstacle;
				}
				else if (type == "robot") {
					robot_element->type = RobotElement::Robot;
				}
				else if (isLetter(type)) {
					robot_element->type = RobotElement::Letter;
					robot_element->letter = QString::fromStdString(field->FirstChildElement("Type")->GetText());
				}
				elements.append(robot_element);
			}
		}
	}

	return elements;
}
