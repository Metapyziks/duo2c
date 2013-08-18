; Generated 18/08/2013 02:23:10
; GlobalUID fd3f9c33-8ce6-489f-a936-910be1596616
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
@.str8 = private constant [5 x i8] c"Init\00"
@.str9 = private constant [3 x i8] c"%s\00"
@.str10 = private constant [2 x i8] c"\0A\00"
@.str11 = private constant [16 x i8] c"InitDisplayMode\00"
@.str12 = private constant [15 x i8] c"InitWindowSize\00"
@.str13 = private constant [19 x i8] c"InitWindowPosition\00"
@.str14 = private constant [13 x i8] c"CreateWindow\00"
@.str15 = private constant [13 x i8] c"Hello World!\00"
@.str16 = private constant [12 x i8] c"DisplayFunc\00"
@.str17 = private constant [9 x i8] c"MainLoop\00"

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
declare dllimport x86_stdcallcc void @glutMainLoop() 
declare i32 @GLUT._init() 

declare void @Out.Ln() nounwind 
declare void @Out.String({i32, %CHAR*}) nounwind 
declare void @Out.Integer(i64) nounwind 
declare void @Out.Real(double) nounwind 
declare void @Out.Boolean(i1) nounwind 
declare i32 @Out._init() 

define void @DisplayHandler() nounwind {
    
    ; GL.Clear(16640)
    call x86_stdcallcc void (i32)* @glClear(i32 16640)
    
    ; GL.Enable(2929)
    call x86_stdcallcc void (i32)* @glEnable(i32 2929)
    
    ; GL.MatrixMode(5889)
    call x86_stdcallcc void (i32)* @glMatrixMode(i32 5889)
    
    ; GL.LoadIdentity()
    call x86_stdcallcc void ()* @glLoadIdentity()
    
    ; GL.Ortho(0xc000000000000000, 0x4000000000000000, 0xc000000000000000, 0x4000000000000000, 0xc000000000000000, 0x407f400000000000)
    call x86_stdcallcc void (double, double, double, double, double, double)* @glOrtho(double 0xc000000000000000, double 0x4000000000000000, double 0xc000000000000000, double 0x4000000000000000, double 0xc000000000000000, double 0x407f400000000000)
    
    ; GL.MatrixMode(5888)
    call x86_stdcallcc void (i32)* @glMatrixMode(i32 5888)
    
    ; GL.LoadIdentity()
    call x86_stdcallcc void ()* @glLoadIdentity()
    
    ; GLUT.LookAt(2, 2, 2, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x3ff0000000000000, 0x0000000000000000)
    call x86_stdcallcc void (double, double, double, double, double, double, double, double, double)* @gluLookAt(double 2.000000e+000, double 2.000000e+000, double 2.000000e+000, double 0x0000000000000000, double 0x0000000000000000, double 0x0000000000000000, double 0x0000000000000000, double 0x3ff0000000000000, double 0x0000000000000000)
    
    ; GL.Scalef(0x3f747ae140000000, 0x3f747ae140000000, 0x3f747ae140000000)
    call x86_stdcallcc void (float, float, float)* @glScalef(float 0x3f747ae140000000, float 0x3f747ae140000000, float 0x3f747ae140000000)
    
    ; GL.Rotatef(20, 0, 1, 0)
    call x86_stdcallcc void (float, float, float, float)* @glRotatef(float 2.000000e+001, float 0.000000e+000, float 1.000000e+000, float 0.000000e+000)
    
    ; GL.Rotatef(30, 0, 0, 1)
    call x86_stdcallcc void (float, float, float, float)* @glRotatef(float 3.000000e+001, float 0.000000e+000, float 0.000000e+000, float 1.000000e+000)
    
    ; GL.Rotatef(5, 1, 0, 0)
    call x86_stdcallcc void (float, float, float, float)* @glRotatef(float 5.000000e+000, float 1.000000e+000, float 0.000000e+000, float 0.000000e+000)
    
    ; GL.Translatef(-300, 0, 0)
    call x86_stdcallcc void (float, float, float)* @glTranslatef(float -3.000000e+002, float 0.000000e+000, float 0.000000e+000)
    
    ; GL.Color3f(1, 1, 1)
    call x86_stdcallcc void (float, float, float)* @glColor3f(float 1.000000e+000, float 1.000000e+000, float 1.000000e+000)
    
    ; GLUT.StrokeCharacter(NIL, "H")
    %1 = getelementptr inbounds [2 x %CHAR]* @.str0, i32 0, i32 0
    %2 = load %CHAR* %1
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %2)
    
    ; GLUT.StrokeCharacter(NIL, "e")
    %3 = getelementptr inbounds [2 x %CHAR]* @.str1, i32 0, i32 0
    %4 = load %CHAR* %3
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %4)
    
    ; GLUT.StrokeCharacter(NIL, "l")
    %5 = getelementptr inbounds [2 x %CHAR]* @.str2, i32 0, i32 0
    %6 = load %CHAR* %5
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %6)
    
    ; GLUT.StrokeCharacter(NIL, "l")
    %7 = getelementptr inbounds [2 x %CHAR]* @.str2, i32 0, i32 0
    %8 = load %CHAR* %7
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %8)
    
    ; GLUT.StrokeCharacter(NIL, "o")
    %9 = getelementptr inbounds [2 x %CHAR]* @.str3, i32 0, i32 0
    %10 = load %CHAR* %9
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %10)
    
    ; GLUT.StrokeCharacter(NIL, "W")
    %11 = getelementptr inbounds [2 x %CHAR]* @.str4, i32 0, i32 0
    %12 = load %CHAR* %11
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %12)
    
    ; GLUT.StrokeCharacter(NIL, "o")
    %13 = getelementptr inbounds [2 x %CHAR]* @.str3, i32 0, i32 0
    %14 = load %CHAR* %13
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %14)
    
    ; GLUT.StrokeCharacter(NIL, "r")
    %15 = getelementptr inbounds [2 x %CHAR]* @.str5, i32 0, i32 0
    %16 = load %CHAR* %15
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %16)
    
    ; GLUT.StrokeCharacter(NIL, "l")
    %17 = getelementptr inbounds [2 x %CHAR]* @.str2, i32 0, i32 0
    %18 = load %CHAR* %17
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %18)
    
    ; GLUT.StrokeCharacter(NIL, "d")
    %19 = getelementptr inbounds [2 x %CHAR]* @.str6, i32 0, i32 0
    %20 = load %CHAR* %19
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %20)
    
    ; GLUT.StrokeCharacter(NIL, "!")
    %21 = getelementptr inbounds [2 x %CHAR]* @.str7, i32 0, i32 0
    %22 = load %CHAR* %21
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %22)
    
    ; GLUT.SwapBuffers()
    call x86_stdcallcc void ()* @glutSwapBuffers()
    
    ret void 
}


