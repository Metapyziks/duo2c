; ModuleID = 'testfiles/reference.c'
target datalayout = "e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-f80:128:128-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32-S32"
target triple = "i686-w64-mingw32"

%struct.HINSTANCE__ = type { i32 }

@sdl_initialized = global i32 0, align 4
@.str = private unnamed_addr constant [3 x i8] c"%i\00", align 1
@SDL_Quit = common global void ()* null, align 4
@sdl = common global %struct.HINSTANCE__* null, align 4
@.str1 = private unnamed_addr constant [9 x i8] c"SDL2.dll\00", align 1
@.str2 = private unnamed_addr constant [25 x i8] c"Could not load library!\0A\00", align 1
@.str3 = private unnamed_addr constant [9 x i8] c"SDL_Init\00", align 1
@.str4 = private unnamed_addr constant [13 x i8] c"SDL_GetError\00", align 1
@.str5 = private unnamed_addr constant [9 x i8] c"SDL_Quit\00", align 1
@.str6 = private unnamed_addr constant [27 x i8] c"Could not find procedure!\0A\00", align 1
@SDL_Init = common global i32 ()* null, align 4
@SDL_GetError = common global i8* ()* null, align 4
@.str7 = private unnamed_addr constant [24 x i8] c"Unable to init SDL: %s\0A\00", align 1
@.str8 = private unnamed_addr constant [16 x i8] c"Great success!\0A\00", align 1

define i32 @test(i32 %arg) nounwind {
  %1 = alloca i32, align 4
  %i = alloca i32, align 4
  store i32 %arg, i32* %1, align 4
  store i32 1, i32* %i, align 4
  br label %2

; <label>:2                                       ; preds = %9, %0
  %3 = load i32* %i, align 4
  %4 = load i32* %1, align 4
  %5 = icmp slt i32 %3, %4
  br i1 %5, label %6, label %12

; <label>:6                                       ; preds = %2
  %7 = load i32* %i, align 4
  %8 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str, i32 0, i32 0), i32 %7) nounwind
  br label %9

; <label>:9                                       ; preds = %6
  %10 = load i32* %i, align 4
  %11 = add nsw i32 %10, 1
  store i32 %11, i32* %i, align 4
  br label %2

; <label>:12                                      ; preds = %2
  ret i32 1
}

declare i32 @printf(i8*, ...) nounwind

define void @cleanup() nounwind {
  %1 = load i32* @sdl_initialized, align 4
  %2 = icmp ne i32 %1, 0
  br i1 %2, label %3, label %5

; <label>:3                                       ; preds = %0
  %4 = load void ()** @SDL_Quit, align 4
  call void %4()
  br label %5

; <label>:5                                       ; preds = %3, %0
  %6 = load %struct.HINSTANCE__** @sdl, align 4
  %7 = icmp ne %struct.HINSTANCE__* %6, null
  br i1 %7, label %8, label %11

; <label>:8                                       ; preds = %5
  %9 = load %struct.HINSTANCE__** @sdl, align 4
  %10 = call x86_stdcallcc i32 @FreeLibrary(%struct.HINSTANCE__* %9)
  br label %11

; <label>:11                                      ; preds = %8, %5
  ret void
}

declare x86_stdcallcc i32 @FreeLibrary(%struct.HINSTANCE__*)

define i32 @main() nounwind {
  %1 = alloca i32, align 4
  %sdl = alloca %struct.HINSTANCE__*, align 4
  %init = alloca i32 (...)*, align 4
  %getError = alloca i32 (...)*, align 4
  %quit = alloca i32 (...)*, align 4
  store i32 0, i32* %1
  %2 = call x86_stdcallcc %struct.HINSTANCE__* @LoadLibraryA(i8* getelementptr inbounds ([9 x i8]* @.str1, i32 0, i32 0))
  store %struct.HINSTANCE__* %2, %struct.HINSTANCE__** %sdl, align 4
  %3 = load %struct.HINSTANCE__** %sdl, align 4
  %4 = icmp eq %struct.HINSTANCE__* %3, null
  br i1 %4, label %5, label %7

; <label>:5                                       ; preds = %0
  %6 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([25 x i8]* @.str2, i32 0, i32 0)) nounwind
  call void @exit(i32 1) noreturn nounwind
  unreachable

