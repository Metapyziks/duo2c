; Generated 30/07/2013 00:32:44
; GlobalUID 8c6ee504-afb1-4eab-a9cd-6f4cb9246ffb
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

@i = global i32 0

define i32 @main() {

    ; i := 1
    store i32 1, i32* @i

    ; i <= 10Out.Integer(i)Out.Ln()i := i + 1
    br label %1

; <label>:1

    %2 = load     i32* @i
    %3 = sext     i8   10  to i32
    %4 = icmp sle i32  %2, %3
    br i1 %4, label %5, label %13

; <label>:5

    ; Out.Integer(i)
    %6 = load i32* @i
    %7 = sext i32  %6 to i64
    %8 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.printistr, i32 0, i32 0), i64 %7) nounwind

    ; Out.Ln()
    %9 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.printnstr, i32 0, i32 0)) nounwind

    ; i := i + 1
    %10   = load i32* @i
    %11   = sext i8   1    to i32
    %12   = add  i32  %10, %11
    store i32    %12, i32* @i

    br label %1

; <label>:13

    ret i32 0
}
; Module end
