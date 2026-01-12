#pragma once

#include <QtCore/qglobal.h>

#ifndef BUILD_STATIC
# if defined(FORM_FACTORY_LIB)
#  define FORM_FACTORY_EXPORT Q_DECL_EXPORT
# else
#  define FORM_FACTORY_EXPORT Q_DECL_IMPORT
# endif
#else
# define FORM_FACTORY_EXPORT
#endif
