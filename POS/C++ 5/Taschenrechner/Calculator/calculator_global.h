#pragma once

#include <QtCore/qglobal.h>

#ifndef BUILD_STATIC
# if defined(CALCULATOR_LIB)
#  define CALCULATOR_EXPORT Q_DECL_EXPORT
# else
#  define CALCULATOR_EXPORT Q_DECL_IMPORT
# endif
#else
# define CALCULATOR_EXPORT
#endif
