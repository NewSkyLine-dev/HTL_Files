#pragma once
#include <QLabel>
#include <QString>
#include <Image.h>

class Verwalter
{
public:
	QList<Image> images;
	void switchImage(QLabel* label, const QString& filename);
};

