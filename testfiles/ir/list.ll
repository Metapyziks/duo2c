; Generated 17/08/2013 00:45:50
; GlobalUID c184655c-3ed9-4f03-ae3e-f91ec9539635
; 
; LLVM IR file for module "List"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET = type i64

@.str0 = private constant [9 x i8] c"ListNode\00"
@.str1 = private constant [17 x i8] c"Hello from List!\00"
@.str2 = private constant [3 x i8] c"%s\00"
@.str3 = private constant [2 x i8] c"\0A\00"

declare i32 @printf(%CHAR*, ...) nounwind 
declare noalias i8* @GC_malloc(i32) 

declare i32 @Out._init() 

%List.Int32 = type i32
%List.List = type %List.ListNode*
%List.ListNode = type {i8*, %List.Int32, %List.List}

@List.ListNode._vtable = global [6 x i8*] [
    i8* getelementptr inbounds ([9 x %CHAR]* @.str0, i32 0, i32 0),
    i8* null,
    i8* bitcast (void (%List.List*, %List.Int32)* @List.ListNode.Add to i8*),
    i8* bitcast (%List.Int32 (%List.List*)* @List.ListNode.Get to i8*),
    i8* bitcast (i1 (%List.List*, %List.Int32)* @List.ListNode.Has to i8*),
    i8* bitcast (i32 (%List.List*)* @List.ListNode.Count to i8*)
]

define void @List.ListNode.Add(%List.List* %l, %List.Int32 %$v) nounwind {
    
    %v = alloca %List.Int32
    store %List.Int32 %$v, %List.Int32* %v
    
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
    store %List.ListNode {i8* bitcast ([6 x i8*]* @List.ListNode._vtable to i8*), %List.Int32 zeroinitializer, %List.List null}, %List.ListNode* %7
    store %List.ListNode* %7, %List.ListNode** %l
    
    ; l^.value := v
    %8 = load %List.List* %l
    %9 = load %List.Int32* %v
    %10 = getelementptr inbounds %List.ListNode* %8, i32 0, i32 1
    store %List.Int32 %9, %List.Int32* %10
    
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
    %19 = bitcast i8* %18 to [6 x i8*]*
    %20 = getelementptr inbounds [6 x i8*]* %19, i32 0, i32 2
    %21 = load i8** %20
    %22 = bitcast i8* %21 to void (%List.List*, %List.Int32)*
    br label %23
    
; <label>:23                                      ; preds = %11, %16
    %24 = phi void (%List.List*, %List.Int32)* [@List.ListNode.Add, %11], [%22, %16]
    %25 = load %List.Int32* %v
    %26 = getelementptr inbounds %List.ListNode* %12, i32 0, i32 2
    call void (%List.List*, %List.Int32)* %24(%List.List* %26, %List.Int32 %25) nounwind
    
    br label %27
    
; <label>:27                                      ; preds = %3, %23
    
    ret void 
}

define %List.Int32 @List.ListNode.Get(%List.List* %l) nounwind {
    
    %v = alloca %List.Int32
    
    ; IF l = NIL THEN
    %1 = load %List.List* %l
    %2 = icmp eq %List.List %1, null
    br i1 %2, label %3, label %4
    
; <label>:3                                       ; preds = %0
    
    ; RETURN 0
    ret %List.Int32 0
; <label>:4                                       ; preds = %0
    
    ; v := l^.value
    %5 = load %List.List* %l
    %6 = getelementptr inbounds %List.ListNode* %5, i32 0, i32 1
    %7 = load %List.Int32* %6
    store %List.Int32 %7, %List.Int32* %v
    
    ; l := l^.next
    %8 = load %List.List* %l
    %9 = getelementptr inbounds %List.ListNode* %8, i32 0, i32 2
    %10 = load %List.List* %9
    store %List.List %10, %List.List* %l
    
    ; RETURN v
    %11 = load %List.Int32* %v
    ret %List.Int32 %11
; <label>:12                                      ; preds = %0
    unreachable 
    
}

