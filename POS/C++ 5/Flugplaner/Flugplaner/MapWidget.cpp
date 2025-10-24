#include "MapWidget.h"
#include "Databaser.h"
#include "Models/Airline.h"
#include <QPainter>
#include <QPen>
#include <QFont>
#include <QDebug>

MapWidget::MapWidget(QWidget *parent)
	: QWidget(parent), preferredAirlineId(-1)
{
	loadAirlineAlliances();
}

MapWidget::~MapWidget()
{
}

void MapWidget::setMapImage(const QString &imagePath)
{
	mapImage = QPixmap(imagePath);
	if (mapImage.isNull())
	{
		qWarning() << "Failed to load map image:" << imagePath;
	}
	update();
}

void MapWidget::setFlightPath(const FlightPath &path, int preferredAirline)
{
	currentPath = path;
	preferredAirlineId = preferredAirline;
	update();
}

void MapWidget::loadAirlineAlliances()
{
	airlineAlliances.clear();
	// QxOrm DAO queries require proper template registration
	/*
	try {
		QList<Airline> airlines;
		qx::dao::fetch_all(airlines, nullptr);
		for (const Airline& airline : airlines) {
			airlineAlliances[airline.getId()] = airline.getAlliance();
		}
		qInfo() << "Loaded" << airlines.size() << "airline alliances";
	}
	catch (const std::exception &e) {
		qWarning() << "Error loading airline alliances:" << e.what();
	}
	*/
}

QPoint MapWidget::latLongToPixel(float latitude, float longitude) const
{
	if (mapImage.isNull())
	{
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

void MapWidget::paintEvent(QPaintEvent *event)
{
	QPainter painter(this);

	if (!mapImage.isNull())
	{
		painter.drawPixmap(rect(), mapImage);
	}

	if (currentPath.segments.isEmpty())
	{
		return;
	}

	// Get the preferred airline's alliance if applicable
	int preferredAlliance = (preferredAirlineId >= 0)
								? airlineAlliances.value(preferredAirlineId, -1)
								: -1;

	// Draw flight routes
	painter.setRenderHint(QPainter::Antialiasing);

	for (const FlightSegment &segment : currentPath.segments)
	{
		Airport from = FlyGraph::GetAirport(segment.fromAirportId);
		Airport to = FlyGraph::GetAirport(segment.toAirportId);

		QPoint fromPoint = latLongToPixel(from.getLatitude(), from.getLongitude());
		QPoint toPoint = latLongToPixel(to.getLatitude(), to.getLongitude());

		QColor lineColor;
		if (segment.airlineId == preferredAirlineId)
		{
			lineColor = Qt::red; // Preferred airline
		}
		else if (preferredAlliance >= 0 &&
				 airlineAlliances.value(segment.airlineId, -1) == preferredAlliance)
		{
			lineColor = Qt::blue; // Same alliance
		}
		else
		{
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

	for (const FlightSegment &segment : currentPath.segments)
	{
		Airport from = FlyGraph::GetAirport(segment.fromAirportId);
		Airport to = FlyGraph::GetAirport(segment.toAirportId);

		if (from.getId() != 0 && !drawnAirports.contains(from.getId()))
		{
			QPoint point = latLongToPixel(from.getLatitude(), from.getLongitude());
			painter.drawText(point.x() - 15, point.y() - 5, from.getIata());
			drawnAirports.insert(from.getId());
		}

		if (to.getId() != 0 && !drawnAirports.contains(to.getId()))
		{
			QPoint point = latLongToPixel(to.getLatitude(), to.getLongitude());
			painter.drawText(point.x() - 15, point.y() - 5, to.getIata());
			drawnAirports.insert(to.getId());
		}
	}
}
