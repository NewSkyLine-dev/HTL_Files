cmake_minimum_required(VERSION 3.24)
project(random VERSION 0.1.0)

set(CMAKE_CXX_STANDARD 20)

add_executable(random src/main.cpp src/dice.cpp)
