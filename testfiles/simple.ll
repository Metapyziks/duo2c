; Generated 31/07/2013 22:50:22
; GlobalUID c44cd028-0dfb-45a1-aba4-b32774da91e5
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
    
    ; LOOP
    br label %1
    
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
    
    ; IF i >= 1000000 THEN
    %17 = load i32* @Simple.i
    %18 = icmp sge  i32   %17, 1000000
    br  i1     %18, label %19, label %20
    
; <label>:19
    
    ; EXIT
    br label %21
    
; <label>:20
    
    br label %1
    
; <label>:21
    
    ; Out.Real(PI)
    %22 = load double* @Simple.PI
    %23 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @const.string.0, i32 0, i32 0), double %22) nounwind
    
    ; Out.Ln()
    %24 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @const.string.1, i32 0, i32 0)) nounwind
    
    ret i32 0
}

