#include "FlyGraph.h"
#include "../Databaser.h"
#include "../Models/Airline.h"
#include <QQueue>
#include <QSet>
#include <QDebug>

// Initialize static members
QMap<int, Airport> FlyGraph::airports;
QMap<int, QList<Route>> FlyGraph::routes;
QMap<int, int> FlyGraph::airlineAlliances;

void FlyGraph::BuildGraph()
{
	airports.clear();
	routes.clear();
	airlineAlliances.clear();

	// Get Databaser singleton instance
	Databaser& db = Databaser::getInstance();

	// Load all airports
	QList<Airport> airportList = db.runQueryList<Airport>("SELECT * FROM Airport;");
	for (const Airport& airport : airportList) {
		airports[airport.id] = airport;
	}
	qInfo() << "Loaded" << airports.size() << "airports";

	// Load all routes
	QList<Route> routeList = db.runQueryList<Route>("SELECT * FROM Route;");
	for (const Route& route : routeList) {
		routes[route.airport1].append(route);
	}
	qInfo() << "Loaded" << routeList.size() << "routes";

	// Load airline alliances
	QList<Airline> airlines = db.runQueryList<Airline>("SELECT * FROM Airline;");
	for (const Airline& airline : airlines) {
		airlineAlliances[airline.id] = airline.alliance;
	}
	qInfo() << "Loaded" << airlines.size() << "airlines";
}

Airport FlyGraph::GetAirport(int airportId)
{
	return airports.value(airportId, Airport());
}

QList<Airport> FlyGraph::GetAllAirports()
{
	return airports.values();
}

QList<int> FlyGraph::GetAirlinesByAirportId(int airportId)
{
	QList<int> airlines;
	if (routes.contains(airportId)) {
		for (const Route& route : routes[airportId]) {
			if (!airlines.contains(route.airline)) {
				airlines.append(route.airline);
			}
		}
	}
	return airlines;
}

FlightPath FlyGraph::FindShortestPath(int fromAirportId, int toAirportId, int preferredAirlineId)
{
	FlightPath result;

	if (!airports.contains(fromAirportId) || !airports.contains(toAirportId)) {
		qWarning() << "Invalid airport IDs:" << fromAirportId << toAirportId;
		return result;
	}

	// BFS to find all shortest paths
	QMap<int, int> distances;
	QMap<int, QList<QPair<int, Route>>> predecessors;
	QQueue<int> queue;
	QSet<int> visited;

	distances[fromAirportId] = 0;
	queue.enqueue(fromAirportId);
	visited.insert(fromAirportId);

	while (!queue.isEmpty()) {
		int currentAirport = queue.dequeue();
		int currentDistance = distances[currentAirport];

		if (routes.contains(currentAirport)) {
			for (const Route& route : routes[currentAirport]) {
				int nextAirport = route.airport2;

				if (!distances.contains(nextAirport)) {
					// First time reaching this airport
					distances[nextAirport] = currentDistance + 1;
					predecessors[nextAirport].append(qMakePair(currentAirport, route));
					
					if (!visited.contains(nextAirport)) {
						queue.enqueue(nextAirport);
						visited.insert(nextAirport);
					}
				}
				else if (distances[nextAirport] == currentDistance + 1) {
					// Found another shortest path to this airport
					predecessors[nextAirport].append(qMakePair(currentAirport, route));
				}
			}
		}
	}

	if (!distances.contains(toAirportId)) {
		qWarning() << "No path found from" << fromAirportId << "to" << toAirportId;
		return result;
	}

	// Reconstruct the best path
	int preferredAlliance = (preferredAirlineId >= 0) ? airlineAlliances.value(preferredAirlineId, -1) : -1;
	
	// Helper function to score a path
	auto scorePath = [preferredAirlineId, preferredAlliance](const QList<FlightSegment>& path) -> int {
		int score = 0;
		for (const FlightSegment& segment : path) {
			if (segment.airlineId == preferredAirlineId) {
				score += 100; // Highest priority
			}
			else if (preferredAlliance >= 0 && airlineAlliances.value(segment.airlineId, -1) == preferredAlliance) {
				score += 50; // Medium priority
			}
		}
		return score;
	};

	// Recursive function to build all possible paths
	QList<QList<FlightSegment>> allPaths;
	
	std::function<void(int, QList<FlightSegment>)> buildPaths = [&](int currentAirport, QList<FlightSegment> currentPath) {
		if (currentAirport == fromAirportId) {
			// Reverse the path since we built it backwards
			QList<FlightSegment> reversedPath;
			for (int i = currentPath.size() - 1; i >= 0; --i) {
				reversedPath.append(currentPath[i]);
			}
			allPaths.append(reversedPath);
			return;
		}

		if (predecessors.contains(currentAirport)) {
			for (const auto& pred : predecessors[currentAirport]) {
				int prevAirport = pred.first;
				const Route& route = pred.second;
				
				QList<FlightSegment> newPath = currentPath;
				newPath.append(FlightSegment(prevAirport, currentAirport, route.airline));
				buildPaths(prevAirport, newPath);
			}
		}
	};

	buildPaths(toAirportId, QList<FlightSegment>());

	// Find the best path based on scoring
	if (!allPaths.isEmpty()) {
		QList<FlightSegment> bestPath = allPaths.first();
		int bestScore = scorePath(bestPath);

		for (const auto& path : allPaths) {
			int pathScore = scorePath(path);
			if (pathScore > bestScore) {
				bestPath = path;
				bestScore = pathScore;
			}
		}

		result.segments = bestPath;
		result.stopCount = bestPath.size() - 1;
	}

	return result;
}
