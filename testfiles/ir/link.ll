; Generated 17/08/2013 17:52:30
; GlobalUID e9f19fd7-6c40-4bf7-867c-643a0dab4850
; 
; LLVM IR file for module "Link"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET = type i64

@.str0 = private constant [6 x i8] c"Used \00"
@.str1 = private constant [3 x i8] c"%s\00"
@.str2 = private constant [3 x i8] c"%i\00"
@.str3 = private constant [5 x i8] c" of \00"
@.str4 = private constant [2 x i8] c"\0A\00"

declare i32 @printf(%CHAR*, ...) nounwind 
declare noalias i8* @GC_malloc(i32) 

declare i32 @Out._init() 

%List.Int32 = type i32
%List.List = type %List.ListNode*
%List.ListNode = type {i8*, %List.Int32, %List.List}

@List.ListNode._vtable = linkonce global [0 x i8*][] 

declare void @List.ListNode.Add(%List.List*, %List.Int32) nounwind 
declare %List.Int32 @List.ListNode.Get(%List.List*) nounwind 
declare i1 @List.ListNode.Has(%List.List*, %List.Int32) nounwind 
declare i32 @List.ListNode.Count(%List.List*) nounwind 
declare i32 @List._init() 

@i = private global i32 zeroinitializer
@n = private global i32 zeroinitializer
@test = private global %List.List null

@Link._hasInit = private global i1 zeroinitializer

