; ModuleID = 'ir/out.ll'
target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

@Out._hasInit = private global i1 false
@GL._hasInit = private global i1 false
@.str0 = private constant [12 x i8] c"GLUT Error \00"
@.str1 = private constant [3 x i8] c"%s\00"
@.str2 = private constant [3 x i8] c"%i\00"
@.str3 = private constant [2 x i8] c"\0A\00"
@GLUT._hasInit = private global i1 false
@.str01 = private constant [2 x i8] c"H\00"
@.str12 = private constant [2 x i8] c"e\00"
@.str23 = private constant [2 x i8] c"l\00"
@.str34 = private constant [2 x i8] c"a\00"
@.str4 = private constant [2 x i8] c"V\00"
@.str5 = private constant [2 x i8] c"u\00"
@.str6 = private constant [2 x i8] c"r\00"
@.str7 = private constant [2 x i8] c"d\00"
@.str8 = private constant [2 x i8] c"!\00"
@.str9 = private constant [13 x i8] c"Hello World!\00"
@r = private global float 0.000000e+00
@g = private global float 0.000000e+00
@b = private global float 0.000000e+00
@lastDraw = private global i64 0
@GLTest._hasInit = private global i1 false

declare i32 @printf(i8*, ...) nounwind

declare noalias i8* @GC_malloc(i32)

declare void @Out.Ln() nounwind

declare void @Out.String({ i32, i8* }) nounwind

declare void @Out.Integer(i64) nounwind

declare void @Out.Real(double) nounwind

declare void @Out.Boolean(i1) nounwind

define i32 @Out._init() nounwind {
  ret i32 0
}

declare x86_stdcallcc void @glClear(i32)

declare x86_stdcallcc void @glEnable(i32)

declare x86_stdcallcc void @glMatrixMode(i32)

declare x86_stdcallcc void @glLoadIdentity()

declare x86_stdcallcc void @glOrtho(double, double, double, double, double, double)

declare x86_stdcallcc void @glScalef(float, float, float)

declare x86_stdcallcc void @glRotatef(float, float, float, float)

declare x86_stdcallcc void @glTranslatef(float, float, float)

declare x86_stdcallcc void @glColor3f(float, float, float)

define i32 @GL._init() nounwind {
  ret i32 0
}

define void @Exit(i32 %"$errorCode") nounwind {
  %errorCode = alloca i32
  store i32 %"$errorCode", i32* %errorCode
  %1 = getelementptr inbounds [12 x i8]* @.str0, i32 0, i32 0
  %2 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str1, i32 0, i32 0), i8* %1) nounwind
  %3 = load i32* %errorCode
  %4 = sext i32 %3 to i64
  %5 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str2, i32 0, i32 0), i64 %4) nounwind
  %6 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.str3, i32 0, i32 0)) nounwind
  ret void
}

declare x86_stdcallcc void @gluLookAt(double, double, double, double, double, double, double, double, double)

declare dllimport x86_stdcallcc void @__glutInitWithExit(i32*, i8**, void (i32)*)

define void @GLUT.Init() nounwind {
  %argc = alloca i32
  %argv = alloca i8**
  store i32 0, i32* %argc
  %1 = getelementptr inbounds i8** null, i32 1
  %2 = ptrtoint i8** %1 to i32
  %3 = call i8* @GC_malloc(i32 %2)
  %4 = bitcast i8* %3 to i8**
  store i8* null, i8** %4
  store i8** %4, i8*** %argv
  %5 = load i8*** %argv
  call x86_stdcallcc void @__glutInitWithExit(i32* %argc, i8** %5, void (i32)* @Exit)
  ret void
}

declare dllimport x86_stdcallcc void @__glutCreateWindowWithExit(i8*, void (i32)*)

define void @GLUT.CreateWindow({ i32, i8* } %"$title") nounwind {
  %title = alloca { i32, i8* }
  store { i32, i8* } %"$title", { i32, i8* }* %title
  %1 = getelementptr inbounds { i32, i8* }* %title, i32 0, i32 1
  %2 = load i8** %1
  call x86_stdcallcc void @__glutCreateWindowWithExit(i8* %2, void (i32)* @Exit)
  ret void
}

define i32 @GLUT._init() nounwind {
  ret i32 0
}

declare dllimport x86_stdcallcc void @glutStrokeCharacter(i8*, i8)

declare dllimport x86_stdcallcc void @glutSwapBuffers()

declare dllimport x86_stdcallcc void @glutInitDisplayMode(i32)

