; Generated 06/08/2013 23:30:19
; GlobalUID 9cb7bd4d-2d52-4ca1-ac55-82d6f3f25c29
; 
; LLVM IR file for module "Simple"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET  = type i64
%Simple.TestRec = type {i32}

@TestRec = global [3 x i8*] [i8* getelementptr inbounds ([8 x %CHAR]* @.str0, i32 0, i32 0), i8* null, i8* bitcast ( void (i32)* @TestRec.Set to i8*) ]

@.str0 = private constant [8  x i8] c"\54\65\73\74\52\65\63\00"
@.str1 = private constant [3  x i8] c"\25\69\00"
@.str2 = private constant [2  x i8] c"\0A\00"
@.str3 = private constant [13 x i8] c"\48\65\6C\6C\6F\20\77\6F\72\6C\64\21\00"
@.str4 = private constant [3  x i8] c"\25\73\00"

declare i32 @printf(%CHAR*, ...) nounwind 

define void @TestRec.Set(i32 %$v) nounwind {
    
    %v    = alloca i32
    store i32      %$v, i32* %v
    
    %this = alloca %Simple.TestRec
    
    ; Out.Integer(v)
    %1 = load i32* %v
    %2 = sext i32  %1 to i64
    %3 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), i64 %2) nounwind
    
    ; Out.Ln()
    %4 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str2, i32 0, i32 0)) nounwind
    
    ret void 
}

define i32 @main() {
    
    ; Out.String(Hello world!)
    %1 = getelementptr inbounds [13 x %CHAR]* @.str3, i32 0, i32 0
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %1) nounwind
    
    ; Out.Ln()
    %3 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str2, i32 0, i32 0)) nounwind
    
    ret i32 0
}

