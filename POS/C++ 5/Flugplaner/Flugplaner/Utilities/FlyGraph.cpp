#include "FlyGraph.h"
#include "../Databaser.h"
#include "../Models/Airline.h"
#include <QQueue>
#include <QSet>
#include <QDebug>

#include <QxOrm_Impl.h>

// Initialize static members
QMap<int, Airport> FlyGraph::airports;
QMap<int, QList<Route>> FlyGraph::routes;
QMap<int, int> FlyGraph::airlineAlliances;

void FlyGraph::BuildGraph()
{
	airports.clear();
	routes.clear();
	airlineAlliances.clear();

	// QxOrm DAO queries require proper template registration
	// This will work after QxOrm configuration is fully completed
	/*
	try {
		// Load all airports using QxORM
		QList<Airport> airportList;
		qx::dao::fetch_all(airportList);
		for (const Airport &airport : airportList)
		{
			airports[airport.getId()] = airport;
		}

		// Load all routes using QxORM
		QList<Route> routeList;
		qx::dao::fetch_all(routeList);
		for (const Route &route : routeList)
		{
			routes[route.getAirport1()].append(route);
		}

		// Load airline alliances using QxORM
		QList<Airline> airlines;
		qx::dao::fetch_all(airlines);
		for (const Airline &airline : airlines)
		{
			airlineAlliances[airline.getId()] = airline.getAlliance();
		}
		qInfo() << "FlyGraph initialized with" << airports.size() << "airports," << routeList.size() << "routes";
	}
	catch (const std::exception &e) {
		qWarning() << "Error loading graph data:" << e.what();
		qInfo() << "FlyGraph initialized with empty data (database error)";
	}
	*/
	qInfo() << "FlyGraph initialized with empty data (QxOrm DAO pending)";
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
	if (routes.contains(airportId))
	{
		for (const Route &route : routes[airportId])
		{
			if (!airlines.contains(route.getAirline()))
			{
				airlines.append(route.getAirline());
			}
		}
	}
	return airlines;
}

FlightPath FlyGraph::FindShortestPath(int fromAirportId, int toAirportId, int preferredAirlineId)
{
	FlightPath result;

	if (!airports.contains(fromAirportId) || !airports.contains(toAirportId))
	{
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

	while (!queue.isEmpty())
	{
		int currentAirport = queue.dequeue();
		int currentDistance = distances[currentAirport];

		if (routes.contains(currentAirport))
		{
			for (const Route &route : routes[currentAirport])
			{
				int nextAirport = route.getAirport2();

				if (!distances.contains(nextAirport))
				{
					// First time reaching this airport
					distances[nextAirport] = currentDistance + 1;
					predecessors[nextAirport].append(qMakePair(currentAirport, route));

					if (!visited.contains(nextAirport))
					{
						queue.enqueue(nextAirport);
						visited.insert(nextAirport);
					}
				}
				else if (distances[nextAirport] == currentDistance + 1)
				{
					// Found another shortest path to this airport
					predecessors[nextAirport].append(qMakePair(currentAirport, route));
				}
			}
		}
	}

	if (!distances.contains(toAirportId))
	{
		qWarning() << "No path found from" << fromAirportId << "to" << toAirportId;
		return result;
	}

	// Reconstruct the best path
	int preferredAlliance = (preferredAirlineId >= 0) ? airlineAlliances.value(preferredAirlineId, -1) : -1;

	// Helper function to score a path
	auto scorePath = [preferredAirlineId, preferredAlliance](const QList<FlightSegment> &path) -> int
	{
		int score = 0;
		for (const FlightSegment &segment : path)
		{
			if (segment.airlineId == preferredAirlineId)
			{
				score += 100; // Highest priority
			}
			else if (preferredAlliance >= 0 && airlineAlliances.value(segment.airlineId, -1) == preferredAlliance)
			{
				score += 50; // Medium priority
			}
		}
		return score;
	};

	// Recursive function to build all possible paths
	QList<QList<FlightSegment>> allPaths;

	std::function<void(int, QList<FlightSegment>)> buildPaths = [&](int currentAirport, QList<FlightSegment> currentPath)
	{
		if (currentAirport == fromAirportId)
		{
			// Reverse the path since we built it backwards
			QList<FlightSegment> reversedPath;
			for (int i = currentPath.size() - 1; i >= 0; --i)
			{
				reversedPath.append(currentPath[i]);
			}
			allPaths.append(reversedPath);
			return;
		}

		if (predecessors.contains(currentAirport))
		{
			for (const auto &pred : predecessors[currentAirport])
			{
				int prevAirport = pred.first;
				const Route &route = pred.second;

				QList<FlightSegment> newPath = currentPath;
				newPath.append(FlightSegment(prevAirport, currentAirport, route.getAirline()));
				buildPaths(prevAirport, newPath);
			}
		}
	};

	buildPaths(toAirportId, QList<FlightSegment>());

	// Find the best path based on scoring
	if (!allPaths.isEmpty())
	{
		QList<FlightSegment> bestPath = allPaths.first();
		int bestScore = scorePath(bestPath);

		for (const auto &path : allPaths)
		{
			int pathScore = scorePath(path);
			if (pathScore > bestScore)
			{
				bestPath = path;
				bestScore = pathScore;
			}
		}

		result.segments = bestPath;
		result.stopCount = bestPath.size() - 1;
	}

	return result;
}
