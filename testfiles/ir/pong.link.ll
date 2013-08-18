; ModuleID = 'ir/out.ll'
target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

@Out._hasInit = private global i1 false
@GL._hasInit = private global i1 false
@.str0 = private constant [12 x i8] c"GLUT Error \00"
@.str1 = private constant [3 x i8] c"%s\00"
@.str2 = private constant [3 x i8] c"%i\00"
@.str3 = private constant [2 x i8] c"\0A\00"
@GLUT._hasInit = private global i1 false
@.str01 = private constant [9 x i8] c"OBERpONg\00"
@Pong._hasInit = private global i1 false

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

define void @VecToScreen(float* %x, float* %y) nounwind {
  %1 = load float* %x
  %2 = fmul float 8.000000e+02, 5.000000e-01
  %3 = fdiv float %1, %2
  store float %3, float* %x
  %4 = load float* %y
  %5 = fsub float 0.000000e+00, %4
  %6 = fmul float 6.000000e+02, 5.000000e-01
  %7 = fdiv float %5, %6
  store float %7, float* %y
  ret void
}

define void @PosToScreen(float* %x, float* %y) nounwind {
  call void @VecToScreen(float* %x, float* %y) nounwind
  %1 = load float* %x
  %2 = fsub float %1, 1.000000e+00
  store float %2, float* %x
  %3 = load float* %y
  %4 = fadd float 1.000000e+00, %3
  store float %4, float* %y
  ret void
}

define void @DrawRect(float %"$x", float %"$y", float %"$w", float %"$h") nounwind {
  %x = alloca float
  store float %"$x", float* %x
  %y = alloca float
  store float %"$y", float* %y
  %w = alloca float
  store float %"$w", float* %w
  %h = alloca float
  store float %"$h", float* %h
  call void @PosToScreen(float* %x, float* %y) nounwind
  call void @VecToScreen(float* %w, float* %h) nounwind
  call x86_stdcallcc void @glBegin(i32 7)
  call x86_stdcallcc void @glColor3f(float 1.000000e+00, float 1.000000e+00, float 1.000000e+00)
  %1 = load float* %x
  %2 = load float* %y
  call x86_stdcallcc void @glVertex2f(float %1, float %2)
  %3 = load float* %x
  %4 = load float* %w
  %5 = fadd float %3, %4
  %6 = load float* %y
  call x86_stdcallcc void @glVertex2f(float %5, float %6)
  %7 = load float* %x
  %8 = load float* %w
  %9 = fadd float %7, %8
  %10 = load float* %y
  %11 = load float* %h
  %12 = fadd float %10, %11
  call x86_stdcallcc void @glVertex2f(float %9, float %12)
  %13 = load float* %x
  %14 = load float* %y
  %15 = load float* %h
  %16 = fadd float %14, %15
  call x86_stdcallcc void @glVertex2f(float %13, float %16)
  call x86_stdcallcc void @glEnd()
  ret void
}

define void @IdleHandler() nounwind {
  ret void
}

define void @DisplayHandler() nounwind {
  call x86_stdcallcc void @glClear(i32 16384)
  call void @DrawRect(float 3.200000e+01, float 3.200000e+01, float 1.280000e+02, float 1.280000e+02) nounwind
  call x86_stdcallcc void @glutSwapBuffers()
  ret void
}

define i32 @Pong._init() nounwind {
  %1 = load i1* @Pong._hasInit
  br i1 %1, label %11, label %2

; <label>:2                                       ; preds = %0
  store i1 true, i1* @Pong._hasInit
  %3 = call i32 @GL._init()
  %4 = icmp eq i32 %3, 0
  br i1 %4, label %5, label %12

; <label>:5                                       ; preds = %2
  %6 = call i32 @GLUT._init()
  %7 = icmp eq i32 %6, 0
  br i1 %7, label %8, label %12

; <label>:8                                       ; preds = %5
  call void @GLUT.Init() nounwind
  %9 = or i8 0, 2
  %10 = sext i8 %9 to i32
  call x86_stdcallcc void @glutInitDisplayMode(i32 %10)
  call x86_stdcallcc void @glutInitWindowSize(i32 800, i32 600)
  call x86_stdcallcc void @glutInitWindowPosition(i32 300, i32 200)
  call void @GLUT.CreateWindow({ i32, i8* } { i32 9, i8* getelementptr inbounds ([9 x i8]* @.str01, i32 0, i32 0) }) nounwind
  call x86_stdcallcc void @glutIdleFunc(void ()* @IdleHandler)
  call x86_stdcallcc void @glutDisplayFunc(void ()* @DisplayHandler)
  call x86_stdcallcc void @glutMainLoop()
  br label %11

; <label>:11                                      ; preds = %8, %0
  ret i32 0

; <label>:12                                      ; preds = %5, %2
  ret i32 1
}

define i32 @main() nounwind {
  %1 = call i32 @Pong._init()
  ret i32 %1
}
