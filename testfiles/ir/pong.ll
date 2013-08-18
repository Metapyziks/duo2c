; Generated 18/08/2013 22:41:23
; GlobalUID 37d10257-0546-4ee8-a75b-517e960e1c94
; 
; LLVM IR file for module "Pong"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET = type i64

@.str0 = private constant [9 x i8] c"OBERpONg\00"

declare i32 @printf(%CHAR*, ...) nounwind 
declare noalias i8* @GC_malloc(i32) 

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

define void @VecToScreen(float* %x, float* %y) nounwind {
    
    ; x := x / (Width * 0x3fe0000000000000)
    %1 = load float* %x
    %2 = fmul float 8.000000e+002, 0x3fe0000000000000
    %3 = fdiv float %1, %2
    store float %3, float* %x
    
    ; y := y / (Height * 0x3fe0000000000000)
    %4 = load float* %y
    %5 = fsub float 0.000000e+000, %4
    %6 = fmul float 6.000000e+002, 0x3fe0000000000000
    %7 = fdiv float %5, %6
    store float %7, float* %y
    
    ret void 
}

define void @PosToScreen(float* %x, float* %y) nounwind {
    
    ; VecToScreen(x, y)
    call void (float*, float*)* @VecToScreen(float* %x, float* %y) nounwind
    
    ; x := x - 1
    %1 = load float* %x
    %2 = fsub float %1, 1.000000e+000
    store float %2, float* %x
    
    ; y := 1 + y
    %3 = load float* %y
    %4 = fadd float 1.000000e+000, %3
    store float %4, float* %y
    
    ret void 
}

define void @DrawRect(float %$x, float %$y, float %$w, float %$h) nounwind {
    
    %x = alloca float
    store float %$x, float* %x
    
    %y = alloca float
    store float %$y, float* %y
    
    %w = alloca float
    store float %$w, float* %w
    
    %h = alloca float
    store float %$h, float* %h
    
    ; PosToScreen(x, y)
    call void (float*, float*)* @PosToScreen(float* %x, float* %y) nounwind
    
    ; VecToScreen(w, h)
    call void (float*, float*)* @VecToScreen(float* %w, float* %h) nounwind
    
    ; GL.Begin(GL.Quads)
    call x86_stdcallcc void (i32)* @glBegin(i32 7)
    
    ; GL.Color3f(0x3ff0000000000000, 0x3ff0000000000000, 0x3ff0000000000000)
    call x86_stdcallcc void (float, float, float)* @glColor3f(float 0x3ff0000000000000, float 0x3ff0000000000000, float 0x3ff0000000000000)
    
    ; GL.Vertex2f(x, y)
    %1 = load float* %x
    %2 = load float* %y
    call x86_stdcallcc void (float, float)* @glVertex2f(float %1, float %2)
    
    ; GL.Vertex2f(x + w, y)
    %3 = load float* %x
    %4 = load float* %w
    %5 = fadd float %3, %4
    %6 = load float* %y
    call x86_stdcallcc void (float, float)* @glVertex2f(float %5, float %6)
    
    ; GL.Vertex2f(x + w, y + h)
    %7 = load float* %x
    %8 = load float* %w
    %9 = fadd float %7, %8
    %10 = load float* %y
    %11 = load float* %h
    %12 = fadd float %10, %11
    call x86_stdcallcc void (float, float)* @glVertex2f(float %9, float %12)
    
    ; GL.Vertex2f(x, y + h)
    %13 = load float* %x
    %14 = load float* %y
    %15 = load float* %h
    %16 = fadd float %14, %15
    call x86_stdcallcc void (float, float)* @glVertex2f(float %13, float %16)
    
    ; GL.End()
    call x86_stdcallcc void ()* @glEnd()
    
    ret void 
}

define void @IdleHandler() nounwind {
    
    ; RETURN
    ret void
    
}

define void @DisplayHandler() nounwind {
    
    ; GL.Clear(GL.ColorBufferBit)
    call x86_stdcallcc void (i32)* @glClear(i32 16384)
    
    ; DrawRect(32, 32, 128, 128)
    call void (float, float, float, float)* @DrawRect(float 3.200000e+001, float 3.200000e+001, float 1.280000e+002, float 1.280000e+002) nounwind
    
    ; GLUT.SwapBuffers()
    call x86_stdcallcc void ()* @glutSwapBuffers()
    
    ret void 
}


@Pong._hasInit = private global i1 zeroinitializer

define i32 @Pong._init() nounwind {
    
    %1 = load i1* @Pong._hasInit
    br i1 %1, label %11, label %2
    
; <label>:2                                       ; preds = %0
    store i1 1, i1* @Pong._hasInit
    
    %3 = call i32 ()* @GL._init()
    %4 = icmp eq i32 %3, 0
    br i1 %4, label %5, label %12
    
; <label>:5                                       ; preds = %2
    %6 = call i32 ()* @GLUT._init()
    %7 = icmp eq i32 %6, 0
    br i1 %7, label %8, label %12
    
; <label>:8                                       ; preds = %5
    
    ; GLUT.Init()
    call void ()* @GLUT.Init() nounwind
    
    ; GLUT.InitDisplayMode(GLUT.RGB OR GLUT.Double)
    %9 = or i8 0, 2
    %10 = sext i8 %9 to i32
    call x86_stdcallcc void (i32)* @glutInitDisplayMode(i32 %10)
    
    ; GLUT.InitWindowSize(Width, Height)
    call x86_stdcallcc void (i32, i32)* @glutInitWindowSize(i32 800, i32 600)
    
    ; GLUT.InitWindowPosition(300, 200)
    call x86_stdcallcc void (i32, i32)* @glutInitWindowPosition(i32 300, i32 200)
    
    ; GLUT.CreateWindow("OBERpONg")
    call void ({i32, %CHAR*})* @GLUT.CreateWindow({i32, %CHAR*} {i32 9, %CHAR* getelementptr inbounds ([9 x %CHAR]* @.str0, i32 0, i32 0)}) nounwind
    
    ; GLUT.IdleFunc(IdleHandler)
    call x86_stdcallcc void (void ()*)* @glutIdleFunc(void ()* @IdleHandler)
    
    ; GLUT.DisplayFunc(DisplayHandler)
    call x86_stdcallcc void (void ()*)* @glutDisplayFunc(void ()* @DisplayHandler)
    
    ; GLUT.MainLoop()
    call x86_stdcallcc void ()* @glutMainLoop()
    
    br label %11
    
; <label>:11                                      ; preds = %0, %8
    ret i32 0
    
; <label>:12                                      ; preds = %2, %5
    ret i32 1
}

define i32 @main() nounwind {
    
    %1 = call i32 ()* @Pong._init()
    ret i32 %1
}

