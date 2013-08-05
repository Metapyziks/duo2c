; ModuleID = 'testfiles/reference.c'
target datalayout = "e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-f80:128:128-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32-S32"
target triple = "i686-w64-mingw32"

@.str = private unnamed_addr constant [20 x i8] c"Hello there world!\0A\00", align 1
@.str1 = private unnamed_addr constant [3 x i8] c"%s\00", align 1

define i32 @main() nounwind {
  %1 = alloca i32, align 4
  %str = alloca i8*, align 4
  store i32 0, i32* %1
  store i8* getelementptr inbounds ([20 x i8]* @.str, i32 0, i32 0), i8** %str, align 4
  %2 = load i8** %str, align 4
  %3 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str1, i32 0, i32 0), i8* %2) nounwind
  ret i32 0
}

declare i32 @printf(i8*, ...) nounwind
