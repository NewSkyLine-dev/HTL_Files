cmake_minimum_required(VERSION 3.21)

project(Flugplaner 
    VERSION 1.0.0
    DESCRIPTION "Flight Planning Application"
    LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_INCLUDE_CURRENT_DIR ON)

# Qt Configuration
set(CMAKE_AUTOMOC ON)
set(CMAKE_AUTORCC ON)
set(CMAKE_AUTOUIC ON)

# Find Qt6 - set the correct path
if(NOT Qt6_DIR)
    if(WIN32)
        # Windows - Qt 6.9.2 with MSVC 2022
        set(Qt6_DIR "C:/Qt/6.9.2/msvc2022_64/lib/cmake/Qt6" CACHE PATH "Qt6 CMake directory")
    endif()
endif()

# Set CMAKE_PREFIX_PATH for Qt6 discovery
message(STATUS "Qt6_DIR: ${Qt6_DIR}")
set(CMAKE_PREFIX_PATH "${Qt6_DIR};${CMAKE_PREFIX_PATH}")

message(STATUS "CMAKE_PREFIX_PATH: ${CMAKE_PREFIX_PATH}")

find_package(Qt6 COMPONENTS Core Gui Widgets Sql REQUIRED)

# QxOrm Library
set(QXORM_INCLUDE_DIR "${CMAKE_CURRENT_SOURCE_DIR}/Libraries/QxOrm/include")
set(QXORM_INL_DIR "${CMAKE_CURRENT_SOURCE_DIR}/Libraries/QxOrm/inl")

# Add QxOrm library if pre-built, otherwise you may need to build it
# For now, we'll assume QxOrm is available as a compiled library
# Adjust the path based on your actual QxOrm build output
set(QXORM_LIB_DIR "${CMAKE_CURRENT_SOURCE_DIR}/Libraries/QxOrm/lib")

# Main Application Target
set(FLUGPLANER_SOURCES
    Flugplaner/main.cpp
    Flugplaner/Flugplaner.cpp
    Flugplaner/MapWidget.cpp
    Flugplaner/Databaser.cpp
    Flugplaner/QxOrmRegistration.cpp
    Flugplaner/Models/Airport.cpp
    Flugplaner/Models/Airline.cpp
    Flugplaner/Models/Route.cpp
    Flugplaner/Models/Alliance.cpp
    Flugplaner/Utilities/FlyGraph.cpp
)

set(FLUGPLANER_HEADERS
    Flugplaner/Flugplaner.h
    Flugplaner/MapWidget.h
    Flugplaner/Databaser.h
    Flugplaner/Models/Airport.h
    Flugplaner/Models/Airline.h
    Flugplaner/Models/Route.h
    Flugplaner/Models/Alliance.h
    Flugplaner/Utilities/FlyGraph.h
)

set(FLUGPLANER_RESOURCES
    Flugplaner/Flugplaner.qrc
)

set(FLUGPLANER_UI
    Flugplaner/Flugplaner.ui
)

add_executable(Flugplaner
    ${FLUGPLANER_SOURCES}
    ${FLUGPLANER_HEADERS}
    ${FLUGPLANER_RESOURCES}
    ${FLUGPLANER_UI}
)

# Skip automoc on model files (they have Q_OBJECT but don't inherit from QObject)
# Model headers have Q_OBJECT and need MOC processing

target_link_libraries(Flugplaner PRIVATE
    Qt6::Core
    Qt6::Gui
    Qt6::Widgets
    Qt6::Sql
)

target_include_directories(Flugplaner PRIVATE
    ${CMAKE_CURRENT_SOURCE_DIR}/Flugplaner
    ${CMAKE_CURRENT_SOURCE_DIR}/Flugplaner/Models
    ${CMAKE_CURRENT_SOURCE_DIR}/Flugplaner/Utilities
    ${QXORM_INCLUDE_DIR}
    ${QXORM_INL_DIR}
)

# Define QxOrm-related configuration
target_compile_definitions(Flugplaner PRIVATE
)

