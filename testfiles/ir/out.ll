; Generated 18/08/2013 03:03:53
; GlobalUID ccf4955f-94de-46f9-8886-5215c4f70cd9
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

