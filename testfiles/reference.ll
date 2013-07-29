; ModuleID = 'testfiles/reference.c'
target datalayout = "e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-f80:128:128-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32-S32"
target triple = "i686-w64-mingw32"

@j = global i32 5, align 4
@.str = private unnamed_addr constant [4 x i8] c"%i\0A\00", align 1

define i32 @main() nounwind {
  %1 = alloca i32, align 4
  %i = alloca i32, align 4
  store i32 0, i32* %1
  store i32 16, i32* %i, align 4
  %2 = load i32* %i, align 4
  %3 = icmp sgt i32 %2, 0
  br i1 %3, label %4, label %7

; <label>:4                                       ; preds = %0
  %5 = load i32* %i, align 4
  %6 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([4 x i8]* @.str, i32 0, i32 0), i32 %5) nounwind
  br label %7

; <label>:7                                       ; preds = %4, %0
  ret i32 0
}

declare i32 @printf(i8*, ...) nounwind
