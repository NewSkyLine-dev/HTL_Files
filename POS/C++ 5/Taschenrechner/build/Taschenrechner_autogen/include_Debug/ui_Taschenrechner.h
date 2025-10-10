/********************************************************************************
** Form generated from reading UI file 'Taschenrechner.ui'
**
** Created by: Qt User Interface Compiler version 6.9.2
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_TASCHENRECHNER_H
#define UI_TASCHENRECHNER_H

#include <QtCore/QVariant>
#include <QtWidgets/QApplication>
#include <QtWidgets/QMainWindow>
#include <QtWidgets/QMenuBar>
#include <QtWidgets/QStatusBar>
#include <QtWidgets/QToolBar>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_TaschenrechnerClass
{
public:
    QMenuBar *menuBar;
    QToolBar *mainToolBar;
    QWidget *centralWidget;
    QStatusBar *statusBar;

    void setupUi(QMainWindow *TaschenrechnerClass)
    {
        if (TaschenrechnerClass->objectName().isEmpty())
            TaschenrechnerClass->setObjectName("TaschenrechnerClass");
        TaschenrechnerClass->resize(600, 400);
        menuBar = new QMenuBar(TaschenrechnerClass);
        menuBar->setObjectName("menuBar");
        TaschenrechnerClass->setMenuBar(menuBar);
        mainToolBar = new QToolBar(TaschenrechnerClass);
        mainToolBar->setObjectName("mainToolBar");
        TaschenrechnerClass->addToolBar(mainToolBar);
        centralWidget = new QWidget(TaschenrechnerClass);
        centralWidget->setObjectName("centralWidget");
        TaschenrechnerClass->setCentralWidget(centralWidget);
        statusBar = new QStatusBar(TaschenrechnerClass);
        statusBar->setObjectName("statusBar");
        TaschenrechnerClass->setStatusBar(statusBar);

        retranslateUi(TaschenrechnerClass);

        QMetaObject::connectSlotsByName(TaschenrechnerClass);
    } // setupUi

    void retranslateUi(QMainWindow *TaschenrechnerClass)
    {
        TaschenrechnerClass->setWindowTitle(QCoreApplication::translate("TaschenrechnerClass", "Taschenrechner", nullptr));
    } // retranslateUi

};

namespace Ui {
    class TaschenrechnerClass: public Ui_TaschenrechnerClass {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_TASCHENRECHNER_H
