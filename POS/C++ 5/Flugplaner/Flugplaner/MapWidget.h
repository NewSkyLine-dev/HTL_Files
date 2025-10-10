#pragma once

#include <QWidget>
#include <QPixmap>
#include <QPainter>
#include "Utilities/FlyGraph.h"
#include "Models/Airport.h"

class MapWidget : public QWidget
{
	Q_OBJECT

public:
	MapWidget(QWidget* parent = nullptr);
	~MapWidget();

	void setFlightPath(const FlightPath& path, int preferredAirlineId = -1);
	void setMapImage(const QString& imagePath);

protected:
	void paintEvent(QPaintEvent* event) override;

private:
	QPixmap mapImage;
	FlightPath currentPath;
	int preferredAirlineId;
	QMap<int, int> airlineAlliances;

	QPoint latLongToPixel(float latitude, float longitude) const;
	void loadAirlineAlliances();
};
