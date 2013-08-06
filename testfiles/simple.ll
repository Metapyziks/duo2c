; Generated 06/08/2013 18:35:18
; GlobalUID 6e98c193-9ffb-4db4-b9fd-218904afef77
; 
; LLVM IR file for module "Simple"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET  = type i64

@.str0 = private constant [3  x i8] c"\25\73\00"
@.str1 = private constant [2  x i8] c"\0A\00"
@.str2 = private constant [13 x i8] c"\48\65\6C\6C\6F\20\77\6F\72\6C\64\21\00"
@.str3 = private constant [18 x i8] c"\48\65\6C\6C\6F\20\74\6F\20\79\6F\75\20\74\6F\6F\21\00"

declare i32 @printf(%CHAR*, ...) nounwind 

define void @WriteLine({i32, %CHAR*} %$str) nounwind {
    
    %str  = alloca {i32, %CHAR*}
    store {i32, %CHAR*} %$str, {i32, %CHAR*}* %str
    
    
    ; Out.String(str)
    %1 = getelementptr inbounds {i32, %CHAR*}* %str, i32 0, i32 1
    %2 = load %CHAR** %1
    %3 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str0, i32 0, i32 0), %CHAR* %2) nounwind
    
    ; Out.Ln()
    %4 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str1, i32 0, i32 0)) nounwind
    
    ret void 
}

define i32 @main() {
    
    ; Out.String(Hello world!)
    %1 = getelementptr inbounds [13 x %CHAR]* @.str2, i32 0, i32 0
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str0, i32 0, i32 0), %CHAR* %1) nounwind
    
    ; Out.Ln()
    %3 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str1, i32 0, i32 0)) nounwind
    
    ; WriteLine(Hello to you too!)
    call void ({i32, %CHAR*})* @WriteLine({i32, %CHAR*} {i32 18, %CHAR* getelementptr inbounds ([18 x %CHAR]* @.str3, i32 0, i32 0)}) nounwind
    
    ret i32 0
}

