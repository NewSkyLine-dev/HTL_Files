#include "XMLExplorer.h"

#include <QFileDialog>
#include <QMessageBox>
#include <QPushButton>
#include <QItemSelectionModel>
#include <QHBoxLayout>
#include <QSplitter>
#include <QStringBuilder>
#include <QFormLayout>
#include <QComboBox>
#include <QLineEdit>
#include <QTextEdit>
#include <QVBoxLayout>
#include <QTreeWidget>
#include <QStatusBar>
#include <QSaveFile>

#include <tinyxml2.h> // make sure this is included somewhere

static void MakeBtn(const QString& text, QWidget* parent, std::function<void()> slot)
{
    auto* btn = new QPushButton(text, parent);
    QObject::connect(btn, &QPushButton::clicked, parent, std::move(slot));
}

XMLExplorer::XMLExplorer(QWidget* parent)
    : QMainWindow(parent)
{
    setWindowTitle(tr("XML Explorer"));
    statusBar(); // ensure it exists

    auto* central = new QWidget(this);
    auto* layout = new QVBoxLayout(central);

    auto* splitter = new QSplitter(Qt::Horizontal, central);

    // Tree
    auto* root = new QTreeWidget(splitter);
    root->setColumnCount(2);
    root->setHeaderLabels(QStringList() << tr("Element") << tr("Value"));
    treeWidget = root;

    // Right side: details + attribute editor
    auto* detailsWidget = new QWidget(splitter);
    auto* detailsLayout = new QVBoxLayout(detailsWidget);

    detailsTextEdit = new QTextEdit(detailsWidget);
    detailsTextEdit->setReadOnly(true);

    auto* attributeFormWidget = new QWidget(detailsWidget);
    auto* attributeFormLayout = new QFormLayout(attributeFormWidget);

    attributeComboBox = new QComboBox(attributeFormWidget);
    attributeComboBox->setEditable(true);                     // NEW: allow creating new attributes
    attributeComboBox->setInsertPolicy(QComboBox::NoInsert);  // user typing doesn't auto-insert duplicates
    attributeFormLayout->addRow(tr("Attribut:"), attributeComboBox);

    attributeValueEdit = new QLineEdit(attributeFormWidget);
    attributeValueEdit->setPlaceholderText(tr("Wert des Attributs"));
    attributeFormLayout->addRow(tr("Wert:"), attributeValueEdit);

    auto* updateButton = new QPushButton(tr("Attributwert aktualisieren"), attributeFormWidget);
    attributeFormLayout->addRow(updateButton);

    connect(attributeComboBox, QOverload<int>::of(&QComboBox::currentIndexChanged),
        this, &XMLExplorer::onAttributeSelected);
    connect(updateButton, &QPushButton::clicked, this, &XMLExplorer::updateAttributeValue);

    detailsLayout->addWidget(detailsTextEdit, 3);
    detailsLayout->addWidget(attributeFormWidget, 1);

    splitter->addWidget(root);
    splitter->addWidget(detailsWidget);
    splitter->setStretchFactor(0, 1);
    splitter->setStretchFactor(1, 1);

    layout->addWidget(splitter);

    auto* buttonArea = new QWidget(central);
    auto* buttonLayout = new QHBoxLayout(buttonArea);
    buttonLayout->setContentsMargins(0, 0, 0, 0);
    layout->addWidget(buttonArea);

    central->setLayout(layout);
    setCentralWidget(central);

    connect(root, &QTreeWidget::itemSelectionChanged, this, &XMLExplorer::onItemSelectionChanged);
    connect(root, &QTreeWidget::currentItemChanged, this, [this](QTreeWidgetItem* current, QTreeWidgetItem* previous) {
        Q_UNUSED(previous);
        if (current) onItemSelectionChanged();
        });

    bool ok = true;

    const auto file = QFileDialog::getOpenFileName(this, tr("XML Datei öffnen"), {}, tr("XML Dateien (*.xml)"));
    if (file.isEmpty()) {
        QMessageBox::information(this, tr("Abbruch"), tr("Keine Datei ausgewählt"));
        ok = false;
    }
    else {
        setWindowTitle(QStringLiteral("XML Explorer - %1").arg(file));
        this->currentFilePath = file;
        ok = true;
    }

    if (ok) {
        xmlDoc = std::make_unique<tinyxml2::XMLDocument>();   // RAII
        const auto err = xmlDoc->LoadFile(file.toUtf8().constData());

        if (err != tinyxml2::XML_SUCCESS) {
            QMessageBox::critical(this, tr("Fehler"),
                tr("Fehler beim Laden der Datei: %1")
                .arg(xmlDoc->ErrorIDToName(err)));
            ok = false;
        }

        if (ok) {
            auto* rootElement = xmlDoc->RootElement();
            if (!rootElement) {
                QMessageBox::critical(this, tr("Fehler"), tr("Die Datei enthält kein Root-Element."));
                ok = false;
            }
            else {
                auto* rootItem = new QTreeWidgetItem(treeWidget);
                rootItem->setText(0, QString::fromUtf8(rootElement->Name()));
                rootItem->setText(1, QString::fromUtf8(rootElement->GetText() ? rootElement->GetText() : ""));
                treeWidget->addTopLevelItem(rootItem);

                itemToElement.insert(rootItem, rootElement);   // NEW: map item -> element
                AddChild(rootElement, rootItem);

                rootItem->setExpanded(true);
            }
        }
        MakeBtn(tr("Speichern"), buttonArea, [this]() { SaveDocument(); });
    }
}

