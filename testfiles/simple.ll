; Generated 31/07/2013 22:18:39
; GlobalUID 36f97463-e0ac-46d6-8611-346a4593c0ea
; 
; LLVM IR file for module "Simple"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

@const.string.0 = private constant [3 x i8] c"\25\66\00"
@const.string.1 = private constant [2 x i8] c"\0A\00"

declare i32 @printf(i8*, ...) nounwind

%type.Simple.CHAR =type i8
%type.Simple.SET  =type i64

@Simple.i  = private global i32 0
@Simple.PI = global  double 0.000000e+000

define i32 @main() {
    
    ; PI := 4
    store double 4.0, double* @Simple.PI
    
    ; i := 1
    store i32 1, i32* @Simple.i
    
    ; REPEAT UNTIL i >= 1000000
    br  label  %1
    
; <label>:1
    
    ; PI := PI - 4.000000e+000 / (i * 4 - 1) + 4.000000e+000 / (i * 4 + 1)
    %2    = load   double* @Simple.PI
    %3    = load   i32*    @Simple.i
    %4    = mul    i32     %3,            4
    %5    = sub    i32     %4,            1
    %6    = sitofp i32     %5             to double
    %7    = fdiv   double  4.000000e+000, %6
    %8    = fsub   double  %2,            %7
    %9    = load   i32*    @Simple.i
    %10   = mul    i32     %9,            4
    %11   = add    i32     %10,           1
    %12   = sitofp i32     %11            to double
    %13   = fdiv   double  4.000000e+000, %12
    %14   = fadd   double  %8,            %13
    store double   %14,    double*        @Simple.PI
    
    ; i := i + 1
    %15   = load i32* @Simple.i
    %16   = add  i32  %15, 1
    store i32    %16, i32* @Simple.i
    
    br  label  %17
    
; <label>:17
    %18 = load i32* @Simple.i
    %19 = icmp sge  i32   %18, 1000000
    br  i1     %19, label %20, label %1
    
; <label>:20
    
    ; Out.Real(PI)
    %21 = load double* @Simple.PI
    %22 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @const.string.0, i32 0, i32 0), double %21) nounwind
    
    ; Out.Ln()
    %23 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @const.string.1, i32 0, i32 0)) nounwind
    
    ret i32 0
}

