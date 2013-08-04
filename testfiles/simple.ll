; Generated 05/08/2013 00:31:17
; GlobalUID 0c324cac-2e93-45ef-bf27-6aa9501f054d
; 
; LLVM IR file for module "Simple"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET  = type i64
%Simple.Test = type i32*

@const.string.0 = private constant [3 x i8] c"\25\67\00"
@const.string.1 = private constant [2 x i8] c"\0A\00"

declare i32 @printf(%CHAR*, ...) nounwind 
@PI = private global double 0.000000e+000

define void @Simple.FindPI(double* %pi, i64 %$iters) nounwind {
    
    %iters = alloca i64
    store  i64      %$iters, i64* %iters
    
    %i = alloca i64
    
    store i64    1,   i64*  %i
    br    label  %1
    
; <label>:1                                       ; preds = %0, %5
    
    %2    = load i64* %iters
    %3    = load i64* %i
    %4    = icmp sgt  i64   %3,  %2
    br    i1     %4,  label %21, label %5
    
; <label>:5                                       ; preds = %1
    
    %6    = load   double* %pi
    %7    = load   i64*    %i
    %8    = mul    i64     %7,     4
    %9    = sub    i64     %8,     1
    %10   = sitofp i64     %9      to double
    %11   = fdiv   double  4.000000e+000, %10
    %12   = fsub   double  %6,     %11
    %13   = load   i64*    %i
    %14   = mul    i64     %13,    4
    %15   = add    i64     %14,    1
    %16   = sitofp i64     %15     to double
    %17   = fdiv   double  4.000000e+000, %16
    %18   = fadd   double  %12,    %17
    store double   %18,    double* %pi
    
    %19   = load i64* %i
    %20   = add  i64  %19,  1
    store i64    %20, i64*  %i
    br    label  %1
    
; <label>:21                                      ; preds = %1
    
    ret void 
}

define i32 @main() {
    
    store double 4.000000e+000, double* @PI
    
    call void (double*, i64)* @Simple.FindPI(double* @PI, i64 10000000) nounwind
    
    %1 = load double* @PI
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @const.string.0, i32 0, i32 0), double %1) nounwind
    
    %3 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @const.string.1, i32 0, i32 0)) nounwind
    
    ret i32 0
}

