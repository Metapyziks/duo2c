; ModuleID = 'testfiles/ir/gl.ll'
target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

@GL._hasInit = private global i1 false
@GLUT._hasInit = private global i1 false
@.str0 = private constant [2 x i8] c"H\00"
@.str1 = private constant [2 x i8] c"e\00"
@.str2 = private constant [2 x i8] c"l\00"
@.str3 = private constant [2 x i8] c"o\00"
@.str4 = private constant [2 x i8] c"W\00"
@.str5 = private constant [2 x i8] c"r\00"
@.str6 = private constant [2 x i8] c"d\00"
@.str7 = private constant [2 x i8] c"!\00"
@.str8 = private constant [13 x i8] c"Hello World!\00"
@GLTest._hasInit = private global i1 false

declare i32 @printf(i8*, ...) nounwind

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

define i32 @GL._init() nounwind {
  ret i32 0
}

declare void @exit(i32)

declare void @gluLookAt(double, double, double, double, double, double, double, double, double)

declare void @glutStrokeCharacter(i8*, i8)

declare void @glutSwapBuffers()

declare void @__glutInitWithExit(i32, i8**, void (i32)*)

define void @GLUT.Init() nounwind {
  call void @__glutInitWithExit(i32 0, i8** null, void (i32)* @exit)
  ret void
}

declare void @glutInitDisplayMode(i32)

declare void @glutInitWindowSize(i32, i32)

declare void @glutInitWindowPosition(i32, i32)

declare void @__glutCreateWindowWithExit(i8*, void (i32)*)

define void @GLUT.CreateWindow({ i32, i8* } %"$title") nounwind {
  %title = alloca { i32, i8* }
  store { i32, i8* } %"$title", { i32, i8* }* %title
  %1 = getelementptr inbounds { i32, i8* }* %title, i32 0, i32 1
  %2 = load i8** %1
  call void @__glutCreateWindowWithExit(i8* %2, void (i32)* @exit)
  ret void
}

declare void @glutDisplayFunc(void ()*)

declare void @glutMainLoop()

define i32 @GLUT._init() nounwind {
  ret i32 0
}

define void @DisplayHandler() nounwind {
  call void @glClear(i32 16640)
  call void @glEnable(i32 2929)
  call void @glMatrixMode(i32 5889)
  call void @glLoadIdentity()
  call void @glOrtho(double -2.000000e+00, double 2.000000e+00, double -2.000000e+00, double 2.000000e+00, double -2.000000e+00, double 5.000000e+02)
  call void @glMatrixMode(i32 5888)
  call void @glLoadIdentity()
  call void @gluLookAt(double 2.000000e+00, double 2.000000e+00, double 2.000000e+00, double 0.000000e+00, double 0.000000e+00, double 0.000000e+00, double 0.000000e+00, double 1.000000e+00, double 0.000000e+00)
  call void @glScalef(float 0x3F747AE140000000, float 0x3F747AE140000000, float 0x3F747AE140000000)
  call void @glRotatef(float 2.000000e+01, float 0.000000e+00, float 1.000000e+00, float 0.000000e+00)
  call void @glRotatef(float 3.000000e+01, float 0.000000e+00, float 0.000000e+00, float 1.000000e+00)
  call void @glRotatef(float 5.000000e+00, float 1.000000e+00, float 0.000000e+00, float 0.000000e+00)
  call void @glTranslatef(float -3.000000e+02, float 0.000000e+00, float 0.000000e+00)
  call void @glColor3f(float 1.000000e+00, float 1.000000e+00, float 1.000000e+00)
  %1 = getelementptr inbounds [2 x i8]* @.str0, i32 0, i32 0
  %2 = load i8* %1
  call void @glutStrokeCharacter(i8* null, i8 %2)
  %3 = getelementptr inbounds [2 x i8]* @.str1, i32 0, i32 0
  %4 = load i8* %3
  call void @glutStrokeCharacter(i8* null, i8 %4)
  %5 = getelementptr inbounds [2 x i8]* @.str2, i32 0, i32 0
  %6 = load i8* %5
  call void @glutStrokeCharacter(i8* null, i8 %6)
  %7 = getelementptr inbounds [2 x i8]* @.str2, i32 0, i32 0
  %8 = load i8* %7
  call void @glutStrokeCharacter(i8* null, i8 %8)
  %9 = getelementptr inbounds [2 x i8]* @.str3, i32 0, i32 0
  %10 = load i8* %9
  call void @glutStrokeCharacter(i8* null, i8 %10)
  %11 = getelementptr inbounds [2 x i8]* @.str4, i32 0, i32 0
  %12 = load i8* %11
  call void @glutStrokeCharacter(i8* null, i8 %12)
  %13 = getelementptr inbounds [2 x i8]* @.str3, i32 0, i32 0
  %14 = load i8* %13
  call void @glutStrokeCharacter(i8* null, i8 %14)
  %15 = getelementptr inbounds [2 x i8]* @.str5, i32 0, i32 0
  %16 = load i8* %15
  call void @glutStrokeCharacter(i8* null, i8 %16)
  %17 = getelementptr inbounds [2 x i8]* @.str2, i32 0, i32 0
  %18 = load i8* %17
  call void @glutStrokeCharacter(i8* null, i8 %18)
  %19 = getelementptr inbounds [2 x i8]* @.str6, i32 0, i32 0
  %20 = load i8* %19
  call void @glutStrokeCharacter(i8* null, i8 %20)
  %21 = getelementptr inbounds [2 x i8]* @.str7, i32 0, i32 0
  %22 = load i8* %21
  call void @glutStrokeCharacter(i8* null, i8 %22)
  call void @glutSwapBuffers()
  ret void
}

define i32 @GLTest._init() nounwind {
  %1 = load i1* @GLTest._hasInit
  br i1 %1, label %9, label %2

; <label>:2                                       ; preds = %0
  store i1 true, i1* @GLTest._hasInit
  %3 = call i32 @GL._init()
  %4 = icmp eq i32 %3, 0
  br i1 %4, label %5, label %10

; <label>:5                                       ; preds = %2
  %6 = call i32 @GLUT._init()
  %7 = icmp eq i32 %6, 0
  br i1 %7, label %8, label %10

; <label>:8                                       ; preds = %5
  call void @GLUT.Init() nounwind
  call void @glutInitDisplayMode(i32 18)
  call void @glutInitWindowSize(i32 500, i32 500)
  call void @glutInitWindowPosition(i32 300, i32 200)
  call void @GLUT.CreateWindow({ i32, i8* } { i32 13, i8* getelementptr inbounds ([13 x i8]* @.str8, i32 0, i32 0) }) nounwind
  call void @glutDisplayFunc(void ()* @DisplayHandler)
  call void @glutMainLoop()
  br label %9

; <label>:9                                       ; preds = %8, %0
  ret i32 0

; <label>:10                                      ; preds = %5, %2
  ret i32 1
}

define i32 @main() nounwind {
  %1 = call i32 @GLTest._init()
  ret i32 %1
}
