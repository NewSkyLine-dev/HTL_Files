#ifndef _QX_BLOG_CATEGORY_H_
#define _QX_BLOG_CATEGORY_H_

class blog;

class QX_BLOG_DLL_EXPORT category
{
public:
// -- typedef
   typedef std::shared_ptr<blog> blog_ptr;
   typedef qx::QxCollection<long, blog_ptr> list_blog;
// -- properties
   QString     m_id;
   QString     m_name;
   QString     m_desc;
   list_blog   m_blogX;
// -- contructor, virtual destructor
   category() { ; }
   virtual ~category() { ; }
};

QX_REGISTER_PRIMARY_KEY(category, QString)
QX_REGISTER_HPP_QX_BLOG(category, qx::trait::no_base_class_defined, 0)

typedef QSharedPointer<category> category_ptr;
typedef qx::QxCollection<QString, category_ptr> list_category;

#endif // _QX_BLOG_CATEGORY_H_
