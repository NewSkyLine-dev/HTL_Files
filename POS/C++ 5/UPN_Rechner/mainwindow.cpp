#include "mainwindow.h"
#include "ui_mainwindow.h"

#include <QVBoxLayout>
#include <QMessageBox>
#include <QKeyEvent>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent), ui(new Ui::MainWindow)
{
    buildUi();
    refreshStack();
    // ui->setupUi(this);
}

MainWindow::~MainWindow()
{
    delete ui;
}

QPushButton *MainWindow::makeBtn(const QString &text, const char *slot)
{
    auto *btn = new QPushButton(text);
    btn->setMinimumHeight(30);
    connect(btn, SIGNAL(clicked()), this, slot);
    return btn;
}

void MainWindow::buildUi()
{
    auto *central = new QWidget(this);
    auto *root = new QVBoxLayout(central);

    stackView_ = new QListWidget(central);
    stackView_->setSelectionMode(QAbstractItemView::NoSelection);
    stackView_->setAlternatingRowColors(true);
    root->addWidget(stackView_, 1);

    input_ = new QLineEdit(central);
    input_->setPlaceholderText("RPN Ausdruck eingeben (z.B. '2 3 +') und ENTER drücken");
    input_->installEventFilter(this);
    root->addWidget(input_);

    auto *grid = new QGridLayout();
    int r = 1;

    grid->addWidget(makeBtn("ENTER", SLOT(onEnter())), r, 0, 1, 2);
    grid->addWidget(makeBtn("CLEAR", SLOT(onClear())), r, 2, 1, 2);
    r++;

    root->addLayout(grid);
    setCentralWidget(central);
    setWindowTitle("RPN Rechner (Qt6)");
    resize(420, 520);
}

void MainWindow::refreshStack()
{
    stackView_->clear();
    const auto st = engine_.getStackAsVector();
    // Show TOS at top:
    for (int i = static_cast<int>(st.size()) - 1, idx = 0; i >= 0; --i, ++idx)
    {
        auto *item = new QListWidgetItem(QString::number(st[static_cast<size_t>(i)], 'g', 15));
        item->setTextAlignment(Qt::AlignRight | Qt::AlignVCenter);
        item->setToolTip(QString("Level %1").arg(idx));
        stackView_->addItem(item);
    }
}

void MainWindow::doEnter()
{
    const QString text = input_->text().trimmed();
    if (!text.isEmpty())
    {
        // Clear the stack before processing a new expression
        engine_.clear();

        try
        {
            double result = engine_.evaluateExpression(text.toStdString());

            // Show result in a message box
            QMessageBox::information(this, "Ergebnis",
                                     QString("Ausdruck: %1\nErgebnis: %2")
                                         .arg(text, QString::number(result, 'g', 15)));

            // Keep the result on the stack and refresh the display
            input_->clear();
            refreshStack();
        }
        catch (const std::exception &ex)
        {
            QMessageBox::warning(this, "Fehler", ex.what());
            // Clear stack on error
            engine_.clear();
            refreshStack();
        }
    }
    else
    {
        refreshStack();
    }
}

void MainWindow::onEnter() { doEnter(); }
void MainWindow::onClear()
{
    engine_.clear();
    input_->clear();
    refreshStack();
}

bool MainWindow::eventFilter(QObject *obj, QEvent *ev)
{
    if (obj == input_ && ev->type() == QEvent::KeyPress)
    {
        auto *ke = static_cast<QKeyEvent *>(ev);
        switch (ke->key())
        {
        case Qt::Key_Return:
        case Qt::Key_Enter:
            onEnter();
            return true;
        default:
            break;
        }
    }
    return QMainWindow::eventFilter(obj, ev);
}