XMLExplorer::~XMLExplorer() = default;

void XMLExplorer::AddChild(tinyxml2::XMLElement* XmlElement, QTreeWidgetItem* TreeWidgetItemParent)
{
    for (auto* element = XmlElement->FirstChildElement(); element; element = element->NextSiblingElement()) {
        auto* item = new QTreeWidgetItem();
        item->setText(0, QString::fromUtf8(element->Name()));
        item->setText(1, QString::fromUtf8(element->GetText() ? element->GetText() : ""));
        TreeWidgetItemParent->addChild(item);

        itemToElement.insert(item, element); // NEW: map item -> element

        AddChild(element, item);
    }
}

void XMLExplorer::SaveDocument()
{
    if (!xmlDoc) {
        QMessageBox::warning(this, tr("Fehler"), tr("Kein XML-Dokument geladen"));
        return;
    }

    const auto filename = QFileDialog::getSaveFileName(this, tr("XML-Datei speichern"), {}, tr("XML-Dateien (*.xml)"));
    if (filename.isEmpty()) return;

    const auto err = xmlDoc->SaveFile(filename.toUtf8().constData());
    if (err != tinyxml2::XML_SUCCESS) {
        QMessageBox::critical(this, tr("Fehler"),
            tr("Fehler beim Speichern der Datei: %1")
            .arg(xmlDoc->ErrorIDToName(err)));
    }
    else {
        QMessageBox::information(this, tr("Erfolg"), tr("Datei wurde erfolgreich gespeichert"));
        statusBar()->showMessage(tr("Datei gespeichert"), 3000);
    }
}

