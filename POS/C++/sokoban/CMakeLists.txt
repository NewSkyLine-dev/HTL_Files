cmake_minimum_required(VERSION 3.0.0)
project(sokoban VERSION 0.1.0)

set(CMAKE_CXX_STANDARD 20)

include_directories(include)

find_package(SFML 2.5 COMPONENTS system window graphics REQUIRED)

file(GLOB SOURCES "src/*.cpp")

add_executable(${PROJECT_NAME} ${SOURCES})

target_link_libraries(${PROJECT_NAME} sfml-system sfml-window sfml-graphics)