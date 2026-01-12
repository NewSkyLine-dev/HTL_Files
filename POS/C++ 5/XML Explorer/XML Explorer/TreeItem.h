#pragma once

#include <QWidget>
#include "tinyxml2.h"

using namespace tinyxml2;

struct TreeItem {
	enum class Kind { Element, Attribute };
	Kind kind = Kind::Element;

	XMLNode* node = nullptr;
	QString attrName;

	TreeItem* parent = nullptr;
	QVector<TreeItem*> children;

	~TreeItem() { qDeleteAll(children); }
	bool isElement() const { return kind == Kind::Element; }
	bool isAttribute() const { return kind == Kind::Attribute; }
};