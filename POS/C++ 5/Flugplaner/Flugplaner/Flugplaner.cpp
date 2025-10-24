#include "Flugplaner.h"
#include <QDir>
#include <QCoreApplication>
#include <QVBoxLayout>
#include <QHBoxLayout>
#include <QLabel>
#include <QPushButton>
#include <QComboBox>
#include <QTableWidget>
#include <QHeaderView>
#include <QMetaObject>
#include <QApplication>
#include <QMessageBox>
#include <QDebug>

#include <QxOrm_Impl.h>

#include "Databaser.h"
#include "Utilities/FlyGraph.h"
#include "Models/Airline.h"
#include "Models/Airport.h"
#include "Models/Alliance.h"
#include "Models/Route.h"

static QPushButton *CreateButton(const QString &text, Flugplaner *base, void (Flugplaner::*member)())
{
	QPushButton *button = new QPushButton(text);
	QObject::connect(button, &QPushButton::clicked, base, member);
	return button;
}

Flugplaner::Flugplaner(QWidget *parent)
	: QMainWindow(parent), selectedFromAirportId(-1), selectedToAirportId(-1), selectedAirlineId(-1)
{
	// Build the graph from database
	FlyGraph::BuildGraph();

	setupUI();
	loadAirports();
	loadAirlines();
}

Flugplaner::~Flugplaner()
{
}

void Flugplaner::setupUI()
{
	QWidget *centralWidget = new QWidget(this);
	QHBoxLayout *mainLayout = new QHBoxLayout(centralWidget);

	// Left side: Map
	mapWidget = new MapWidget(this);
	QString mapPath = QCoreApplication::applicationDirPath() + "/resources/Earthmap.jpg";
	mapWidget->setMapImage(mapPath);
	mapWidget->setMinimumSize(800, 600);
	mainLayout->addWidget(mapWidget, 1);

	// Right side: Controls and table
	QWidget *rightPanel = new QWidget();
	QVBoxLayout *rightLayout = new QVBoxLayout(rightPanel);
	rightLayout->setContentsMargins(10, 10, 10, 10);

	// From airport selection
	QLabel *fromLabel = new QLabel("Von (From):");
	fromCombo = new QComboBox();
	fromCombo->setMinimumWidth(250);
	fromCombo->setEditable(true);
	fromCombo->setInsertPolicy(QComboBox::NoInsert);
	connect(fromCombo, qOverload<int>(&QComboBox::currentIndexChanged),
			this, &Flugplaner::onFromAirportChanged);

	// To airport selection
	QLabel *toLabel = new QLabel("Nach (To):");
	toCombo = new QComboBox();
	toCombo->setMinimumWidth(250);
	toCombo->setEditable(true);
	toCombo->setInsertPolicy(QComboBox::NoInsert);
	connect(toCombo, qOverload<int>(&QComboBox::currentIndexChanged),
			this, &Flugplaner::onToAirportChanged);

	// Airline selection (optional)
	QLabel *airlineLabel = new QLabel("Fluglinie (Optional):");
	airlineCombo = new QComboBox();
	airlineCombo->setMinimumWidth(250);
	airlineCombo->addItem("Keine Präferenz", -1);
	connect(airlineCombo, qOverload<int>(&QComboBox::currentIndexChanged),
			this, &Flugplaner::onAirlineChanged);

	// Search button
	QPushButton *searchButton = CreateButton("Suchen", this, &Flugplaner::on_searchButton_clicked);
	searchButton->setMinimumHeight(30);

	// Flight details table
	QLabel *tableLabel = new QLabel("Flugdetails:");
	flightTable = new QTableWidget(0, 4);
	flightTable->setHorizontalHeaderLabels(QStringList() << "Von" << "Nach" << "Fluglinie" << "IATA");
	flightTable->horizontalHeader()->setStretchLastSection(true);
	flightTable->setEditTriggers(QAbstractItemView::NoEditTriggers);
	flightTable->setSelectionBehavior(QAbstractItemView::SelectRows);

	// Add widgets to right layout
	rightLayout->addWidget(fromLabel);
	rightLayout->addWidget(fromCombo);
	rightLayout->addWidget(toLabel);
	rightLayout->addWidget(toCombo);
	rightLayout->addWidget(airlineLabel);
	rightLayout->addWidget(airlineCombo);
	rightLayout->addWidget(searchButton);
	rightLayout->addSpacing(20);
	rightLayout->addWidget(tableLabel);
	rightLayout->addWidget(flightTable);
	rightLayout->addStretch();

	rightPanel->setMinimumWidth(300);
	rightPanel->setMaximumWidth(400);
	mainLayout->addWidget(rightPanel);

	this->setCentralWidget(centralWidget);
	this->setWindowTitle("Flugplaner");
	this->resize(1200, 700);
}

