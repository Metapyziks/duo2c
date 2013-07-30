; Generated 30/07/2013 22:49:48
; GlobalUID cb1b1efe-d65e-45e4-b9ac-ca9fd2df01fc
; 
; LLVM IR file for module "Simple"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

@const.string.0 = private constant [3 x i8] c"\25\66\00"
@const.string.1 = private constant [2 x i8] c"\0A\00"

declare i32 @printf(i8*, ...) nounwind

; CHAR = BYTE
%type.Simple.CHAR = type i8

; SET = LONGINT
%type.Simple.SET  = type i64

@i                = global i32    0
@Simple.PI        = global double 0.0

define i32 @main() {
    
    ; PI := 4
    store double 4.0, double* @Simple.PI
    
    ; i := 1
    store i32 1, i32* @i
    
    ; WHILE i < 1000000 DO
    br label %1
    
; <label>:1
    
    %2 = load     i32* @i
    %3 = icmp slt i32  %2, 1000000
    br i1 %3, label %4, label %20
    
; <label>:4
    
    ; PI := PI - 4.000000e+000 / (i * 4 - 1) + 4.000000e+000 / (i * 4 + 1)
    %5    = load   double* @Simple.PI
    %6    = load   i32*    @i
    %7    = mul    i32     %6,            4
    %8    = sub    i32     %7,            1
    %9    = sitofp i32     %8             to double
    %10   = fdiv   double  4.000000e+000, %9
    %11   = fsub   double  %5,            %10
    %12   = load   i32*    @i
    %13   = mul    i32     %12,           4
    %14   = add    i32     %13,           1
    %15   = sitofp i32     %14            to double
    %16   = fdiv   double  4.000000e+000, %15
    %17   = fadd   double  %11,           %16
    store double   %17,    double*        @Simple.PI
    
    ; i := i + 1
    %18   = load i32* @i
    %19   = add  i32  %18, 1
    store i32    %19, i32* @i
    
    br label %1
    
; <label>:20
    
    ; Out.Real(PI)
    %21 = load double* @Simple.PI
    %22 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @const.string.0, i32 0, i32 0), double %21) nounwind
    
    ; Out.Ln()
    %23 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @const.string.1, i32 0, i32 0)) nounwind
    
    ret i32 0
}
; Module end

