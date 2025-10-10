#pragma once

#include <QtCore/qglobal.h>

#ifndef BUILD_STATIC
# if defined(MYLIB_LIB)
#  define MYLIB_EXPORT Q_DECL_EXPORT
# else
#  define MYLIB_EXPORT Q_DECL_IMPORT
# endif
#else
# define MYLIB_EXPORT
#endif
