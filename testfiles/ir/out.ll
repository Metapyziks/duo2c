; Generated 17/08/2013 00:45:50
; GlobalUID 7bd82aa8-41ec-4051-8967-4efdb75f57bc
; 
; LLVM IR file for module "Out"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET = type i64

@.str0 = private constant [16 x i8] c"Hello from Out!\00"
@.str1 = private constant [3 x i8] c"%s\00"
@.str2 = private constant [2 x i8] c"\0A\00"

declare i32 @printf(%CHAR*, ...) nounwind 
declare noalias i8* @GC_malloc(i32) 

declare void @Out.Ln() nounwind 
declare void @Out.String({i32, %CHAR*}) nounwind 
declare void @Out.Integer(i64) nounwind 
declare void @Out.Real(double) nounwind 
declare void @Out.Boolean(i1) nounwind 

@Out._hasInit = private global i1 zeroinitializer

define i32 @Out._init() nounwind {
    
    %1 = load i1* @Out._hasInit
    br i1 %1, label %6, label %2
    
; <label>:2                                       ; preds = %0
    store i1 1, i1* @Out._hasInit
    
    ; Out.String("Hello from Out!")
    %3 = getelementptr inbounds [16 x %CHAR]* @.str0, i32 0, i32 0
    %4 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), %CHAR* %3) nounwind
    
    ; Out.Ln()
    %5 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str2, i32 0, i32 0)) nounwind
    
    br label %6
    
; <label>:6                                       ; preds = %0, %2
    ret i32 0
    
; <label>:7                                       ; preds = %0
    ret i32 1
}

