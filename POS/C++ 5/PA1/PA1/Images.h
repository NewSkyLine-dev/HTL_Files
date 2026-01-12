#pragma once
class Images
{
public:
	long id;
	QString filename;
	QString width;
	QString height;

	Images() : id(0), filename(""), width(""), height("") { ; }
	Images(const QString& fn, const QString& w, const QString& h)
		: id(0), filename(fn), width(w), height(h) {
		;
	}
	virtual ~Images() { ; }
};

QX_REGISTER_PRIMARY_KEY(Images, long)
QX_REGISTER_HPP_QX_PA1(Images, qx::trait::no_base_class_defined, 0)