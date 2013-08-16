; Generated 16/08/2013 23:34:05
; GlobalUID 52fab4f7-6557-4235-9188-8431e144ea5e
; 
; LLVM IR file for module "Link"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET = type i64

@.str0 = private constant [3 x i8] c"%i\00"
@.str1 = private constant [2 x i8] c"\0A\00"

declare i32 @printf(%CHAR*, ...) nounwind 
declare noalias i8* @GC_malloc(i32) 

@i = private global i32 zeroinitializer
@n = private global i32 zeroinitializer
@test = private global %List.List null

define i32 @Link._init() nounwind {
    
    ; n := 1
    store i32 1, i32* @n
    
    ; FOR i := 1 TO 10 DO
    store i32 1, i32* @i
    br label %1
    
; <label>:1                                       ; preds = %0, %18
    
    %2 = load i32* @i
    %3 = icmp sgt i32 %2, 10
    br i1 %3, label %23, label %4
    
; <label>:4                                       ; preds = %1
    
    ; n := (n * 8723 + 181) MOD 256
    %5 = load i32* @n
    %6 = mul i32 %5, 8723
    %7 = add i32 %6, 181
    %8 = srem i32 %7, 256
    store i32 %8, i32* @n
    
    ; test.Add(n)
    %9 = load %List.List* @test
    %10 = icmp eq %List.List %9, null
    br i1 %10, label %18, label %11
    
; <label>:11                                      ; preds = %4
    %12 = getelementptr inbounds %List.List %9, i32 0, i32 0
    %13 = load i8** %12
    %14 = bitcast i8* %13 to [4 x i8*]*
    %15 = getelementptr inbounds [4 x i8*]* %14, i32 0, i32 2
    %16 = load i8** %15
    %17 = bitcast i8* %16 to void (%List.List*, %List.Int32)*
    br label %18
    
; <label>:18                                      ; preds = %4, %11
    %19 = phi void (%List.List*, %List.Int32)* [@List.ListNode.Add, %4], [%17, %11]
    %20 = load i32* @n
    call void (%List.List*, %List.Int32)* %19(%List.List* @test, %List.Int32 %20) nounwind
    
    %21 = load i32* @i
    %22 = add i32 %21, 1
    store i32 %22, i32* @i
    br label %1
    
; <label>:23                                      ; preds = %1
    
    ; REPEAT UNTIL i = 0
    br label %24
    
; <label>:24                                      ; preds = %23, %44
    
    ; i := test.Get()
    %25 = load %List.List* @test
    %26 = icmp eq %List.List %25, null
    br i1 %26, label %34, label %27
    
; <label>:27                                      ; preds = %24
    %28 = getelementptr inbounds %List.List %25, i32 0, i32 0
    %29 = load i8** %28
    %30 = bitcast i8* %29 to [4 x i8*]*
    %31 = getelementptr inbounds [4 x i8*]* %30, i32 0, i32 3
    %32 = load i8** %31
    %33 = bitcast i8* %32 to %List.Int32 (%List.List*)*
    br label %34
    
; <label>:34                                      ; preds = %24, %27
    %35 = phi %List.Int32 (%List.List*)* [@List.ListNode.Get, %24], [%33, %27]
    %36 = call %List.Int32 (%List.List*)* %35(%List.List* @test) nounwind
    store i32 %36, i32* @i
    
    ; IF i > 0 THEN
    %37 = load i32* @i
    %38 = icmp sgt i32 %37, 0
    br i1 %38, label %39, label %44
    
; <label>:39                                      ; preds = %34
    
    ; Out.Integer(i)
    %40 = load i32* @i
    %41 = sext i32 %40 to i64
    %42 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str0, i32 0, i32 0), i64 %41) nounwind
    
    ; Out.Ln()
    %43 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str1, i32 0, i32 0)) nounwind
    
    br label %44
    
; <label>:44                                      ; preds = %34, %39
    
    %45 = load i32* @i
    %46 = icmp eq i32 %45, 0
    br i1 %46, label %47, label %24
    
; <label>:47                                      ; preds = %44
    
    ret i32 0
}

define i32 @main() nounwind {
    
    %1 = call i32 ()* @Link._init() nounwind
    ret i32 %1
}