define i1 @List.ListNode.Has(%List.List* %l, %List.Int32 %$v) nounwind {
    
    %v = alloca %List.Int32
    store %List.Int32 %$v, %List.Int32* %v
    
    ; IF l = NIL THEN
    %1 = load %List.List* %l
    %2 = icmp eq %List.List %1, null
    br i1 %2, label %3, label %4
    
; <label>:3                                       ; preds = %0
    
    ; RETURN FALSE
    ret i1 false
; <label>:4                                       ; preds = %0
    
    ; IF l^.value = v THEN
    %5 = load %List.List* %l
    %6 = getelementptr inbounds %List.ListNode* %5, i32 0, i32 1
    %7 = load %List.Int32* %6
    %8 = load %List.Int32* %v
    %9 = icmp eq i32 %7, %8
    br i1 %9, label %10, label %11
    
; <label>:10                                      ; preds = %4
    
    ; RETURN TRUE
    ret i1 true
; <label>:11                                      ; preds = %4
    
    ; RETURN l^.next.Has(v)
    %12 = load %List.List* %l
    %13 = getelementptr inbounds %List.ListNode* %12, i32 0, i32 2
    %14 = load %List.List* %13
    %15 = icmp eq %List.List %14, null
    br i1 %15, label %23, label %16
    
; <label>:16                                      ; preds = %11
    %17 = getelementptr inbounds %List.List %14, i32 0, i32 0
    %18 = load i8** %17
    %19 = bitcast i8* %18 to [6 x i8*]*
    %20 = getelementptr inbounds [6 x i8*]* %19, i32 0, i32 4
    %21 = load i8** %20
    %22 = bitcast i8* %21 to i1 (%List.List*, %List.Int32)*
    br label %23
    
; <label>:23                                      ; preds = %11, %16
    %24 = phi i1 (%List.List*, %List.Int32)* [@List.ListNode.Has, %11], [%22, %16]
    %25 = load %List.Int32* %v
    %26 = getelementptr inbounds %List.ListNode* %12, i32 0, i32 2
    %27 = call i1 (%List.List*, %List.Int32)* %24(%List.List* %26, %List.Int32 %25) nounwind
    ret i1 %27
; <label>:28                                      ; preds = %0
    unreachable 
    
    br label %29
    
; <label>:29                                      ; preds = %28
    
}

define i32 @List.ListNode.Count(%List.List* %l) nounwind {
    
    ; IF l = NIL THEN
    %1 = load %List.List* %l
    %2 = icmp eq %List.List %1, null
    br i1 %2, label %3, label %4
    
; <label>:3                                       ; preds = %0
    
    ; RETURN 0
    ret i32 0
; <label>:4                                       ; preds = %0
    
    ; RETURN 1 + l^.next.Count()
    %5 = load %List.List* %l
    %6 = getelementptr inbounds %List.ListNode* %5, i32 0, i32 2
    %7 = load %List.List* %6
    %8 = icmp eq %List.List %7, null
    br i1 %8, label %16, label %9
    
; <label>:9                                       ; preds = %4
    %10 = getelementptr inbounds %List.List %7, i32 0, i32 0
    %11 = load i8** %10
    %12 = bitcast i8* %11 to [6 x i8*]*
    %13 = getelementptr inbounds [6 x i8*]* %12, i32 0, i32 5
    %14 = load i8** %13
    %15 = bitcast i8* %14 to i32 (%List.List*)*
    br label %16
    
; <label>:16                                      ; preds = %4, %9
    %17 = phi i32 (%List.List*)* [@List.ListNode.Count, %4], [%15, %9]
    %18 = getelementptr inbounds %List.ListNode* %5, i32 0, i32 2
    %19 = call i32 (%List.List*)* %17(%List.List* %18) nounwind
    %20 = add i32 1, %19
    ret i32 %20
; <label>:21                                      ; preds = %0
    unreachable 
    
}


@List._hasInit = private global i1 zeroinitializer

define i32 @List._init() nounwind {
    
    %1 = load i1* @List._hasInit
    br i1 %1, label %9, label %2
    
; <label>:2                                       ; preds = %0
    store i1 1, i1* @List._hasInit
    
    %3 = call i32 ()* @Out._init() nounwind
    %4 = icmp eq i32 %3, 0
    br i1 %4, label %5, label %10
    
; <label>:5                                       ; preds = %2
    
    ; Out.String("Hello from List!")
    %6 = getelementptr inbounds [17 x %CHAR]* @.str1, i32 0, i32 0
    %7 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str2, i32 0, i32 0), %CHAR* %6) nounwind
    
    ; Out.Ln()
    %8 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str3, i32 0, i32 0)) nounwind
    
    br label %9
    
; <label>:9                                       ; preds = %0, %5
    ret i32 0
    
; <label>:10                                      ; preds = %2
    ret i32 1
}

