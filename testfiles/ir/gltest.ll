; Generated 18/08/2013 22:27:16
; GlobalUID 68ac8701-a39e-4278-ab8f-81f015f80e60
; 
; LLVM IR file for module "GLTest"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET = type i64

@.str0 = private constant [13 x i8] c"Hello World!\00"

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

@r = private global float zeroinitializer
@g = private global float zeroinitializer
@b = private global float zeroinitializer
@lastDraw = private global i64 zeroinitializer

declare x86_stdcallcc i64 @GetTickCount() 
define void @IdleHandler() nounwind {
    
    %curTime = alloca i64
    
    ; curTime := GetTickCount()
    %1 = call x86_stdcallcc i64 ()* @GetTickCount()
    store i64 %1, i64* %curTime
    
    ; IF curTime - lastDraw > 16 THEN
    %2 = load i64* %curTime
    %3 = load i64* @lastDraw
    %4 = sub i64 %2, %3
    %5 = icmp sgt i64 %4, 16
    br i1 %5, label %6, label %29
    
; <label>:6                                       ; preds = %0
    
    ; r := r + 0x3ff0000000000000 / 0x4030000000000000
    %7 = load float* @r
    %8 = fdiv float 0x3ff0000000000000, 0x4030000000000000
    %9 = fadd float %7, %8
    store float %9, float* @r
    
    ; IF r >= 2 THEN
    %10 = load float* @r
    %11 = fcmp oge float %10, 2.000000e+000
    br i1 %11, label %12, label %27
    
; <label>:12                                      ; preds = %6
    
    ; r := 0
    store float 0.000000e+000, float* @r
    
    ; g := g + 0x3ff0000000000000 / 0x4030000000000000
    %13 = load float* @g
    %14 = fdiv float 0x3ff0000000000000, 0x4030000000000000
    %15 = fadd float %13, %14
    store float %15, float* @g
    
    ; IF g >= 2 THEN
    %16 = load float* @g
    %17 = fcmp oge float %16, 2.000000e+000
    br i1 %17, label %18, label %26
    
; <label>:18                                      ; preds = %12
    
    ; g := 0
    store float 0.000000e+000, float* @g
    
    ; b := b + 0x3ff0000000000000 / 0x4030000000000000
    %19 = load float* @b
    %20 = fdiv float 0x3ff0000000000000, 0x4030000000000000
    %21 = fadd float %19, %20
    store float %21, float* @b
    
    ; IF b >= 2 THEN
    %22 = load float* @b
    %23 = fcmp oge float %22, 2.000000e+000
    br i1 %23, label %24, label %25
    
; <label>:24                                      ; preds = %18
    
    ; b := 0
    store float 0.000000e+000, float* @b
    
    br label %25
    
; <label>:25                                      ; preds = %18, %24
    
    br label %26
    
; <label>:26                                      ; preds = %12, %25
    
    br label %27
    
; <label>:27                                      ; preds = %6, %26
    
    ; lastDraw := curTime
    %28 = load i64* %curTime
    store i64 %28, i64* @lastDraw
    
    ; GLUT.PostRedisplay()
    call x86_stdcallcc void ()* @glutPostRedisplay()
    
    br label %29
    
; <label>:29                                      ; preds = %0, %27
    
    ret void 
}