@GLTest._hasInit = private global i1 zeroinitializer

define i32 @GLTest._init() nounwind {
    
    %1 = load i1* @GLTest._hasInit
    br i1 %1, label %33, label %2
    
; <label>:2                                       ; preds = %0
    store i1 1, i1* @GLTest._hasInit
    
    %3 = call i32 ()* @GL._init()
    %4 = icmp eq i32 %3, 0
    br i1 %4, label %5, label %34
    
; <label>:5                                       ; preds = %2
    %6 = call i32 ()* @GLUT._init()
    %7 = icmp eq i32 %6, 0
    br i1 %7, label %8, label %34
    
; <label>:8                                       ; preds = %5
    %9 = call i32 ()* @Out._init()
    %10 = icmp eq i32 %9, 0
    br i1 %10, label %11, label %34
    
; <label>:11                                      ; preds = %8
    
    ; Out.String("Init")
    %12 = getelementptr inbounds [5 x %CHAR]* @.str8, i32 0, i32 0
    %13 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str9, i32 0, i32 0), %CHAR* %12) nounwind
    
    ; Out.Ln()
    %14 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str10, i32 0, i32 0)) nounwind
    
    ; GLUT.Init()
    call void ()* @GLUT.Init() nounwind
    
    ; Out.String("InitDisplayMode")
    %15 = getelementptr inbounds [16 x %CHAR]* @.str11, i32 0, i32 0
    %16 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str9, i32 0, i32 0), %CHAR* %15) nounwind
    
    ; Out.Ln()
    %17 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str10, i32 0, i32 0)) nounwind
    
    ; GLUT.InitDisplayMode(18)
    call x86_stdcallcc void (i32)* @glutInitDisplayMode(i32 18)
    
    ; Out.String("InitWindowSize")
    %18 = getelementptr inbounds [15 x %CHAR]* @.str12, i32 0, i32 0
    %19 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str9, i32 0, i32 0), %CHAR* %18) nounwind
    
    ; Out.Ln()
    %20 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str10, i32 0, i32 0)) nounwind
    
    ; GLUT.InitWindowSize(500, 500)
    call x86_stdcallcc void (i32, i32)* @glutInitWindowSize(i32 500, i32 500)
    
    ; Out.String("InitWindowPosition")
    %21 = getelementptr inbounds [19 x %CHAR]* @.str13, i32 0, i32 0
    %22 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str9, i32 0, i32 0), %CHAR* %21) nounwind
    
    ; Out.Ln()
    %23 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str10, i32 0, i32 0)) nounwind
    
    ; GLUT.InitWindowPosition(300, 200)
    call x86_stdcallcc void (i32, i32)* @glutInitWindowPosition(i32 300, i32 200)
    
    ; Out.String("CreateWindow")
    %24 = getelementptr inbounds [13 x %CHAR]* @.str14, i32 0, i32 0
    %25 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str9, i32 0, i32 0), %CHAR* %24) nounwind
    
    ; Out.Ln()
    %26 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str10, i32 0, i32 0)) nounwind
    
    ; GLUT.CreateWindow("Hello World!")
    call void ({i32, %CHAR*})* @GLUT.CreateWindow({i32, %CHAR*} {i32 13, %CHAR* getelementptr inbounds ([13 x %CHAR]* @.str15, i32 0, i32 0)}) nounwind
    
    ; Out.String("DisplayFunc")
    %27 = getelementptr inbounds [12 x %CHAR]* @.str16, i32 0, i32 0
    %28 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str9, i32 0, i32 0), %CHAR* %27) nounwind
    
    ; Out.Ln()
    %29 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str10, i32 0, i32 0)) nounwind
    
    ; GLUT.DisplayFunc(DisplayHandler)
    call x86_stdcallcc void (void ()*)* @glutDisplayFunc(void ()* @DisplayHandler)
    
    ; Out.String("MainLoop")
    %30 = getelementptr inbounds [9 x %CHAR]* @.str17, i32 0, i32 0
    %31 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str9, i32 0, i32 0), %CHAR* %30) nounwind
    
    ; Out.Ln()
    %32 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str10, i32 0, i32 0)) nounwind
    
    ; GLUT.MainLoop()
    call x86_stdcallcc void ()* @glutMainLoop()
    
    br label %33
    
; <label>:33                                      ; preds = %0, %11
    ret i32 0
    
; <label>:34                                      ; preds = %2, %5, %8
    ret i32 1
}

define i32 @main() nounwind {
    
    %1 = call i32 ()* @GLTest._init()
    ret i32 %1
}

