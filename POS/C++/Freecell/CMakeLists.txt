cmake_minimum_required(VERSION 3.0.0)
project(freecell VERSION 0.1.0)

set(CMAKE_CXX_STANDARD 20)

file(GLOB SOURCES src/*.cpp)

add_executable(freecell ${SOURCES})