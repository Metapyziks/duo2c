; Generated 18/08/2013 22:27:16
; GlobalUID ecb00356-6af1-4d60-96cf-50f1acae7cf1
; 
; LLVM IR file for module "Out"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET = type i64

declare i32 @printf(%CHAR*, ...) nounwind 
declare noalias i8* @GC_malloc(i32) 

declare void @Out.Ln() nounwind 
declare void @Out.String({i32, %CHAR*}) nounwind 
declare void @Out.Integer(i64) nounwind 
declare void @Out.Real(double) nounwind 
declare void @Out.Boolean(i1) nounwind 

@Out._hasInit = private global i1 zeroinitializer

define i32 @Out._init() nounwind {
    
    ret i32 0
}

