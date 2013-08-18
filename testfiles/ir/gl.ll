; Generated 18/08/2013 22:41:23
; GlobalUID 0f39ec5d-2987-47d8-9f81-ed2982b4ead8
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

declare x86_stdcallcc void @glClear(i32) 
declare x86_stdcallcc void @glEnable(i32) 
declare x86_stdcallcc void @glBegin(i32) 
declare x86_stdcallcc void @glEnd() 
declare x86_stdcallcc void @glMatrixMode(i32) 
declare x86_stdcallcc void @glLoadIdentity() 
declare x86_stdcallcc void @glOrtho(double, double, double, double, double, double) 
declare x86_stdcallcc void @glScalef(float, float, float) 
declare x86_stdcallcc void @glRotatef(float, float, float, float) 
declare x86_stdcallcc void @glTranslatef(float, float, float) 
declare x86_stdcallcc void @glColor3f(float, float, float) 
declare x86_stdcallcc void @glVertex2f(float, float) 
declare x86_stdcallcc void @glVertex3f(float, float, float) 

@GL._hasInit = private global i1 zeroinitializer

define i32 @GL._init() nounwind {
    
    ret i32 0
}

