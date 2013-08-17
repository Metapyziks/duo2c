; ModuleID = 'testfiles/reference.c'
target datalayout = "e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-f80:128:128-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32-S32"
target triple = "i686-w64-mingw32"

@.str = private unnamed_addr constant [13 x i8] c"Hello World!\00", align 1

define void @displayCall() nounwind {
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
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i32 72)
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i32 101)
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i32 108)
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i32 108)
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i32 111)
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i32 87)
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i32 111)
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i32 114)
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i32 108)
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i32 100)
  call x86_stdcallcc void @glutStrokeCharacter(i8* null, i32 33)
  call x86_stdcallcc void @glutSwapBuffers()
  ret void
}

declare x86_stdcallcc void @glClear(i32)

declare x86_stdcallcc void @glEnable(i32)

declare x86_stdcallcc void @glMatrixMode(i32)

declare x86_stdcallcc void @glLoadIdentity()

declare x86_stdcallcc void @glOrtho(double, double, double, double, double, double)

declare x86_stdcallcc void @gluLookAt(double, double, double, double, double, double, double, double, double)

declare x86_stdcallcc void @glScalef(float, float, float)

declare x86_stdcallcc void @glRotatef(float, float, float, float)

declare x86_stdcallcc void @glTranslatef(float, float, float)

declare x86_stdcallcc void @glColor3f(float, float, float)

declare dllimport x86_stdcallcc void @glutStrokeCharacter(i8*, i32)

declare dllimport x86_stdcallcc void @glutSwapBuffers()

define i32 @main(i32 %argc, i8** %argv) nounwind {
  %1 = alloca i32, align 4
  %2 = alloca i32, align 4
  %3 = alloca i8**, align 4
  store i32 0, i32* %1
  store i32 %argc, i32* %2, align 4
  store i8** %argv, i8*** %3, align 4
  %4 = load i8*** %3, align 4
  call x86_stdcallcc void @glutInit_ATEXIT_HACK(i32* %2, i8** %4)
  call x86_stdcallcc void @glutInitDisplayMode(i32 18)
  call x86_stdcallcc void @glutInitWindowSize(i32 500, i32 500)
  call x86_stdcallcc void @glutInitWindowPosition(i32 300, i32 200)
  %5 = call x86_stdcallcc i32 @glutCreateWindow_ATEXIT_HACK(i8* getelementptr inbounds ([13 x i8]* @.str, i32 0, i32 0))
  call x86_stdcallcc void @glutDisplayFunc(void ()* @displayCall)
  call x86_stdcallcc void @glutMainLoop()
  ret i32 0
}

define internal x86_stdcallcc void @glutInit_ATEXIT_HACK(i32* %argcp, i8** %argv) nounwind {
  %1 = alloca i32*, align 4
  %2 = alloca i8**, align 4
  store i32* %argcp, i32** %1, align 4
  store i8** %argv, i8*** %2, align 4
  %3 = load i32** %1, align 4
  %4 = load i8*** %2, align 4
  call x86_stdcallcc void @__glutInitWithExit(i32* %3, i8** %4, void (i32)* @exit)
  ret void
}

declare dllimport x86_stdcallcc void @glutInitDisplayMode(i32)

declare dllimport x86_stdcallcc void @glutInitWindowSize(i32, i32)

declare dllimport x86_stdcallcc void @glutInitWindowPosition(i32, i32)

define internal x86_stdcallcc i32 @glutCreateWindow_ATEXIT_HACK(i8* %title) nounwind {
  %1 = alloca i8*, align 4
  store i8* %title, i8** %1, align 4
  %2 = load i8** %1, align 4
  %3 = call x86_stdcallcc i32 @__glutCreateWindowWithExit(i8* %2, void (i32)* @exit)
  ret i32 %3
}

declare dllimport x86_stdcallcc void @glutDisplayFunc(void ()*)

declare dllimport x86_stdcallcc void @glutMainLoop()

declare dllimport x86_stdcallcc i32 @__glutCreateWindowWithExit(i8*, void (i32)*)

declare void @exit(i32) noreturn nounwind

declare dllimport x86_stdcallcc void @__glutInitWithExit(i32*, i8**, void (i32)*)