void Flugplaner::loadAirports()
{
	QList<Airport> airports = FlyGraph::GetAllAirports();

	fromCombo->clear();
	toCombo->clear();

	fromCombo->addItem("-- Wählen Sie einen Flughafen --", -1);
	toCombo->addItem("-- Wählen Sie einen Flughafen --", -1);

	for (const Airport &airport : airports)
	{
		if (!airport.getIata().isEmpty())
		{
			QString displayText = QString("%1 - %2 (%3)")
									  .arg(airport.getIata())
									  .arg(airport.getName())
									  .arg(airport.getId());

			fromCombo->addItem(displayText, airport.getId());
			toCombo->addItem(displayText, airport.getId());
		}
	}

	qInfo() << "Loaded" << airports.size() << "airports into combo boxes";
}

void Flugplaner::loadAirlines()
{
	// QxOrm DAO queries require proper template registration
	// This will be available after QxOrm configuration is fully set up
	/*
	try {
		QList<Airline> airlines;
		qx::dao::fetch_all(airlines, nullptr);
		for (const Airline &airline : airlines)
		{
			airlineCombo->addItem(airline.getName(), airline.getId());
		}
		qInfo() << "Loaded" << airlines.size() << "airlines from database";
	}
	catch (const std::exception &e) {
		qWarning() << "Error loading airlines:" << e.what();
		qInfo() << "Loaded 0 airlines";
	}
	*/
	qInfo() << "Loaded 0 airlines (QxOrm DAO configuration pending)";
}

void Flugplaner::onFromAirportChanged(int index)
{
	selectedFromAirportId = fromCombo->itemData(index).toInt();
	qDebug() << "Selected from airport ID:" << selectedFromAirportId;
}

void Flugplaner::onToAirportChanged(int index)
{
	selectedToAirportId = toCombo->itemData(index).toInt();
	qDebug() << "Selected to airport ID:" << selectedToAirportId;
}

void Flugplaner::onAirlineChanged(int index)
{
	selectedAirlineId = airlineCombo->itemData(index).toInt();
	qDebug() << "Selected airline ID:" << selectedAirlineId;
}

void Flugplaner::on_searchButton_clicked()
{
	qInfo() << "Search button clicked!";
	qInfo() << "From Airport ID:" << selectedFromAirportId;
	qInfo() << "To Airport ID:" << selectedToAirportId;
	qInfo() << "Preferred Airline ID:" << selectedAirlineId;

	// Validate input
	if (selectedFromAirportId <= 0 || selectedToAirportId <= 0)
	{
		QMessageBox::warning(this, "Eingabefehler",
							 "Bitte wählen Sie Start- und Zielflughafen aus.");
		return;
	}

	if (selectedFromAirportId == selectedToAirportId)
	{
		QMessageBox::warning(this, "Eingabefehler",
							 "Start- und Zielflughafen müssen unterschiedlich sein.");
		return;
	}

	// Find shortest path
	FlightPath path = FlyGraph::FindShortestPath(
		selectedFromAirportId,
		selectedToAirportId,
		selectedAirlineId);

	if (path.segments.isEmpty())
	{
		QMessageBox::information(this, "Keine Route gefunden",
								 "Es konnte keine Verbindung zwischen den ausgewählten Flughäfen gefunden werden.");
		return;
	}

	// Update map
	mapWidget->setFlightPath(path, selectedAirlineId);

	// Update table
	updateFlightTable(path);

	QMessageBox::information(this, "Route gefunden",
							 QString("Route mit %1 Zwischenstopp(s) gefunden.").arg(path.stopCount));
}

void Flugplaner::updateFlightTable(const FlightPath &path)
{
	flightTable->setRowCount(path.segments.size());

	for (int i = 0; i < path.segments.size(); ++i)
	{
		const FlightSegment &segment = path.segments[i];

		Airport fromAirport = FlyGraph::GetAirport(segment.fromAirportId);
		Airport toAirport = FlyGraph::GetAirport(segment.toAirportId);

		// QxOrm DAO queries require proper template registration
		QString airlineName = "Unknown";
		/*
		try {
			Airline airline;
			airline.setId(segment.airlineId);
			qx::dao::fetch_by_id(airline, nullptr);
			airlineName = airline.getName();
		}
		catch (const std::exception &e) {
			qWarning() << "Error loading airline:" << e.what();
		}
		*/

		// Set table cells
		flightTable->setItem(i, 0, new QTableWidgetItem(fromAirport.getName()));
		flightTable->setItem(i, 1, new QTableWidgetItem(toAirport.getName()));
		flightTable->setItem(i, 2, new QTableWidgetItem(airlineName));
		flightTable->setItem(i, 3, new QTableWidgetItem(QString("%1 → %2").arg(fromAirport.getIata()).arg(toAirport.getIata())));
	}

	flightTable->resizeColumnsToContents();
}
