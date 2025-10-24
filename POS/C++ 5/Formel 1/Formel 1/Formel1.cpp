#include "stdafx.h"
#include "Formel1.h"

#include <QxOrm.h>

#include <QValueAxis>
#include <Models/Drivers.h>

Formel1::Formel1(QWidget* parent)
	: QMainWindow(parent),
	m_chart(new QChart),
	m_series(new QLineSeries)
{
	std::vector<std::shared_ptr<Drivers>> drugs;
	QSqlError err = qx::dao::fetch_all(drugs);

	if (err.isValid()) {
		qDebug() << "Fehler beim Laden der Daten:" << err.text();
	}
	else {
		qDebug() << "Erfolgreich" << drugs.size() << "Fahrer geladen";
	}

	auto centralWidget = new QWidget(this);
	auto mainLayout = new QVBoxLayout(centralWidget);

	m_chartView = new QChartView(m_chart, centralWidget);

	initChart();

	m_series->append(0, 6);
	m_series->append(1, 8);
	m_series->append(2, 5);
	m_series->append(3, 10);

	*m_series << QPointF(4, 12) << QPointF(5, 7) << QPointF(6, 14);

	// Top
	auto topLayout = new QHBoxLayout();
	m_nameInput = new QLineEdit(centralWidget);
	m_nameInput->setPlaceholderText("Gib den Namen des Formel 1 Fahrers ein...");
	m_nameInput->setMaximumWidth(300);

	m_searchResults = new QComboBox(centralWidget);
	m_searchResults->setPlaceholderText("Suchergebnisse...");

	topLayout->addWidget(m_nameInput);
	topLayout->addWidget(m_searchResults);

	// Bottom
	m_chart->addSeries(m_series);
	m_series->attachAxis(m_chart->axes(Qt::Horizontal).first());
	m_series->attachAxis(m_chart->axes(Qt::Vertical).first());

	mainLayout->addLayout(topLayout);
	mainLayout->addWidget(m_chartView);

	setCentralWidget(centralWidget);
}

Formel1::~Formel1()
{
}

void Formel1::initChart()
{
	auto axisX = new QValueAxis();
	axisX->setTitleText("Rennen");

	auto axisY = new QValueAxis();
	axisY->setTitleText("Punkte");

	m_chart->addAxis(axisX, Qt::AlignBottom);
	m_chart->addAxis(axisY, Qt::AlignLeft);
	m_chart->legend()->hide();
}

