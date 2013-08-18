; Generated 18/08/2013 02:23:10
; GlobalUID ca68350a-451e-4c70-9375-22b88cecbf8a
; 
; LLVM IR file for module "GLUT"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET = type i64

@.str0 = private constant [12 x i8] c"GLUT Error \00"
@.str1 = private constant [3 x i8] c"%s\00"
@.str2 = private constant [3 x i8] c"%i\00"
@.str3 = private constant [2 x i8] c"\0A\00"

declare i32 @printf(%CHAR*, ...) nounwind 
declare noalias i8* @GC_malloc(i32) 

declare void @Out.Ln() nounwind 
declare void @Out.String({i32, %CHAR*}) nounwind 
declare void @Out.Integer(i64) nounwind 
declare void @Out.Real(double) nounwind 
declare void @Out.Boolean(i1) nounwind 
declare i32 @Out._init() 

%GLUT.ExitType = type void (i32)

define void @Exit(i32 %$errorCode) nounwind {
    
    %errorCode = alloca i32
    store i32 %$errorCode, i32* %errorCode
    
    ; Out.String("GLUT Error ")
    %1 = getelementptr inbounds [12 x %CHAR]* @.str0, i32 0, i32 0
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), %CHAR* %1) nounwind
    
    ; Out.Integer(errorCode)
    %3 = load i32* %errorCode
    %4 = sext i32 %3 to i64
    %5 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str2, i32 0, i32 0), i64 %4) nounwind
    
    ; Out.Ln()
    %6 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str3, i32 0, i32 0)) nounwind
    
    ret void 
}

declare x86_stdcallcc void @gluLookAt(double, double, double, double, double, double, double, double, double) 
declare dllimport x86_stdcallcc void @glutStrokeCharacter(i8*, %CHAR) 
declare dllimport x86_stdcallcc void @glutSwapBuffers() 
declare dllimport x86_stdcallcc void @__glutInitWithExit(i32*, %CHAR**, %GLUT.ExitType*) 
define void @GLUT.Init() nounwind {
    
    %argc = alloca i32
    %argv = alloca %CHAR**
    
    ; argc := 0
    store i32 0, i32* %argc
    
    ; NEW(argv)
    %1 = getelementptr inbounds %CHAR** null, i32 1
    %2 = ptrtoint %CHAR** %1 to i32
    %3 = call i8* (i32)* @GC_malloc(i32 %2)
    %4 = bitcast i8* %3 to %CHAR**
    store %CHAR* null, %CHAR** %4
    store %CHAR** %4, %CHAR*** %argv
    
    ; InitInternal(argc, argv, Exit)
    %5 = load %CHAR*** %argv
    call x86_stdcallcc void (i32*, %CHAR**, %GLUT.ExitType*)* @__glutInitWithExit(i32* %argc, %CHAR** %5, %GLUT.ExitType* @Exit)
    
    ret void 
}

declare dllimport x86_stdcallcc void @glutInitDisplayMode(i32) 
declare dllimport x86_stdcallcc void @glutInitWindowSize(i32, i32) 
declare dllimport x86_stdcallcc void @glutInitWindowPosition(i32, i32) 
declare dllimport x86_stdcallcc void @__glutCreateWindowWithExit(%CHAR*, %GLUT.ExitType*) 
define void @GLUT.CreateWindow({i32, %CHAR*} %$title) nounwind {
    
    %title = alloca {i32, %CHAR*}
    store {i32, %CHAR*} %$title, {i32, %CHAR*}* %title
    
    ; CreateWindowInternal(title, Exit)
    %1 = getelementptr inbounds {i32, %CHAR*}* %title, i32 0, i32 1
    %2 = load %CHAR** %1
    call x86_stdcallcc void (%CHAR*, %GLUT.ExitType*)* @__glutCreateWindowWithExit(%CHAR* %2, %GLUT.ExitType* @Exit)
    
    ret void 
}

declare dllimport x86_stdcallcc void @glutDisplayFunc(void ()*) 
declare dllimport x86_stdcallcc void @glutMainLoop() 

@GLUT._hasInit = private global i1 zeroinitializer

define i32 @GLUT._init() nounwind {
    
    ret i32 0
}

