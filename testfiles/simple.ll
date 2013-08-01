; Generated 02/08/2013 00:06:13
; GlobalUID 92a4e95c-055c-4c66-8e7e-688e6cd15496
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
declare double @Simple.FindPI(i32) nounwind 


define double @FindPI(i32) nounwind {
    
    ; PI := 4
    store double 4.0, double* @Simple.PI
    
    ; i := 0
    store i32 0, i32* @Simple.i
    
    ; WHILE i < iters DO
    br label  %1
    
; <label>:1
    
    %2 = load i32* @Simple.i
    %3 = load i32* @Simple.iters
    %4 = icmp slt  i32   %2, %3
    br i1     %4,  label %5, label %21
    
; <label>:5
    
    ; PI := PI - 4.000000e+000 / (i * 4 - 1) + 4.000000e+000 / (i * 4 + 1)
    %6    = load   double* @Simple.PI
    %7    = load   i32*    @Simple.i
    %8    = mul    i32     %7,            4
    %9    = sub    i32     %8,            1
    %10   = sitofp i32     %9             to double
    %11   = fdiv   double  4.000000e+000, %10
    %12   = fsub   double  %6,            %11
    %13   = load   i32*    @Simple.i
    %14   = mul    i32     %13,           4
    %15   = add    i32     %14,           1
    %16   = sitofp i32     %15            to double
    %17   = fdiv   double  4.000000e+000, %16
    %18   = fadd   double  %12,           %17
    store double   %18,    double*        @Simple.PI
    
    ; i := i + 1
    %19   = load i32* @Simple.i
    %20   = add  i32  %19, 1
    store i32    %20, i32* @Simple.i
    
    br label  %1
    
; <label>:21
    
}

define i32 @main() {
    
    ; Out.Real(FindPI(1000000))
    %1 = call double (i32)* @Simple.FindPI(i32 1000000) nounwind
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @const.string.0, i32 0, i32 0), double %1) nounwind
    
    ; Out.Ln()
    %3 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @const.string.1, i32 0, i32 0)) nounwind
    
    ret i32 0
}

