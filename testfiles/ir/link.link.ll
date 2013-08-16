; ModuleID = 'testfiles/ir/out.ll'
target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%List.ListNode = type { i8*, i32, %List.ListNode* }

@.str0 = private constant [16 x i8] c"Hello from Out!\00"
@.str1 = private constant [3 x i8] c"%s\00"
@.str2 = private constant [2 x i8] c"\0A\00"
@Out._hasInit = private global i1 false
@.str01 = private constant [9 x i8] c"ListNode\00"
@.str12 = private constant [17 x i8] c"Hello from List!\00"
@.str23 = private constant [3 x i8] c"%s\00"
@.str3 = private constant [2 x i8] c"\0A\00"
@List.ListNode._vtable = global [6 x i8*] [i8* getelementptr inbounds ([9 x i8]* @.str01, i32 0, i32 0), i8* null, i8* bitcast (void (%List.ListNode**, i32)* @List.ListNode.Add to i8*), i8* bitcast (i32 (%List.ListNode**)* @List.ListNode.Get to i8*), i8* bitcast (i1 (%List.ListNode**, i32)* @List.ListNode.Has to i8*), i8* bitcast (i32 (%List.ListNode**)* @List.ListNode.Count to i8*)]
@List._hasInit = private global i1 false
@.str04 = private constant [6 x i8] c"Used \00"
@.str15 = private constant [3 x i8] c"%s\00"
@.str26 = private constant [3 x i8] c"%i\00"
@.str37 = private constant [5 x i8] c" of \00"
@.str4 = private constant [2 x i8] c"\0A\00"
@i = private global i32 0
@n = private global i32 0
@test = private global %List.ListNode* null
@Link._hasInit = private global i1 false

declare i32 @printf(i8*, ...) nounwind

declare noalias i8* @GC_malloc(i32)

declare void @Out.Ln() nounwind

declare void @Out.String({ i32, i8* }) nounwind

declare void @Out.Integer(i64) nounwind

declare void @Out.Real(double) nounwind

declare void @Out.Boolean(i1) nounwind

define i32 @Out._init() nounwind {
  %1 = load i1* @Out._hasInit
  br i1 %1, label %6, label %2

; <label>:2                                       ; preds = %0
  store i1 true, i1* @Out._hasInit
  %3 = getelementptr inbounds [16 x i8]* @.str0, i32 0, i32 0
  %4 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str1, i32 0, i32 0), i8* %3) nounwind
  %5 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.str2, i32 0, i32 0)) nounwind
  br label %6

; <label>:6                                       ; preds = %2, %0
  ret i32 0
                                                  ; No predecessors!
  ret i32 1
}

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
  %1 = load i1* @List._hasInit
  br i1 %1, label %9, label %2

; <label>:2                                       ; preds = %0
  store i1 true, i1* @List._hasInit
  %3 = call i32 @Out._init() nounwind
  %4 = icmp eq i32 %3, 0
  br i1 %4, label %5, label %10

; <label>:5                                       ; preds = %2
  %6 = getelementptr inbounds [17 x i8]* @.str12, i32 0, i32 0
  %7 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str23, i32 0, i32 0), i8* %6) nounwind
  %8 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.str3, i32 0, i32 0)) nounwind
  br label %9

; <label>:9                                       ; preds = %5, %0
  ret i32 0

; <label>:10                                      ; preds = %2
  ret i32 1
}

