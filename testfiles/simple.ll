; Generated 29/07/2013 23:30:26
; GlobalUID 9ee201fd-4909-4ba6-b1b8-bb104ead8c67
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

    ; y := 7
    store i32 7, i32* @y

    ; x > yOut.Integer(6)Out.Ln()Out.Integer(12)Out.Ln()
    %1 = load     i32* @x
    %2 = load     i32* @y
    %3 = icmp sgt i32  %1, %2
    br i1 %3, label %4, label %8

; <label>:4 ; preds = %0

    ; Out.Integer(6)
    %5 = sext i8 6 to i64
    %6 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.printistr, i32 0, i32 0), i64 %5) nounwind

    ; Out.Ln()
    %7 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.printnstr, i32 0, i32 0)) nounwind

    br label %12

; <label>:8 ; preds = %0

    ; Out.Integer(12)
    %9  = sext i8 12 to i64
    %10 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.printistr, i32 0, i32 0), i64 %9) nounwind

    ; Out.Ln()
    %11 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.printnstr, i32 0, i32 0)) nounwind

    br label %12

; <label>:12 ; preds = %4, %8

    ret i32 0
}
; Module end
