; Generated 29/07/2013 19:43:14
; GlobalUID 9573c0cb-367b-4616-9723-f4c54bd7ff6b
;
; LLVM IR file for module "Simple"
;
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-f80:128:128-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32-S32"

@.printistr = private unnamed_addr constant [3 x i8] c"%i\00", align 1
@.printfstr = private unnamed_addr constant [3 x i8] c"%f\00", align 1
@.printnstr = private unnamed_addr constant [2 x i8] c"\0A\00", align 1

declare i32 @printf(i8*, ...) nounwind

; Begin type aliases
  
  ; CHAR = BYTE
  %_typeCHAR = type i8
  
  ; SET = LONGINT
  %_typeSET  = type i64
  
; End type aliases

@x = global i32   0
@y = global i16   0
@z = global i32   0
@n = global float 0.0

define i32 @main() {    
    
    ; x := 1
    store i32 1, i32* @x
    
    ; y := 1 - 2
    %1    = sext i8  1    to i16
    %2    = sext i8  2    to i16
    %3    = sub  i16 %1,  %2
    store i16    %3, i16* @y
    
    ; z := 3
    store i32 3, i32* @z
    
    ; Out.Integer(x + y * (z + 1))
    %4  = load i32* @x
    %5  = load i16* @y
    %6  = sext i16  %5  to i32
    %7  = load i32* @z
    %8  = sext i8   1   to i32
    %9  = add  i32  %7, %8
    %10 = mul  i32  %6, %9
    %11 = add  i32  %4, %10
    %12 = sext i32  %11 to i64
    %13 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.printistr, i32 0, i32 0), i64 %12) nounwind
    
    ; Out.Ln()
    %14 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.printnstr, i32 0, i32 0)) nounwind
    
    ; n := z / 2
    %15   = load   i32*  @z
    %16   = sitofp i32   %15    to float
    %17   = fdiv   float %16,   2.0
    store float    %17,  float* @n
    
    ; Out.Real(n)
    %18 = load  float* @n
    %19 = fpext float  %18 to double
    %20 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.printfstr, i32 0, i32 0), double %19) nounwind
    
    ; Out.Ln()
    %21 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.printnstr, i32 0, i32 0)) nounwind
    
    ret i32 0
}
; Module end
