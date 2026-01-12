#include "XMLExplorer.h"

#include <QToolBar>
#include <QFileDialog>
#include <QMessageBox>
#include <QSplitter>
#include <QVBoxLayout>
#include <QLabel>
#include <QWidget>

XMLExplorer::XMLExplorer(QWidget *parent)
	: QMainWindow(parent)
{
	resize(1000, 900);

	// Create main splitter
	QSplitter *splitter = new QSplitter(Qt::Horizontal, this);

	// Left side: Tree view
	m_treeView = new QTreeView(splitter);
	m_treeView->setEditTriggers(QAbstractItemView::DoubleClicked | QAbstractItemView::SelectedClicked | QAbstractItemView::EditKeyPressed);

	// Right side: Detail panel
	QWidget *detailPanel = new QWidget(splitter);
	QVBoxLayout *detailLayout = new QVBoxLayout(detailPanel);

	detailLayout->addWidget(new QLabel("Element Details:"));
	m_detailText = new QTextEdit(detailPanel);
	m_detailText->setReadOnly(true);
	detailLayout->addWidget(m_detailText);

	detailLayout->addWidget(new QLabel("Attributes:"));
	m_attributeCombo = new QComboBox(detailPanel);
	detailLayout->addWidget(m_attributeCombo);

	detailLayout->addWidget(new QLabel("Attribute Value:"));
	m_attributeValueEdit = new QLineEdit(detailPanel);
	detailLayout->addWidget(m_attributeValueEdit);

	detailPanel->setLayout(detailLayout);

	// Set splitter proportions
	splitter->addWidget(m_treeView);
	splitter->addWidget(detailPanel);
	splitter->setStretchFactor(0, 2);
	splitter->setStretchFactor(1, 1);

	setCentralWidget(splitter);

	model = new XmlTreeModel(this);
	m_treeView->setModel(model);

	// Connect signals
	connect(m_treeView->selectionModel(), &QItemSelectionModel::selectionChanged,
			this, &XMLExplorer::onSelectionChanged);
	connect(m_attributeCombo, QOverload<int>::of(&QComboBox::currentIndexChanged),
			this, &XMLExplorer::onAttributeSelected);
	connect(m_attributeValueEdit, &QLineEdit::editingFinished,
			this, &XMLExplorer::onAttributeValueChanged);

	auto *openAct = new QAction("Open", this);
	auto *saveAct = new QAction("Save", this);

	connect(openAct, &QAction::triggered, this, &XMLExplorer::openFile);
	connect(saveAct, &QAction::triggered, this, &XMLExplorer::saveFile);

	auto *tb = addToolBar("Main");
	tb->addAction(openAct);
	tb->addAction(saveAct);
}

XMLExplorer::~XMLExplorer()
{
}

void XMLExplorer::openFile()
{
	QString f = QFileDialog::getOpenFileName(this, "Open XML", {}, "XML Files (*.xml);;All files (*)");
	if (f.isEmpty())
		return;
	QString err;
	if (!model->loadFile(f, &err))
	{
		QMessageBox::critical(this, "Open failed", err);
		return;
	}
	m_treeView->expandToDepth(1);
	setWindowTitle(QString("XML Explorer - %1").arg(QFileInfo(f).fileName()));
}

void XMLExplorer::saveFile()
{
	QString err;
	if (!model->saveFile(&err))
	{
		QMessageBox::critical(this, "Save failed", err);
	}
}

void XMLExplorer::onSelectionChanged()
{
	QModelIndexList selection = m_treeView->selectionModel()->selectedIndexes();
	if (selection.isEmpty())
	{
		m_detailText->clear();
		m_attributeCombo->clear();
		m_attributeValueEdit->clear();
		return;
	}

	// Get the first selected index (in the first column)
	QModelIndex index = selection.first();
	if (index.column() != 0)
		index = index.sibling(index.row(), 0);

	updateDetailPanel(index);
}

void XMLExplorer::updateDetailPanel(const QModelIndex &index)
{
	if (!index.isValid())
	{
		return;
	}

	TreeItem *item = static_cast<TreeItem *>(index.internalPointer());

	// Clear previous content
	m_detailText->clear();
	m_attributeValueEdit->clear();

	m_attributeCombo->clear();

	if (!item || !item->node)
	{
		return;
	}

	XMLElement *element = item->node->ToElement();
	if (!element)
	{
		return;
	}

	// Build detail text
	QString details;
	details += QString("Element: %1\n\n").arg(element->Name());

	// Show attributes
	details += "Attributes:\n";
	const XMLAttribute *attr = element->FirstAttribute();
	int attrCount = 0;
	if (attr)
	{
		while (attr)
		{
			details += QString("  @%1 = \"%2\"\n").arg(attr->Name()).arg(attr->Value());
			m_attributeCombo->addItem(attr->Name(), QVariant::fromValue((void *)attr));
			attrCount++;
			attr = attr->Next();
		}
	}
	else
	{
		details += "  (none)\n";
	}

	// Show text content
	const char *text = element->GetText();
	if (text && text[0] != '\0')
	{
		details += QString("\nText Content:\n%1").arg(text);
	}

	m_detailText->setPlainText(details);

	if (m_attributeCombo->count() > 0)
	{
		m_attributeCombo->setCurrentIndex(0);
		onAttributeSelected(0);
	}
}

void XMLExplorer::onAttributeSelected(int index)
{
	if (index < 0 || index >= m_attributeCombo->count())
	{
		m_attributeValueEdit->clear();
		return;
	}

	const XMLAttribute *attr = static_cast<const XMLAttribute *>(
		m_attributeCombo->itemData(index).value<void *>());

	if (attr)
		m_attributeValueEdit->setText(attr->Value());
}

void XMLExplorer::onAttributeValueChanged()
{
	int index = m_attributeCombo->currentIndex();
	if (index < 0)
		return;

	QModelIndexList selection = m_treeView->selectionModel()->selectedIndexes();
	if (selection.isEmpty())
		return;

	QModelIndex treeIndex = selection.first();
	if (treeIndex.column() != 0)
		treeIndex = treeIndex.sibling(treeIndex.row(), 0);

	TreeItem *item = static_cast<TreeItem *>(treeIndex.internalPointer());
	if (!item || !item->node)
		return;

	XMLElement *element = item->node->ToElement();
	if (!element)
		return;

	QString attrName = m_attributeCombo->currentText();
	QString newValue = m_attributeValueEdit->text();

	qDebug() << "Updating attribute" << attrName << "to" << newValue;

	// Update the attribute in the XML
	element->SetAttribute(attrName.toUtf8().constData(), newValue.toUtf8().constData());

	// Refresh the node in the tree to show updated attributes
	model->refreshNode(treeIndex);

	// Refresh the detail panel to show the updated value
	updateDetailPanel(treeIndex);
}
