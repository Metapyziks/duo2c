; Generated 11/08/2013 02:55:55
; GlobalUID de5d087f-2336-42df-9295-355143fef255
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
    
    ; FOR i := 1 TO 10 DO
    store i32 1, i32* @i
    br label %5
    
; <label>:5                                       ; preds = %0, %18
    
    %6 = load i32* @i
    %7 = icmp sgt i32 %6, 10
    br i1 %7, label %23, label %8
    
; <label>:8                                       ; preds = %5
    
    ; test.Add(i)
    %9 = load %List.List* @test
    %10 = icmp eq %List.List %9, null
    br i1 %10, label %18, label %11
    
; <label>:11                                      ; preds = %8
    %12 = getelementptr inbounds %List.List %9, i32 0, i32 0
    %13 = load i8** %12
    %14 = bitcast i8* %13 to [4 x i8*]*
    %15 = getelementptr inbounds [4 x i8*]* %14, i32 0, i32 2
    %16 = load i8** %15
    %17 = bitcast i8* %16 to void (%List.List*, i32)*
    br label %18
    
; <label>:18                                      ; preds = %8, %11
    %19 = select i1 %10, void (%List.List*, i32)* @List.ListNode.Add, void (%List.List*, i32)* %17
    %20 = load i32* @i
    call void (%List.List*, i32)* %19(%List.List* @test, i32 %20) nounwind
    
    %21 = load i32* @i
    %22 = add i32 %21, 1
    store i32 %22, i32* @i
    br label %5
    
; <label>:23                                      ; preds = %5
    
    ; REPEAT UNTIL i = 0
    br label %24
    
; <label>:24                                      ; preds = %23, %34
    
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
    
    ; Out.Integer(i)
    %37 = load i32* @i
    %38 = sext i32 %37 to i64
    %39 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), i64 %38) nounwind
    
    ; Out.Ln()
    %40 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str2, i32 0, i32 0)) nounwind
    
    %41 = load i32* @i
    %42 = icmp eq i32 %41, 0
    br i1 %42, label %43, label %24
    
; <label>:43                                      ; preds = %34
    
    ret i32 0
}