void XMLExplorer::onItemSelectionChanged()
{
    const auto selectedItems = treeWidget->selectedItems();
    if (selectedItems.isEmpty()) {
        detailsTextEdit->clear();
        attributeComboBox->clear();
        attributeValueEdit->clear();
        currentElement = nullptr;
        attributeComboBox->setEnabled(false);
        attributeValueEdit->setEnabled(false);
        return;
    }

    QTreeWidgetItem* selectedItem = selectedItems.first();

    // NEW: robust lookup via map (no sibling-index walking)
    currentElement = itemToElement.value(selectedItem, nullptr);

    if (!currentElement) {
        detailsTextEdit->setText(tr("Element nicht gefunden"));
        attributeComboBox->clear();
        attributeValueEdit->clear();
        attributeComboBox->setEnabled(false);
        attributeValueEdit->setEnabled(false);
        return;
    }

    // Details anzeigen
    QStringList lines;
    lines << tr("Details zum ausgewählten Element:") << "";
    lines << QStringLiteral("Name: %1").arg(QString::fromUtf8(currentElement->Name()));

    // Attribute in Details
    lines << "" << tr("Attribute:");
    if (auto* a = currentElement->FirstAttribute()) {
        for (; a; a = a->Next()) {
            lines << QStringLiteral(" - %1 = %2")
                .arg(QString::fromUtf8(a->Name()),
                    QString::fromUtf8(a->Value()));
        }
    }
    else {
        lines << tr(" (keine)");
    }

    // Text/Inhalt anzeigen
    lines << "" << tr("Inhalt:");
    if (currentElement->GetText()) {
        lines << QString::fromUtf8(currentElement->GetText());
    }
    else {
        lines << tr("(kein Textinhalt)");
    }

    detailsTextEdit->setText(lines.join('\n'));

    // NEW: actually populate the combobox with attribute names
    populateAttributes(currentElement);
}

void XMLExplorer::populateAttributes(tinyxml2::XMLElement* el)
{
    attributeComboBox->blockSignals(true);
    attributeComboBox->clear();

    for (auto* attr = el->FirstAttribute(); attr; attr = attr->Next()) {
        attributeComboBox->addItem(QString::fromUtf8(attr->Name()));
    }

    const bool hasAttrs = attributeComboBox->count() > 0;
    attributeComboBox->setEnabled(true);        // editable: allow adding new ones
    attributeValueEdit->setEnabled(true);

    if (hasAttrs) {
        attributeComboBox->setCurrentIndex(0);  // triggers onAttributeSelected(0)
        onAttributeSelected(0);
    }
    else {
        attributeValueEdit->clear();
        attributeComboBox->setEditText(QString()); // allow entering a new attribute key
    }
    attributeComboBox->blockSignals(false);
}

void XMLExplorer::onAttributeSelected(int index)
{
    if (!currentElement) {
        attributeValueEdit->clear();
        return;
    }

    // If no items, user may be typing a NEW attribute name (editable combobox)
    if (attributeComboBox->count() == 0) {
        attributeValueEdit->clear();
        return;
    }

    if (index < 0) index = attributeComboBox->currentIndex();
    if (index < 0) { attributeValueEdit->clear(); return; }

    const auto attributeName = attributeComboBox->itemText(index);
    const char* value = currentElement->Attribute(attributeName.toUtf8().constData());
    attributeValueEdit->setText(value ? QString::fromUtf8(value) : QString());
}

void XMLExplorer::updateAttributeValue()
{
    if (!currentElement) return;

    // supports both selecting existing and typing a new attribute name
    const auto attributeName = attributeComboBox->currentText().trimmed();
    const auto attributeValue = attributeValueEdit->text();

    if (attributeName.isEmpty()) {
        QMessageBox::warning(this, tr("Hinweis"), tr("Bitte einen Attributnamen eingeben."));
        return;
    }

    currentElement->SetAttribute(attributeName.toUtf8().constData(),
        attributeValue.toUtf8().constData());

    // Refresh UI
    populateAttributes(currentElement);
    onItemSelectionChanged(); // refresh details text

    tinyxml2::XMLPrinter printer;
    xmlDoc->Print(&printer);
    QByteArray bytes(printer.CStr(), static_cast<int>(strlen(printer.CStr())));

    QSaveFile out(currentFilePath);
    if (!out.open(QIODevice::WriteOnly | QIODevice::Truncate)) {
        QMessageBox::critical(this, tr("Fehler"),
            tr("Fehler beim Öffnen der Datei zum Schreiben: %1")
			.arg(out.errorString()));
        return;
    }
    if (out.write(bytes) != bytes.size() || !out.commit()) {
        QMessageBox::critical(this, tr("Fehler"),
			tr("Fehler beim Schreiben der Datei: %1")
            .arg(out.errorString()));
		return;
    }

    dirty = false;

    statusBar()->showMessage(tr("Attribut aktualisiert"), 3000);
}
