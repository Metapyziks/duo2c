; Generated 01/08/2013 23:28:25
; GlobalUID 9414dc41-4063-4a74-9429-15f58f3746e6
; 
; LLVM IR file for module "Simple"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR        = type i8
%SET         = type i64
%Simple.Test = type i32*

@const.string.0 = private constant [3 x i8] c"\25\66\00"
@const.string.1 = private constant [2 x i8] c"\0A\00"

declare i32 @printf(%CHAR*, ...) nounwind 
declare double @Simple.FindPI(i32) nounwind 


define i32 @main() {
    
    ; Out.Real(FindPI(1000000))
    %1 = call double (i32)*   @Simple.FindPI(i32   1000000) nounwind
    %2 = call i32    (%CHAR*, ...)*         @printf(%CHAR*  getelementptr inbounds ([3 x %CHAR]* @const.string.0, i32 0, i32 0), double %1) nounwind
    
    ; Out.Ln()
    %3 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @const.string.1, i32 0, i32 0)) nounwind
    
    ret i32 0
}

