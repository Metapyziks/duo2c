; Generated 29/07/2013 21:35:44
; GlobalUID decf6762-7cf9-4ddd-a9cd-2617fdbce72a
;
; LLVM IR file for module "Simple"
;
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-f80:128:128-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32-S32"

@.printistr = private constant [3 x i8] c"%i\00", align 1
@.printfstr = private constant [3 x i8] c"%f\00", align 1
@.printnstr = private constant [2 x i8] c"\0A\00", align 1
@.truestr = private constant [5 x i8] c"TRUE\00", align 1
@.falsestr = private constant [6 x i8] c"FALSE\00", align 1

declare i32 @printf(i8*, ...) nounwind

; Begin type aliases
  
  ; CHAR = BYTE
  %_typeCHAR = type i8
  
  ; SET = LONGINT
  %_typeSET  = type i64
  
; End type aliases

@x = global i32 0
@y = global i32 0

define i32 @main() {    
    
    ; x := 5
    store i32 5, i32* @x
    
    ; y := 6
    store i32 6, i32* @y
    
    ; Out.Boolean(x < y)
    %1 = load     i32* @x
    %2 = load     i32* @y
    %3 = icmp slt i32  %1, %2
    %4 = select   i1   %3, i8* getelementptr inbounds ([5 x i8]* @.truestr, i32 0, i32 0), i8* getelementptr inbounds ([6 x i8]* @.falsestr, i32 0, i32 0)
    %5 = call i32 (i8*, ...)* @printf(i8* %4) nounwind
    
    ; Out.Ln()
    %6 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.printnstr, i32 0, i32 0)) nounwind
    
    ; Out.Boolean(x > y)
    %7  = load     i32* @x
    %8  = load     i32* @y
    %9  = icmp sgt i32  %7, %8
    %10 = select   i1   %9, i8* getelementptr inbounds ([5 x i8]* @.truestr, i32 0, i32 0), i8* getelementptr inbounds ([6 x i8]* @.falsestr, i32 0, i32 0)
    %11 = call i32 (i8*, ...)* @printf(i8* %10) nounwind
    
    ; Out.Ln()
    %12 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.printnstr, i32 0, i32 0)) nounwind
    
    ret i32 0
}
; Module end
