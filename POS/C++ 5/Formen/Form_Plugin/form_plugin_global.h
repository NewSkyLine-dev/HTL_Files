#pragma once

#include <QtCore/qglobal.h>

#ifndef BUILD_STATIC
# if defined(FORM_PLUGIN_LIB)
#  define FORM_PLUGIN_EXPORT Q_DECL_EXPORT
# else
#  define FORM_PLUGIN_EXPORT Q_DECL_IMPORT
# endif
#else
# define FORM_PLUGIN_EXPORT
#endif
