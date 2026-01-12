#pragma once

#include <QtWidgets/QMainWindow>
#include <QTreeView>
#include <QTextEdit>
#include <QComboBox>
#include <QLineEdit>
#include <XmlTreeModel.h>

class XMLExplorer : public QMainWindow
{
    Q_OBJECT

public:
    XMLExplorer(QWidget *parent = nullptr);
    ~XMLExplorer();

private slots:
    void openFile();
    void saveFile();
    void onSelectionChanged();
    void onAttributeSelected(int index);
    void onAttributeValueChanged();

private:
    QTreeView *m_treeView;
    XmlTreeModel *model;

    // Detail panel widgets
    QTextEdit *m_detailText;
    QComboBox *m_attributeCombo;
    QLineEdit *m_attributeValueEdit;

    void updateDetailPanel(const QModelIndex &index);
};
