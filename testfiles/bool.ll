; Generated 12/08/2013 17:22:44
; GlobalUID dac29bbc-24c2-497d-a7e4-0dfa70f34432
; 
; LLVM IR file for module "Bool"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET = type i64

@.str0 = private constant [2 x i8] c"+\00"
@.str1 = private constant [3 x i8] c"%s\00"
@.str2 = private constant [2 x i8] c"\0A\00"
@.str3 = private constant [2 x i8] c"-\00"
@.str4 = private constant [6 x i8] c"Start\00"
@.str5 = private constant [2 x i8] c"A\00"
@.str6 = private constant [2 x i8] c"B\00"
@.str7 = private constant [2 x i8] c"C\00"
@.str8 = private constant [2 x i8] c"D\00"
@.str9 = private constant [5 x i8] c"True\00"
@.str10 = private constant [6 x i8] c"False\00"

declare i32 @printf(%CHAR*, ...) nounwind 
declare noalias i8* @GC_malloc(i32) 
define i1 @True({i32, %CHAR*} %$msg) nounwind {
    
    %msg = alloca {i32, %CHAR*}
    store {i32, %CHAR*} %$msg, {i32, %CHAR*}* %msg
    
    ; Out.String("+")
    %1 = getelementptr inbounds [2 x %CHAR]* @.str0, i32 0, i32 0
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), %CHAR* %1) nounwind
    
    ; Out.String(msg)
    %3 = getelementptr inbounds {i32, %CHAR*}* %msg, i32 0, i32 1
    %4 = load %CHAR** %3
    %5 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), %CHAR* %4) nounwind
    
    ; Out.Ln()
    %6 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str2, i32 0, i32 0)) nounwind
    
    ; RETURN TRUE
    ret i1 true
}

define i1 @False({i32, %CHAR*} %$msg) nounwind {
    
    %msg = alloca {i32, %CHAR*}
    store {i32, %CHAR*} %$msg, {i32, %CHAR*}* %msg
    
    ; Out.String("-")
    %1 = getelementptr inbounds [2 x %CHAR]* @.str3, i32 0, i32 0
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), %CHAR* %1) nounwind
    
    ; Out.String(msg)
    %3 = getelementptr inbounds {i32, %CHAR*}* %msg, i32 0, i32 1
    %4 = load %CHAR** %3
    %5 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), %CHAR* %4) nounwind
    
    ; Out.Ln()
    %6 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str2, i32 0, i32 0)) nounwind
    
    ; RETURN FALSE
    ret i1 false
}

define i32 @main() nounwind {
    
    ; Out.String("Start")
    %1 = getelementptr inbounds [6 x %CHAR]* @.str4, i32 0, i32 0
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), %CHAR* %1) nounwind
    
    ; Out.Ln()
    %3 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str2, i32 0, i32 0)) nounwind
    
    ; IF False("A") OR True("B") OR True("C") OR False("D") THEN
    %4 = call i1 ({i32, %CHAR*})* @False({i32, %CHAR*} {i32 2, %CHAR* getelementptr inbounds ([2 x %CHAR]* @.str5, i32 0, i32 0)}) nounwind
    br i1 %4, label %7, label %5
    
; <label>:5                                       ; preds = %0
    %6 = call i1 ({i32, %CHAR*})* @True({i32, %CHAR*} {i32 2, %CHAR* getelementptr inbounds ([2 x %CHAR]* @.str6, i32 0, i32 0)}) nounwind
    br label %7
    
; <label>:7                                       ; preds = %0, %5
    %8 = phi i1 [1, %0], [%6, %5]
    br i1 %8, label %11, label %9
    
; <label>:9                                       ; preds = %7
    %10 = call i1 ({i32, %CHAR*})* @True({i32, %CHAR*} {i32 2, %CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)}) nounwind
    br label %11
    
; <label>:11                                      ; preds = %7, %9
    %12 = phi i1 [1, %7], [%10, %9]
    br i1 %12, label %15, label %13
    
; <label>:13                                      ; preds = %11
    %14 = call i1 ({i32, %CHAR*})* @False({i32, %CHAR*} {i32 2, %CHAR* getelementptr inbounds ([2 x %CHAR]* @.str8, i32 0, i32 0)}) nounwind
    br label %15
    
; <label>:15                                      ; preds = %11, %13
    %16 = phi i1 [1, %11], [%14, %13]
    br i1 %16, label %17, label %21
    
; <label>:17                                      ; preds = %15
    
    ; Out.String("True")
    %18 = getelementptr inbounds [5 x %CHAR]* @.str9, i32 0, i32 0
    %19 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), %CHAR* %18) nounwind
    
    ; Out.Ln()
    %20 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str2, i32 0, i32 0)) nounwind
    
    br label %25
    
; <label>:21                                      ; preds = %15
    
    ; Out.String("False")
    %22 = getelementptr inbounds [6 x %CHAR]* @.str10, i32 0, i32 0
    %23 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), %CHAR* %22) nounwind
    
    ; Out.Ln()
    %24 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str2, i32 0, i32 0)) nounwind
    
    br label %25
    
; <label>:25                                      ; preds = %17, %21
    
    ret i32 0
}

