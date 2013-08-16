; ModuleID = 'testfiles/ir/list.ll'
target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%List.ListNode = type { i8*, i32, %List.ListNode* }

@.str0 = private constant [9 x i8] c"ListNode\00"
@List.ListNode._vtable = global [6 x i8*] [i8* getelementptr inbounds ([9 x i8]* @.str0, i32 0, i32 0), i8* null, i8* bitcast (void (%List.ListNode**, i32)* @List.ListNode.Add to i8*), i8* bitcast (i32 (%List.ListNode**)* @List.ListNode.Get to i8*), i8* bitcast (i1 (%List.ListNode**, i32)* @List.ListNode.Has to i8*), i8* bitcast (i32 (%List.ListNode**)* @List.ListNode.Count to i8*)]
@.str01 = private constant [6 x i8] c"Used \00"
@.str1 = private constant [3 x i8] c"%s\00"
@.str2 = private constant [3 x i8] c"%i\00"
@.str3 = private constant [5 x i8] c" of \00"
@.str4 = private constant [2 x i8] c"\0A\00"
@i = private global i32 0
@n = private global i32 0
@test = private global %List.ListNode* null

declare i32 @printf(i8*, ...) nounwind

declare noalias i8* @GC_malloc(i32)

declare i32 @Out._init()

define void @List.ListNode.Add(%List.ListNode** %l, i32 %"$v") nounwind {
  %v = alloca i32
  store i32 %"$v", i32* %v
  %1 = load %List.ListNode** %l
  %2 = icmp eq %List.ListNode* %1, null
  br i1 %2, label %3, label %11

; <label>:3                                       ; preds = %0
  %4 = getelementptr inbounds %List.ListNode* null, i32 1
  %5 = ptrtoint %List.ListNode* %4 to i32
  %6 = call i8* @GC_malloc(i32 %5) nounwind
  %7 = bitcast i8* %6 to %List.ListNode*
  store %List.ListNode { i8* bitcast ([6 x i8*]* @List.ListNode._vtable to i8*), i32 0, %List.ListNode* null }, %List.ListNode* %7
  store %List.ListNode* %7, %List.ListNode** %l
  %8 = load %List.ListNode** %l
  %9 = load i32* %v
  %10 = getelementptr inbounds %List.ListNode* %8, i32 0, i32 1
  store i32 %9, i32* %10
  br label %27

; <label>:11                                      ; preds = %0
  %12 = load %List.ListNode** %l
  %13 = getelementptr inbounds %List.ListNode* %12, i32 0, i32 2
  %14 = load %List.ListNode** %13
  %15 = icmp eq %List.ListNode* %14, null
  br i1 %15, label %23, label %16

; <label>:16                                      ; preds = %11
  %17 = getelementptr inbounds %List.ListNode* %14, i32 0, i32 0
  %18 = load i8** %17
  %19 = bitcast i8* %18 to [6 x i8*]*
  %20 = getelementptr inbounds [6 x i8*]* %19, i32 0, i32 2
  %21 = load i8** %20
  %22 = bitcast i8* %21 to void (%List.ListNode**, i32)*
  br label %23

; <label>:23                                      ; preds = %16, %11
  %24 = phi void (%List.ListNode**, i32)* [ @List.ListNode.Add, %11 ], [ %22, %16 ]
  %25 = load i32* %v
  %26 = getelementptr inbounds %List.ListNode* %12, i32 0, i32 2
  call void %24(%List.ListNode** %26, i32 %25) nounwind
  br label %27

; <label>:27                                      ; preds = %23, %3
  ret void
}

define i32 @List.ListNode.Get(%List.ListNode** %l) nounwind {
  %v = alloca i32
  %1 = load %List.ListNode** %l
  %2 = icmp eq %List.ListNode* %1, null
  br i1 %2, label %3, label %4

; <label>:3                                       ; preds = %0
  ret i32 0

; <label>:4                                       ; preds = %0
  %5 = load %List.ListNode** %l
  %6 = getelementptr inbounds %List.ListNode* %5, i32 0, i32 1
  %7 = load i32* %6
  store i32 %7, i32* %v
  %8 = load %List.ListNode** %l
  %9 = getelementptr inbounds %List.ListNode* %8, i32 0, i32 2
  %10 = load %List.ListNode** %9
  store %List.ListNode* %10, %List.ListNode** %l
  %11 = load i32* %v
  ret i32 %11
                                                  ; No predecessors!
  unreachable
}

