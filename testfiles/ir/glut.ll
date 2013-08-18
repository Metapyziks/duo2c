; Generated 18/08/2013 01:27:41
; GlobalUID 9be3cc41-47f5-4953-beb5-639e9969757c
; 
; LLVM IR file for module "GLUT"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET = type i64

declare i32 @printf(%CHAR*, ...) nounwind 
declare noalias i8* @GC_malloc(i32) 

%GLUT.ExitType = type void (i32)

declare void @exit(i32) 
declare void @gluLookAt(double, double, double, double, double, double, double, double, double) 
declare void @glutStrokeCharacter(i8*, %CHAR) 
declare void @glutSwapBuffers() 
declare void @__glutInitWithExit(i32, %CHAR**, %GLUT.ExitType*) 
define void @GLUT.Init() nounwind {
    
    ; InitInternal(0, NIL, exit)
    call void (i32, %CHAR**, %GLUT.ExitType*)* @__glutInitWithExit(i32 0, %CHAR** null, %GLUT.ExitType* @exit)
    
    ret void 
}

declare void @glutInitDisplayMode(i32) 
declare void @glutInitWindowSize(i32, i32) 
declare void @glutInitWindowPosition(i32, i32) 
declare void @__glutCreateWindowWithExit(%CHAR*, %GLUT.ExitType*) 
define void @GLUT.CreateWindow({i32, %CHAR*} %$title) nounwind {
    
    %title = alloca {i32, %CHAR*}
    store {i32, %CHAR*} %$title, {i32, %CHAR*}* %title
    
    ; CreateWindowInternal(title, exit)
    %1 = getelementptr inbounds {i32, %CHAR*}* %title, i32 0, i32 1
    %2 = load %CHAR** %1
    call void (%CHAR*, %GLUT.ExitType*)* @__glutCreateWindowWithExit(%CHAR* %2, %GLUT.ExitType* @exit)
    
    ret void 
}

declare void @glutDisplayFunc(void ()*) 
declare void @glutMainLoop() 

@GLUT._hasInit = private global i1 zeroinitializer

define i32 @GLUT._init() nounwind {
    
    ret i32 0
}