; <label>:7                                       ; preds = %0
  %8 = load %struct.HINSTANCE__** %sdl, align 4
  %9 = call x86_stdcallcc i32 (...)* (%struct.HINSTANCE__*, i8*)* @GetProcAddress(%struct.HINSTANCE__* %8, i8* getelementptr inbounds ([9 x i8]* @.str3, i32 0, i32 0))
  store i32 (...)* %9, i32 (...)** %init, align 4
  %10 = load %struct.HINSTANCE__** %sdl, align 4
  %11 = call x86_stdcallcc i32 (...)* (%struct.HINSTANCE__*, i8*)* @GetProcAddress(%struct.HINSTANCE__* %10, i8* getelementptr inbounds ([13 x i8]* @.str4, i32 0, i32 0))
  store i32 (...)* %11, i32 (...)** %getError, align 4
  %12 = load %struct.HINSTANCE__** %sdl, align 4
  %13 = call x86_stdcallcc i32 (...)* (%struct.HINSTANCE__*, i8*)* @GetProcAddress(%struct.HINSTANCE__* %12, i8* getelementptr inbounds ([9 x i8]* @.str5, i32 0, i32 0))
  store i32 (...)* %13, i32 (...)** %quit, align 4
  %14 = load i32 (...)** %init, align 4
  %15 = icmp eq i32 (...)* %14, null
  br i1 %15, label %22, label %16

; <label>:16                                      ; preds = %7
  %17 = load i32 (...)** %getError, align 4
  %18 = icmp eq i32 (...)* %17, null
  br i1 %18, label %22, label %19

; <label>:19                                      ; preds = %16
  %20 = load i32 (...)** %quit, align 4
  %21 = icmp eq i32 (...)* %20, null
  br i1 %21, label %22, label %24

; <label>:22                                      ; preds = %19, %16, %7
  %23 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([27 x i8]* @.str6, i32 0, i32 0)) nounwind
  call void @exit(i32 1) noreturn nounwind
  unreachable

; <label>:24                                      ; preds = %19
  %25 = load i32 (...)** %init, align 4
  %26 = bitcast i32 (...)* %25 to i32 ()*
  store i32 ()* %26, i32 ()** @SDL_Init, align 4
  %27 = load i32 (...)** %getError, align 4
  %28 = bitcast i32 (...)* %27 to i8* ()*
  store i8* ()* %28, i8* ()** @SDL_GetError, align 4
  %29 = load i32 (...)** %quit, align 4
  %30 = bitcast i32 (...)* %29 to void ()*
  store void ()* %30, void ()** @SDL_Quit, align 4
  %31 = load i32 ()** @SDL_Init, align 4
  %32 = call i32 %31()
  %33 = icmp slt i32 %32, 0
  br i1 %33, label %34, label %38

; <label>:34                                      ; preds = %24
  %35 = load i8* ()** @SDL_GetError, align 4
  %36 = call i8* %35()
  %37 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([24 x i8]* @.str7, i32 0, i32 0), i8* %36) nounwind
  call void @exit(i32 1) noreturn nounwind
  unreachable

; <label>:38                                      ; preds = %24
  store i32 1, i32* @sdl_initialized, align 4
  %39 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([16 x i8]* @.str8, i32 0, i32 0)) nounwind
  call void @exit(i32 0) noreturn nounwind
  unreachable
                                                  ; No predecessors!
  %41 = load i32* %1
  ret i32 %41
}

declare x86_stdcallcc %struct.HINSTANCE__* @LoadLibraryA(i8*)

declare void @exit(i32) noreturn nounwind

declare x86_stdcallcc i32 (...)* @GetProcAddress(%struct.HINSTANCE__*, i8*)
