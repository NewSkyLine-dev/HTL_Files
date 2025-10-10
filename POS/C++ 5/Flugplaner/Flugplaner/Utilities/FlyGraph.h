#pragma once

#include <QMap>
#include <QList>
#include <QPair>
#include "../Models/Airport.h"
#include "../Models/Route.h"

struct FlightSegment {
	int fromAirportId;
	int toAirportId;
	int airlineId;

	FlightSegment(int from, int to, int airline) 
		: fromAirportId(from), toAirportId(to), airlineId(airline) {}
};

struct FlightPath {
	QList<FlightSegment> segments;
	int stopCount;

	FlightPath() : stopCount(0) {}
};

class FlyGraph
{
public:
	static void BuildGraph();
	static FlightPath FindShortestPath(int fromAirportId, int toAirportId, int preferredAirlineId = -1);
	static Airport GetAirport(int airportId);
	static QList<Airport> GetAllAirports();
	static QList<int> GetAirlinesByAirportId(int airportId);
	
private:
	static QMap<int, Airport> airports;
	static QMap<int, QList<Route>> routes;
	static QMap<int, int> airlineAlliances;
};
