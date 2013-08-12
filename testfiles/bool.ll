; Generated 12/08/2013 14:39:46
; GlobalUID a7ee4f7b-6994-4bee-bc8b-58db49d0accf
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
@.str4 = private constant [2 x i8] c"A\00"
@.str5 = private constant [2 x i8] c"B\00"
@.str6 = private constant [2 x i8] c"C\00"
@.str7 = private constant [2 x i8] c"D\00"
@.str8 = private constant [5 x i8] c"True\00"
@.str9 = private constant [6 x i8] c"False\00"

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
    
    ; IF True("A") & True("B") & False("C") & True("D") THEN
    %1 = call i1 ({i32, %CHAR*})* @True({i32, %CHAR*} {i32 2, %CHAR* getelementptr inbounds ([2 x %CHAR]* @.str4, i32 0, i32 0)}) nounwind
    %2 = call i1 ({i32, %CHAR*})* @True({i32, %CHAR*} {i32 2, %CHAR* getelementptr inbounds ([2 x %CHAR]* @.str5, i32 0, i32 0)}) nounwind
    %3 = and i1 %1, %2
    %4 = call i1 ({i32, %CHAR*})* @False({i32, %CHAR*} {i32 2, %CHAR* getelementptr inbounds ([2 x %CHAR]* @.str6, i32 0, i32 0)}) nounwind
    %5 = and i1 %3, %4
    %6 = call i1 ({i32, %CHAR*})* @True({i32, %CHAR*} {i32 2, %CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)}) nounwind
    %7 = and i1 %5, %6
    br i1 %7, label %8, label %12
    
; <label>:8                                       ; preds = %0
    
    ; Out.String("True")
    %9 = getelementptr inbounds [5 x %CHAR]* @.str8, i32 0, i32 0
    %10 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), %CHAR* %9) nounwind
    
    ; Out.Ln()
    %11 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str2, i32 0, i32 0)) nounwind
    
    br label %16
    
; <label>:12                                      ; preds = %0
    
    ; Out.String("False")
    %13 = getelementptr inbounds [6 x %CHAR]* @.str9, i32 0, i32 0
    %14 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), %CHAR* %13) nounwind
    
    ; Out.Ln()
    %15 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str2, i32 0, i32 0)) nounwind
    
    br label %16
    
; <label>:16                                      ; preds = %8, %12
    
    ret i32 0
}

