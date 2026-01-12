cmake_minimum_required(VERSION 3.0.0)
project(roman_numbers VERSION 0.1.0)

set(CMAKE_CXX_STANDARD 20)

file(GLOB SOURCE_FILES "src/*.cpp")

add_executable(roman_numbers ${SOURCE_FILES})
