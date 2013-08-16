; ModuleID = 'testfiles/ir/list.ll'
target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%List.ListNode = type { i8*, i32, %List.ListNode* }

@.str0 = private constant [9 x i8] c"ListNode\00"
@List.ListNode._vtable = global [4 x i8*] [i8* getelementptr inbounds ([9 x i8]* @.str0, i32 0, i32 0), i8* null, i8* bitcast (void (%List.ListNode**, i32)* @List.ListNode.Add to i8*), i8* bitcast (i32 (%List.ListNode**)* @List.ListNode.Get to i8*)]
@.str01 = private constant [3 x i8] c"%i\00"
@.str1 = private constant [2 x i8] c"\0A\00"
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
  store %List.ListNode { i8* bitcast ([4 x i8*]* @List.ListNode._vtable to i8*), i32 0, %List.ListNode* null }, %List.ListNode* %7
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
  %19 = bitcast i8* %18 to [4 x i8*]*
  %20 = getelementptr inbounds [4 x i8*]* %19, i32 0, i32 2
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
}

define i32 @List._init() nounwind {
  ret i32 0
}

define i32 @Link._init() nounwind {
  store i32 1, i32* @n
  store i32 1, i32* @i
  br label %1

; <label>:1                                       ; preds = %18, %0
  %2 = load i32* @i
  %3 = icmp sgt i32 %2, 10
  br i1 %3, label %23, label %4

; <label>:4                                       ; preds = %1
  %5 = load i32* @n
  %6 = mul i32 %5, 8723
  %7 = add i32 %6, 181
  %8 = srem i32 %7, 256
  store i32 %8, i32* @n
  %9 = load %List.ListNode** @test
  %10 = icmp eq %List.ListNode* %9, null
  br i1 %10, label %18, label %11

; <label>:11                                      ; preds = %4
  %12 = getelementptr inbounds %List.ListNode* %9, i32 0, i32 0
  %13 = load i8** %12
  %14 = bitcast i8* %13 to [4 x i8*]*
  %15 = getelementptr inbounds [4 x i8*]* %14, i32 0, i32 2
  %16 = load i8** %15
  %17 = bitcast i8* %16 to void (%List.ListNode**, i32)*
  br label %18

; <label>:18                                      ; preds = %11, %4
  %19 = phi void (%List.ListNode**, i32)* [ @List.ListNode.Add, %4 ], [ %17, %11 ]
  %20 = load i32* @n
  call void %19(%List.ListNode** @test, i32 %20) nounwind
  %21 = load i32* @i
  %22 = add i32 %21, 1
  store i32 %22, i32* @i
  br label %1

; <label>:23                                      ; preds = %1
  br label %24

; <label>:24                                      ; preds = %44, %23
  %25 = load %List.ListNode** @test
  %26 = icmp eq %List.ListNode* %25, null
  br i1 %26, label %34, label %27

; <label>:27                                      ; preds = %24
  %28 = getelementptr inbounds %List.ListNode* %25, i32 0, i32 0
  %29 = load i8** %28
  %30 = bitcast i8* %29 to [4 x i8*]*
  %31 = getelementptr inbounds [4 x i8*]* %30, i32 0, i32 3
  %32 = load i8** %31
  %33 = bitcast i8* %32 to i32 (%List.ListNode**)*
  br label %34

; <label>:34                                      ; preds = %27, %24
  %35 = phi i32 (%List.ListNode**)* [ @List.ListNode.Get, %24 ], [ %33, %27 ]
  %36 = call i32 %35(%List.ListNode** @test) nounwind
  store i32 %36, i32* @i
  %37 = load i32* @i
  %38 = icmp sgt i32 %37, 0
  br i1 %38, label %39, label %44

; <label>:39                                      ; preds = %34
  %40 = load i32* @i
  %41 = sext i32 %40 to i64
  %42 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str01, i32 0, i32 0), i64 %41) nounwind
  %43 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.str1, i32 0, i32 0)) nounwind
  br label %44

; <label>:44                                      ; preds = %39, %34
  %45 = load i32* @i
  %46 = icmp eq i32 %45, 0
  br i1 %46, label %47, label %24

; <label>:47                                      ; preds = %44
  ret i32 0
}

define i32 @main() nounwind {
  %1 = call i32 @Link._init() nounwind
  ret i32 %1
}
