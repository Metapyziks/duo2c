; Generated 04/08/2013 22:21:57
; GlobalUID 0eb55048-3f4d-4eed-b0f2-104d67d282be
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
@PI = private global double 0.000000e+000

define void @Simple.FindPI(double* %pi, i32 %$iters) nounwind {
    
    %iters = alloca i32
    store  i32      %$iters, i32* %iters
    
    %i = alloca i32
    
    ; FOR i := 1 TO iters DO
    store i32    1,   i32*  %i
    br    label  %1
    
; <label>:1
    %2    = load i32* %iters
    %3    = load i32* %i
    %4    = icmp sgt  i32   %3,  %2
    br    i1     %4,  label %21, label %5
    
; <label>:5
    ; pi := pi - 4.000000e+000 / (i * 4 - 1) + 4.000000e+000 / (i * 4 + 1)
    %6    = load   double* %pi
    %7    = load   i32*    %i
    %8    = mul    i32     %7,            4
    %9    = sub    i32     %8,            1
    %10   = sitofp i32     %9             to double
    %11   = fdiv   double  4.000000e+000, %10
    %12   = fsub   double  %6,            %11
    %13   = load   i32*    %i
    %14   = mul    i32     %13,           4
    %15   = add    i32     %14,           1
    %16   = sitofp i32     %15            to double
    %17   = fdiv   double  4.000000e+000, %16
    %18   = fadd   double  %12,           %17
    store double   %18,    double*        %pi
    
    %19   = load i32* %i
    %20   = add  i32  %19,  1
    store i32    %20, i32*  %i
    br    label  %1
    
; <label>:21
    
    ret void 
}

define i32 @main() {
    
    ; PI := 8
    store double 8.000000e+000, double* @PI
    
    ; FindPI(PI, 1000000)
    call void (double*, i32)* @Simple.FindPI(double* @PI, i32 1000000) nounwind
    
    ; FindPI(PI, 1000000)
    call void (double*, i32)* @Simple.FindPI(double* @PI, i32 1000000) nounwind
    
    ; Out.Real(PI / 2)
    %1 = load double*                                         @PI
    %2 = fdiv double                                          %1,      2.000000e+000
    %3 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @const.string.0, i32 0, i32 0), double %2) nounwind
    
    ; Out.Ln()
    %4 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @const.string.1, i32 0, i32 0)) nounwind
    
    ret i32 0
}

