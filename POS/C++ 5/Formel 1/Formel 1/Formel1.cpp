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
    // Central widget and main layout
    auto centralWidget = new QWidget(this);
    auto mainLayout = new QHBoxLayout(centralWidget);

    // Left side widget and layout
    auto leftWidget = new QWidget(centralWidget);
    auto leftLayout = new QVBoxLayout(leftWidget);

    // Top layout inside the left side for input and search results
    auto topLayout = new QHBoxLayout();
    auto m_nameInput = new QLineEdit(centralWidget);
    m_nameInput->setPlaceholderText("Namen des Formel 1 Fahrers...");
    m_nameInput->setMaximumWidth(300);

    auto m_searchResults = new QComboBox(centralWidget);
    m_searchResults->setPlaceholderText("Suchergebnisse...");

    topLayout->addWidget(m_nameInput);
    topLayout->addWidget(m_searchResults);

    // Chart setup
    auto chart = new QChart();
    chart->createDefaultAxes();

    auto chartView = new QChartView(chart, centralWidget);

    // Add widgets to left layout
    leftLayout->addLayout(topLayout);
    leftLayout->addWidget(chartView);

    // How to add left widget to main layout
    mainLayout->addWidget(leftWidget);

    // Right widget and layout (initially hidden)
    auto rightWidget = new QWidget(centralWidget);
    auto rightLayout = new QVBoxLayout(rightWidget);

    // Info of driver
	auto nameLabel = new QLabel("Name: ", rightWidget);
	rightLayout->addWidget(nameLabel);
	nameUnderLabel = new QLabel(rightWidget);
	rightLayout->addWidget(nameUnderLabel);
    
    auto surnameLabel = new QLabel("Nachname: ", rightWidget);
	rightLayout->addWidget(surnameLabel);
	surnameUnderLabel = new QLabel(rightWidget);
	rightLayout->addWidget(surnameUnderLabel);

    auto birthDateLabel = new QLabel("Geburtsdatum: ", rightWidget);
	rightLayout->addWidget(birthDateLabel);
	birthDateUnderLabel = new QLabel(rightWidget);
	rightLayout->addWidget(birthDateUnderLabel);
    
    auto nationalityLabel = new QLabel("Nationalität: ", rightWidget);
	rightLayout->addWidget(nationalityLabel);
	nationalityUnderLabel = new QLabel(rightWidget);
	rightLayout->addWidget(nationalityUnderLabel);
    
    auto countDrivesLabel = new QLabel("Anzahl der Rennen: ", rightWidget);
	rightLayout->addWidget(countDrivesLabel);
	countDrivesUnderLabel = new QLabel(rightWidget);
	rightLayout->addWidget(countDrivesUnderLabel);
    
    auto teamLabel = new QLabel("Team: ", rightWidget);
	rightLayout->addWidget(teamLabel);
	teamUnderLabel = new QLabel(rightWidget);
	rightLayout->addWidget(teamUnderLabel);

    rightLayout->addStretch();

    // Add right widget to main layout
    mainLayout->addWidget(rightWidget);

    // Hide the right layout container initially
    //rightWidget->hide();

    // Set the main widget and layout
    centralWidget->setLayout(mainLayout);
    setCentralWidget(centralWidget);

    // Signals and slots
    connect(m_searchResults, qOverload<int>(&QComboBox::currentIndexChanged), this, &Formel1::onDriverSelected);
}

void Formel1::onDriverSelected()
{
	auto selected = m_searchResults->currentData().toInt();

    std::shared_ptr<Drivers> d_tmp; 
    d_tmp.reset(new Drivers());
    d_tmp->driverId = 3;
    auto daoError = qx::dao::fetch_by_id(d_tmp);
    if (daoError.isValid())
    {
        QMessageBox::critical(this, "Fehler", "Fehler beim Laden des Fahrers: " + daoError.text());
        return;
	}
    nameUnderLabel->setText(d_tmp->forename);
    surnameUnderLabel->setText(d_tmp->surname);
    birthDateUnderLabel->setText(d_tmp->dob);
    nationalityUnderLabel->setText(d_tmp->nationality);
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

