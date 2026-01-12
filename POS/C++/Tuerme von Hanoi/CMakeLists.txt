cmake_minimum_required(VERSION 3.0.0)
project(hanoi VERSION 0.1.0)

set(CMAKE_CXX_STANDARD 20)

add_executable(hanoi src/main.cpp src/tower.cpp)