; Generated 17/08/2013 21:14:11
; GlobalUID e9330da0-051f-40e0-88b8-cd9b43bc78f3
; 
; LLVM IR file for module "GLTest"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET = type i64

@.str0 = private constant [2 x i8] c"H\00"
@.str1 = private constant [2 x i8] c"e\00"
@.str2 = private constant [2 x i8] c"l\00"
@.str3 = private constant [2 x i8] c"o\00"
@.str4 = private constant [2 x i8] c"W\00"
@.str5 = private constant [2 x i8] c"r\00"
@.str6 = private constant [2 x i8] c"d\00"
@.str7 = private constant [2 x i8] c"!\00"
@.str8 = private constant [13 x i8] c"Hello World!\00"

declare i32 @printf(%CHAR*, ...) nounwind 
declare noalias i8* @GC_malloc(i32) 

declare void glClear(i32) 
declare void glEnable(i32) 
declare void glMatrixMode(i32) 
declare void glLoadIdentity() 
declare void glOrtho(double, double, double, double, double, double) 
declare void glScalef(float, float, float) 
declare void glRotatef(float, float, float, float) 
declare void glTranslatef(float, float, float) 
declare void glColor3f(float, float, float) 
declare i32 @GL._init() 

declare void gluLookAt(double, double, double, double, double, double, double, double, double) 
declare void glutStrokeCharacter(i8*, %CHAR) 
declare void glutSwapBuffers() 
declare void @GLUT.Init() nounwind 
declare void glutInitDisplayMode(i32) 
declare void glutInitWindowSize(i32, i32) 
declare void glutInitWindowPosition(i32, i32) 
declare void @GLUT.CreateWindow({i32, %CHAR*}) nounwind 
declare void glutDisplayFunc(void ()*) 
declare void glutMainLoop() 
declare i32 @GLUT._init() 

define void @DisplayHandler() nounwind {
    
    ; GL.Clear(16640)
    call void (i32)* glClear(i32 16640)
    
    ; GL.Enable(2929)
    call void (i32)* glEnable(i32 2929)
    
    ; GL.MatrixMode(5889)
    call void (i32)* glMatrixMode(i32 5889)
    
    ; GL.LoadIdentity()
    call void ()* glLoadIdentity()
    
    ; GL.Ortho(2.000000e+000, 2.000000e+000, 2.000000e+000, 2.000000e+000, 2.000000e+000, 5.000000e+002)
    call void (double, double, double, double, double, double)* glOrtho(double -2.000000e+000, double 2.000000e+000, double -2.000000e+000, double 2.000000e+000, double -2.000000e+000, double 5.000000e+002)
    
    ; GL.MatrixMode(5888)
    call void (i32)* glMatrixMode(i32 5888)
    
    ; GL.LoadIdentity()
    call void ()* glLoadIdentity()
    
    ; GLUT.LookAt(2, 2, 2, 0.000000e+000, 0.000000e+000, 0.000000e+000, 0.000000e+000, 1.000000e+000, 0.000000e+000)
    call void (double, double, double, double, double, double, double, double, double)* gluLookAt(double 2.000000e+000, double 2.000000e+000, double 2.000000e+000, double 0.000000e+000, double 0.000000e+000, double 0.000000e+000, double 0.000000e+000, double 1.000000e+000, double 0.000000e+000)
    
    ; GL.Scalef(5.000000e-003, 5.000000e-003, 5.000000e-003)
    call void (float, float, float)* glScalef(float 5.000000e-003, float 5.000000e-003, float 5.000000e-003)
    
    ; GL.Rotatef(20, 0, 1, 0)
    call void (float, float, float, float)* glRotatef(float 2.000000e+001, float 0.000000e+000, float 1.000000e+000, float 0.000000e+000)
    
    ; GL.Rotatef(30, 0, 0, 1)
    call void (float, float, float, float)* glRotatef(float 3.000000e+001, float 0.000000e+000, float 0.000000e+000, float 1.000000e+000)
    
    ; GL.Rotatef(5, 1, 0, 0)
    call void (float, float, float, float)* glRotatef(float 5.000000e+000, float 1.000000e+000, float 0.000000e+000, float 0.000000e+000)
    
    ; GL.Translatef(300, 0, 0)
    call void (float, float, float)* glTranslatef(float -3.000000e+002, float 0.000000e+000, float 0.000000e+000)
    
    ; GL.Color3f(1, 1, 1)
    call void (float, float, float)* glColor3f(float 1.000000e+000, float 1.000000e+000, float 1.000000e+000)
    
    ; GLUT.StrokeCharacter(NIL, "H")
    %1 = getelementptr inbounds [2 x %CHAR]* @.str0, i32 0, i32 0
    %2 = load %CHAR* %1
    call void (i8*, %CHAR)* glutStrokeCharacter(i8* null, %CHAR %2)
    
    ; GLUT.StrokeCharacter(NIL, "e")
    %3 = getelementptr inbounds [2 x %CHAR]* @.str1, i32 0, i32 0
    %4 = load %CHAR* %3
    call void (i8*, %CHAR)* glutStrokeCharacter(i8* null, %CHAR %4)
    
    ; GLUT.StrokeCharacter(NIL, "l")
    %5 = getelementptr inbounds [2 x %CHAR]* @.str2, i32 0, i32 0
    %6 = load %CHAR* %5
    call void (i8*, %CHAR)* glutStrokeCharacter(i8* null, %CHAR %6)
    
    ; GLUT.StrokeCharacter(NIL, "l")
    %7 = getelementptr inbounds [2 x %CHAR]* @.str2, i32 0, i32 0
    %8 = load %CHAR* %7
    call void (i8*, %CHAR)* glutStrokeCharacter(i8* null, %CHAR %8)
    
    ; GLUT.StrokeCharacter(NIL, "o")
    %9 = getelementptr inbounds [2 x %CHAR]* @.str3, i32 0, i32 0
    %10 = load %CHAR* %9
    call void (i8*, %CHAR)* glutStrokeCharacter(i8* null, %CHAR %10)
    
    ; GLUT.StrokeCharacter(NIL, "W")
    %11 = getelementptr inbounds [2 x %CHAR]* @.str4, i32 0, i32 0
    %12 = load %CHAR* %11
    call void (i8*, %CHAR)* glutStrokeCharacter(i8* null, %CHAR %12)
    
    ; GLUT.StrokeCharacter(NIL, "o")
    %13 = getelementptr inbounds [2 x %CHAR]* @.str3, i32 0, i32 0
    %14 = load %CHAR* %13
    call void (i8*, %CHAR)* glutStrokeCharacter(i8* null, %CHAR %14)
    
    ; GLUT.StrokeCharacter(NIL, "r")
    %15 = getelementptr inbounds [2 x %CHAR]* @.str5, i32 0, i32 0
    %16 = load %CHAR* %15
    call void (i8*, %CHAR)* glutStrokeCharacter(i8* null, %CHAR %16)
    
    ; GLUT.StrokeCharacter(NIL, "l")
    %17 = getelementptr inbounds [2 x %CHAR]* @.str2, i32 0, i32 0
    %18 = load %CHAR* %17
    call void (i8*, %CHAR)* glutStrokeCharacter(i8* null, %CHAR %18)
    
    ; GLUT.StrokeCharacter(NIL, "d")
    %19 = getelementptr inbounds [2 x %CHAR]* @.str6, i32 0, i32 0
    %20 = load %CHAR* %19
    call void (i8*, %CHAR)* glutStrokeCharacter(i8* null, %CHAR %20)
    
    ; GLUT.StrokeCharacter(NIL, "!")
    %21 = getelementptr inbounds [2 x %CHAR]* @.str7, i32 0, i32 0
    %22 = load %CHAR* %21
    call void (i8*, %CHAR)* glutStrokeCharacter(i8* null, %CHAR %22)
    
    ; GLUT.SwapBuffers()
    call void ()* glutSwapBuffers()
    
    ret void 
}