define i32 @Link._init() nounwind {
    
    %1 = load i1* @Link._hasInit
    br i1 %1, label %92, label %2
    
; <label>:2                                       ; preds = %0
    store i1 1, i1* @Link._hasInit
    
    %3 = call i32 ()* @Out._init() nounwind
    %4 = icmp eq i32 %3, 0
    br i1 %4, label %5, label %93
    
; <label>:5                                       ; preds = %2
    %6 = call i32 ()* @List._init() nounwind
    %7 = icmp eq i32 %6, 0
    br i1 %7, label %8, label %93
    
; <label>:8                                       ; preds = %5
    
    ; n := 1
    store i32 1, i32* @n
    
    ; FOR i := 1 TO 256 DO
    store i32 1, i32* @i
    br label %9
    
; <label>:9                                       ; preds = %8, %28
    
    %10 = load i32* @i
    %11 = icmp sgt i32 %10, 256
    br i1 %11, label %33, label %12
    
; <label>:12                                      ; preds = %9
    
    ; n := (((n - 1) * 78721 + 1213) MOD 256) + 1
    %13 = load i32* @n
    %14 = sub i32 %13, 1
    %15 = mul i32 %14, 78721
    %16 = add i32 %15, 1213
    %17 = srem i32 %16, 256
    %18 = add i32 %17, 1
    store i32 %18, i32* @n
    
    ; test.Add(n)
    %19 = load %List.List* @test
    %20 = icmp eq %List.List %19, null
    br i1 %20, label %28, label %21
    
; <label>:21                                      ; preds = %12
    %22 = getelementptr inbounds %List.List %19, i32 0, i32 0
    %23 = load i8** %22
    %24 = bitcast i8* %23 to [6 x i8*]*
    %25 = getelementptr inbounds [6 x i8*]* %24, i32 0, i32 2
    %26 = load i8** %25
    %27 = bitcast i8* %26 to void (%List.List*, %List.Int32)*
    br label %28
    
; <label>:28                                      ; preds = %12, %21
    %29 = phi void (%List.List*, %List.Int32)* [@List.ListNode.Add, %12], [%27, %21]
    %30 = load i32* @n
    call void (%List.List*, %List.Int32)* %29(%List.List* @test, %List.Int32 %30) nounwind
    
    %31 = load i32* @i
    %32 = add i32 %31, 1
    store i32 %32, i32* @i
    br label %9
    
; <label>:33                                      ; preds = %9
    
    ; n := 0
    store i32 0, i32* @n
    
    ; FOR i := 1 TO test.Count() DO
    store i32 1, i32* @i
    br label %34
    
; <label>:34                                      ; preds = %33, %66
    
    %35 = load %List.List* @test
    %36 = icmp eq %List.List %35, null
    br i1 %36, label %44, label %37
    
; <label>:37                                      ; preds = %34
    %38 = getelementptr inbounds %List.List %35, i32 0, i32 0
    %39 = load i8** %38
    %40 = bitcast i8* %39 to [6 x i8*]*
    %41 = getelementptr inbounds [6 x i8*]* %40, i32 0, i32 5
    %42 = load i8** %41
    %43 = bitcast i8* %42 to i32 (%List.List*)*
    br label %44
    
; <label>:44                                      ; preds = %34, %37
    %45 = phi i32 (%List.List*)* [@List.ListNode.Count, %34], [%43, %37]
    %46 = call i32 (%List.List*)* %45(%List.List* @test) nounwind
    %47 = load i32* @i
    %48 = icmp sgt i32 %47, %46
    br i1 %48, label %69, label %49
    
; <label>:49                                      ; preds = %44
    
    ; IF test.Has(i) THEN
    %50 = load %List.List* @test
    %51 = icmp eq %List.List %50, null
    br i1 %51, label %59, label %52
    
; <label>:52                                      ; preds = %49
    %53 = getelementptr inbounds %List.List %50, i32 0, i32 0
    %54 = load i8** %53
    %55 = bitcast i8* %54 to [6 x i8*]*
    %56 = getelementptr inbounds [6 x i8*]* %55, i32 0, i32 4
    %57 = load i8** %56
    %58 = bitcast i8* %57 to i1 (%List.List*, %List.Int32)*
    br label %59
    
; <label>:59                                      ; preds = %49, %52
    %60 = phi i1 (%List.List*, %List.Int32)* [@List.ListNode.Has, %49], [%58, %52]
    %61 = load i32* @i
    %62 = call i1 (%List.List*, %List.Int32)* %60(%List.List* @test, %List.Int32 %61) nounwind
    br i1 %62, label %63, label %66
    
; <label>:63                                      ; preds = %59
    
    ; n := n + 1
    %64 = load i32* @n
    %65 = add i32 %64, 1
    store i32 %65, i32* @n
    
    br label %66
    
; <label>:66                                      ; preds = %59, %63
    
    %67 = load i32* @i
    %68 = add i32 %67, 1
    store i32 %68, i32* @i
    br label %34
    
; <label>:69                                      ; preds = %44
    
    ; Out.String("Used ")
    %70 = getelementptr inbounds [6 x %CHAR]* @.str0, i32 0, i32 0
    %71 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), %CHAR* %70) nounwind
    
    ; Out.Integer(n)
    %72 = load i32* @n
    %73 = sext i32 %72 to i64
    %74 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str2, i32 0, i32 0), i64 %73) nounwind
    
    ; Out.String(" of ")
    %75 = getelementptr inbounds [5 x %CHAR]* @.str3, i32 0, i32 0
    %76 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), %CHAR* %75) nounwind
    
    ; Out.Integer(test.Count())
    %77 = load %List.List* @test
    %78 = icmp eq %List.List %77, null
    br i1 %78, label %86, label %79
    
; <label>:79                                      ; preds = %69
    %80 = getelementptr inbounds %List.List %77, i32 0, i32 0
    %81 = load i8** %80
    %82 = bitcast i8* %81 to [6 x i8*]*
    %83 = getelementptr inbounds [6 x i8*]* %82, i32 0, i32 5
    %84 = load i8** %83
    %85 = bitcast i8* %84 to i32 (%List.List*)*
    br label %86
    
; <label>:86                                      ; preds = %69, %79
    %87 = phi i32 (%List.List*)* [@List.ListNode.Count, %69], [%85, %79]
    %88 = call i32 (%List.List*)* %87(%List.List* @test) nounwind
    %89 = sext i32 %88 to i64
    %90 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str2, i32 0, i32 0), i64 %89) nounwind
    
    ; Out.Ln()
    %91 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str4, i32 0, i32 0)) nounwind
    
    br label %92
    
; <label>:92                                      ; preds = %0, %86
    ret i32 0
    
; <label>:93                                      ; preds = %2, %5
    ret i32 1
}

define i32 @main() nounwind {
    
    %1 = call i32 ()* @Link._init() nounwind
    ret i32 %1
}

