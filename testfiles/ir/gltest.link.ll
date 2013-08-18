; ModuleID = 'testfiles/ir/out.ll'
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
@.str34 = private constant [2 x i8] c"o\00"
@.str4 = private constant [2 x i8] c"W\00"
@.str5 = private constant [2 x i8] c"r\00"
@.str6 = private constant [2 x i8] c"d\00"
@.str7 = private constant [2 x i8] c"!\00"
@.str8 = private constant [5 x i8] c"Init\00"
@.str9 = private constant [3 x i8] c"%s\00"
@.str10 = private constant [2 x i8] c"\0A\00"
@.str11 = private constant [16 x i8] c"InitDisplayMode\00"
@.str125 = private constant [15 x i8] c"InitWindowSize\00"
@.str13 = private constant [19 x i8] c"InitWindowPosition\00"
@.str14 = private constant [13 x i8] c"CreateWindow\00"
@.str15 = private constant [13 x i8] c"Hello World!\00"
@.str16 = private constant [12 x i8] c"DisplayFunc\00"
@.str17 = private constant [9 x i8] c"MainLoop\00"
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

declare dllimport x86_stdcallcc void @glutMainLoop()

define void @DisplayHandler() nounwind {
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
  call x86_stdcallcc void @glColor3f(float 1.000000e+00, float 1.000000e+00, float 1.000000e+00)
  %1 = getelementptr inbounds [2 x i8]* @.str01, i32 0, i32 0
  %2 = load i8* %1
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %2)
  %3 = getelementptr inbounds [2 x i8]* @.str12, i32 0, i32 0
  %4 = load i8* %3
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %4)
  %5 = getelementptr inbounds [2 x i8]* @.str23, i32 0, i32 0
  %6 = load i8* %5
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %6)
  %7 = getelementptr inbounds [2 x i8]* @.str23, i32 0, i32 0
  %8 = load i8* %7
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %8)
  %9 = getelementptr inbounds [2 x i8]* @.str34, i32 0, i32 0
  %10 = load i8* %9
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %10)
  %11 = getelementptr inbounds [2 x i8]* @.str4, i32 0, i32 0
  %12 = load i8* %11
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %12)
  %13 = getelementptr inbounds [2 x i8]* @.str34, i32 0, i32 0
  %14 = load i8* %13
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %14)
  %15 = getelementptr inbounds [2 x i8]* @.str5, i32 0, i32 0
  %16 = load i8* %15
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %16)
  %17 = getelementptr inbounds [2 x i8]* @.str23, i32 0, i32 0
  %18 = load i8* %17
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %18)
  %19 = getelementptr inbounds [2 x i8]* @.str6, i32 0, i32 0
  %20 = load i8* %19
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %20)
  %21 = getelementptr inbounds [2 x i8]* @.str7, i32 0, i32 0
  %22 = load i8* %21
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i8 %22)
  call x86_stdcallcc void @glutSwapBuffers()
  ret void
}

define i32 @GLTest._init() nounwind {
  %1 = load i1* @GLTest._hasInit
  br i1 %1, label %33, label %2

; <label>:2                                       ; preds = %0
  store i1 true, i1* @GLTest._hasInit
  %3 = call i32 @GL._init()
  %4 = icmp eq i32 %3, 0
  br i1 %4, label %5, label %34

; <label>:5                                       ; preds = %2
  %6 = call i32 @GLUT._init()
  %7 = icmp eq i32 %6, 0
  br i1 %7, label %8, label %34

; <label>:8                                       ; preds = %5
  %9 = call i32 @Out._init()
  %10 = icmp eq i32 %9, 0
  br i1 %10, label %11, label %34

; <label>:11                                      ; preds = %8
  %12 = getelementptr inbounds [5 x i8]* @.str8, i32 0, i32 0
  %13 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str9, i32 0, i32 0), i8* %12) nounwind
  %14 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.str10, i32 0, i32 0)) nounwind
  call void @GLUT.Init() nounwind
  %15 = getelementptr inbounds [16 x i8]* @.str11, i32 0, i32 0
  %16 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str9, i32 0, i32 0), i8* %15) nounwind
  %17 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.str10, i32 0, i32 0)) nounwind
  call x86_stdcallcc void @glutInitDisplayMode(i32 18)
  %18 = getelementptr inbounds [15 x i8]* @.str125, i32 0, i32 0
  %19 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str9, i32 0, i32 0), i8* %18) nounwind
  %20 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.str10, i32 0, i32 0)) nounwind
  call x86_stdcallcc void @glutInitWindowSize(i32 500, i32 500)
  %21 = getelementptr inbounds [19 x i8]* @.str13, i32 0, i32 0
  %22 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str9, i32 0, i32 0), i8* %21) nounwind
  %23 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.str10, i32 0, i32 0)) nounwind
  call x86_stdcallcc void @glutInitWindowPosition(i32 300, i32 200)
  %24 = getelementptr inbounds [13 x i8]* @.str14, i32 0, i32 0
  %25 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str9, i32 0, i32 0), i8* %24) nounwind
  %26 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.str10, i32 0, i32 0)) nounwind
  call void @GLUT.CreateWindow({ i32, i8* } { i32 13, i8* getelementptr inbounds ([13 x i8]* @.str15, i32 0, i32 0) }) nounwind
  %27 = getelementptr inbounds [12 x i8]* @.str16, i32 0, i32 0
  %28 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str9, i32 0, i32 0), i8* %27) nounwind
  %29 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.str10, i32 0, i32 0)) nounwind
  call x86_stdcallcc void @glutDisplayFunc(void ()* @DisplayHandler)
  %30 = getelementptr inbounds [9 x i8]* @.str17, i32 0, i32 0
  %31 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str9, i32 0, i32 0), i8* %30) nounwind
  %32 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.str10, i32 0, i32 0)) nounwind
  call x86_stdcallcc void @glutMainLoop()
  br label %33

; <label>:33                                      ; preds = %11, %0
  ret i32 0

; <label>:34                                      ; preds = %8, %5, %2
  ret i32 1
}

define i32 @main() nounwind {
  %1 = call i32 @GLTest._init()
  ret i32 %1
}