@GLTest._hasInit = private global i1 zeroinitializer

define i32 @GLTest._init() nounwind {
    
    %1 = load i1* @GLTest._hasInit
    br i1 %1, label %9, label %2
    
; <label>:2                                       ; preds = %0
    store i1 1, i1* @GLTest._hasInit
    
    %3 = call i32 ()* @GL._init()
    %4 = icmp eq i32 %3, 0
    br i1 %4, label %5, label %10
    
; <label>:5                                       ; preds = %2
    %6 = call i32 ()* @GLUT._init()
    %7 = icmp eq i32 %6, 0
    br i1 %7, label %8, label %10
    
; <label>:8                                       ; preds = %5
    
    ; GLUT.Init()
    call void ()* @GLUT.Init() nounwind
    
    ; GLUT.InitDisplayMode(18)
    call void (i32)* glutInitDisplayMode(i32 18)
    
    ; GLUT.InitWindowSize(500, 500)
    call void (i32, i32)* glutInitWindowSize(i32 500, i32 500)
    
    ; GLUT.InitWindowPosition(300, 200)
    call void (i32, i32)* glutInitWindowPosition(i32 300, i32 200)
    
    ; GLUT.CreateWindow("Hello World!")
    call void ({i32, %CHAR*})* @GLUT.CreateWindow({i32, %CHAR*} {i32 13, %CHAR* getelementptr inbounds ([13 x %CHAR]* @.str8, i32 0, i32 0)}) nounwind
    
    ; GLUT.DisplayFunc(DisplayHandler)
    call void (void ()*)* glutDisplayFunc(void ()* @DisplayHandler)
    
    ; GLUT.MainLoop()
    call void ()* glutMainLoop()
    
    br label %9
    
; <label>:9                                       ; preds = %0, %8
    ret i32 0
    
; <label>:10                                      ; preds = %2, %5
    ret i32 1
}

define i32 @main() nounwind {
    
    %1 = call i32 ()* @GLTest._init()
    ret i32 %1
}

