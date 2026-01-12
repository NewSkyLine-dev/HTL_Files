#pragma once

#include <QtWidgets/QMainWindow>
#include <QPushButton>
#include <QLabel>
#include <QTableView>
#include "ui_PA1.h"
#include "Verwalter.h"

class PA1 : public QMainWindow
{
    Q_OBJECT

public:
    PA1(QWidget *parent = nullptr);
    ~PA1();

private:
    Ui::PA1Class ui;
    QPushButton* m_selectFolder;
    QPushButton* m_addPicture;
	QLabel* m_imageDisplay;
    QTableView* m_tableView;
    QStandardItemModel* m_model;
    Verwalter m_imageClass;

    int timerId;

private slots:
    void onSelectFolderClicked();
	void onAddPictureClicked();

protected:
    void timerEvent(QTimerEvent* event) override;
};

