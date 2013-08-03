; Generated 03/08/2013 21:57:45
; GlobalUID 07a6bc70-234c-4a61-a9e9-071212dbc53f
; 
; LLVM IR file for module "Simple"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR        = type i8
%SET         = type i64
%Simple.Test = type i32*

@const.string.0 = private constant [3 x i8] c"\25\66\00"
@const.string.1 = private constant [2 x i8] c"\0A\00"

declare i32 @printf(%CHAR*, ...) nounwind 

define double @Simple.FindPI(i32 %iters) nounwind {
    
    %i  = alloca i32
    %pi = alloca double
    
    store double 4.000000e+000, double* %pi
    
    store i32    1,   i32*  %i
    br    label  %1
    
; <label>:1
    %2    = load i32* %i
    %3    = icmp sgt  i32   %2,  %iters
    br    i1     %3,  label %20, label %4
    
; <label>:4
    %5    = load   double* %pi
    %6    = load   i32*    %i
    %7    = mul    i32     %6,            4
    %8    = sub    i32     %7,            1
    %9    = sitofp i32     %8             to double
    %10   = fdiv   double  4.000000e+000, %9
    %11   = fsub   double  %5,            %10
    %12   = load   i32*    %i
    %13   = mul    i32     %12,           4
    %14   = add    i32     %13,           1
    %15   = sitofp i32     %14            to double
    %16   = fdiv   double  4.000000e+000, %15
    %17   = fadd   double  %11,           %16
    store double   %17,    double*        %pi
    
    %18   = load i32* %i
    %19   = add  i32  %18,  1
    store i32    %19, i32*  %i
    br    label  %1
    
; <label>:20
    
    %21 = load double* %pi
    ret double %21
}

define i32 @main() {
    
    %1 = call double (i32)* @Simple.FindPI(i32 1000000) nounwind
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @const.string.0, i32 0, i32 0), double %1) nounwind
    
    %3 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @const.string.1, i32 0, i32 0)) nounwind
    
    ret i32 0
}