define i1 @List.ListNode.Has(%List.ListNode** %l, i32 %"$v") nounwind {
  %v = alloca i32
  store i32 %"$v", i32* %v
  %1 = load %List.ListNode** %l
  %2 = icmp eq %List.ListNode* %1, null
  br i1 %2, label %3, label %4

; <label>:3                                       ; preds = %0
  ret i1 false

; <label>:4                                       ; preds = %0
  %5 = load %List.ListNode** %l
  %6 = getelementptr inbounds %List.ListNode* %5, i32 0, i32 1
  %7 = load i32* %6
  %8 = load i32* %v
  %9 = icmp eq i32 %7, %8
  br i1 %9, label %10, label %11

; <label>:10                                      ; preds = %4
  ret i1 true

; <label>:11                                      ; preds = %4
  %12 = load %List.ListNode** %l
  %13 = getelementptr inbounds %List.ListNode* %12, i32 0, i32 2
  %14 = load %List.ListNode** %13
  %15 = icmp eq %List.ListNode* %14, null
  br i1 %15, label %23, label %16

; <label>:16                                      ; preds = %11
  %17 = getelementptr inbounds %List.ListNode* %14, i32 0, i32 0
  %18 = load i8** %17
  %19 = bitcast i8* %18 to [6 x i8*]*
  %20 = getelementptr inbounds [6 x i8*]* %19, i32 0, i32 4
  %21 = load i8** %20
  %22 = bitcast i8* %21 to i1 (%List.ListNode**, i32)*
  br label %23

; <label>:23                                      ; preds = %16, %11
  %24 = phi i1 (%List.ListNode**, i32)* [ @List.ListNode.Has, %11 ], [ %22, %16 ]
  %25 = load i32* %v
  %26 = getelementptr inbounds %List.ListNode* %12, i32 0, i32 2
  %27 = call i1 %24(%List.ListNode** %26, i32 %25) nounwind
  ret i1 %27
                                                  ; No predecessors!
  unreachable

; <label>:29                                      ; preds = %29
  br label %29
}

define i32 @List.ListNode.Count(%List.ListNode** %l) nounwind {
  %1 = load %List.ListNode** %l
  %2 = icmp eq %List.ListNode* %1, null
  br i1 %2, label %3, label %4

; <label>:3                                       ; preds = %0
  ret i32 0

; <label>:4                                       ; preds = %0
  %5 = load %List.ListNode** %l
  %6 = getelementptr inbounds %List.ListNode* %5, i32 0, i32 2
  %7 = load %List.ListNode** %6
  %8 = icmp eq %List.ListNode* %7, null
  br i1 %8, label %16, label %9

; <label>:9                                       ; preds = %4
  %10 = getelementptr inbounds %List.ListNode* %7, i32 0, i32 0
  %11 = load i8** %10
  %12 = bitcast i8* %11 to [6 x i8*]*
  %13 = getelementptr inbounds [6 x i8*]* %12, i32 0, i32 5
  %14 = load i8** %13
  %15 = bitcast i8* %14 to i32 (%List.ListNode**)*
  br label %16

; <label>:16                                      ; preds = %9, %4
  %17 = phi i32 (%List.ListNode**)* [ @List.ListNode.Count, %4 ], [ %15, %9 ]
  %18 = getelementptr inbounds %List.ListNode* %5, i32 0, i32 2
  %19 = call i32 %17(%List.ListNode** %18) nounwind
  %20 = add i32 1, %19
  ret i32 %20
                                                  ; No predecessors!
  unreachable
}

define i32 @List._init() nounwind {
  ret i32 0
}

