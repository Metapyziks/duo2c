; Generated 18/08/2013 22:27:14
; GlobalUID c1b1c650-3338-47a3-b864-12978fc0cb96
; 
; LLVM IR file for module "Pong"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET = type i64

@.str0 = private constant [9 x i8] c"OBERpONg\00"

declare i32 @printf(%CHAR*, ...) nounwind 
declare noalias i8* @GC_malloc(i32) 

declare x86_stdcallcc void @glClear(i32) 
declare x86_stdcallcc void @glEnable(i32) 
declare x86_stdcallcc void @glMatrixMode(i32) 
declare x86_stdcallcc void @glLoadIdentity() 
declare x86_stdcallcc void @glOrtho(double, double, double, double, double, double) 
declare x86_stdcallcc void @glScalef(float, float, float) 
declare x86_stdcallcc void @glRotatef(float, float, float, float) 
declare x86_stdcallcc void @glTranslatef(float, float, float) 
declare x86_stdcallcc void @glColor3f(float, float, float) 
declare i32 @GL._init() 

declare x86_stdcallcc void @gluLookAt(double, double, double, double, double, double, double, double, double) 
declare dllimport x86_stdcallcc void @glutStrokeCharacter(i8*, %CHAR) 
declare dllimport x86_stdcallcc void @glutSwapBuffers() 
declare void @GLUT.Init() nounwind 
declare dllimport x86_stdcallcc void @glutInitDisplayMode(i32) 
declare dllimport x86_stdcallcc void @glutInitWindowSize(i32, i32) 
declare dllimport x86_stdcallcc void @glutInitWindowPosition(i32, i32) 
declare void @GLUT.CreateWindow({i32, %CHAR*}) nounwind 
declare dllimport x86_stdcallcc void @glutDisplayFunc(void ()*) 
declare dllimport x86_stdcallcc void @glutIdleFunc(void ()*) 
declare dllimport x86_stdcallcc void @glutMainLoop() 
declare dllimport x86_stdcallcc void @glutPostRedisplay() 
declare i32 @GLUT._init() 

declare void @Out.Ln() nounwind 
declare void @Out.String({i32, %CHAR*}) nounwind 
declare void @Out.Integer(i64) nounwind 
declare void @Out.Real(double) nounwind 
declare void @Out.Boolean(i1) nounwind 
declare i32 @Out._init() 

define void @IdleHandler() nounwind {
    
    ; RETURN
    ret void
    
}

define void @DisplayHandler() nounwind {
    
    ; GL.Clear(16640)
    call x86_stdcallcc void (i32)* @glClear(i32 16640)
    
    ; GLUT.SwapBuffers()
    call x86_stdcallcc void ()* @glutSwapBuffers()
    
    ret void 
}


@Pong._hasInit = private global i1 zeroinitializer

define i32 @Pong._init() nounwind {
    
    %1 = load i1* @Pong._hasInit
    br i1 %1, label %12, label %2
    
; <label>:2                                       ; preds = %0
    store i1 1, i1* @Pong._hasInit
    
    %3 = call i32 ()* @GL._init()
    %4 = icmp eq i32 %3, 0
    br i1 %4, label %5, label %13
    
; <label>:5                                       ; preds = %2
    %6 = call i32 ()* @GLUT._init()
    %7 = icmp eq i32 %6, 0
    br i1 %7, label %8, label %13
    
; <label>:8                                       ; preds = %5
    %9 = call i32 ()* @Out._init()
    %10 = icmp eq i32 %9, 0
    br i1 %10, label %11, label %13
    
; <label>:11                                      ; preds = %8
    
    ; GLUT.Init()
    call void ()* @GLUT.Init() nounwind
    
    ; GLUT.InitDisplayMode(18)
    call x86_stdcallcc void (i32)* @glutInitDisplayMode(i32 18)
    
    ; GLUT.InitWindowSize(800, 600)
    call x86_stdcallcc void (i32, i32)* @glutInitWindowSize(i32 800, i32 600)
    
    ; GLUT.InitWindowPosition(300, 200)
    call x86_stdcallcc void (i32, i32)* @glutInitWindowPosition(i32 300, i32 200)
    
    ; GLUT.CreateWindow("OBERpONg")
    call void ({i32, %CHAR*})* @GLUT.CreateWindow({i32, %CHAR*} {i32 9, %CHAR* getelementptr inbounds ([9 x %CHAR]* @.str0, i32 0, i32 0)}) nounwind
    
    ; GLUT.IdleFunc(IdleHandler)
    call x86_stdcallcc void (void ()*)* @glutIdleFunc(void ()* @IdleHandler)
    
    ; GLUT.DisplayFunc(DisplayHandler)
    call x86_stdcallcc void (void ()*)* @glutDisplayFunc(void ()* @DisplayHandler)
    
    ; GLUT.MainLoop()
    call x86_stdcallcc void ()* @glutMainLoop()
    
    br label %12
    
; <label>:12                                      ; preds = %0, %11
    ret i32 0
    
; <label>:13                                      ; preds = %2, %5, %8
    ret i32 1
}

define i32 @main() nounwind {
    
    %1 = call i32 ()* @Pong._init()
    ret i32 %1
}

