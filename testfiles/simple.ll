; Generated 31/07/2013 16:01:43
; GlobalUID 9df5f441-53b2-4cfc-857a-c6185f6ccf66
; 
; LLVM IR file for module "Simple"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

@const.string.0 = private constant [3 x i8] c"\25\66\00"
@const.string.1 = private constant [2 x i8] c"\0A\00"

declare i32 @printf(i8*, ...) nounwind

%type.Simple.CHAR = type i8
%type.Simple.SET  = type i64

@Simple.i  = private global i32 0
@Simple.PI = global  double 0.000000e+000

define i32 @main() {
    
    ; PI := 4
    store double 4.0, double* @Simple.PI
    
    ; i := 1
    store i32 1, i32* @Simple.i
    
    ; WHILE i < 1000000 DO
    br label %1
    
; <label>:1
    
    %10 = load i32* @Simple.i
    %11 = icmp slt  i32 %10, 1000000
    br i1 %11, label %2, label %9
    
; <label>:2
    
    ; PI := PI - 4.000000e+000 / (i * 4 - 1) + 4.000000e+000 / (i * 4 + 1)
    %12   =      load   double* @Simple.PI
    %13   =      load   i32*    @Simple.i
    %14   =      mul    i32     %13,           4
    %15   =      sub    i32     %14,           1
    %3    =      sitofp i32     %15to          double
    %4    =      fdiv   double  4.000000e+000, %3
    %16   =      fsub   double  %12,           %4
    %17   =      load   i32*    @Simple.i
    %18   =      mul    i32     %17,           4
    %19   =      add    i32     %18,           1
    %5    =      sitofp i32     %19to          double
    %6    =      fdiv   double  4.000000e+000, %5
    %7    =      fadd   double  %16,           %6
    store double %7,    double* @Simple.PI
    
    ; i := i + 1
    %20   =   load i32* @Simple.i
    %8    =   add  i32  %20, 1
    store i32 %8,  i32* @Simple.i
    
    br label %1
    
; <label>:9
    
    ; Out.Real(PI)
    %21 = load double* @Simple.PI
    %22 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @const.string.0, i32 0, i32 0), double %21) nounwind
    
    ; Out.Ln()
    %23 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @const.string.1, i32 0, i32 0)) nounwind
    
    ret i32 0
}