define i32 @Link._init() nounwind {
  store i32 1, i32* @n
  store i32 1, i32* @i
  br label %1

; <label>:1                                       ; preds = %20, %0
  %2 = load i32* @i
  %3 = icmp sgt i32 %2, 256
  br i1 %3, label %25, label %4

; <label>:4                                       ; preds = %1
  %5 = load i32* @n
  %6 = sub i32 %5, 1
  %7 = mul i32 %6, 78721
  %8 = add i32 %7, 1213
  %9 = srem i32 %8, 256
  %10 = add i32 %9, 1
  store i32 %10, i32* @n
  %11 = load %List.ListNode** @test
  %12 = icmp eq %List.ListNode* %11, null
  br i1 %12, label %20, label %13

; <label>:13                                      ; preds = %4
  %14 = getelementptr inbounds %List.ListNode* %11, i32 0, i32 0
  %15 = load i8** %14
  %16 = bitcast i8* %15 to [6 x i8*]*
  %17 = getelementptr inbounds [6 x i8*]* %16, i32 0, i32 2
  %18 = load i8** %17
  %19 = bitcast i8* %18 to void (%List.ListNode**, i32)*
  br label %20

; <label>:20                                      ; preds = %13, %4
  %21 = phi void (%List.ListNode**, i32)* [ @List.ListNode.Add, %4 ], [ %19, %13 ]
  %22 = load i32* @n
  call void %21(%List.ListNode** @test, i32 %22) nounwind
  %23 = load i32* @i
  %24 = add i32 %23, 1
  store i32 %24, i32* @i
  br label %1

; <label>:25                                      ; preds = %1
  store i32 0, i32* @n
  store i32 1, i32* @i
  br label %26

; <label>:26                                      ; preds = %58, %25
  %27 = load %List.ListNode** @test
  %28 = icmp eq %List.ListNode* %27, null
  br i1 %28, label %36, label %29

; <label>:29                                      ; preds = %26
  %30 = getelementptr inbounds %List.ListNode* %27, i32 0, i32 0
  %31 = load i8** %30
  %32 = bitcast i8* %31 to [6 x i8*]*
  %33 = getelementptr inbounds [6 x i8*]* %32, i32 0, i32 5
  %34 = load i8** %33
  %35 = bitcast i8* %34 to i32 (%List.ListNode**)*
  br label %36

; <label>:36                                      ; preds = %29, %26
  %37 = phi i32 (%List.ListNode**)* [ @List.ListNode.Count, %26 ], [ %35, %29 ]
  %38 = call i32 %37(%List.ListNode** @test) nounwind
  %39 = load i32* @i
  %40 = icmp sgt i32 %39, %38
  br i1 %40, label %61, label %41

; <label>:41                                      ; preds = %36
  %42 = load %List.ListNode** @test
  %43 = icmp eq %List.ListNode* %42, null
  br i1 %43, label %51, label %44

; <label>:44                                      ; preds = %41
  %45 = getelementptr inbounds %List.ListNode* %42, i32 0, i32 0
  %46 = load i8** %45
  %47 = bitcast i8* %46 to [6 x i8*]*
  %48 = getelementptr inbounds [6 x i8*]* %47, i32 0, i32 4
  %49 = load i8** %48
  %50 = bitcast i8* %49 to i1 (%List.ListNode**, i32)*
  br label %51

; <label>:51                                      ; preds = %44, %41
  %52 = phi i1 (%List.ListNode**, i32)* [ @List.ListNode.Has, %41 ], [ %50, %44 ]
  %53 = load i32* @i
  %54 = call i1 %52(%List.ListNode** @test, i32 %53) nounwind
  br i1 %54, label %55, label %58

; <label>:55                                      ; preds = %51
  %56 = load i32* @n
  %57 = add i32 %56, 1
  store i32 %57, i32* @n
  br label %58

; <label>:58                                      ; preds = %55, %51
  %59 = load i32* @i
  %60 = add i32 %59, 1
  store i32 %60, i32* @i
  br label %26

; <label>:61                                      ; preds = %36
  %62 = getelementptr inbounds [6 x i8]* @.str01, i32 0, i32 0
  %63 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str1, i32 0, i32 0), i8* %62) nounwind
  %64 = load i32* @n
  %65 = sext i32 %64 to i64
  %66 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str2, i32 0, i32 0), i64 %65) nounwind
  %67 = getelementptr inbounds [5 x i8]* @.str3, i32 0, i32 0
  %68 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str1, i32 0, i32 0), i8* %67) nounwind
  %69 = load %List.ListNode** @test
  %70 = icmp eq %List.ListNode* %69, null
  br i1 %70, label %78, label %71

; <label>:71                                      ; preds = %61
  %72 = getelementptr inbounds %List.ListNode* %69, i32 0, i32 0
  %73 = load i8** %72
  %74 = bitcast i8* %73 to [6 x i8*]*
  %75 = getelementptr inbounds [6 x i8*]* %74, i32 0, i32 5
  %76 = load i8** %75
  %77 = bitcast i8* %76 to i32 (%List.ListNode**)*
  br label %78

; <label>:78                                      ; preds = %71, %61
  %79 = phi i32 (%List.ListNode**)* [ @List.ListNode.Count, %61 ], [ %77, %71 ]
  %80 = call i32 %79(%List.ListNode** @test) nounwind
  %81 = sext i32 %80 to i64
  %82 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str2, i32 0, i32 0), i64 %81) nounwind
  %83 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.str4, i32 0, i32 0)) nounwind
  ret i32 0
}

define i32 @main() nounwind {
  %1 = call i32 @Link._init() nounwind
  ret i32 %1
}
