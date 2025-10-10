#[==[
 * File: CMakeLists.txt
 * Project: Taschenrechner
 * File Created: Tuesday, 16th September 2025 8:41:15 am
 * Author: Fabian Oppermann (fabian.oppermann@gmx.net)
 * -----
 * Last Modified: Tuesday, 16th September 2025 8:41:15 am
 * Modified By: Fabian Oppermann (fabian.oppermann@gmx.net)
#]==]

cmake_minimum_required(VERSION 3.25)
project(Taschenrechner VERSION 1.0.0 LANGUAGES CXX)

# C++ Standard
set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)

# Qt6 path - adjust this to your Qt installation path
set(CMAKE_PREFIX_PATH "C:/Qt/6.9.2/msvc2022_64/lib/cmake" ${CMAKE_PREFIX_PATH})

# Qt6 configuration
find_package(Qt6 REQUIRED COMPONENTS Core Widgets Gui)
set(CMAKE_AUTOMOC ON)
set(CMAKE_AUTOUIC ON)
set(CMAKE_AUTORCC ON)
set(CMAKE_INCLUDE_CURRENT_DIR ON)

# Define BUILD_STATIC for static library builds
add_definitions(-DBUILD_STATIC)

# Visual Studio 2022 specific settings
if(MSVC)
    # Enable big object files for exprtk
    add_compile_options(/bigobj)
    # Enable multi-processor compilation
    add_compile_options(/MP)
endif()

# Collect source files (exclude build directories)
set(TR_SOURCES
    "Taschenrechner/main.cpp"
    "Taschenrechner/Taschenrechner.cpp"
    "Taschenrechner/Taschenrechner.h"
    "Taschenrechner/Taschenrechner.ui"
    "Taschenrechner/Taschenrechner.qrc"
)

set(CALC_SOURCES
    "Calculator/Calculator.cpp"
    "Calculator/Calculator.h"
    "Calculator/calculator_global.h"
    "Calculator/exprtk.hpp"
)

# Calculator static library
add_library(Calculator STATIC ${CALC_SOURCES})
target_include_directories(Calculator PUBLIC 
    ${CMAKE_CURRENT_SOURCE_DIR}/Calculator
)
target_link_libraries(Calculator PUBLIC Qt6::Core Qt6::Widgets Qt6::Gui)

# Main executable
add_executable(Taschenrechner ${TR_SOURCES})
target_include_directories(Taschenrechner PRIVATE 
    ${CMAKE_CURRENT_SOURCE_DIR}/Taschenrechner
)
target_link_libraries(Taschenrechner PRIVATE 
    Calculator
    Qt6::Core 
    Qt6::Widgets 
    Qt6::Gui
)

# Visual Studio 2022 specific target properties
if(MSVC)
    # Additional properties for exprtk (big object files are already handled globally above)
    set_target_properties(Calculator PROPERTIES
        VS_DEBUGGER_WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
    )
    set_target_properties(Taschenrechner PROPERTIES
        VS_DEBUGGER_WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
        WIN32_EXECUTABLE TRUE
    )
endif()

# Deploy Qt libraries automatically
if(WIN32)
    # Find Qt6 deployment tool
    find_program(QT_DEPLOY_EXECUTABLE windeployqt HINTS ${Qt6_DIR}/../../../bin)
    
    if(QT_DEPLOY_EXECUTABLE)
        # Custom target to deploy Qt libraries using windeployqt
        add_custom_target(deploy_qt
            COMMAND ${QT_DEPLOY_EXECUTABLE} --debug --compiler-runtime $<TARGET_FILE:Taschenrechner>
            DEPENDS Taschenrechner
            COMMENT "Deploying Qt libraries with windeployqt"
        )
        
        # Auto-deploy after build
        add_custom_command(TARGET Taschenrechner POST_BUILD
            COMMAND ${QT_DEPLOY_EXECUTABLE} --debug --compiler-runtime $<TARGET_FILE:Taschenrechner>
            COMMENT "Auto-deploying Qt libraries"
        )
    else()
        message(WARNING "windeployqt not found. Manual DLL copying will be used.")
    endif()
    
    # Manual platform plugin deployment (essential for Qt GUI)
    set(QT_PLATFORMS_DIR "${Qt6_DIR}/../../../plugins/platforms")
    set(QT_STYLES_DIR "${Qt6_DIR}/../../../plugins/styles")
    set(QT_IMAGEFORMATS_DIR "${Qt6_DIR}/../../../plugins/imageformats")
    
    # Create platforms directory in output
    add_custom_command(TARGET Taschenrechner POST_BUILD
        COMMAND ${CMAKE_COMMAND} -E make_directory
        $<TARGET_FILE_DIR:Taschenrechner>/platforms
        COMMENT "Creating platforms directory"
    )
    
    # Copy Windows platform plugin
    if(EXISTS "${QT_PLATFORMS_DIR}/qwindowsd.dll")
        add_custom_command(TARGET Taschenrechner POST_BUILD
            COMMAND ${CMAKE_COMMAND} -E copy_if_different
            "${QT_PLATFORMS_DIR}/qwindowsd.dll"
            $<TARGET_FILE_DIR:Taschenrechner>/platforms/
            COMMENT "Copying Windows platform plugin (Debug)"
        )
    endif()
    
    if(EXISTS "${QT_PLATFORMS_DIR}/qwindows.dll")
        add_custom_command(TARGET Taschenrechner POST_BUILD
            COMMAND ${CMAKE_COMMAND} -E copy_if_different
            "${QT_PLATFORMS_DIR}/qwindows.dll"
            $<TARGET_FILE_DIR:Taschenrechner>/platforms/
            COMMENT "Copying Windows platform plugin (Release)"
        )
    endif()
    
    # Fallback: Copy Qt DLLs manually for development
    if(CMAKE_BUILD_TYPE STREQUAL "Debug" OR CMAKE_CONFIGURATION_TYPES)
        # Debug libraries
        set(QT_DLLS
            "${Qt6_DIR}/../../../bin/Qt6Cored.dll"
            "${Qt6_DIR}/../../../bin/Qt6Widgetsd.dll" 
            "${Qt6_DIR}/../../../bin/Qt6Guid.dll"
        )
    else()
        # Release libraries
        set(QT_DLLS
            "${Qt6_DIR}/../../../bin/Qt6Core.dll"
            "${Qt6_DIR}/../../../bin/Qt6Widgets.dll"
            "${Qt6_DIR}/../../../bin/Qt6Gui.dll"
        )
    endif()
    
    # Copy Qt DLLs to output directory (fallback if windeployqt fails)
    foreach(QT_DLL ${QT_DLLS})
        if(EXISTS ${QT_DLL})
            add_custom_command(TARGET Taschenrechner POST_BUILD
                COMMAND ${CMAKE_COMMAND} -E copy_if_different
                ${QT_DLL}
                $<TARGET_FILE_DIR:Taschenrechner>
                COMMENT "Copying ${QT_DLL}"
            )
        endif()
    endforeach()
endif()