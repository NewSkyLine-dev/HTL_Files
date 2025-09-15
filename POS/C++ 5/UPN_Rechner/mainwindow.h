#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QLineEdit>
#include <QListWidget>
#include <QPushButton>

#include "rpncalc.h"

QT_BEGIN_NAMESPACE
namespace Ui
{
    class MainWindow;
}
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

private slots:
    void onEnter();
    void onClear();

private:
    Ui::MainWindow *ui;
    RPNCalc engine_;
    QListWidget *stackView_ = nullptr;
    QLineEdit *input_ = nullptr;

    QPushButton *makeBtn(const QString &text, const char *slot);
    void buildUi();
    void refreshStack();
    void doEnter();

protected:
    bool eventFilter(QObject *obj, QEvent *event) override;
};
#endif // MAINWINDOW_H
