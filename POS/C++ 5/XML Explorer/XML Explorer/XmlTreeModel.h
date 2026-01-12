#pragma once

#include "TreeItem.h"

#include <QCoreApplication>

#include <QAbstractItemModel>
#include <tinyxml2.h>

class XmlTreeModel : public QAbstractItemModel
{
    Q_OBJECT
public:
    enum Columns
    {
        ColName,
        ColValue,
        ColCount
    };

    explicit XmlTreeModel(QObject *parent = nullptr) : QAbstractItemModel(parent)
    {
        root = new TreeItem;
    }

    bool loadFile(const QString &filepath, QString *err = nullptr);
    bool saveFile(QString *err = nullptr);
    void refreshNode(const QModelIndex &index);

    // Geerbt über QAbstractItemModel
    QModelIndex index(int row, int column, const QModelIndex &parent) const override;
    QModelIndex parent(const QModelIndex &child) const override;
    int rowCount(const QModelIndex &parent) const override;
    int columnCount(const QModelIndex &parent) const override;
    QVariant data(const QModelIndex &index, int role) const override;
    bool setData(const QModelIndex &index, const QVariant &value, int role) override;
    Qt::ItemFlags flags(const QModelIndex &index) const override;
    QVariant headerData(int section, Qt::Orientation orientation, int role) const override;

private:
    TreeItem *root = nullptr;
    std::unique_ptr<tinyxml2::XMLDocument> doc;
    QString filepath;

    static XMLElement *itemAsElement(TreeItem *item)
    {
        return item && item->node ? item->node->ToElement() : nullptr;
    }

    static const XMLAttribute *findAttribute(XMLElement *el, const QString &name)
    {
        if (!el)
            return nullptr;
        for (const XMLAttribute *a = el->FirstAttribute(); a; a = a->Next())
        {
            if (name == a->Name())
                return a;
        }
        return nullptr;
    }

    void rebuildTree();
    void populateChildren(TreeItem *item);
};