define i32 @Link._init() nounwind {
  %1 = load i1* @Link._hasInit
  br i1 %1, label %92, label %2

; <label>:2                                       ; preds = %0
  store i1 true, i1* @Link._hasInit
  %3 = call i32 @Out._init() nounwind
  %4 = icmp eq i32 %3, 0
  br i1 %4, label %5, label %93

; <label>:5                                       ; preds = %2
  %6 = call i32 @List._init() nounwind
  %7 = icmp eq i32 %6, 0
  br i1 %7, label %8, label %93

; <label>:8                                       ; preds = %5
  store i32 1, i32* @n
  store i32 1, i32* @i
  br label %9

; <label>:9                                       ; preds = %28, %8
  %10 = load i32* @i
  %11 = icmp sgt i32 %10, 256
  br i1 %11, label %33, label %12

; <label>:12                                      ; preds = %9
  %13 = load i32* @n
  %14 = sub i32 %13, 1
  %15 = mul i32 %14, 78721
  %16 = add i32 %15, 1213
  %17 = srem i32 %16, 256
  %18 = add i32 %17, 1
  store i32 %18, i32* @n
  %19 = load %List.ListNode** @test
  %20 = icmp eq %List.ListNode* %19, null
  br i1 %20, label %28, label %21

; <label>:21                                      ; preds = %12
  %22 = getelementptr inbounds %List.ListNode* %19, i32 0, i32 0
  %23 = load i8** %22
  %24 = bitcast i8* %23 to [6 x i8*]*
  %25 = getelementptr inbounds [6 x i8*]* %24, i32 0, i32 2
  %26 = load i8** %25
  %27 = bitcast i8* %26 to void (%List.ListNode**, i32)*
  br label %28

; <label>:28                                      ; preds = %21, %12
  %29 = phi void (%List.ListNode**, i32)* [ @List.ListNode.Add, %12 ], [ %27, %21 ]
  %30 = load i32* @n
  call void %29(%List.ListNode** @test, i32 %30) nounwind
  %31 = load i32* @i
  %32 = add i32 %31, 1
  store i32 %32, i32* @i
  br label %9

; <label>:33                                      ; preds = %9
  store i32 0, i32* @n
  store i32 1, i32* @i
  br label %34

; <label>:34                                      ; preds = %66, %33
  %35 = load %List.ListNode** @test
  %36 = icmp eq %List.ListNode* %35, null
  br i1 %36, label %44, label %37

; <label>:37                                      ; preds = %34
  %38 = getelementptr inbounds %List.ListNode* %35, i32 0, i32 0
  %39 = load i8** %38
  %40 = bitcast i8* %39 to [6 x i8*]*
  %41 = getelementptr inbounds [6 x i8*]* %40, i32 0, i32 5
  %42 = load i8** %41
  %43 = bitcast i8* %42 to i32 (%List.ListNode**)*
  br label %44

; <label>:44                                      ; preds = %37, %34
  %45 = phi i32 (%List.ListNode**)* [ @List.ListNode.Count, %34 ], [ %43, %37 ]
  %46 = call i32 %45(%List.ListNode** @test) nounwind
  %47 = load i32* @i
  %48 = icmp sgt i32 %47, %46
  br i1 %48, label %69, label %49

; <label>:49                                      ; preds = %44
  %50 = load %List.ListNode** @test
  %51 = icmp eq %List.ListNode* %50, null
  br i1 %51, label %59, label %52

; <label>:52                                      ; preds = %49
  %53 = getelementptr inbounds %List.ListNode* %50, i32 0, i32 0
  %54 = load i8** %53
  %55 = bitcast i8* %54 to [6 x i8*]*
  %56 = getelementptr inbounds [6 x i8*]* %55, i32 0, i32 4
  %57 = load i8** %56
  %58 = bitcast i8* %57 to i1 (%List.ListNode**, i32)*
  br label %59

; <label>:59                                      ; preds = %52, %49
  %60 = phi i1 (%List.ListNode**, i32)* [ @List.ListNode.Has, %49 ], [ %58, %52 ]
  %61 = load i32* @i
  %62 = call i1 %60(%List.ListNode** @test, i32 %61) nounwind
  br i1 %62, label %63, label %66

; <label>:63                                      ; preds = %59
  %64 = load i32* @n
  %65 = add i32 %64, 1
  store i32 %65, i32* @n
  br label %66

; <label>:66                                      ; preds = %63, %59
  %67 = load i32* @i
  %68 = add i32 %67, 1
  store i32 %68, i32* @i
  br label %34

; <label>:69                                      ; preds = %44
  %70 = getelementptr inbounds [6 x i8]* @.str04, i32 0, i32 0
  %71 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str15, i32 0, i32 0), i8* %70) nounwind
  %72 = load i32* @n
  %73 = sext i32 %72 to i64
  %74 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str26, i32 0, i32 0), i64 %73) nounwind
  %75 = getelementptr inbounds [5 x i8]* @.str37, i32 0, i32 0
  %76 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str15, i32 0, i32 0), i8* %75) nounwind
  %77 = load %List.ListNode** @test
  %78 = icmp eq %List.ListNode* %77, null
  br i1 %78, label %86, label %79

; <label>:79                                      ; preds = %69
  %80 = getelementptr inbounds %List.ListNode* %77, i32 0, i32 0
  %81 = load i8** %80
  %82 = bitcast i8* %81 to [6 x i8*]*
  %83 = getelementptr inbounds [6 x i8*]* %82, i32 0, i32 5
  %84 = load i8** %83
  %85 = bitcast i8* %84 to i32 (%List.ListNode**)*
  br label %86

; <label>:86                                      ; preds = %79, %69
  %87 = phi i32 (%List.ListNode**)* [ @List.ListNode.Count, %69 ], [ %85, %79 ]
  %88 = call i32 %87(%List.ListNode** @test) nounwind
  %89 = sext i32 %88 to i64
  %90 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.str26, i32 0, i32 0), i64 %89) nounwind
  %91 = call i32 (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.str4, i32 0, i32 0)) nounwind
  br label %92

; <label>:92                                      ; preds = %86, %0
  ret i32 0

; <label>:93                                      ; preds = %5, %2
  ret i32 1
}

define i32 @main() nounwind {
  %1 = call i32 @Link._init() nounwind
  ret i32 %1
}
