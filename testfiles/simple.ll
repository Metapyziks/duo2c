; Generated 29/07/2013 20:41:44
; GlobalUID 07487b89-afcd-4c2b-b6af-75abe0c77873
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

@x = global i1 false
@y = global i1 false

define i32 @main() {    
    
    ; x := TRUE
    store i1 true, i1* @x
    
    ; y := FALSE
    store i1 false, i1* @y
    
    ; Out.Boolean(x)
    %1 = load   i1* @x
    %2 = select i1  %1, i8* getelementptr inbounds ([5 x i8]* @.truestr, i32 0, i32 0), i8* getelementptr inbounds ([6 x i8]* @.falsestr, i32 0, i32 0)
    %3 = call i32 (i8*, ...)* @printf(i8* %2) nounwind
    
    ; Out.Ln()
    %4 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.printnstr, i32 0, i32 0)) nounwind
    
    ; Out.Boolean(y)
    %5 = load   i1* @y
    %6 = select i1  %5, i8* getelementptr inbounds ([5 x i8]* @.truestr, i32 0, i32 0), i8* getelementptr inbounds ([6 x i8]* @.falsestr, i32 0, i32 0)
    %7 = call i32 (i8*, ...)* @printf(i8* %6) nounwind
    
    ; Out.Ln()
    %8 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.printnstr, i32 0, i32 0)) nounwind
    
    ret i32 0
}
; Module end
