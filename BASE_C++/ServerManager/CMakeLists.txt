cmake_minimum_required(VERSION 2.8)

message ( "test" )
project ( ServerManager CXX)
file ( GLOB_RECURSE SRC *.?pp )
add_library(ServerManager SHARED ${SRC})
target_link_libraries(ServerManager Utility)
