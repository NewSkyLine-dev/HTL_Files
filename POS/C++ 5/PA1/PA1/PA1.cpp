#include "stdafx.h"
#include "PA1.h"

#include <QStandardItemModel>
#include <QTableView>

#include <QFileDialog>
#include <Images.h>

PA1::PA1(QWidget *parent)
    : QMainWindow(parent)
{
    //ui.setupUi(this);
	auto mainWidget = new QWidget(this);
	auto centralLayout = new QHBoxLayout(mainWidget);

	auto rightLayout = new QVBoxLayout();
	auto leftLayout = new QVBoxLayout();

	centralLayout->addLayout(leftLayout);
	centralLayout->addLayout(rightLayout);

	m_selectFolder = new QPushButton("Ordner wählen", this);

	m_addPicture = new QPushButton("Bild einfügen", this);
	m_imageDisplay = new QLabel(this);
	m_tableView = new QTableView(this);

	connect(m_selectFolder, &QPushButton::clicked, this, &PA1::onSelectFolderClicked);
	connect(m_addPicture, &QPushButton::clicked, this, &PA1::onAddPictureClicked);

	leftLayout->addWidget(m_selectFolder);
	leftLayout->addWidget(new QLabel("Bilder"));
	leftLayout->addWidget(m_tableView);

	rightLayout->addWidget(m_addPicture);
	rightLayout->addWidget(new QLabel("Bilderanzeige:"));
	rightLayout->addWidget(m_imageDisplay);

	this->setCentralWidget(mainWidget);

	m_model = new QStandardItemModel(this);
	m_model->setColumnCount(3);
	m_model->setHeaderData(0, Qt::Horizontal, "Dateiname");
	m_model->setHeaderData(1, Qt::Horizontal, "Breite");
	m_model->setHeaderData(2, Qt::Horizontal, "Hoehe");

	m_tableView->setModel(m_model);
}

PA1::~PA1()
{}

void PA1::onAddPictureClicked()
{
	auto selectedIndexes = m_tableView->selectionModel()->selectedRows();

	if (!selectedIndexes.isEmpty())
	{
		int row = selectedIndexes.first().row();
		QString filename = m_model->item(row, 0)->text();
		m_imageClass.switchImage(m_imageDisplay, filename);
	}
	else
	{
		QMessageBox::information(this, "Keine Auswahl", "Bitte wählen Sie ein Bild aus der Tabelle aus.");
	}
}

void PA1::timerEvent(QTimerEvent * event)
{
	if (event->timerId() == timerId)
	{
		m_imageClass.switchImage(m_imageDisplay, "");
	}
	else
	{
		QMainWindow::timerEvent(event);
	}
}

void PA1::onSelectFolderClicked()
{
	m_model->removeRows(0, m_model->rowCount());
	QString f = QFileDialog::getExistingDirectory(this, "Ordner wählen", QString(),
		QFileDialog::ShowDirsOnly | QFileDialog::DontResolveSymlinks);

	QList<std::shared_ptr<Images>> lst_images;

	QDir dir(f);
	for (QFileInfo& fi : dir.entryInfoList())
	{
		auto suffix = fi.completeSuffix();
		if (fi.isFile() && (fi.completeSuffix().compare("jpeg") || fi.completeSuffix().compare("jpg"))) {
			std::string absPath = fi.absoluteFilePath().toStdString();
			std::ifstream file(fi.absoluteFilePath().toStdString(), std::ifstream::binary);
			TinyEXIF::EXIFInfo exifInfo(file);
			QString widthStr = "";
			QString heightStr = "";

			if (exifInfo.Fields) {
				widthStr = QString::number(exifInfo.ImageWidth);
				heightStr = QString::number(exifInfo.ImageHeight);
			}
			else {
				widthStr = "";
				heightStr = "";
			}

			auto ins_img = std::make_shared<Images>(fi.fileName(), widthStr, heightStr);
			lst_images.append(ins_img);

			QList<QStandardItem*> items;
			items.append(new QStandardItem(fi.fileName()));
			QImage img(fi.absoluteFilePath());
			items.append(new QStandardItem(widthStr));
			items.append(new QStandardItem(heightStr));
			m_model->appendRow(items);
			m_imageClass.images.append(Image(absPath, img.width(), img.height()));
		}
	}

	timerId = startTimer(5000);

	// Insert into DB
	auto daoError = qx::dao::insert(lst_images);

	if (daoError.isValid()) {
		QMessageBox::critical(this, "Datenbankfehler", "Fehler beim Einfügen in die Datenbank: " + daoError.text());
	}
}