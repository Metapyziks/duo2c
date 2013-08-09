; ModuleID = 'testfiles/reference.c'
target datalayout = "e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-f80:128:128-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32-S32"
target triple = "i686-w64-mingw32"

@.str = private unnamed_addr constant [8 x i8] c"*p == 0\00", align 1
@.str1 = private unnamed_addr constant [22 x i8] c"testfiles/reference.c\00", align 1
@.str2 = private unnamed_addr constant [16 x i8] c"Heap size = %i\0A\00", align 1

define i32 @main() nounwind {
  %1 = alloca i32, align 4
  %i = alloca i32, align 4
  %p = alloca i32**, align 4
  %q = alloca i32*, align 4
  store i32 0, i32* %1
  call void @GC_init()
  store i32 0, i32* %i, align 4
  br label %2

; <label>:2                                       ; preds = %29, %0
  %3 = load i32* %i, align 4
  %4 = icmp slt i32 %3, 10000000
  br i1 %4, label %5, label %32

; <label>:5                                       ; preds = %2
  %6 = call noalias i8* @GC_malloc(i32 4)
  %7 = bitcast i8* %6 to i32**
  store i32** %7, i32*** %p, align 4
  %8 = call noalias i8* @GC_malloc_atomic(i32 4)
  %9 = bitcast i8* %8 to i32*
  store i32* %9, i32** %q, align 4
  %10 = load i32*** %p, align 4
  %11 = load i32** %10, align 4
  %12 = icmp eq i32* %11, null
  br i1 %12, label %13, label %14

; <label>:13                                      ; preds = %5
  br label %16

; <label>:14                                      ; preds = %5
  call void @_assert(i8* getelementptr inbounds ([8 x i8]* @.str, i32 0, i32 0), i8* getelementptr inbounds ([22 x i8]* @.str1, i32 0, i32 0), i32 16) noreturn nounwind
  unreachable
                                                  ; No predecessors!
  br label %16

; <label>:16                                      ; preds = %15, %13
  %17 = load i32** %q, align 4
  %18 = bitcast i32* %17 to i8*
  %19 = call i8* @GC_realloc(i8* %18, i32 8)
  %20 = bitcast i8* %19 to i32*
  %21 = load i32*** %p, align 4
  store i32* %20, i32** %21, align 4
  %22 = load i32* %i, align 4
  %23 = srem i32 %22, 100000
  %24 = icmp eq i32 %23, 0
  br i1 %24, label %25, label %28

; <label>:25                                      ; preds = %16
  %26 = call i32 @GC_get_heap_size()
  %27 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([16 x i8]* @.str2, i32 0, i32 0), i32 %26) nounwind
  br label %28

; <label>:28                                      ; preds = %25, %16
  br label %29

; <label>:29                                      ; preds = %28
  %30 = load i32* %i, align 4
  %31 = add nsw i32 %30, 1
  store i32 %31, i32* %i, align 4
  br label %2

; <label>:32                                      ; preds = %2
  ret i32 0
}

declare void @GC_init()

declare noalias i8* @GC_malloc(i32)

declare noalias i8* @GC_malloc_atomic(i32)

declare void @_assert(i8*, i8*, i32) noreturn nounwind

declare i8* @GC_realloc(i8*, i32)

declare i32 @printf(i8*, ...) nounwind

declare i32 @GC_get_heap_size()
