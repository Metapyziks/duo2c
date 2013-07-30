; Generated 30/07/2013 15:32:09
; GlobalUID 136932ac-44fa-46a0-a94a-c03604f76d7b
;
; LLVM IR file for module "Simple"
;
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

@.str0 = private constant [3 x i8] c"\25\69\00"
@.str1 = private constant [3 x i8] c"\25\66\00"
@.str2 = private constant [2 x i8] c"\0A\00"
@.str3 = private constant [5 x i8] c"\54\52\55\45\00"
@.str4 = private constant [6 x i8] c"\46\41\4C\53\45\00"

declare i32 @printf(i8*, ...) nounwind

; Begin type aliases

  ; CHAR = BYTE
  %_typeCHAR = type i8

  ; SET = LONGINT
  %_typeSET  = type i64

; End type aliases

@i         = global i32    0
@Simple.PI = global double 0.0

define i32 @main() {

    ; PI := 4
    store double 4.0, double* @Simple.PI

    ; i := 1
    %1    = sext i8  1    to i32
    store i32    %1, i32* @i

    ; WHILE i < 1000000 DO
    br label %2

; <label>:2

    %3 = load     i32* @i
    %4 = icmp slt i32  %3, 1000000
    br i1 %4, label %5, label %26

; <label>:5

    ; PI := PI - 4.000000e+000 / (i * 4 - 1) + 4.000000e+000 / (i * 4 + 1)
    %6    = load   double* @Simple.PI
    %7    = load   i32*    @i
    %8    = sext   i8      4              to i32
    %9    = mul    i32     %7,            %8
    %10   = sext   i8      1              to i32
    %11   = sub    i32     %9,            %10
    %12   = sitofp i32     %11            to double
    %13   = fdiv   double  4.000000e+000, %12
    %14   = fsub   double  %6,            %13
    %15   = load   i32*    @i
    %16   = sext   i8      4              to i32
    %17   = mul    i32     %15,           %16
    %18   = sext   i8      1              to i32
    %19   = add    i32     %17,           %18
    %20   = sitofp i32     %19            to double
    %21   = fdiv   double  4.000000e+000, %20
    %22   = fadd   double  %14,           %21
    store double   %22,    double*        @Simple.PI

    ; i := i + 1
    %23   = load i32* @i
    %24   = sext i8   1    to i32
    %25   = add  i32  %23, %24
    store i32    %25, i32* @i

    br label %2

; <label>:26

    ; Out.Real(PI)
    %27 = load double* @Simple.PI
    %28 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.printfstr, i32 0, i32 0), double %27) nounwind

    ; Out.Ln()
    %29 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.printnstr, i32 0, i32 0)) nounwind

    ret i32 0
}
; Module end
