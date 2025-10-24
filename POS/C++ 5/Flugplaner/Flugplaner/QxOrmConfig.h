#pragma once

// QxOrm configuration for CMake builds
// This header properly handles QxOrm macros that would otherwise cause compilation errors

#define QX_REGISTER_HPP_EXPORT_DLL_WATCOM_BORLAND(className, version) \
    namespace qx                                                      \
    {                                                                 \
        template <>                                                   \
        void register_class(QxClass<className> &t);                   \
    }

#define QX_REGISTER_CPP_EXPORT_DLL_WATCOM_BORLAND(className)

// Only define if not already defined to avoid redef warnings
#ifndef QX_USE_NAMESPACES
#define QX_USE_NAMESPACES
#endif

// Disable RTTI since we don't use dynamic_cast
#define QX_NO_RTTI
