cmake_minimum_required(VERSION 3.0.0)
project(upn_calc VERSION 0.1.0)

set(CMAKE_CXX_STANDARD 20)

include_directories(include)

file(GLOB SRC_FILES_EXE src/*.cpp)

add_executable(upn_calc ${SRC_FILES_EXE})