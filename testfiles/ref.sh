#! /bin/sh

clang testfiles/reference.c -emit-llvm -S -o testfiles/reference.ll