define void @DisplayHandler() nounwind {
    
    %rn = alloca float
    %gn = alloca float
    %bn = alloca float
    
    ; GL.Clear(GL.ColorBufferBit OR GL.DepthBufferBit)
    %1 = or i16 16384, 256
    %2 = sext i16 %1 to i32
    call x86_stdcallcc void (i32)* @glClear(i32 %2)
    
    ; GL.Enable(GL.DepthTest)
    call x86_stdcallcc void (i32)* @glEnable(i32 2929)
    
    ; GL.MatrixMode(GL.Projection)
    call x86_stdcallcc void (i32)* @glMatrixMode(i32 5889)
    
    ; GL.LoadIdentity()
    call x86_stdcallcc void ()* @glLoadIdentity()
    
    ; GL.Ortho(0xc000000000000000, 0x4000000000000000, 0xc000000000000000, 0x4000000000000000, 0xc000000000000000, 0x407f400000000000)
    call x86_stdcallcc void (double, double, double, double, double, double)* @glOrtho(double 0xc000000000000000, double 0x4000000000000000, double 0xc000000000000000, double 0x4000000000000000, double 0xc000000000000000, double 0x407f400000000000)
    
    ; GL.MatrixMode(GL.Modelview)
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
    
    ; IF r < 1 THEN
    %3 = load float* @r
    %4 = fcmp olt float %3, 1.000000e+000
    br i1 %4, label %5, label %7
    
; <label>:5                                       ; preds = %0
    
    ; rn := r
    %6 = load float* @r
    store float %6, float* %rn
    
    br label %10
    
; <label>:7                                       ; preds = %0
    
    ; rn := 2 - r
    %8 = load float* @r
    %9 = fsub float 2.000000e+000, %8
    store float %9, float* %rn
    
    br label %10
    
; <label>:10                                      ; preds = %5, %7
    
    ; IF g < 1 THEN
    %11 = load float* @g
    %12 = fcmp olt float %11, 1.000000e+000
    br i1 %12, label %13, label %15
    
; <label>:13                                      ; preds = %10
    
    ; gn := g
    %14 = load float* @g
    store float %14, float* %gn
    
    br label %18
    
; <label>:15                                      ; preds = %10
    
    ; gn := 2 - g
    %16 = load float* @g
    %17 = fsub float 2.000000e+000, %16
    store float %17, float* %gn
    
    br label %18
    
; <label>:18                                      ; preds = %13, %15
    
    ; IF b < 1 THEN
    %19 = load float* @b
    %20 = fcmp olt float %19, 1.000000e+000
    br i1 %20, label %21, label %23
    
; <label>:21                                      ; preds = %18
    
    ; bn := b
    %22 = load float* @b
    store float %22, float* %bn
    
    br label %26
    
; <label>:23                                      ; preds = %18
    
    ; bn := 2 - b
    %24 = load float* @b
    %25 = fsub float 2.000000e+000, %24
    store float %25, float* %bn
    
    br label %26
    
; <label>:26                                      ; preds = %21, %23
    
    ; GL.Color3f(rn, gn, bn)
    %27 = load float* %rn
    %28 = load float* %gn
    %29 = load float* %bn
    call x86_stdcallcc void (float, float, float)* @glColor3f(float %27, float %28, float %29)
    
    ; GLUT.StrokeCharacter(NIL, "H")
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR 72)
    
    ; GLUT.StrokeCharacter(NIL, "e")
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR 101)
    
    ; GLUT.StrokeCharacter(NIL, "l")
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR 108)
    
    ; GLUT.StrokeCharacter(NIL, "l")
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR 108)
    
    ; GLUT.StrokeCharacter(NIL, "a")
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR 97)
    
    ; GLUT.StrokeCharacter(NIL, "V")
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR 86)
    
    ; GLUT.StrokeCharacter(NIL, "u")
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR 117)
    
    ; GLUT.StrokeCharacter(NIL, "r")
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR 114)
    
    ; GLUT.StrokeCharacter(NIL, "l")
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR 108)
    
    ; GLUT.StrokeCharacter(NIL, "d")
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR 100)
    
    ; GLUT.StrokeCharacter(NIL, "!")
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR 33)
    
    ; GLUT.SwapBuffers()
    call x86_stdcallcc void ()* @glutSwapBuffers()
    
    ret void 
}


@GLTest._hasInit = private global i1 zeroinitializer

define i32 @GLTest._init() nounwind {
    
    %1 = load i1* @GLTest._hasInit
    br i1 %1, label %15, label %2
    
; <label>:2                                       ; preds = %0
    store i1 1, i1* @GLTest._hasInit
    
    %3 = call i32 ()* @GL._init()
    %4 = icmp eq i32 %3, 0
    br i1 %4, label %5, label %16
    
; <label>:5                                       ; preds = %2
    %6 = call i32 ()* @GLUT._init()
    %7 = icmp eq i32 %6, 0
    br i1 %7, label %8, label %16
    
; <label>:8                                       ; preds = %5
    %9 = call i32 ()* @Out._init()
    %10 = icmp eq i32 %9, 0
    br i1 %10, label %11, label %16
    
; <label>:11                                      ; preds = %8
    
    ; GLUT.Init()
    call void ()* @GLUT.Init() nounwind
    
    ; GLUT.InitDisplayMode(GLUT.RGB OR GLUT.Double OR GLUT.Depth)
    %12 = or i8 0, 2
    %13 = or i8 %12, 16
    %14 = sext i8 %13 to i32
    call x86_stdcallcc void (i32)* @glutInitDisplayMode(i32 %14)
    
    ; GLUT.InitWindowSize(500, 500)
    call x86_stdcallcc void (i32, i32)* @glutInitWindowSize(i32 500, i32 500)
    
    ; GLUT.InitWindowPosition(300, 200)
    call x86_stdcallcc void (i32, i32)* @glutInitWindowPosition(i32 300, i32 200)
    
    ; GLUT.CreateWindow("Hello World!")
    call void ({i32, %CHAR*})* @GLUT.CreateWindow({i32, %CHAR*} {i32 13, %CHAR* getelementptr inbounds ([13 x %CHAR]* @.str0, i32 0, i32 0)}) nounwind
    
    ; GLUT.DisplayFunc(DisplayHandler)
    call x86_stdcallcc void (void ()*)* @glutDisplayFunc(void ()* @DisplayHandler)
    
    ; GLUT.IdleFunc(IdleHandler)
    call x86_stdcallcc void (void ()*)* @glutIdleFunc(void ()* @IdleHandler)
    
    ; GLUT.MainLoop()
    call x86_stdcallcc void ()* @glutMainLoop()
    
    br label %15
    
; <label>:15                                      ; preds = %0, %11
    ret i32 0
    
; <label>:16                                      ; preds = %2, %5, %8
    ret i32 1
}

define i32 @main() nounwind {
    
    %1 = call i32 ()* @GLTest._init()
    ret i32 %1
}

