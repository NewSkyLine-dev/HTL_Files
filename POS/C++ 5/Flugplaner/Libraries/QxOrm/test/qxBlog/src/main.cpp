#include "../include/precompiled.h"

#include <QtCore/qcoreapplication.h>

#include "../include/blog.h"
#include "../include/author.h"
#include "../include/comment.h"
#include "../include/category.h"

#include <QxOrm_Impl.h>

int main(int argc, char * argv[])
{
   // Qt application
   QCoreApplication app(argc, argv);
   QFile::remove("./qxBlog.sqlite");

   // Parameters to connect to database
   qx::QxSqlDatabase::getSingleton()->setDriverName("QSQLITE");
   qx::QxSqlDatabase::getSingleton()->setDatabaseName("./qxBlog.sqlite");
   qx::QxSqlDatabase::getSingleton()->setHostName("localhost");
   qx::QxSqlDatabase::getSingleton()->setUserName("root");
   qx::QxSqlDatabase::getSingleton()->setPassword("");
   qx::QxSqlDatabase::getSingleton()->setFormatSqlQueryBeforeLogging(true);
   qx::QxSqlDatabase::getSingleton()->setDisplayTimerDetails(true);

   // Only for debug purpose : assert if invalid offset detected fetching a relation
   qx::QxSqlDatabase::getSingleton()->setVerifyOffsetRelation(true);

   // Create all tables in database
   QSqlError daoError = qx::dao::create_table<author>();
   daoError = qx::dao::create_table<comment>();
   daoError = qx::dao::create_table<category>();
   daoError = qx::dao::create_table<blog>();

   // Create a list of 3 author
   author_ptr author_1; author_1.reset(new author());
   author_ptr author_2; author_2.reset(new author());
   author_ptr author_3; author_3.reset(new author());

   author_1->m_id = "author_id_1"; author_1->m_name = "author_1";
   author_1->m_sex = author::male; author_1->m_birthdate = QDate::currentDate();
   author_2->m_id = "author_id_2"; author_2->m_name = "author_2";
   author_2->m_sex = author::female; author_2->m_birthdate = QDate::currentDate();
   author_3->m_id = "author_id_3"; author_3->m_name = "author_3";
   author_3->m_sex = author::female; author_3->m_birthdate = QDate::currentDate();

   list_author authorX;
   authorX.insert(author_1->m_id, author_1);
   authorX.insert(author_2->m_id, author_2);
   authorX.insert(author_3->m_id, author_3);

   // Insert list of 3 author into database
   daoError = qx::dao::insert(authorX);
   qAssert(qx::dao::count<author>() == 3);

   // Delete all authors in database and try to insert them using exec batch method
   daoError = qx::dao::delete_all<author>(); qAssert(! daoError.isValid());
   qAssert(qx::dao::count<author>() == 0);
   daoError = qx::dao::insert(authorX, NULL, true); qAssert(! daoError.isValid());
   qAssert(qx::dao::count<author>() == 3);

   // Clone author 2 : 'author_id_2'
   author_ptr author_clone = qx::clone(* author_2);
   qAssert(author_clone->m_id == "author_id_2");
   qAssert(author_clone->m_sex == author::female);

   // Create a query to fetch only female author : 'author_id_2' and 'author_id_3'
   qx::QxSqlQuery query("WHERE author.sex = :sex");
   query.bind(":sex", author::female);

   list_author list_of_female_author;
   daoError = qx::dao::fetch_by_query(query, list_of_female_author);
   qAssert(list_of_female_author.count() == 2);

   // Dump list of female author (xml serialization)
   qx::dump(list_of_female_author, false);
   qx::dump(list_of_female_author, true);

   // Test qx::QxSqlQuery::freeText() with/without placeholders
   query = qx_query(); query.freeText("WHERE author.sex = " + QString::number(static_cast<int>(author::female)));
   daoError = qx::dao::fetch_by_query(query, list_of_female_author);
   qAssert(list_of_female_author.count() == 2);
   query = qx_query(); query.freeText("WHERE author.sex = :sex", QVariantList() << author::female);
   daoError = qx::dao::fetch_by_query(query, list_of_female_author);
   qAssert(list_of_female_author.count() == 2);
   query = qx_query(); query.freeText("WHERE author.sex=:sex AND author.author_id=:author_id", QVariantList() << author::female << "author_id_2");
   daoError = qx::dao::fetch_by_query(query, list_of_female_author);
   qAssert(list_of_female_author.count() == 1);
   query = qx_query(); query.freeText("WHERE (author.sex = :sex) AND (author.author_id = :author_id)", QVariantList() << author::female << "author_id_2");
   daoError = qx::dao::fetch_by_query(query, list_of_female_author);
   qAssert(list_of_female_author.count() == 1);

   // Create 3 categories
   category_ptr category_1 = category_ptr(new category());
   category_ptr category_2 = category_ptr(new category());
   category_ptr category_3 = category_ptr(new category());

   category_1->m_name = "category_1"; category_1->m_desc = "desc_1";
   category_2->m_name = "category_2"; category_2->m_desc = "desc_2";
   category_3->m_name = "category_3"; category_3->m_desc = "desc_3";

   { // Create a scope to destroy temporary connexion to database

   // Open a transaction to database
   QSqlDatabase db = qx::QxSqlDatabase::getDatabase();
   bool bCommit = db.transaction();

   // Insert 3 categories into database, use 'db' parameter for the transaction
   daoError = qx::dao::insert(category_1, (& db));    bCommit = (bCommit && ! daoError.isValid());
   daoError = qx::dao::insert(category_2, (& db));    bCommit = (bCommit && ! daoError.isValid());
   daoError = qx::dao::insert(category_3, (& db));    bCommit = (bCommit && ! daoError.isValid());

   qAssert(bCommit);
   qAssert(category_1->m_id != 0);
   qAssert(category_2->m_id != 0);
   qAssert(category_3->m_id != 0);

   // Terminate transaction => commit or rollback if there is error
   if (bCommit) { db.commit(); }
   else { db.rollback(); }

   } // End of scope : 'db' is destroyed

   // Create a blog with the class name (factory)
   qx::any blog_any = qx::create("blog");
   blog_ptr blog_1;
   try { blog_1 = qx::any_cast<blog_ptr>(blog_any); }
   catch (...) { blog_1.reset(new blog()); }
   blog_1->m_text = "blog_text_1";
   blog_1->m_dt_creation = QDateTime::currentDateTime();
   blog_1->m_author = author_1;

   // Insert 'blog_1' into database with 'save()' method
   daoError = qx::dao::save(blog_1);

   // Modify 'blog_1' properties and save into database
   blog_1->m_text = "update blog_text_1";
   blog_1->m_author = author_2;
   daoError = qx::dao::save(blog_1);

   // Add 2 comments to 'blog_1'
   comment_ptr comment_1; comment_1.reset(new comment());
   comment_ptr comment_2; comment_2.reset(new comment());

   comment_1->m_text = "comment_1 text";
   comment_1->m_dt_create = QDateTime::currentDateTime();
   comment_1->m_blog = blog_1;
   comment_2->m_text = "comment_2 text";
   comment_2->m_dt_create = QDateTime::currentDateTime();
   comment_2->m_blog = blog_1;

   daoError = qx::dao::insert(comment_1);
   daoError = qx::dao::insert(comment_2);
   qAssert(qx::dao::count<comment>() == 2);

   // Add 2 categories to 'blog_1' => must insert into extra-table 'category_blog'
   blog_1->m_categoryX.insert(category_1->m_id, category_1);
   blog_1->m_categoryX.insert(category_3->m_id, category_3);
   daoError = qx::dao::save_with_relation("list_category", blog_1);

   // Fetch blog into a new variable with all relation : 'author', 'comment' and 'category'
   blog_ptr blog_tmp; blog_tmp.reset(new blog());
   blog_tmp->m_id = blog_1->m_id;
   daoError = qx::dao::fetch_by_id_with_all_relation(blog_tmp);

   qAssert(blog_tmp->m_commentX.count() == 2);
   qAssert(blog_tmp->m_categoryX.count() == 2);
   qAssert(blog_tmp->m_text == "update blog_text_1");
   qAssert(blog_tmp->m_author && blog_tmp->m_author->m_id == "author_id_2");

   // Fetch blog into a new variable with many relations using "*->*->*->*" (4 levels of relationships)
   blog_tmp.reset(new blog());
   blog_tmp->m_id = blog_1->m_id;
   daoError = qx::dao::fetch_by_id_with_relation("*->*->*->*", blog_tmp);

   qAssert(blog_tmp->m_commentX.count() == 2);
   qAssert(blog_tmp->m_categoryX.count() == 2);
   qAssert(blog_tmp->m_text == "update blog_text_1");
   qAssert(blog_tmp->m_author && blog_tmp->m_author->m_id == "author_id_2");

   // Dump 'blog_tmp' result from database (xml serialization)
   qx::dump(blog_tmp, false);
   qx::dump(blog_tmp, true);

   // Fetch relations defining columns to fetch with syntax { col_1, col_2, etc... }
   list_blog lstBlogComplexRelation;
   daoError = qx::dao::fetch_all_with_relation(QStringList() << "{ blog_text }" << "author_id { name, birthdate }" << "list_comment { comment_text } -> blog_id -> *", lstBlogComplexRelation);
   qx::dump(lstBlogComplexRelation);
   qAssert(lstBlogComplexRelation.size() > 0);
   qAssert(lstBlogComplexRelation[0]->m_text != ""); // Fetched
   qAssert(lstBlogComplexRelation[0]->m_dt_creation.isNull()); // Not fetched
   qAssert(lstBlogComplexRelation[0]->m_author->m_sex == author::unknown); // Not fetched
   qAssert(lstBlogComplexRelation[0]->m_author->m_name != ""); // Fetched
   qAssert(lstBlogComplexRelation[0]->m_commentX.size() > 0);
   qAssert(lstBlogComplexRelation[0]->m_commentX[0]->m_dt_create.isNull()); // Not fetched
   qAssert(lstBlogComplexRelation[0]->m_commentX[0]->m_text != ""); // Fetched
   qAssert(lstBlogComplexRelation[0]->m_commentX[0]->m_blog);

   // Fetch relations defining columns to remove before fetching with syntax -{ col_1, col_2, etc... }
   list_blog lstBlogComplexRelation2;
   daoError = qx::dao::fetch_all_with_relation(QStringList() << "-{ blog_text }" << "author_id -{ name, birthdate }" << "list_comment -{ comment_text } -> blog_id -> *", lstBlogComplexRelation2);
   qx::dump(lstBlogComplexRelation2);
   qAssert(lstBlogComplexRelation2.size() > 0);
   qAssert(lstBlogComplexRelation2[0]->m_text == ""); // Not fetched
   qAssert(! lstBlogComplexRelation2[0]->m_dt_creation.isNull()); // Fetched
   qAssert(lstBlogComplexRelation2[0]->m_author->m_sex != author::unknown); // Fetched
   qAssert(lstBlogComplexRelation2[0]->m_author->m_name == ""); // Not fetched
   qAssert(lstBlogComplexRelation2[0]->m_commentX.size() > 0);
   qAssert(! lstBlogComplexRelation2[0]->m_commentX[0]->m_dt_create.isNull()); // Fetched
   qAssert(lstBlogComplexRelation2[0]->m_commentX[0]->m_text == ""); // Not fetched
   qAssert(lstBlogComplexRelation2[0]->m_commentX[0]->m_blog);

#ifndef _QX_NO_JSON
   // Custom JSON serialization process
   QString customJsonFull = qx::serialization::json::to_string(blog_tmp, 1);
   QString customJsonFiltered = qx::serialization::json::to_string(blog_tmp, 1, "filter: { blog_text } | author_id { name, birthdate } | list_comment { comment_text } -> blog_id -> *");
   qDebug("[QxOrm] custom JSON serialization process (full) : \n%s", qPrintable(customJsonFull));
   qDebug("[QxOrm] custom JSON serialization process (filtered) : \n%s", qPrintable(customJsonFiltered));

   blog_ptr blogFromJsonFull; blogFromJsonFull.reset(new blog());
   blog_ptr blogFromJsonFiltered; blogFromJsonFiltered.reset(new blog());
   qx::serialization::json::from_string(blogFromJsonFull, customJsonFull, 1);
   qx::serialization::json::from_string(blogFromJsonFiltered, customJsonFull, 1, "filter: { blog_text } | author_id { name, birthdate } | list_comment { comment_text } -> blog_id -> *");

   qx::dump(blogFromJsonFull);
   qAssert(blogFromJsonFull->m_commentX.count() == 2);
   qAssert(blogFromJsonFull->m_categoryX.count() == 2);
   qAssert(blogFromJsonFull->m_text == "update blog_text_1");
   qAssert(blogFromJsonFull->m_author && blogFromJsonFull->m_author->m_id == "author_id_2");

   qx::dump(blogFromJsonFiltered);
   qAssert(blogFromJsonFiltered->m_text != ""); // Fetched
   qAssert(blogFromJsonFiltered->m_dt_creation.isNull()); // Not fetched
   qAssert(blogFromJsonFiltered->m_author->m_sex == author::unknown); // Not fetched
   qAssert(blogFromJsonFiltered->m_author->m_name != ""); // Fetched
   qAssert(blogFromJsonFiltered->m_commentX.size() > 0);
   qAssert(blogFromJsonFiltered->m_commentX[0]->m_dt_create.isNull()); // Not fetched
   qAssert(blogFromJsonFiltered->m_commentX[0]->m_text != ""); // Fetched
   qAssert(blogFromJsonFiltered->m_commentX[0]->m_blog);
#endif // _QX_NO_JSON

   // Fetch relations defining columns to fetch with syntax { col_1, col_2, etc... } + custom table alias using syntax <my_table_alias> + custom table alias suffix using syntax <..._my_alias_suffix>
   list_blog lstBlogComplexRelation3;
   daoError = qx::dao::fetch_all_with_relation(QStringList() << "<blog_alias> { blog_text }" << "author_id <author_alias> { name, birthdate }" << "list_comment <list_comment_alias> { comment_text } -> blog_id <blog_alias_2> -> * <..._my_alias_suffix>", lstBlogComplexRelation3);
   qx::dump(lstBlogComplexRelation3);
   qAssert(lstBlogComplexRelation3.size() > 0);
   qAssert(lstBlogComplexRelation3[0]->m_text != ""); // Fetched
   qAssert(lstBlogComplexRelation3[0]->m_dt_creation.isNull()); // Not fetched
   qAssert(lstBlogComplexRelation3[0]->m_author->m_sex == author::unknown); // Not fetched
   qAssert(lstBlogComplexRelation3[0]->m_author->m_name != ""); // Fetched
   qAssert(lstBlogComplexRelation3[0]->m_commentX.size() > 0);
   qAssert(lstBlogComplexRelation3[0]->m_commentX[0]->m_dt_create.isNull()); // Not fetched
   qAssert(lstBlogComplexRelation3[0]->m_commentX[0]->m_text != ""); // Fetched
   qAssert(lstBlogComplexRelation3[0]->m_commentX[0]->m_blog);

   // Test to add join SQL sub-queries (inside LEFT OUTER JOIN or INNER JOIN)
   list_blog lstBlogWithJoinQueries;
   query = qx_query().where("blog_alias.blog_text").isEqualTo("update blog_text_1");
   query.addJoinQuery("list_comment_alias", "AND list_comment_alias.comment_text IS NOT NULL");
   query.addJoinQuery("author_alias", qx_query().freeText("AND author_alias.sex = :sex", QVariantList() << author::female));
   daoError = qx::dao::fetch_by_query_with_relation(QStringList() << "<blog_alias> { blog_text }" << "author_id <author_alias> { name, birthdate, sex }" << "list_comment <list_comment_alias> { comment_text }", query, lstBlogWithJoinQueries);
   qx::dump(lstBlogWithJoinQueries);
   qAssert(lstBlogWithJoinQueries.size() > 0);
   qAssert(lstBlogWithJoinQueries[0]->m_text == "update blog_text_1");
   qAssert(lstBlogWithJoinQueries[0]->m_author->m_sex == author::female);

   // When join SQL sub-queries are used, then relationships should keep user defined order (in this example : 'list_comment' before 'author')
   lstBlogWithJoinQueries.clear();
   query = qx_query().where("blog_alias.blog_text").isEqualTo("update blog_text_1");
   query.addJoinQuery("list_comment_alias", "AND list_comment_alias.comment_text IS NOT NULL");
   query.addJoinQuery("author_alias", qx_query("AND author_alias.sex = :sex", QVariantList() << author::female));
   daoError = qx::dao::fetch_by_query_with_relation(QStringList() << "<blog_alias> { blog_text }" << "list_comment <list_comment_alias> { comment_text }" << "author_id <author_alias> { name, birthdate, sex }", query, lstBlogWithJoinQueries);
   qx::dump(lstBlogWithJoinQueries);
   qAssert(lstBlogWithJoinQueries.size() > 0);
   qAssert(lstBlogWithJoinQueries[0]->m_text == "update blog_text_1");
   qAssert(lstBlogWithJoinQueries[0]->m_author->m_sex == author::female);

   // Check qx::dao::save_with_relation_recursive() function
   daoError = qx::dao::save_with_relation_recursive(blog_tmp);
   qAssert(! daoError.isValid());
   daoError = qx::dao::save_with_relation_recursive(blog_tmp, qx::dao::save_mode::e_update_only);
   qAssert(! daoError.isValid());

   // Call 'age()' method with class name and method name (reflexion)
   qx_bool bInvokeOk = qx::QxClassX::invoke("author", "age", author_1);
   qAssert(bInvokeOk);

   // Check count with relations and filter
   long lBlogCountWithRelation = 0; qx_query queryBlogCountWithRelation;
   daoError = qx::dao::count_with_relation<blog>(lBlogCountWithRelation, QStringList() << "author_id" << "list_comment -> blog_id -> *", queryBlogCountWithRelation);
   qAssert(! daoError.isValid() && (lBlogCountWithRelation > 0));

   // Test 'isDirty()' method
   qx::dao::ptr<blog> blog_isdirty = qx::dao::ptr<blog>(new blog());
   blog_isdirty->m_id = blog_1->m_id;
   daoError = qx::dao::fetch_by_id(blog_isdirty);
   qAssert(! daoError.isValid() && ! blog_isdirty.isDirty());

   blog_isdirty->m_text = "blog property 'text' modified => blog is dirty !!!";
   QStringList lstDiff; bool bDirty = blog_isdirty.isDirty(lstDiff);
   qAssert(bDirty && (lstDiff.count() == 1) && (lstDiff.at(0) == "blog_text"));
   if (bDirty) { qDebug("[QxOrm] test dirty 1 : blog is dirty => '%s'", qPrintable(lstDiff.join("|"))); }

   // Update only property 'm_text' of 'blog_isdirty'
   daoError = qx::dao::update_optimized(blog_isdirty);
   qAssert(! daoError.isValid() && ! blog_isdirty.isDirty());
   qx::dump(blog_isdirty, false);
   qx::dump(blog_isdirty, true);

   // Test 'isDirty()' method with a container
   typedef qx::dao::ptr< QList<author_ptr> > type_lst_author_test_is_dirty;
   type_lst_author_test_is_dirty container_isdirty = type_lst_author_test_is_dirty(new QList<author_ptr>());
   daoError = qx::dao::fetch_all(container_isdirty);
   qAssert(! daoError.isValid() && ! container_isdirty.isDirty() && (container_isdirty->count() == 3));

   author_ptr author_ptr_dirty = container_isdirty->at(1);
   author_ptr_dirty->m_name = "author name modified at index 1 => container is dirty !!!";
   bDirty = container_isdirty.isDirty(lstDiff);
   qAssert(bDirty && (lstDiff.count() == 1));
   if (bDirty) { qDebug("[QxOrm] test dirty 2 : container is dirty => '%s'", qPrintable(lstDiff.join("|"))); }

   author_ptr_dirty = container_isdirty->at(2);
   author_ptr_dirty->m_birthdate = QDate(1998, 03, 06);
   bDirty = container_isdirty.isDirty(lstDiff);
   qAssert(bDirty && (lstDiff.count() == 2));
   if (bDirty) { qDebug("[QxOrm] test dirty 3 : container is dirty => '%s'", qPrintable(lstDiff.join("|"))); }

   // Update only property 'm_name' at position 1, only property 'm_birthdate' at position 2 and nothing at position 0
   daoError = qx::dao::update_optimized(container_isdirty);
   qAssert(! daoError.isValid() && ! container_isdirty.isDirty());
   qx::dump(container_isdirty, false);
   qx::dump(container_isdirty, true);

   // Fetch only property 'm_dt_creation' of blog
   QStringList lstColumns = QStringList() << "date_creation";
   list_blog lst_blog_with_only_date_creation;
   daoError = qx::dao::fetch_all(lst_blog_with_only_date_creation, NULL, lstColumns);
   qAssert(! daoError.isValid() && (lst_blog_with_only_date_creation.size() > 0));
   if ((lst_blog_with_only_date_creation.size() > 0) && (lst_blog_with_only_date_creation[0].get() != NULL))
   { qAssert(lst_blog_with_only_date_creation[0]->m_text.isEmpty()); }
   qx::dump(lst_blog_with_only_date_creation, false);
   qx::dump(lst_blog_with_only_date_creation, true);

   // Dump all registered classes into QxOrm context (introspection engine)
   qx::QxClassX::dumpAllClasses();

   // Call a custom SQL query or a stored procedure
   qx_query testStoredProc("SELECT * FROM author");
   daoError = qx::dao::call_query(testStoredProc);
   qAssert(! daoError.isValid());
   testStoredProc.dumpSqlResult();
   QVariant valFromSqlResult = testStoredProc.getSqlResultAt(0, "birthdate"); qAssert(! valFromSqlResult.isNull());
   valFromSqlResult = testStoredProc.getSqlResultAt(0, "BIRTHDATE", false); qAssert(! valFromSqlResult.isNull());
   valFromSqlResult = testStoredProc.getSqlResultAt(0, "BIRTHDATE", true); qAssert(valFromSqlResult.isNull());

   // Call a custom SQL query or a stored procedure and fetch automatically properties (with a collection of items)
   qx_query testStoredProcBis("SELECT * FROM author");
   authorX.clear();
   daoError = qx::dao::execute_query(testStoredProcBis, authorX);
   qAssert(! daoError.isValid()); qAssert(authorX.count() > 0);
   qx::dump(authorX, false);
   qx::dump(authorX, true);

   // Call a custom SQL query or a stored procedure and fetch automatically properties
   qx_query testStoredProcThird("SELECT name, category_id FROM category");
   category_ptr category_tmp = category_ptr(new category());
   daoError = qx::dao::execute_query(testStoredProcThird, category_tmp);
   qAssert(! daoError.isValid()); qAssert(category_tmp->m_id != 0);
   qx::dump(category_tmp, false);
   qx::dump(category_tmp, true);

   // Test SQL DISTINCT keyword
   QList<blog> listOfBlogDistinct;
   qx_query queryDistinct; queryDistinct.distinct().limit(10);
   daoError = qx::dao::fetch_by_query(queryDistinct, listOfBlogDistinct, NULL, QStringList() << "blog_text");
   qAssert(! daoError.isValid());
   qx::dump(listOfBlogDistinct);
   qAssert(listOfBlogDistinct.count() > 0);
   qAssert(listOfBlogDistinct.at(0).m_id == 0);
   qAssert(! listOfBlogDistinct.at(0).m_text.isEmpty());

   // Test SQL DISTINCT keyword with relationships
   listOfBlogDistinct.clear();
   qx_query queryDistinctWithRelations; queryDistinctWithRelations.distinct().limit(10);
   daoError = qx::dao::fetch_by_query_with_relation(QStringList() << "<blog_alias> { blog_text }" << "list_comment <list_comment_alias> { comment_text }" << "author_id <author_alias> { name, birthdate }", queryDistinctWithRelations, listOfBlogDistinct);
   qAssert(! daoError.isValid());
   qx::dump(listOfBlogDistinct);
   qAssert(listOfBlogDistinct.count() > 0);
   qAssert(listOfBlogDistinct.at(0).m_id == 0);
   qAssert(! listOfBlogDistinct.at(0).m_text.isEmpty());
   qAssert(listOfBlogDistinct.at(0).m_author.get() != NULL);
   qAssert(listOfBlogDistinct.at(0).m_author->m_id == "0"); // Not fetched
   qAssert(! listOfBlogDistinct.at(0).m_author->m_name.isEmpty());
   qAssert(listOfBlogDistinct.at(0).m_commentX.size() > 1);
   qAssert(listOfBlogDistinct.at(0).m_commentX.at(0)->m_id == 0); // Not fetched
   qAssert(! listOfBlogDistinct.at(0).m_commentX.at(0)->m_text.isEmpty());

   // Test SQL DISTINCT keyword with relationships forcing ID in root level
   listOfBlogDistinct.clear();
   qx_query queryDistinctWithRelationsAndId; queryDistinctWithRelationsAndId.distinct().limit(10);
   daoError = qx::dao::fetch_by_query_with_relation(QStringList() << "<blog_alias> { blog_id, blog_text }" << "list_comment <list_comment_alias> { comment_text }" << "author_id <author_alias> { name, birthdate }", queryDistinctWithRelationsAndId, listOfBlogDistinct);
   qAssert(! daoError.isValid());
   qx::dump(listOfBlogDistinct);
   qAssert(listOfBlogDistinct.count() > 0);
   qAssert(listOfBlogDistinct.at(0).m_id != 0); // Force fetched even if DISTINCT keyword is used
   qAssert(! listOfBlogDistinct.at(0).m_text.isEmpty());
   qAssert(listOfBlogDistinct.at(0).m_author.get() != NULL);
   qAssert(listOfBlogDistinct.at(0).m_author->m_id == "0"); // Not fetched
   qAssert(! listOfBlogDistinct.at(0).m_author->m_name.isEmpty());
   qAssert(listOfBlogDistinct.at(0).m_commentX.size() > 1);
   qAssert(listOfBlogDistinct.at(0).m_commentX.at(0)->m_id == 0); // Not fetched
   qAssert(! listOfBlogDistinct.at(0).m_commentX.at(0)->m_text.isEmpty());

   // Test fetch relationships (with alias) only in LEFT OUTER/INNER JOIN and WHERE clauses (so no columns in SELECT part) : use {NULL} syntax to define no relation columns in SELECT part
   list_blog lstBlogComplexRelation4;
   daoError = qx::dao::fetch_all_with_relation(QStringList() << "<blog_alias> { blog_text }" << "author_id <author_alias> { NULL }" << "list_comment <list_comment_alias> { NULL }", lstBlogComplexRelation4);
   qAssert(! daoError.isValid());
   qx::dump(lstBlogComplexRelation4);
   qAssert((lstBlogComplexRelation4.size() > 0) && (lstBlogComplexRelation4[0].get() != NULL));
   qAssert(lstBlogComplexRelation4[0]->m_author.get() == NULL); // Not fetched
   qAssert(lstBlogComplexRelation4[0]->m_commentX.size() == 0); // Not fetched

   return 0;
}
