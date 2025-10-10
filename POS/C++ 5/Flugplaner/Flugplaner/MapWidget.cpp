#include "MapWidget.h"
#include "Databaser.h"
#include "Models/Airline.h"
#include <QPainter>
#include <QPen>
#include <QFont>
#include <QDebug>

MapWidget::MapWidget(QWidget* parent)
	: QWidget(parent), preferredAirlineId(-1)
{
	loadAirlineAlliances();
}

MapWidget::~MapWidget()
{
}

void MapWidget::setMapImage(const QString& imagePath)
{
	mapImage = QPixmap(imagePath);
	if (mapImage.isNull()) {
		qWarning() << "Failed to load map image:" << imagePath;
	}
	update();
}

void MapWidget::setFlightPath(const FlightPath& path, int preferredAirline)
{
	currentPath = path;
	preferredAirlineId = preferredAirline;
	update();
}

void MapWidget::loadAirlineAlliances()
{
	airlineAlliances.clear();
	Databaser& db = Databaser::getInstance();
	QList<Airline> airlines = db.runQueryList<Airline>("SELECT * FROM Airline;");
	for (const Airline& airline : airlines) {
		airlineAlliances[airline.id] = airline.alliance;
	}
}

QPoint MapWidget::latLongToPixel(float latitude, float longitude) const
{
	if (mapImage.isNull()) {
		return QPoint(0, 0);
	}

	int width = this->width();
	int height = this->height();

	// Latitude: +90 (top) to -90 (bottom)
	// Longitude: -180 (left) to +180 (right)
	
	// Convert latitude: +90 -> y=0, -90 -> y=height
	int y = static_cast<int>((90.0f - latitude) / 180.0f * height);
	
	// Convert longitude: -180 -> x=0, +180 -> x=width
	int x = static_cast<int>((longitude + 180.0f) / 360.0f * width);

	return QPoint(x, y);
}

void MapWidget::paintEvent(QPaintEvent* event)
{
	QPainter painter(this);
	
	if (!mapImage.isNull()) {
		painter.drawPixmap(rect(), mapImage);
	}

	if (currentPath.segments.isEmpty()) {
		return;
	}

	// Get the preferred airline's alliance if applicable
	int preferredAlliance = (preferredAirlineId >= 0) 
		? airlineAlliances.value(preferredAirlineId, -1) 
		: -1;

	// Draw flight routes
	painter.setRenderHint(QPainter::Antialiasing);
	
	for (const FlightSegment& segment : currentPath.segments) {
		Airport from = FlyGraph::GetAirport(segment.fromAirportId);
		Airport to = FlyGraph::GetAirport(segment.toAirportId);

		QPoint fromPoint = latLongToPixel(from.latitude, from.longitude);
		QPoint toPoint = latLongToPixel(to.latitude, to.longitude);

		QColor lineColor;
		if (segment.airlineId == preferredAirlineId) {
			lineColor = Qt::red; // Preferred airline
		}
		else if (preferredAlliance >= 0 && 
			airlineAlliances.value(segment.airlineId, -1) == preferredAlliance) {
			lineColor = Qt::blue; // Same alliance
		}
		else {
			lineColor = Qt::gray; // Other airlines
		}

		QPen pen(lineColor, 2);
		painter.setPen(pen);
		painter.drawLine(fromPoint, toPoint);
	}

	// Draw airport IATA codes
	painter.setPen(Qt::black);
	QFont font = painter.font();
	font.setBold(true);
	font.setPointSize(10);
	painter.setFont(font);

	QSet<int> drawnAirports;
	
	for (const FlightSegment& segment : currentPath.segments) {
		Airport from = FlyGraph::GetAirport(segment.fromAirportId);
		Airport to = FlyGraph::GetAirport(segment.toAirportId);

		if (from.id != 0 && !drawnAirports.contains(from.id)) {
			QPoint point = latLongToPixel(from.latitude, from.longitude);
			painter.drawText(point.x() - 15, point.y() - 5, from.iata);
			drawnAirports.insert(from.id);
		}

		if (to.id != 0 && !drawnAirports.contains(to.id)) {
			QPoint point = latLongToPixel(to.latitude, to.longitude);
			painter.drawText(point.x() - 15, point.y() - 5, to.iata);
			drawnAirports.insert(to.id);
		}
	}
}
