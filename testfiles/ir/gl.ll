; Generated 18/08/2013 01:27:41
; GlobalUID 14091c1c-9c12-446d-ae1a-9693474e0407
; 
; LLVM IR file for module "GL"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET = type i64

declare i32 @printf(%CHAR*, ...) nounwind 
declare noalias i8* @GC_malloc(i32) 

declare void @glClear(i32) 
declare void @glEnable(i32) 
declare void @glMatrixMode(i32) 
declare void @glLoadIdentity() 
declare void @glOrtho(double, double, double, double, double, double) 
declare void @glScalef(float, float, float) 
declare void @glRotatef(float, float, float, float) 
declare void @glTranslatef(float, float, float) 
declare void @glColor3f(float, float, float) 

@GL._hasInit = private global i1 zeroinitializer

define i32 @GL._init() nounwind {
    
    ret i32 0
}

