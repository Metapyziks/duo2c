; Generated 18/08/2013 03:03:53
; GlobalUID 7f84f4f1-fbd6-4b9a-8e9f-e60cc9b9f350
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
@.str3 = private constant [2 x i8] c"a\00"
@.str4 = private constant [2 x i8] c"V\00"
@.str5 = private constant [2 x i8] c"u\00"
@.str6 = private constant [2 x i8] c"r\00"
@.str7 = private constant [2 x i8] c"d\00"
@.str8 = private constant [2 x i8] c"!\00"
@.str9 = private constant [13 x i8] c"Hello World!\00"

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
    
    %1 = call x86_stdcallcc i64 ()* @GetTickCount()
    store i64 %1, i64* %curTime
    
    %2 = load i64* %curTime
    %3 = load i64* @lastDraw
    %4 = sub i64 %2, %3
    %5 = icmp sgt i64 %4, 16
    br i1 %5, label %6, label %29
    
; <label>:6                                       ; preds = %0
    
    %7 = load float* @r
    %8 = fdiv float 0x3ff0000000000000, 0x4030000000000000
    %9 = fadd float %7, %8
    store float %9, float* @r
    
    %10 = load float* @r
    %11 = fcmp oge float %10, 2.000000e+000
    br i1 %11, label %12, label %27
    
; <label>:12                                      ; preds = %6
    
    store float 0.000000e+000, float* @r
    
    %13 = load float* @g
    %14 = fdiv float 0x3ff0000000000000, 0x4030000000000000
    %15 = fadd float %13, %14
    store float %15, float* @g
    
    %16 = load float* @g
    %17 = fcmp oge float %16, 2.000000e+000
    br i1 %17, label %18, label %26
    
; <label>:18                                      ; preds = %12
    
    store float 0.000000e+000, float* @g
    
    %19 = load float* @b
    %20 = fdiv float 0x3ff0000000000000, 0x4030000000000000
    %21 = fadd float %19, %20
    store float %21, float* @b
    
    %22 = load float* @b
    %23 = fcmp oge float %22, 2.000000e+000
    br i1 %23, label %24, label %25
    
; <label>:24                                      ; preds = %18
    
    store float 0.000000e+000, float* @b
    
    br label %25
    
; <label>:25                                      ; preds = %18, %24
    
    br label %26
    
; <label>:26                                      ; preds = %12, %25
    
    br label %27
    
; <label>:27                                      ; preds = %6, %26
    
    %28 = load i64* %curTime
    store i64 %28, i64* @lastDraw
    
    call x86_stdcallcc void ()* @glutPostRedisplay()
    
    br label %29
    
; <label>:29                                      ; preds = %0, %27
    
    ret void 
}

