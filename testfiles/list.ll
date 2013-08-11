; Generated 11/08/2013 13:11:39
; GlobalUID 2a9adbeb-257f-4feb-b7ec-c811340c3773
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
    
    ; IF l = NIL THEN
    %1 = load %List.List* %l
    %2 = icmp eq %List.List %1, null
    br i1 %2, label %3, label %11
    
; <label>:3                                       ; preds = %0
    
    ; NEW(l)
    %4 = getelementptr inbounds %List.ListNode* null, i32 1
    %5 = ptrtoint %List.ListNode* %4 to i32
    %6 = call i8* (i32)* @GC_malloc(i32 %5) nounwind
    %7 = bitcast i8* %6 to %List.ListNode*
    store %List.ListNode {i8* bitcast ([4 x i8*]* @.rec0 to i8*), i32 zeroinitializer, %List.List null}, %List.ListNode* %7
    store %List.ListNode* %7, %List.ListNode** %l
    
    ; l^.value := v
    %8 = load %List.List* %l
    %9 = load i32* %v
    %10 = getelementptr inbounds %List.ListNode* %8, i32 0, i32 1
    store i32 %9, i32* %10
    
    br label %27
    
; <label>:11                                      ; preds = %0
    
    ; l^.next.Add(v)
    %12 = load %List.List* %l
    %13 = getelementptr inbounds %List.ListNode* %12, i32 0, i32 2
    %14 = load %List.List* %13
    %15 = icmp eq %List.List %14, null
    br i1 %15, label %23, label %16
    
; <label>:16                                      ; preds = %11
    %17 = getelementptr inbounds %List.List %14, i32 0, i32 0
    %18 = load i8** %17
    %19 = bitcast i8* %18 to [4 x i8*]*
    %20 = getelementptr inbounds [4 x i8*]* %19, i32 0, i32 2
    %21 = load i8** %20
    %22 = bitcast i8* %21 to void (%List.List*, i32)*
    br label %23
    
; <label>:23                                      ; preds = %11, %16
    %24 = select i1 %15, void (%List.List*, i32)* @List.ListNode.Add, void (%List.List*, i32)* %22
    %25 = load i32* %v
    %26 = getelementptr inbounds %List.ListNode* %12, i32 0, i32 2
    call void (%List.List*, i32)* %24(%List.List* %26, i32 %25) nounwind
    
    br label %27
    
; <label>:27                                      ; preds = %3, %23
    
    ret void 
}

define i32 @List.ListNode.Get(%List.List* %l) nounwind {
    
    %v = alloca i32
    
    ; IF l = NIL THEN
    %1 = load %List.List* %l
    %2 = icmp eq %List.List %1, null
    br i1 %2, label %3, label %4
    
; <label>:3                                       ; preds = %0
    
    ; RETURN 0
    ret i32 0
; <label>:4                                       ; preds = %0
    
    ; v := l^.value
    %5 = load %List.List* %l
    %6 = getelementptr inbounds %List.ListNode* %5, i32 0, i32 1
    %7 = load i32* %6
    store i32 %7, i32* %v
    
    ; l := l^.next
    %8 = load %List.List* %l
    %9 = getelementptr inbounds %List.ListNode* %8, i32 0, i32 2
    %10 = load %List.List* %9
    store %List.List %10, %List.List* %l
    
    ; RETURN v
    %11 = load i32* %v
    ret i32 %11
; <label>:12                                      ; preds = %0
    
}

define i32 @main() nounwind {
    
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
    %17 = bitcast i8* %16 to void (%List.List*, i32)*
    br label %18
    
; <label>:18                                      ; preds = %4, %11
    %19 = select i1 %10, void (%List.List*, i32)* @List.ListNode.Add, void (%List.List*, i32)* %17
    %20 = load i32* @n
    call void (%List.List*, i32)* %19(%List.List* @test, i32 %20) nounwind
    
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
    %33 = bitcast i8* %32 to i32 (%List.List*)*
    br label %34
    
; <label>:34                                      ; preds = %24, %27
    %35 = select i1 %26, i32 (%List.List*)* @List.ListNode.Get, i32 (%List.List*)* %33
    %36 = call i32 (%List.List*)* %35(%List.List* @test) nounwind
    store i32 %36, i32* @i
    
    ; IF i > 0 THEN
    %37 = load i32* @i
    %38 = icmp sgt i32 %37, 0
    br i1 %38, label %39, label %44
    
; <label>:39                                      ; preds = %34
    
    ; Out.Integer(i)
    %40 = load i32* @i
    %41 = sext i32 %40 to i64
    %42 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), i64 %41) nounwind
    
    ; Out.Ln()
    %43 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str2, i32 0, i32 0)) nounwind
    
    br label %44
    
; <label>:44                                      ; preds = %34, %39
    
    %45 = load i32* @i
    %46 = icmp eq i32 %45, 0
    br i1 %46, label %47, label %24
    
; <label>:47                                      ; preds = %44
    
    ret i32 0
}

