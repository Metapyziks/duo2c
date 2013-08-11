; Generated 11/08/2013 03:00:25
; GlobalUID 4787c729-97e2-4278-a439-627cf6f4d986
; 
; LLVM IR file for module "List"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET = type i64
%List.List = type %List.ListNode*
%List.ListNode = type {i8*, i32, %List.List}

@.rec0 = global [4 x i8*] [
    i8* getelementptr inbounds ([9 x %CHAR]* @.str0, i32 0, i32 0),
    i8* null,
    i8* bitcast (void (%List.List*, i32)* @List.ListNode.Add to i8*),
    i8* bitcast (i32 (%List.List*)* @List.ListNode.Get to i8*)
]

@.str0 = private constant [9 x i8] c"ListNode\00"
@.str1 = private constant [3 x i8] c"%i\00"
@.str2 = private constant [2 x i8] c"\0A\00"

declare i32 @printf(%CHAR*, ...) nounwind 
declare noalias i8* @GC_malloc(i32) 

@test = private global %List.List null
@i = private global i32 zeroinitializer
@n = private global i32 zeroinitializer

define void @List.ListNode.Add(%List.List* %l, i32 %$v) nounwind {
    
    %v = alloca i32
    store i32 %$v, i32* %v
    
    ; IF l^.next = NIL THEN
    %1 = load %List.List* %l
    %2 = getelementptr inbounds %List.ListNode* %1, i32 0, i32 2
    %3 = load %List.List* %2
    %4 = icmp eq %List.List %3, null
    br i1 %4, label %5, label %15
    
; <label>:5                                       ; preds = %0
    
    ; NEW(l^.next)
    %6 = load %List.List* %l
    %7 = getelementptr inbounds %List.ListNode* %6, i32 0, i32 2
    %8 = getelementptr inbounds %List.ListNode* null, i32 1
    %9 = ptrtoint %List.ListNode* %8 to i32
    %10 = call i8* (i32)* @GC_malloc(i32 %9) nounwind
    %11 = bitcast i8* %10 to %List.ListNode*
    store %List.ListNode {i8* bitcast ([4 x i8*]* @.rec0 to i8*), i32 zeroinitializer, %List.List null}, %List.ListNode* %11
    store %List.ListNode* %11, %List.ListNode** %7
    
    ; l^.value := v
    %12 = load %List.List* %l
    %13 = load i32* %v
    %14 = getelementptr inbounds %List.ListNode* %12, i32 0, i32 1
    store i32 %13, i32* %14
    
    br label %31
    
; <label>:15                                      ; preds = %0
    
    ; l^.next.Add(v)
    %16 = load %List.List* %l
    %17 = getelementptr inbounds %List.ListNode* %16, i32 0, i32 2
    %18 = load %List.List* %17
    %19 = icmp eq %List.List %18, null
    br i1 %19, label %27, label %20
    
; <label>:20                                      ; preds = %15
    %21 = getelementptr inbounds %List.List %18, i32 0, i32 0
    %22 = load i8** %21
    %23 = bitcast i8* %22 to [4 x i8*]*
    %24 = getelementptr inbounds [4 x i8*]* %23, i32 0, i32 2
    %25 = load i8** %24
    %26 = bitcast i8* %25 to void (%List.List*, i32)*
    br label %27
    
; <label>:27                                      ; preds = %15, %20
    %28 = select i1 %19, void (%List.List*, i32)* @List.ListNode.Add, void (%List.List*, i32)* %26
    %29 = load i32* %v
    %30 = getelementptr inbounds %List.ListNode* %16, i32 0, i32 2
    call void (%List.List*, i32)* %28(%List.List* %30, i32 %29) nounwind
    
    br label %31
    
; <label>:31                                      ; preds = %5, %27
    
    ret void 
}

define i32 @List.ListNode.Get(%List.List* %l) nounwind {
    
    %v = alloca i32
    
    ; IF l^.next = NIL THEN
    %1 = load %List.List* %l
    %2 = getelementptr inbounds %List.ListNode* %1, i32 0, i32 2
    %3 = load %List.List* %2
    %4 = icmp eq %List.List %3, null
    br i1 %4, label %5, label %6
    
; <label>:5                                       ; preds = %0
    
    ; RETURN 0
    ret i32 0
; <label>:6                                       ; preds = %0
    
    ; v := l^.value
    %7 = load %List.List* %l
    %8 = getelementptr inbounds %List.ListNode* %7, i32 0, i32 1
    %9 = load i32* %8
    store i32 %9, i32* %v
    
    ; l := l^.next
    %10 = load %List.List* %l
    %11 = getelementptr inbounds %List.ListNode* %10, i32 0, i32 2
    %12 = load %List.List* %11
    store %List.List %12, %List.List* %l
    
    ; RETURN v
    %13 = load i32* %v
    ret i32 %13
; <label>:14                                      ; preds = %0
    
}

