; Generated 17/08/2013 00:13:31
; GlobalUID ffff8abe-985a-4b86-b5e8-090c5a3ce6f8
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

%Out.String = type {i32, %CHAR*}

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

define i32 @Link._init() nounwind {
    
    ; n := 1
    store i32 1, i32* @n
    
    ; FOR i := 1 TO 256 DO
    store i32 1, i32* @i
    br label %1
    
; <label>:1                                       ; preds = %0, %20
    
    %2 = load i32* @i
    %3 = icmp sgt i32 %2, 256
    br i1 %3, label %25, label %4
    
; <label>:4                                       ; preds = %1
    
    ; n := (((n - 1) * 78721 + 1213) MOD 256) + 1
    %5 = load i32* @n
    %6 = sub i32 %5, 1
    %7 = mul i32 %6, 78721
    %8 = add i32 %7, 1213
    %9 = srem i32 %8, 256
    %10 = add i32 %9, 1
    store i32 %10, i32* @n
    
    ; test.Add(n)
    %11 = load %List.List* @test
    %12 = icmp eq %List.List %11, null
    br i1 %12, label %20, label %13
    
; <label>:13                                      ; preds = %4
    %14 = getelementptr inbounds %List.List %11, i32 0, i32 0
    %15 = load i8** %14
    %16 = bitcast i8* %15 to [6 x i8*]*
    %17 = getelementptr inbounds [6 x i8*]* %16, i32 0, i32 2
    %18 = load i8** %17
    %19 = bitcast i8* %18 to void (%List.List*, %List.Int32)*
    br label %20
    
; <label>:20                                      ; preds = %4, %13
    %21 = phi void (%List.List*, %List.Int32)* [@List.ListNode.Add, %4], [%19, %13]
    %22 = load i32* @n
    call void (%List.List*, %List.Int32)* %21(%List.List* @test, %List.Int32 %22) nounwind
    
    %23 = load i32* @i
    %24 = add i32 %23, 1
    store i32 %24, i32* @i
    br label %1
    
; <label>:25                                      ; preds = %1
    
    ; n := 0
    store i32 0, i32* @n
    
    ; FOR i := 1 TO test.Count() DO
    store i32 1, i32* @i
    br label %26
    
; <label>:26                                      ; preds = %25, %58
    
    %27 = load %List.List* @test
    %28 = icmp eq %List.List %27, null
    br i1 %28, label %36, label %29
    
; <label>:29                                      ; preds = %26
    %30 = getelementptr inbounds %List.List %27, i32 0, i32 0
    %31 = load i8** %30
    %32 = bitcast i8* %31 to [6 x i8*]*
    %33 = getelementptr inbounds [6 x i8*]* %32, i32 0, i32 5
    %34 = load i8** %33
    %35 = bitcast i8* %34 to i32 (%List.List*)*
    br label %36
    
; <label>:36                                      ; preds = %26, %29
    %37 = phi i32 (%List.List*)* [@List.ListNode.Count, %26], [%35, %29]
    %38 = call i32 (%List.List*)* %37(%List.List* @test) nounwind
    %39 = load i32* @i
    %40 = icmp sgt i32 %39, %38
    br i1 %40, label %61, label %41
    
; <label>:41                                      ; preds = %36
    
    ; IF test.Has(i) THEN
    %42 = load %List.List* @test
    %43 = icmp eq %List.List %42, null
    br i1 %43, label %51, label %44
    
; <label>:44                                      ; preds = %41
    %45 = getelementptr inbounds %List.List %42, i32 0, i32 0
    %46 = load i8** %45
    %47 = bitcast i8* %46 to [6 x i8*]*
    %48 = getelementptr inbounds [6 x i8*]* %47, i32 0, i32 4
    %49 = load i8** %48
    %50 = bitcast i8* %49 to i1 (%List.List*, %List.Int32)*
    br label %51
    
; <label>:51                                      ; preds = %41, %44
    %52 = phi i1 (%List.List*, %List.Int32)* [@List.ListNode.Has, %41], [%50, %44]
    %53 = load i32* @i
    %54 = call i1 (%List.List*, %List.Int32)* %52(%List.List* @test, %List.Int32 %53) nounwind
    br i1 %54, label %55, label %58
    
; <label>:55                                      ; preds = %51
    
    ; n := n + 1
    %56 = load i32* @n
    %57 = add i32 %56, 1
    store i32 %57, i32* @n
    
    br label %58
    
; <label>:58                                      ; preds = %51, %55
    
    %59 = load i32* @i
    %60 = add i32 %59, 1
    store i32 %60, i32* @i
    br label %26
    
; <label>:61                                      ; preds = %36
    
    ; Out.String("Used ")
    %62 = getelementptr inbounds [6 x %CHAR]* @.str0, i32 0, i32 0
    %63 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), %CHAR* %62) nounwind
    
    ; Out.Integer(n)
    %64 = load i32* @n
    %65 = sext i32 %64 to i64
    %66 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str2, i32 0, i32 0), i64 %65) nounwind
    
    ; Out.String(" of ")
    %67 = getelementptr inbounds [5 x %CHAR]* @.str3, i32 0, i32 0
    %68 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str1, i32 0, i32 0), %CHAR* %67) nounwind
    
    ; Out.Integer(test.Count())
    %69 = load %List.List* @test
    %70 = icmp eq %List.List %69, null
    br i1 %70, label %78, label %71
    
; <label>:71                                      ; preds = %61
    %72 = getelementptr inbounds %List.List %69, i32 0, i32 0
    %73 = load i8** %72
    %74 = bitcast i8* %73 to [6 x i8*]*
    %75 = getelementptr inbounds [6 x i8*]* %74, i32 0, i32 5
    %76 = load i8** %75
    %77 = bitcast i8* %76 to i32 (%List.List*)*
    br label %78
    
; <label>:78                                      ; preds = %61, %71
    %79 = phi i32 (%List.List*)* [@List.ListNode.Count, %61], [%77, %71]
    %80 = call i32 (%List.List*)* %79(%List.List* @test) nounwind
    %81 = sext i32 %80 to i64
    %82 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str2, i32 0, i32 0), i64 %81) nounwind
    
    ; Out.Ln()
    %83 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str4, i32 0, i32 0)) nounwind
    
    ret i32 0
}

define i32 @main() nounwind {
    
    %1 = call i32 ()* @Link._init() nounwind
    ret i32 %1
}