declare dllimport x86_stdcallcc void @glutInitWindowSize(i32, i32)

declare dllimport x86_stdcallcc void @glutInitWindowPosition(i32, i32)

declare dllimport x86_stdcallcc void @glutDisplayFunc(void ()*)

declare dllimport x86_stdcallcc void @glutIdleFunc(void ()*)

declare dllimport x86_stdcallcc void @glutMainLoop()

declare dllimport x86_stdcallcc void @glutPostRedisplay()

declare x86_stdcallcc i64 @GetTickCount()

define void @IdleHandler() nounwind {
  %curTime = alloca i64
  %1 = call x86_stdcallcc i64 @GetTickCount()
  store i64 %1, i64* %curTime
  %2 = load i64* %curTime
  %3 = load i64* @lastDraw
  %4 = sub i64 %2, %3
  %5 = icmp sgt i64 %4, 16
  br i1 %5, label %6, label %29

; <label>:6                                       ; preds = %0
  %7 = load float* @r
  %8 = fdiv float 1.000000e+00, 1.600000e+01
  %9 = fadd float %7, %8
  store float %9, float* @r
  %10 = load float* @r
  %11 = fcmp oge float %10, 2.000000e+00
  br i1 %11, label %12, label %27

; <label>:12                                      ; preds = %6
  store float 0.000000e+00, float* @r
  %13 = load float* @g
  %14 = fdiv float 1.000000e+00, 1.600000e+01
  %15 = fadd float %13, %14
  store float %15, float* @g
  %16 = load float* @g
  %17 = fcmp oge float %16, 2.000000e+00
  br i1 %17, label %18, label %26

; <label>:18                                      ; preds = %12
  store float 0.000000e+00, float* @g
  %19 = load float* @b
  %20 = fdiv float 1.000000e+00, 1.600000e+01
  %21 = fadd float %19, %20
  store float %21, float* @b
  %22 = load float* @b
  %23 = fcmp oge float %22, 2.000000e+00
  br i1 %23, label %24, label %25

; <label>:24                                      ; preds = %18
  store float 0.000000e+00, float* @b
  br label %25

; <label>:25                                      ; preds = %24, %18
  br label %26

; <label>:26                                      ; preds = %25, %12
  br label %27

; <label>:27                                      ; preds = %26, %6
  %28 = load i64* %curTime
  store i64 %28, i64* @lastDraw
  call x86_stdcallcc void @glutPostRedisplay()
  br label %29

; <label>:29                                      ; preds = %27, %0
  ret void
}