define void @DisplayHandler() nounwind {
    
    %rn = alloca float
    %gn = alloca float
    %bn = alloca float
    
    call x86_stdcallcc void (i32)* @glClear(i32 16640)
    
    call x86_stdcallcc void (i32)* @glEnable(i32 2929)
    
    call x86_stdcallcc void (i32)* @glMatrixMode(i32 5889)
    
    call x86_stdcallcc void ()* @glLoadIdentity()
    
    call x86_stdcallcc void (double, double, double, double, double, double)* @glOrtho(double 0xc000000000000000, double 0x4000000000000000, double 0xc000000000000000, double 0x4000000000000000, double 0xc000000000000000, double 0x407f400000000000)
    
    call x86_stdcallcc void (i32)* @glMatrixMode(i32 5888)
    
    call x86_stdcallcc void ()* @glLoadIdentity()
    
    call x86_stdcallcc void (double, double, double, double, double, double, double, double, double)* @gluLookAt(double 2.000000e+000, double 2.000000e+000, double 2.000000e+000, double 0x0000000000000000, double 0x0000000000000000, double 0x0000000000000000, double 0x0000000000000000, double 0x3ff0000000000000, double 0x0000000000000000)
    
    call x86_stdcallcc void (float, float, float)* @glScalef(float 0x3f747ae140000000, float 0x3f747ae140000000, float 0x3f747ae140000000)
    
    call x86_stdcallcc void (float, float, float, float)* @glRotatef(float 2.000000e+001, float 0.000000e+000, float 1.000000e+000, float 0.000000e+000)
    
    call x86_stdcallcc void (float, float, float, float)* @glRotatef(float 3.000000e+001, float 0.000000e+000, float 0.000000e+000, float 1.000000e+000)
    
    call x86_stdcallcc void (float, float, float, float)* @glRotatef(float 5.000000e+000, float 1.000000e+000, float 0.000000e+000, float 0.000000e+000)
    
    call x86_stdcallcc void (float, float, float)* @glTranslatef(float -3.000000e+002, float 0.000000e+000, float 0.000000e+000)
    
    %1 = load float* @r
    %2 = fcmp olt float %1, 1.000000e+000
    br i1 %2, label %3, label %5
    
; <label>:3                                       ; preds = %0
    
    %4 = load float* @r
    store float %4, float* %rn
    
    br label %8
    
; <label>:5                                       ; preds = %0
    
    %6 = load float* @r
    %7 = fsub float 2.000000e+000, %6
    store float %7, float* %rn
    
    br label %8
    
; <label>:8                                       ; preds = %3, %5
    
    %9 = load float* @g
    %10 = fcmp olt float %9, 1.000000e+000
    br i1 %10, label %11, label %13
    
; <label>:11                                      ; preds = %8
    
    %12 = load float* @g
    store float %12, float* %gn
    
    br label %16
    
; <label>:13                                      ; preds = %8
    
    %14 = load float* @g
    %15 = fsub float 2.000000e+000, %14
    store float %15, float* %gn
    
    br label %16
    
; <label>:16                                      ; preds = %11, %13
    
    %17 = load float* @b
    %18 = fcmp olt float %17, 1.000000e+000
    br i1 %18, label %19, label %21
    
; <label>:19                                      ; preds = %16
    
    %20 = load float* @b
    store float %20, float* %bn
    
    br label %24
    
; <label>:21                                      ; preds = %16
    
    %22 = load float* @b
    %23 = fsub float 2.000000e+000, %22
    store float %23, float* %bn
    
    br label %24
    
; <label>:24                                      ; preds = %19, %21
    
    %25 = load float* %rn
    %26 = load float* %gn
    %27 = load float* %bn
    call x86_stdcallcc void (float, float, float)* @glColor3f(float %25, float %26, float %27)
    
    %28 = getelementptr inbounds [2 x %CHAR]* @.str0, i32 0, i32 0
    %29 = load %CHAR* %28
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %29)
    
    %30 = getelementptr inbounds [2 x %CHAR]* @.str1, i32 0, i32 0
    %31 = load %CHAR* %30
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %31)
    
    %32 = getelementptr inbounds [2 x %CHAR]* @.str2, i32 0, i32 0
    %33 = load %CHAR* %32
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %33)
    
    %34 = getelementptr inbounds [2 x %CHAR]* @.str2, i32 0, i32 0
    %35 = load %CHAR* %34
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %35)
    
    %36 = getelementptr inbounds [2 x %CHAR]* @.str3, i32 0, i32 0
    %37 = load %CHAR* %36
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %37)
    
    %38 = getelementptr inbounds [2 x %CHAR]* @.str4, i32 0, i32 0
    %39 = load %CHAR* %38
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %39)
    
    %40 = getelementptr inbounds [2 x %CHAR]* @.str5, i32 0, i32 0
    %41 = load %CHAR* %40
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %41)
    
    %42 = getelementptr inbounds [2 x %CHAR]* @.str6, i32 0, i32 0
    %43 = load %CHAR* %42
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %43)
    
    %44 = getelementptr inbounds [2 x %CHAR]* @.str2, i32 0, i32 0
    %45 = load %CHAR* %44
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %45)
    
    %46 = getelementptr inbounds [2 x %CHAR]* @.str7, i32 0, i32 0
    %47 = load %CHAR* %46
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %47)
    
    %48 = getelementptr inbounds [2 x %CHAR]* @.str8, i32 0, i32 0
    %49 = load %CHAR* %48
    call x86_stdcallcc void (i8*, %CHAR)* @glutStrokeCharacter(i8* null, %CHAR %49)
    
    call x86_stdcallcc void ()* @glutSwapBuffers()
    
    ret void 
}


@GLTest._hasInit = private global i1 zeroinitializer

define i32 @GLTest._init() nounwind {
    
    %1 = load i1* @GLTest._hasInit
    br i1 %1, label %12, label %2
    
; <label>:2                                       ; preds = %0
    store i1 1, i1* @GLTest._hasInit
    
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
    
    call void ()* @GLUT.Init() nounwind
    
    call x86_stdcallcc void (i32)* @glutInitDisplayMode(i32 18)
    
    call x86_stdcallcc void (i32, i32)* @glutInitWindowSize(i32 500, i32 500)
    
    call x86_stdcallcc void (i32, i32)* @glutInitWindowPosition(i32 300, i32 200)
    
    call void ({i32, %CHAR*})* @GLUT.CreateWindow({i32, %CHAR*} {i32 13, %CHAR* getelementptr inbounds ([13 x %CHAR]* @.str9, i32 0, i32 0)}) nounwind
    
    call x86_stdcallcc void (void ()*)* @glutDisplayFunc(void ()* @DisplayHandler)
    
    call x86_stdcallcc void (void ()*)* @glutIdleFunc(void ()* @IdleHandler)
    
    call x86_stdcallcc void ()* @glutMainLoop()
    
    br label %12
    
; <label>:12                                      ; preds = %0, %11
    ret i32 0
    
; <label>:13                                      ; preds = %2, %5, %8
    ret i32 1
}

define i32 @main() nounwind {
    
    %1 = call i32 ()* @GLTest._init()
    ret i32 %1
}

