#pragma once
#include <QMainWindow>
#include <QHash>

namespace tinyxml2 { class XMLDocument; class XMLElement; }

class QTreeWidget;
class QTreeWidgetItem;
class QTextEdit;
class QComboBox;
class QLineEdit;

class XMLExplorer : public QMainWindow
{
    Q_OBJECT
public:
    explicit XMLExplorer(QWidget* parent = nullptr);
    ~XMLExplorer();

private slots:
    void onItemSelectionChanged();
    void onAttributeSelected(int index);
    void updateAttributeValue();
    void SaveDocument();

private:
    void AddChild(tinyxml2::XMLElement* XmlElement, QTreeWidgetItem* TreeWidget);
    void populateAttributes(tinyxml2::XMLElement* el);           

    QTreeWidget* treeWidget = nullptr;
    QTextEdit* detailsTextEdit = nullptr;
    QComboBox* attributeComboBox = nullptr;
    QLineEdit* attributeValueEdit = nullptr;

    std::unique_ptr<tinyxml2::XMLDocument> xmlDoc;               
    tinyxml2::XMLElement* currentElement = nullptr;

    QHash<QTreeWidgetItem*, tinyxml2::XMLElement*> itemToElement; 
    QString currentFilePath;   
    bool dirty = false;       
};
