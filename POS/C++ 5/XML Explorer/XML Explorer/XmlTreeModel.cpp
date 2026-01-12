#include "XmlTreeModel.h"

bool XmlTreeModel::loadFile(const QString &filepath, QString *err)
{
    XMLDocument tmp;
    XMLError rc = tmp.LoadFile(filepath.toUtf8().constData());
    if (rc != XML_SUCCESS)
    {
        if (err)
            *err = QString("Load error %1 at line %2").arg(int(rc)).arg(tmp.ErrorLineNum());
        return false;
    }

    beginResetModel();
    doc = std::make_unique<XMLDocument>();

    // Use DeepClone instead of DeepCopy - DeepCopy doesn't work properly
    XMLNode *rootCopy = tmp.RootElement()->DeepClone(doc.get());
    doc->InsertEndChild(rootCopy);

    this->filepath = filepath;
    rebuildTree();
    endResetModel();

    return true;
}

void XmlTreeModel::refreshNode(const QModelIndex &index)
{
    if (!index.isValid())
        return;

    TreeItem *item = static_cast<TreeItem *>(index.internalPointer());
    if (!item)
        return;

    // Rebuild the children of this node
    populateChildren(item);

    // Notify the view that the data has changed
    emit dataChanged(index, index.sibling(index.row(), ColCount - 1));

    // If this item has children, notify about layout change
    if (item->children.size() > 0)
    {
        QModelIndex firstChild = this->index(0, 0, index);
        QModelIndex lastChild = this->index(item->children.size() - 1, ColCount - 1, index);
        emit dataChanged(firstChild, lastChild);
    }
}

bool XmlTreeModel::saveFile(QString *err)
{
    if (!doc)
    {
        if (err)
            *err = "No document loaded";
        return false;
    }
    if (filepath.isEmpty())
    {
        if (err)
            *err = "No file path";
        return false;
    }
    XMLError rc = doc->SaveFile(filepath.toUtf8().constData());
    if (rc != XML_SUCCESS)
    {
        if (err)
            *err = QString("Save error %1").arg(int(rc));
        return false;
    }
    return true;
}

QModelIndex XmlTreeModel::index(int row, int column, const QModelIndex &parent) const
{
    if (!hasIndex(row, column, parent))
        return {};
    TreeItem *parentItem = parent.isValid()
                               ? static_cast<TreeItem *>(parent.internalPointer())
                               : root;
    return createIndex(row, column, parentItem->children.at(row));
}

QModelIndex XmlTreeModel::parent(const QModelIndex &child) const
{
    if (!child.isValid())
        return {};
    TreeItem *childItem = static_cast<TreeItem *>(child.internalPointer());
    TreeItem *parentItem = childItem->parent;
    if (parentItem == root || !parentItem)
        return {};
    TreeItem *grandParentItem = parentItem->parent;
    int row = grandParentItem ? grandParentItem->children.indexOf(parentItem) : 0;
    return createIndex(row, 0, parentItem);
}

int XmlTreeModel::rowCount(const QModelIndex &parent) const
{
    TreeItem *parentItem = parent.isValid()
                               ? static_cast<TreeItem *>(parent.internalPointer())
                               : root;
    return parentItem->children.size();
}

int XmlTreeModel::columnCount(const QModelIndex &parent) const
{
    return ColCount;
}

QVariant XmlTreeModel::headerData(int section, Qt::Orientation orientation, int role) const
{
    if (orientation != Qt::Horizontal || role != Qt::DisplayRole)
        return {};

    switch (section)
    {
    case ColName:
        return "Name";
    case ColValue:
        return "Value";
    default:
        return {};
    }
}

QVariant XmlTreeModel::data(const QModelIndex &index, int role) const
{
    if (!index.isValid())
        return {};

    if (role != Qt::DisplayRole && role != Qt::EditRole)
        return {};

    TreeItem *item = static_cast<TreeItem *>(index.internalPointer());

    if (index.column() == ColName)
    {
        if (item->isAttribute())
            return "@" + item->attrName;
        if (auto *el = itemAsElement(item))
            return el->Name();
        return {};
    }
    else
    {
        if (item->isAttribute())
        {
            if (auto *el = itemAsElement(item))
                if (auto *a = findAttribute(el, item->attrName))
                    return a->Value();
            return {};
        }
        else if (auto *el = itemAsElement(item))
        {
            const char *text = el->GetText();
            return text ? QString::fromUtf8(text) : QVariant{};
        }
        return {};
    }
}

void XmlTreeModel::rebuildTree()
{
    delete root;
    root = new TreeItem;
    if (!doc)
        return;

    XMLElement *rootEl = doc->RootElement();
    if (!rootEl)
        return;

    auto *child = new TreeItem{TreeItem::Kind::Element, rootEl, {}, root, {}};
    root->children.push_back(child);
    populateChildren(child);
}

void XmlTreeModel::populateChildren(TreeItem *item)
{
    item->children.clear();
    auto *el = itemAsElement(item);
    if (!el)
        return;

    // Attributes first
    for (const XMLAttribute *a = el->FirstAttribute(); a; a = a->Next())
    {
        auto *attrItem = new TreeItem{TreeItem::Kind::Attribute, el, a->Name(), item, {}};
        item->children.push_back(attrItem);
    }

    // Element children
    for (XMLNode *c = el->FirstChild(); c; c = c->NextSibling())
    {
        if (auto *ce = c->ToElement())
        {
            auto *child = new TreeItem{TreeItem::Kind::Element, ce, {}, item, {}};
            item->children.push_back(child);
            populateChildren(child);
        }
    }
}

Qt::ItemFlags XmlTreeModel::flags(const QModelIndex &index) const
{
    if (!index.isValid())
        return Qt::NoItemFlags;

    Qt::ItemFlags baseFlags = QAbstractItemModel::flags(index);

    // Make the value column editable
    if (index.column() == ColValue)
        return baseFlags | Qt::ItemIsEditable;

    return baseFlags;
}

bool XmlTreeModel::setData(const QModelIndex &index, const QVariant &value, int role)
{
    if (!index.isValid() || role != Qt::EditRole)
        return false;

    if (index.column() != ColValue)
        return false;

    TreeItem *item = static_cast<TreeItem *>(index.internalPointer());
    QString newValue = value.toString();

    if (item->isAttribute())
    {
        // Edit attribute value
        auto *el = itemAsElement(item);
        if (!el)
            return false;

        el->SetAttribute(item->attrName.toUtf8().constData(), newValue.toUtf8().constData());
        emit dataChanged(index, index, {Qt::DisplayRole, Qt::EditRole});
        return true;
    }
    else
    {
        // Edit element text content
        auto *el = itemAsElement(item);
        if (!el)
            return false;

        el->SetText(newValue.toUtf8().constData());
        emit dataChanged(index, index, {Qt::DisplayRole, Qt::EditRole});
        return true;
    }

    return false;
}
