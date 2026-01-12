cmake_minimum_required(VERSION 3.25)
project(koffer)

set(CMAKE_CXX_STANDARD 20)

target_compile_definitions(koffer PRIVATE -DUNICODE)

file(GLOB SOURCES
    "src/*.cpp"
)
add_executable(koffer ${SOURCES})