define void @DisplayHandler() nounwind {
  %rn = alloca float
  %gn = alloca float
  %bn = alloca float
  call x86_stdcallcc void @glClear(i32 16640)
  call x86_stdcallcc void @glEnable(i32 2929)
  call x86_stdcallcc void @glMatrixMode(i32 5889)
  call x86_stdcallcc void @glLoadIdentity()
  call x86_stdcallcc void @glOrtho(double -2.000000e+00, double 2.000000e+00, double -2.000000e+00, double 2.000000e+00, double -2.000000e+00, double 5.000000e+02)
  call x86_stdcallcc void @glMatrixMode(i32 5888)
  call x86_stdcallcc void @glLoadIdentity()
  call x86_stdcallcc void @gluLookAt(double 2.000000e+00, double 2.000000e+00, double 2.000000e+00, double 0.000000e+00, double 0.000000e+00, double 0.000000e+00, double 0.000000e+00, double 1.000000e+00, double 0.000000e+00)
  call x86_stdcallcc void @glScalef(float 0x3F747AE140000000, float 0x3F747AE140000000, float 0x3F747AE140000000)
  call x86_stdcallcc void @glRotatef(float 2.000000e+01, float 0.000000e+00, float 1.000000e+00, float 0.000000e+00)
  call x86_stdcallcc void @glRotatef(float 3.000000e+01, float 0.000000e+00, float 0.000000e+00, float 1.000000e+00)
  call x86_stdcallcc void @glRotatef(float 5.000000e+00, float 1.000000e+00, float 0.000000e+00, float 0.000000e+00)
  call x86_stdcallcc void @glTranslatef(float -3.000000e+02, float 0.000000e+00, float 0.000000e+00)
  %1 = load float* @r
  %2 = fcmp olt float %1, 1.000000e+00
  br i1 %2, label %3, label %5

; <label>:3                                       ; preds = %0
  %4 = load float* @r
  store float %4, float* %rn
  br label %8

; <label>:5                                       ; preds = %0
  %6 = load float* @r
  %7 = fsub float 2.000000e+00, %6
  store float %7, float* %rn
  br label %8

; <label>:8                                       ; preds = %5, %3
  %9 = load float* @g
  %10 = fcmp olt float %9, 1.000000e+00
  br i1 %10, label %11, label %13

; <label>:11                                      ; preds = %8
  %12 = load float* @g
  store float %12, float* %gn
  br label %16

; <label>:13                                      ; preds = %8
  %14 = load float* @g
  %15 = fsub float 2.000000e+00, %14
  store float %15, float* %gn
  br label %16

; <label>:16                                      ; preds = %13, %11
  %17 = load float* @b
  %18 = fcmp olt float %17, 1.000000e+00
  br i1 %18, label %19, label %21

; <label>:19                                      ; preds = %16
  %20 = load float* @b
  store float %20, float* %bn
  br label %24

; <label>:21                                      ; preds = %16
  %22 = load float* @b
  %23 = fsub float 2.000000e+00, %22
  store float %23, float* %bn
  br label %24

; <label>:24                                      ; preds = %21, %19
  %25 = load float* %rn
  %26 = load float* %gn
  %27 = load float* %bn
  call x86_stdcallcc void @glColor3f(float %25, float %26, float %27)
  %28 = getelementptr inbounds [2 x i8]* @.str01, i32 0, i32 0
  %29 = load i8* %28
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %29)
  %30 = getelementptr inbounds [2 x i8]* @.str12, i32 0, i32 0
  %31 = load i8* %30
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %31)
  %32 = getelementptr inbounds [2 x i8]* @.str23, i32 0, i32 0
  %33 = load i8* %32
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %33)
  %34 = getelementptr inbounds [2 x i8]* @.str23, i32 0, i32 0
  %35 = load i8* %34
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %35)
  %36 = getelementptr inbounds [2 x i8]* @.str34, i32 0, i32 0
  %37 = load i8* %36
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %37)
  %38 = getelementptr inbounds [2 x i8]* @.str4, i32 0, i32 0
  %39 = load i8* %38
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %39)
  %40 = getelementptr inbounds [2 x i8]* @.str5, i32 0, i32 0
  %41 = load i8* %40
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %41)
  %42 = getelementptr inbounds [2 x i8]* @.str6, i32 0, i32 0
  %43 = load i8* %42
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %43)
  %44 = getelementptr inbounds [2 x i8]* @.str23, i32 0, i32 0
  %45 = load i8* %44
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %45)
  %46 = getelementptr inbounds [2 x i8]* @.str7, i32 0, i32 0
  %47 = load i8* %46
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %47)
  %48 = getelementptr inbounds [2 x i8]* @.str8, i32 0, i32 0
  %49 = load i8* %48
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %49)
  call x86_stdcallcc void @glutSwapBuffers()
  ret void
}

define i32 @GLTest._init() nounwind {
  %1 = load i1* @GLTest._hasInit
  br i1 %1, label %12, label %2

; <label>:2                                       ; preds = %0
  store i1 true, i1* @GLTest._hasInit
  %3 = call i32 @GL._init()
  %4 = icmp eq i32 %3, 0
  br i1 %4, label %5, label %13

; <label>:5                                       ; preds = %2
  %6 = call i32 @GLUT._init()
  %7 = icmp eq i32 %6, 0
  br i1 %7, label %8, label %13

; <label>:8                                       ; preds = %5
  %9 = call i32 @Out._init()
  %10 = icmp eq i32 %9, 0
  br i1 %10, label %11, label %13

; <label>:11                                      ; preds = %8
  call void @GLUT.Init() nounwind
  call x86_stdcallcc void @glutInitDisplayMode(i32 18)
  call x86_stdcallcc void @glutInitWindowSize(i32 500, i32 500)
  call x86_stdcallcc void @glutInitWindowPosition(i32 300, i32 200)
  call void @GLUT.CreateWindow({ i32, i8* } { i32 13, i8* getelementptr inbounds ([13 x i8]* @.str9, i32 0, i32 0) }) nounwind
  call x86_stdcallcc void @glutDisplayFunc(void ()* @DisplayHandler)
  call x86_stdcallcc void @glutIdleFunc(void ()* @IdleHandler)
  call x86_stdcallcc void @glutMainLoop()
  br label %12

; <label>:12                                      ; preds = %11, %0
  ret i32 0

; <label>:13                                      ; preds = %8, %5, %2
  ret i32 1
}

define i32 @main() nounwind {
  %1 = call i32 @GLTest._init()
  ret i32 %1
}
