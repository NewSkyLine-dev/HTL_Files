#include "stdafx.h"
#include "Verwalter.h"

void Verwalter::switchImage(QLabel* label, const QString& filename)
{
	if (filename.isEmpty())
	{
		auto random = QRandomGenerator::global()->bounded(images.size());
		QString pixmapPath = QString::fromStdString(images[random].filename);
		QPixmap pixmap(pixmapPath);
		label->setPixmap(pixmap);
	}
	else
	{
		QString pixmapPath = filename;
		QPixmap pixmap(pixmapPath);
		label->setPixmap(pixmap);
	}
}
