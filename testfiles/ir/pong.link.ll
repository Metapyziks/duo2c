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

define void @IdleHandler() nounwind {
  ret void
}

define void @DisplayHandler() nounwind {
  call x86_stdcallcc void @glClear(i32 16640)
  call x86_stdcallcc void @glutSwapBuffers()
  ret void
}

define i32 @Pong._init() nounwind {
  %1 = load i1* @Pong._hasInit
  br i1 %1, label %12, label %2

; <label>:2                                       ; preds = %0
  store i1 true, i1* @Pong._hasInit
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
  call x86_stdcallcc void @glutInitWindowSize(i32 800, i32 600)
  call x86_stdcallcc void @glutInitWindowPosition(i32 300, i32 200)
  call void @GLUT.CreateWindow({ i32, i8* } { i32 9, i8* getelementptr inbounds ([9 x i8]* @.str01, i32 0, i32 0) }) nounwind
  call x86_stdcallcc void @glutIdleFunc(void ()* @IdleHandler)
  call x86_stdcallcc void @glutDisplayFunc(void ()* @DisplayHandler)
  call x86_stdcallcc void @glutMainLoop()
  br label %12

; <label>:12                                      ; preds = %11, %0
  ret i32 0

; <label>:13                                      ; preds = %8, %5, %2
  ret i32 1
}

define i32 @main() nounwind {
  %1 = call i32 @Pong._init()
  ret i32 %1
}
