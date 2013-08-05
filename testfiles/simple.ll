; Generated 05/08/2013 21:26:09
; GlobalUID de25b8a7-5716-45f7-aad3-9e9d458dfb4c
; 
; LLVM IR file for module "Simple"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET  = type i64

@const.string.0 = private constant [2 x i8] c"\23\00"
@const.string.1 = private constant [2 x i8] c"\2D\00"
@const.string.2 = private constant [3 x i8] c"\25\73\00"
@const.string.3 = private constant [2 x i8] c"\0A\00"

declare i32 @printf(%CHAR*, ...) nounwind 

define void @Test() nounwind {
    
    
    %i    = alloca i32
    %n    = alloca i32
    %stra = alloca {i32, %CHAR*}
    %strb = alloca {i32, %CHAR*}
    
    ; stra := #
    store {i32, %CHAR*} {i32 2, i8* getelementptr inbounds ([2 x i8]* @const.string.0, i32 0, i32 0)}, {i32, %CHAR*}* %stra
    
    ; strb := -
    store {i32, %CHAR*} {i32 2, i8* getelementptr inbounds ([2 x i8]* @const.string.1, i32 0, i32 0)}, {i32, %CHAR*}* %strb
    
    ; FOR i := 1 TO 9 DO
    store i32    1,   i32*  %i
    br    label  %1
    
; <label>:1                                       ; preds = %0, %29
    
    %2    = load i32* %i
    %3    = icmp sgt  i32   %2,  9
    br    i1     %3,  label %33, label %4
    
; <label>:4                                       ; preds = %1
    
    ; FOR n := 0 TO 16 DO
    store i32    0,   i32*  %n
    br    label  %5
    
; <label>:5                                       ; preds = %4, %26
    
    %6    = load i32* %n
    %7    = icmp sgt  i32   %6,  16
    br    i1     %7,  label %29, label %8
    
; <label>:8                                       ; preds = %5
    
    ; IF n < i * i - 10 * i + 25 THEN
    %9  = load i32* %n
    %10 = load i32* %i
    %11 = load i32* %i
    %12 = mul  i32  %10,  %11
    %13 = load i32* %i
    %14 = mul  i32  10,   %13
    %15 = sub  i32  %12,  %14
    %16 = add  i32  %15,  25
    %17 = icmp slt  i32   %9,  %16
    br  i1     %17, label %18, label %22
    
; <label>:18                                      ; preds = %8
    
    ; Out.String(stra)
    %19 = getelementptr inbounds {i32, %CHAR*}* %stra, i32 0, i32 1
    %20 = load %CHAR** %19
    %21 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @const.string.2, i32 0, i32 0), %CHAR* %20) nounwind
    
    br  label  %26
    
; <label>:22                                      ; preds = %8
    
    ; Out.String(strb)
    %23 = getelementptr inbounds {i32, %CHAR*}* %strb, i32 0, i32 1
    %24 = load %CHAR** %23
    %25 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @const.string.2, i32 0, i32 0), %CHAR* %24) nounwind
    
    br  label  %26
    
; <label>:26                                      ; preds = %18, %22
    
    %27   = load i32* %n
    %28   = add  i32  %27,  1
    store i32    %28, i32*  %n
    br    label  %5
    
; <label>:29                                      ; preds = %5
    
    ; Out.Ln()
    %30 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @const.string.3, i32 0, i32 0)) nounwind
    
    %31   = load i32* %i
    %32   = add  i32  %31,  1
    store i32    %32, i32*  %i
    br    label  %1
    
; <label>:33                                      ; preds = %1
    
    ret void 
}

define i32 @main() {
    
    ; Test()
    call void ()* @Test() nounwind
    
    ret i32 0
}

