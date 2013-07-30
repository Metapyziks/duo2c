; ModuleID = 'testfiles/reference.c'
target datalayout = "e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-f80:128:128-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32-S32"
target triple = "i686-w64-mingw32"

@i = common global i32 0, align 4
@pi = common global double 0.000000e+00, align 8
@.str = private unnamed_addr constant [3 x i8] c"%f\00", align 1
@.str1 = private unnamed_addr constant [2 x i8] c"\0A\00", align 1

define i32 @main() nounwind {
  %1 = alloca i32, align 4
  store i32 0, i32* %1
  store i32 1, i32* @i, align 4
  store double 4.000000e+00, double* @pi, align 8
  br label %2

; <label>:2                                       ; preds = %5, %0
  %3 = load i32* @i, align 4
  %4 = icmp slt i32 %3, 1000000
  br i1 %4, label %5, label %21

; <label>:5                                       ; preds = %2
  %6 = load double* @pi, align 8
  %7 = load i32* @i, align 4
  %8 = mul nsw i32 %7, 4
  %9 = sub nsw i32 %8, 1
  %10 = sitofp i32 %9 to double
  %11 = fdiv double 4.000000e+00, %10
  %12 = fsub double %6, %11
  %13 = load i32* @i, align 4
  %14 = mul nsw i32 %13, 4
  %15 = add nsw i32 %14, 1
  %16 = sitofp i32 %15 to double
  %17 = fdiv double 4.000000e+00, %16
  %18 = fadd double %12, %17
  store double %18, double* @pi, align 8
  %19 = load i32* @i, align 4
  %20 = add nsw i32 %19, 1
  store i32 %20, i32* @i, align 4
  br label %2

; <label>:21                                      ; preds = %2
  %22 = load double* @pi, align 8
  %23 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str, i32 0, i32 0), double %22) nounwind
  %24 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.str1, i32 0, i32 0)) nounwind
  ret i32 0
}

declare i32 @printf(i8*, ...) nounwind
