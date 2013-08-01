; Generated 01/08/2013 23:06:07
; GlobalUID 8d051a13-4725-40bc-9c4a-f531d658d180
; 
; LLVM IR file for module "Simple"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR        = type i8
%SET         = type i64
%Simple.Test = type i32*

@const.string.0 = private constant [3 x i8] c"\25\69\00"
@const.string.1 = private constant [2 x i8] c"\0A\00"
@const.string.2 = private constant [3 x i8] c"\25\66\00"
@const.string.3 = private constant [5 x i8] c"\54\52\55\45\00"
@const.string.4 = private constant [6 x i8] c"\46\41\4C\53\45\00"

declare i32 @printf(%CHAR*, ...) nounwind 
declare double @Simple.FindPI(i32) nounwind 


define i32 @main() {
    
    ; Out.Integer(193)
    %1 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @const.string.0, i32 0, i32 0), i64 193) nounwind
    
    ; Out.Ln()
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @const.string.1, i32 0, i32 0)) nounwind
    
    ; Out.Real(1.190000e+001)
    %3 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @const.string.2, i32 0, i32 0), double 1.190000e+001) nounwind
    
    ; Out.Ln()
    %4 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @const.string.1, i32 0, i32 0)) nounwind
    
    ; Out.Boolean(TRUE)
    %5 = select i1  true,    %CHAR* getelementptr inbounds ([5     x %CHAR]* @const.string.3, i32 0, i32 0), %CHAR* getelementptr inbounds ([6 x %CHAR]* @const.string.4, i32 0, i32 0)
    %6 = call   i32 (%CHAR*, ...    )*            @printf  (%CHAR* %5)    nounwind
    
    ; Out.Ln()
    %7 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @const.string.1, i32 0, i32 0)) nounwind
    
    ret i32 0
}