# Force include QxOrmConfig.h to define problematic macros before QxOrm.h is included
if(MSVC)
    target_compile_options(Flugplaner PRIVATE "/FI${CMAKE_CURRENT_SOURCE_DIR}/Flugplaner/QxOrmConfig.h")
else()
    target_compile_options(Flugplaner PRIVATE "-include" "${CMAKE_CURRENT_SOURCE_DIR}/Flugplaner/QxOrmConfig.h")
endif()

# Link QxOrm library - use debug version for Debug config
target_link_directories(Flugplaner PRIVATE ${QXORM_LIB_DIR})
target_link_libraries(Flugplaner PRIVATE $<IF:$<CONFIG:Debug>,QxOrmd,QxOrm>)

# Set output directory
set_target_properties(Flugplaner PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin"
)

# Copy resources and Qt6 DLLs to build directory
if(MSVC)
    # Copy resources
    add_custom_command(TARGET Flugplaner POST_BUILD
        COMMAND ${CMAKE_COMMAND} -E copy_directory
            "${CMAKE_CURRENT_SOURCE_DIR}/Flugplaner/resources"
            "$<TARGET_FILE_DIR:Flugplaner>/resources"
        COMMENT "Copying resources to build directory"
    )
    
    # Copy Qt6 core DLLs
    add_custom_command(TARGET Flugplaner POST_BUILD
        COMMAND ${CMAKE_COMMAND} -E copy
            "C:/Qt/6.9.2/msvc2022_64/bin/Qt6Cored.dll"
            "$<TARGET_FILE_DIR:Flugplaner>/Qt6Cored.dll"
        COMMAND ${CMAKE_COMMAND} -E copy
            "C:/Qt/6.9.2/msvc2022_64/bin/Qt6Guid.dll"
            "$<TARGET_FILE_DIR:Flugplaner>/Qt6Guid.dll"
        COMMAND ${CMAKE_COMMAND} -E copy
            "C:/Qt/6.9.2/msvc2022_64/bin/Qt6Widgetsd.dll"
            "$<TARGET_FILE_DIR:Flugplaner>/Qt6Widgetsd.dll"
        COMMAND ${CMAKE_COMMAND} -E copy
            "C:/Qt/6.9.2/msvc2022_64/bin/Qt6Sqld.dll"
            "$<TARGET_FILE_DIR:Flugplaner>/Qt6Sqld.dll"
        COMMENT "Copying Qt6 DLLs to build directory"
    )
    
    # Copy Qt6 plugins
    add_custom_command(TARGET Flugplaner POST_BUILD
        COMMAND ${CMAKE_COMMAND} -E copy_directory
            "C:/Qt/6.9.2/msvc2022_64/plugins/platforms"
            "$<TARGET_FILE_DIR:Flugplaner>/platforms"
        COMMENT "Copying Qt6 platform plugins"
    )
    
    add_custom_command(TARGET Flugplaner POST_BUILD
        COMMAND ${CMAKE_COMMAND} -E copy_directory
            "C:/Qt/6.9.2/msvc2022_64/plugins/sqldrivers"
            "$<TARGET_FILE_DIR:Flugplaner>/sqldrivers"
        COMMENT "Copying Qt6 SQL drivers"
    )
    
    # Copy QxOrm DLL
    add_custom_command(TARGET Flugplaner POST_BUILD
        COMMAND ${CMAKE_COMMAND} -E copy
            "${CMAKE_CURRENT_SOURCE_DIR}/Libraries/QxOrm/lib/QxOrmd.dll"
            "$<TARGET_FILE_DIR:Flugplaner>/QxOrmd.dll"
        COMMENT "Copying QxOrm DLL"
    )

    # Copy resource files into build
    add_custom_command(TARGET Flugplaner POST_BUILD
        COMMAND ${CMAKE_COMMAND} -E copy_directory
            "${CMAKE_CURRENT_SOURCE_DIR}/Flugplaner/resources"
            "$<TARGET_FILE_DIR:Flugplaner>/resources"
        COMMENT "Copying resource files into build"
    )
endif()