define i32 @main() nounwind {
    
    ; NEW(test)
    %1 = getelementptr inbounds %List.ListNode* null, i32 1
    %2 = ptrtoint %List.ListNode* %1 to i32
    %3 = call i8* (i32)* @GC_malloc(i32 %2) nounwind
    %4 = bitcast i8* %3 to %List.ListNode*
    store %List.ListNode {i8* bitcast ([4 x i8*]* @.rec0 to i8*), i32 zeroinitializer, %List.List null}, %List.ListNode* %4
    store %List.ListNode* %4, %List.ListNode** @test
    
    ; n := 1
    store i32 1, i32* @n
    
    ; FOR i := 1 TO 10 DO
    store i32 1, i32* @i
    br label %5
    
; <label>:5                                       ; preds = %0, %22
    
    %6 = load i32* @i
    %7 = icmp sgt i32 %6, 10
    br i1 %7, label %27, label %8
    
; <label>:8                                       ; preds = %5
    
    ; n := (n * 8723 + 181) MOD 256
    %9 = load i32* @n
    %10 = mul i32 %9, 8723
    %11 = add i32 %10, 181
    %12 = srem i32 %11, 256
    store i32 %12, i32* @n
    
    ; test.Add(n)
    %13 = load %List.List* @test
    %14 = icmp eq %List.List %13, null
    br i1 %14, label %22, label %15
    
; <label>:15                                      ; preds = %8
    %16 = getelementptr inbounds %List.List %13, i32 0, i32 0
    %17 = load i8** %16
    %18 = bitcast i8* %17 to [4 x i8*]*
    %19 = getelementptr inbounds [4 x i8*]* %18, i32 0, i32 2
    %20 = load i8** %19
    %21 = bitcast i8* %20 to void (%List.List*, i32)*
    br label %22
    
; <label>:22                                      ; preds = %8, %15
    %23 = select i1 %14, void (%List.List*, i32)* @List.ListNode.Add, void (%List.List*, i32)* %21
    %24 = load i32* @n
    call void (%List.List*, i32)* %23(%List.List* @test, i32 %24) nounwind
    
    %25 = load i32* @i
    %26 = add i32 %25, 1
    store i32 %26, i32* @i
    br label %5
    
; <label>:27                                      ; preds = %5
    
    ; REPEAT UNTIL i = 0
    br label %28
    
; <label>:28                                      ; preds = %27, %48
    
    ; i := test.Get()
    %29 = load %List.List* @test
    %30 = icmp eq %List.List %29, null
    br i1 %30, label %38, label %31
    
; <label>:31                                      ; preds = %28
    %32 = getelementptr inbounds %List.List %29, i32 0, i32 0
    %33 = load i8** %32
    %34 = bitcast i8* %33 to [4 x i8*]*
    %35 = getelementptr inbounds [4 x i8*]* %34, i32 0, i32 3
    %36 = load i8** %35
    %37 = bitcast i8* %36 to i32 (%List.List*)*
    br label %38
    
; <label>:38                                      ; preds = %28, %31
    %39 = select i1 %30, i32 (%List.List*)* @List.ListNode.Get, i32 (%List.List*)* %37
    %40 = call i32 (%List.List*)* %39(%List.List* @test) nounwind
    store i32 %40, i32* @i
    
    ; IF i > 0 THEN
    %41 = load i32* @i
    %42 = icmp sgt i32 %41, 0
    br i1 %42, label %43, label %48
    
; <label>:43                                      ; preds = %38
    
    ; Out.Integer(i)
    %44 = load i32* @i
    %45 = sext i32 %44 to i64
    %46 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), i64 %45) nounwind
    
    ; Out.Ln()
    %47 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str2, i32 0, i32 0)) nounwind
    
    br label %48
    
; <label>:48                                      ; preds = %38, %43
    
    %49 = load i32* @i
    %50 = icmp eq i32 %49, 0
    br i1 %50, label %51, label %28
    
; <label>:51                                      ; preds = %48
    
    ret i32 0
}

