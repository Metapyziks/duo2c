; Generated 11/08/2013 14:47:28
; GlobalUID 6ec519b6-a7d2-4c04-abd4-543f205ef91c
; 
; LLVM IR file for module "Lists"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET = type i64
%Lists.Int32 = type i32
%Lists.List = type %Lists.ListNode*
%Lists.ListNode = type {i8*, %Lists.Int32, %Lists.List}

@.rec0 = global [4 x i8*] [
    i8* getelementptr inbounds ([9 x %CHAR]* @.str0, i32 0, i32 0),
    i8* null,
    i8* bitcast (void (%Lists.List*, %Lists.Int32)* @Lists.ListNode.Add to i8*),
    i8* bitcast (%Lists.Int32 (%Lists.List*)* @Lists.ListNode.Get to i8*)
]

@.str0 = private constant [9 x i8] c"ListNode\00"
@.str1 = private constant [3 x i8] c"%i\00"
@.str2 = private constant [2 x i8] c"\0A\00"

declare i32 @printf(%CHAR*, ...) nounwind 
declare noalias i8* @GC_malloc(i32) 

@i = private global i32 zeroinitializer
@n = private global i32 zeroinitializer
@test = private global %Lists.List null

define void @Lists.ListNode.Add(%Lists.List* %l, %Lists.Int32 %$v) nounwind {
    
    %v = alloca %Lists.Int32
    store %Lists.Int32 %$v, %Lists.Int32* %v
    
    ; IF l = NIL THEN
    %1 = load %Lists.List* %l
    %2 = icmp eq %Lists.List %1, null
    br i1 %2, label %3, label %11
    
; <label>:3                                       ; preds = %0
    
    ; NEW(l)
    %4 = getelementptr inbounds %Lists.ListNode* null, i32 1
    %5 = ptrtoint %Lists.ListNode* %4 to i32
    %6 = call i8* (i32)* @GC_malloc(i32 %5) nounwind
    %7 = bitcast i8* %6 to %Lists.ListNode*
    store %Lists.ListNode {i8* bitcast ([4 x i8*]* @.rec0 to i8*), %Lists.Int32 zeroinitializer, %Lists.List null}, %Lists.ListNode* %7
    store %Lists.ListNode* %7, %Lists.ListNode** %l
    
    ; l^.value := v
    %8 = load %Lists.List* %l
    %9 = load %Lists.Int32* %v
    %10 = getelementptr inbounds %Lists.ListNode* %8, i32 0, i32 1
    store %Lists.Int32 %9, %Lists.Int32* %10
    
    br label %27
    
; <label>:11                                      ; preds = %0
    
    ; l^.next.Add(v)
    %12 = load %Lists.List* %l
    %13 = getelementptr inbounds %Lists.ListNode* %12, i32 0, i32 2
    %14 = load %Lists.List* %13
    %15 = icmp eq %Lists.List %14, null
    br i1 %15, label %23, label %16
    
; <label>:16                                      ; preds = %11
    %17 = getelementptr inbounds %Lists.List %14, i32 0, i32 0
    %18 = load i8** %17
    %19 = bitcast i8* %18 to [4 x i8*]*
    %20 = getelementptr inbounds [4 x i8*]* %19, i32 0, i32 2
    %21 = load i8** %20
    %22 = bitcast i8* %21 to void (%Lists.List*, %Lists.Int32)*
    br label %23
    
; <label>:23                                      ; preds = %11, %16
    %24 = phi void (%Lists.List*, %Lists.Int32)* [@Lists.ListNode.Add, %11], [%22, %16]
    %25 = load %Lists.Int32* %v
    %26 = getelementptr inbounds %Lists.ListNode* %12, i32 0, i32 2
    call void (%Lists.List*, %Lists.Int32)* %24(%Lists.List* %26, %Lists.Int32 %25) nounwind
    
    br label %27
    
; <label>:27                                      ; preds = %3, %23
    
    ret void 
}

define %Lists.Int32 @Lists.ListNode.Get(%Lists.List* %l) nounwind {
    
    %v = alloca %Lists.Int32
    
    ; IF l = NIL THEN
    %1 = load %Lists.List* %l
    %2 = icmp eq %Lists.List %1, null
    br i1 %2, label %3, label %4
    
; <label>:3                                       ; preds = %0
    
    ; RETURN 0
    ret %Lists.Int32 0
; <label>:4                                       ; preds = %0
    
    ; v := l^.value
    %5 = load %Lists.List* %l
    %6 = getelementptr inbounds %Lists.ListNode* %5, i32 0, i32 1
    %7 = load %Lists.Int32* %6
    store %Lists.Int32 %7, %Lists.Int32* %v
    
    ; l := l^.next
    %8 = load %Lists.List* %l
    %9 = getelementptr inbounds %Lists.ListNode* %8, i32 0, i32 2
    %10 = load %Lists.List* %9
    store %Lists.List %10, %Lists.List* %l
    
    ; RETURN v
    %11 = load %Lists.Int32* %v
    ret %Lists.Int32 %11
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
    %9 = load %Lists.List* @test
    %10 = icmp eq %Lists.List %9, null
    br i1 %10, label %18, label %11
    
; <label>:11                                      ; preds = %4
    %12 = getelementptr inbounds %Lists.List %9, i32 0, i32 0
    %13 = load i8** %12
    %14 = bitcast i8* %13 to [4 x i8*]*
    %15 = getelementptr inbounds [4 x i8*]* %14, i32 0, i32 2
    %16 = load i8** %15
    %17 = bitcast i8* %16 to void (%Lists.List*, %Lists.Int32)*
    br label %18
    
; <label>:18                                      ; preds = %4, %11
    %19 = phi void (%Lists.List*, %Lists.Int32)* [@Lists.ListNode.Add, %4], [%17, %11]
    %20 = load i32* @n
    call void (%Lists.List*, %Lists.Int32)* %19(%Lists.List* @test, %Lists.Int32 %20) nounwind
    
    %21 = load i32* @i
    %22 = add i32 %21, 1
    store i32 %22, i32* @i
    br label %1
    
; <label>:23                                      ; preds = %1
    
    ; REPEAT UNTIL i = 0
    br label %24
    
; <label>:24                                      ; preds = %23, %44
    
    ; i := test.Get()
    %25 = load %Lists.List* @test
    %26 = icmp eq %Lists.List %25, null
    br i1 %26, label %34, label %27
    
; <label>:27                                      ; preds = %24
    %28 = getelementptr inbounds %Lists.List %25, i32 0, i32 0
    %29 = load i8** %28
    %30 = bitcast i8* %29 to [4 x i8*]*
    %31 = getelementptr inbounds [4 x i8*]* %30, i32 0, i32 3
    %32 = load i8** %31
    %33 = bitcast i8* %32 to %Lists.Int32 (%Lists.List*)*
    br label %34
    
; <label>:34                                      ; preds = %24, %27
    %35 = phi %Lists.Int32 (%Lists.List*)* [@Lists.ListNode.Get, %24], [%33, %27]
    %36 = call %Lists.Int32 (%Lists.List*)* %35(%Lists.List* @test) nounwind
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

