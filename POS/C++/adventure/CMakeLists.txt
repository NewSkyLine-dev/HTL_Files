cmake_minimum_required(VERSION 3.0.0)
project(adventure VERSION 0.1.0)

set(CMAKE_CXX_STANDARD 20)

include_directories(include)
file(GLOB SOURCES "src/*.cpp")

add_executable(${PROJECT_NAME} ${SOURCES})
