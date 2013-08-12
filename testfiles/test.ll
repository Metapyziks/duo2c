; Generated 12/08/2013 16:59:54
; GlobalUID 92813ad5-d8e6-45d3-b4aa-64399c992a1f
; 
; LLVM IR file for module "Test"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET = type i64
%Test.PointType = type double
%Test.Point2D = type %Test.Point2DRec*
%Test.Point2DRec = type {i8*, %Test.PointType, %Test.PointType}
%Test.Point3D = type %Test.Point3DRec*
%Test.Point3DRec = type {i8*, %Test.PointType, %Test.PointType, %Test.PointType}

@.rec0 = global [6 x i8*] [
    i8* getelementptr inbounds ([11 x %CHAR]* @.str0, i32 0, i32 0),
    i8* null,
    i8* bitcast (void (%Test.Point2D*, %Test.PointType, %Test.PointType)* @Test.Point2DRec.New to i8*),
    i8* bitcast (void (%Test.Point2D*)* @Test.Point2DRec.Print to i8*),
    i8* bitcast (%Test.Point2D (%Test.Point2D*, %Test.Point2D)* @Test.Point2DRec.Add to i8*),
    i8* bitcast (%Test.Point2D (%Test.Point2D*, %Test.Point2D)* @Test.Point2DRec.Sub to i8*)
]

@.rec1 = global [6 x i8*] [
    i8* getelementptr inbounds ([11 x %CHAR]* @.str1, i32 0, i32 0),
    i8* bitcast ([6 x i8*]* @.rec0 to i8*),
    i8* bitcast (void (%Test.Point3D*, %Test.PointType, %Test.PointType, %Test.PointType)* @Test.Point3DRec.New to i8*),
    i8* bitcast (void (%Test.Point3D*)* @Test.Point3DRec.Print to i8*),
    i8* bitcast (%Test.Point3D (%Test.Point3D*, %Test.Point3D)* @Test.Point3DRec.Add to i8*),
    i8* bitcast (%Test.Point3D (%Test.Point3D*, %Test.Point3D)* @Test.Point3DRec.Sub to i8*)
]

@.str0 = private constant [11 x i8] c"Point2DRec\00"
@.str1 = private constant [11 x i8] c"Point3DRec\00"
@.str2 = private constant [2 x i8] c"(\00"
@.str3 = private constant [3 x i8] c"%s\00"
@.str4 = private constant [3 x i8] c"%g\00"
@.str5 = private constant [2 x i8] c" \00"
@.str6 = private constant [2 x i8] c")\00"
@.str7 = private constant [2 x i8] c"\0A\00"

declare i32 @printf(%CHAR*, ...) nounwind 
declare noalias i8* @GC_malloc(i32) 

@a = private global %Test.Point3D null
@b = private global %Test.Point3D null
@c = private global %Test.Point3D null

define void @Test.Point2DRec.New(%Test.Point2D* %this, %Test.PointType %$x, %Test.PointType %$y) nounwind {
    
    %x = alloca %Test.PointType
    store %Test.PointType %$x, %Test.PointType* %x
    
    %y = alloca %Test.PointType
    store %Test.PointType %$y, %Test.PointType* %y
    
    ; NEW(this)
    %1 = getelementptr inbounds %Test.Point2DRec* null, i32 1
    %2 = ptrtoint %Test.Point2DRec* %1 to i32
    %3 = call i8* (i32)* @GC_malloc(i32 %2) nounwind
    %4 = bitcast i8* %3 to %Test.Point2DRec*
    store %Test.Point2DRec {i8* bitcast ([6 x i8*]* @.rec0 to i8*), %Test.PointType zeroinitializer, %Test.PointType zeroinitializer}, %Test.Point2DRec* %4
    store %Test.Point2DRec* %4, %Test.Point2DRec** %this
    
    ; this^.x := x
    %5 = load %Test.Point2D* %this
    %6 = load %Test.PointType* %x
    %7 = getelementptr inbounds %Test.Point2DRec* %5, i32 0, i32 1
    store %Test.PointType %6, %Test.PointType* %7
    
    ; this^.y := y
    %8 = load %Test.Point2D* %this
    %9 = load %Test.PointType* %y
    %10 = getelementptr inbounds %Test.Point2DRec* %8, i32 0, i32 2
    store %Test.PointType %9, %Test.PointType* %10
    
    ret void 
}

define void @Test.Point3DRec.New(%Test.Point3D* %this, %Test.PointType %$x, %Test.PointType %$y, %Test.PointType %$z) nounwind {
    
    %x = alloca %Test.PointType
    store %Test.PointType %$x, %Test.PointType* %x
    
    %y = alloca %Test.PointType
    store %Test.PointType %$y, %Test.PointType* %y
    
    %z = alloca %Test.PointType
    store %Test.PointType %$z, %Test.PointType* %z
    
    ; NEW(this)
    %1 = getelementptr inbounds %Test.Point3DRec* null, i32 1
    %2 = ptrtoint %Test.Point3DRec* %1 to i32
    %3 = call i8* (i32)* @GC_malloc(i32 %2) nounwind
    %4 = bitcast i8* %3 to %Test.Point3DRec*
    store %Test.Point3DRec {i8* bitcast ([6 x i8*]* @.rec1 to i8*), %Test.PointType zeroinitializer, %Test.PointType zeroinitializer, %Test.PointType zeroinitializer}, %Test.Point3DRec* %4
    store %Test.Point3DRec* %4, %Test.Point3DRec** %this
    
    ; this^.x := x
    %5 = load %Test.Point3D* %this
    %6 = load %Test.PointType* %x
    %7 = getelementptr inbounds %Test.Point3DRec* %5, i32 0, i32 1
    store %Test.PointType %6, %Test.PointType* %7
    
    ; this^.y := y
    %8 = load %Test.Point3D* %this
    %9 = load %Test.PointType* %y
    %10 = getelementptr inbounds %Test.Point3DRec* %8, i32 0, i32 2
    store %Test.PointType %9, %Test.PointType* %10
    
    ; this^.z := z
    %11 = load %Test.Point3D* %this
    %12 = load %Test.PointType* %z
    %13 = getelementptr inbounds %Test.Point3DRec* %11, i32 0, i32 3
    store %Test.PointType %12, %Test.PointType* %13
    
    ret void 
}

define void @Test.Point2DRec.Print(%Test.Point2D* %this) nounwind {
    
    ; Out.String("(")
    %1 = getelementptr inbounds [2 x %CHAR]* @.str2, i32 0, i32 0
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str3, i32 0, i32 0), %CHAR* %1) nounwind
    
    ; Out.Real(this^.x)
    %3 = load %Test.Point2D* %this
    %4 = getelementptr inbounds %Test.Point2DRec* %3, i32 0, i32 1
    %5 = load %Test.PointType* %4
    %6 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), double %5) nounwind
    
    ; Out.String(" ")
    %7 = getelementptr inbounds [2 x %CHAR]* @.str5, i32 0, i32 0
    %8 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str3, i32 0, i32 0), %CHAR* %7) nounwind
    
    ; Out.Real(this^.y)
    %9 = load %Test.Point2D* %this
    %10 = getelementptr inbounds %Test.Point2DRec* %9, i32 0, i32 2
    %11 = load %Test.PointType* %10
    %12 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), double %11) nounwind
    
    ; Out.String(")")
    %13 = getelementptr inbounds [2 x %CHAR]* @.str6, i32 0, i32 0
    %14 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str3, i32 0, i32 0), %CHAR* %13) nounwind
    
    ret void 
}

define void @Test.Point3DRec.Print(%Test.Point3D* %this) nounwind {
    
    ; Out.String("(")
    %1 = getelementptr inbounds [2 x %CHAR]* @.str2, i32 0, i32 0
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str3, i32 0, i32 0), %CHAR* %1) nounwind
    
    ; Out.Real(this^.x)
    %3 = load %Test.Point3D* %this
    %4 = getelementptr inbounds %Test.Point3DRec* %3, i32 0, i32 1
    %5 = load %Test.PointType* %4
    %6 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), double %5) nounwind
    
    ; Out.String(" ")
    %7 = getelementptr inbounds [2 x %CHAR]* @.str5, i32 0, i32 0
    %8 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str3, i32 0, i32 0), %CHAR* %7) nounwind
    
    ; Out.Real(this^.y)
    %9 = load %Test.Point3D* %this
    %10 = getelementptr inbounds %Test.Point3DRec* %9, i32 0, i32 2
    %11 = load %Test.PointType* %10
    %12 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), double %11) nounwind
    
    ; Out.String(" ")
    %13 = getelementptr inbounds [2 x %CHAR]* @.str5, i32 0, i32 0
    %14 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str3, i32 0, i32 0), %CHAR* %13) nounwind
    
    ; Out.Real(this^.z)
    %15 = load %Test.Point3D* %this
    %16 = getelementptr inbounds %Test.Point3DRec* %15, i32 0, i32 3
    %17 = load %Test.PointType* %16
    %18 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), double %17) nounwind
    
    ; Out.String(")")
    %19 = getelementptr inbounds [2 x %CHAR]* @.str6, i32 0, i32 0
    %20 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str3, i32 0, i32 0), %CHAR* %19) nounwind
    
    ret void 
}

define %Test.Point2D @Test.Point2DRec.Add(%Test.Point2D* %this, %Test.Point2D %$that) nounwind {
    
    %that = alloca %Test.Point2D
    store %Test.Point2D %$that, %Test.Point2D* %that
    
    %sum = alloca %Test.Point2D
    
    ; NEW(sum)
    %1 = getelementptr inbounds %Test.Point2DRec* null, i32 1
    %2 = ptrtoint %Test.Point2DRec* %1 to i32
    %3 = call i8* (i32)* @GC_malloc(i32 %2) nounwind
    %4 = bitcast i8* %3 to %Test.Point2DRec*
    store %Test.Point2DRec {i8* bitcast ([6 x i8*]* @.rec0 to i8*), %Test.PointType zeroinitializer, %Test.PointType zeroinitializer}, %Test.Point2DRec* %4
    store %Test.Point2DRec* %4, %Test.Point2DRec** %sum
    
    ; sum^.x := this^.x + that^.x
    %5 = load %Test.Point2D* %sum
    %6 = load %Test.Point2D* %this
    %7 = getelementptr inbounds %Test.Point2DRec* %6, i32 0, i32 1
    %8 = load %Test.PointType* %7
    %9 = load %Test.Point2D* %that
    %10 = getelementptr inbounds %Test.Point2DRec* %9, i32 0, i32 1
    %11 = load %Test.PointType* %10
    %12 = fadd double %8, %11
    %13 = getelementptr inbounds %Test.Point2DRec* %5, i32 0, i32 1
    store %Test.PointType %12, %Test.PointType* %13
    
    ; sum^.y := this^.y + that^.y
    %14 = load %Test.Point2D* %sum
    %15 = load %Test.Point2D* %this
    %16 = getelementptr inbounds %Test.Point2DRec* %15, i32 0, i32 2
    %17 = load %Test.PointType* %16
    %18 = load %Test.Point2D* %that
    %19 = getelementptr inbounds %Test.Point2DRec* %18, i32 0, i32 2
    %20 = load %Test.PointType* %19
    %21 = fadd double %17, %20
    %22 = getelementptr inbounds %Test.Point2DRec* %14, i32 0, i32 2
    store %Test.PointType %21, %Test.PointType* %22
    
    ; RETURN sum
    %23 = load %Test.Point2D* %sum
    ret %Test.Point2D %23
}

define %Test.Point2D @Test.Point2DRec.Sub(%Test.Point2D* %this, %Test.Point2D %$that) nounwind {
    
    %that = alloca %Test.Point2D
    store %Test.Point2D %$that, %Test.Point2D* %that
    
    %dif = alloca %Test.Point2D
    
    ; NEW(dif)
    %1 = getelementptr inbounds %Test.Point2DRec* null, i32 1
    %2 = ptrtoint %Test.Point2DRec* %1 to i32
    %3 = call i8* (i32)* @GC_malloc(i32 %2) nounwind
    %4 = bitcast i8* %3 to %Test.Point2DRec*
    store %Test.Point2DRec {i8* bitcast ([6 x i8*]* @.rec0 to i8*), %Test.PointType zeroinitializer, %Test.PointType zeroinitializer}, %Test.Point2DRec* %4
    store %Test.Point2DRec* %4, %Test.Point2DRec** %dif
    
    ; dif^.x := this^.x - that^.x
    %5 = load %Test.Point2D* %dif
    %6 = load %Test.Point2D* %this
    %7 = getelementptr inbounds %Test.Point2DRec* %6, i32 0, i32 1
    %8 = load %Test.PointType* %7
    %9 = load %Test.Point2D* %that
    %10 = getelementptr inbounds %Test.Point2DRec* %9, i32 0, i32 1
    %11 = load %Test.PointType* %10
    %12 = fsub double %8, %11
    %13 = getelementptr inbounds %Test.Point2DRec* %5, i32 0, i32 1
    store %Test.PointType %12, %Test.PointType* %13
    
    ; dif^.y := this^.y - that^.y
    %14 = load %Test.Point2D* %dif
    %15 = load %Test.Point2D* %this
    %16 = getelementptr inbounds %Test.Point2DRec* %15, i32 0, i32 2
    %17 = load %Test.PointType* %16
    %18 = load %Test.Point2D* %that
    %19 = getelementptr inbounds %Test.Point2DRec* %18, i32 0, i32 2
    %20 = load %Test.PointType* %19
    %21 = fsub double %17, %20
    %22 = getelementptr inbounds %Test.Point2DRec* %14, i32 0, i32 2
    store %Test.PointType %21, %Test.PointType* %22
    
    ; RETURN dif
    %23 = load %Test.Point2D* %dif
    ret %Test.Point2D %23
}

define %Test.Point3D @Test.Point3DRec.Add(%Test.Point3D* %this, %Test.Point3D %$that) nounwind {
    
    %that = alloca %Test.Point3D
    store %Test.Point3D %$that, %Test.Point3D* %that
    
    %sum = alloca %Test.Point3D
    
    ; NEW(sum)
    %1 = getelementptr inbounds %Test.Point3DRec* null, i32 1
    %2 = ptrtoint %Test.Point3DRec* %1 to i32
    %3 = call i8* (i32)* @GC_malloc(i32 %2) nounwind
    %4 = bitcast i8* %3 to %Test.Point3DRec*
    store %Test.Point3DRec {i8* bitcast ([6 x i8*]* @.rec1 to i8*), %Test.PointType zeroinitializer, %Test.PointType zeroinitializer, %Test.PointType zeroinitializer}, %Test.Point3DRec* %4
    store %Test.Point3DRec* %4, %Test.Point3DRec** %sum
    
    ; sum^.x := this^.x + that^.x
    %5 = load %Test.Point3D* %sum
    %6 = load %Test.Point3D* %this
    %7 = getelementptr inbounds %Test.Point3DRec* %6, i32 0, i32 1
    %8 = load %Test.PointType* %7
    %9 = load %Test.Point3D* %that
    %10 = getelementptr inbounds %Test.Point3DRec* %9, i32 0, i32 1
    %11 = load %Test.PointType* %10
    %12 = fadd double %8, %11
    %13 = getelementptr inbounds %Test.Point3DRec* %5, i32 0, i32 1
    store %Test.PointType %12, %Test.PointType* %13
    
    ; sum^.y := this^.y + that^.y
    %14 = load %Test.Point3D* %sum
    %15 = load %Test.Point3D* %this
    %16 = getelementptr inbounds %Test.Point3DRec* %15, i32 0, i32 2
    %17 = load %Test.PointType* %16
    %18 = load %Test.Point3D* %that
    %19 = getelementptr inbounds %Test.Point3DRec* %18, i32 0, i32 2
    %20 = load %Test.PointType* %19
    %21 = fadd double %17, %20
    %22 = getelementptr inbounds %Test.Point3DRec* %14, i32 0, i32 2
    store %Test.PointType %21, %Test.PointType* %22
    
    ; sum^.z := this^.z + that^.z
    %23 = load %Test.Point3D* %sum
    %24 = load %Test.Point3D* %this
    %25 = getelementptr inbounds %Test.Point3DRec* %24, i32 0, i32 3
    %26 = load %Test.PointType* %25
    %27 = load %Test.Point3D* %that
    %28 = getelementptr inbounds %Test.Point3DRec* %27, i32 0, i32 3
    %29 = load %Test.PointType* %28
    %30 = fadd double %26, %29
    %31 = getelementptr inbounds %Test.Point3DRec* %23, i32 0, i32 3
    store %Test.PointType %30, %Test.PointType* %31
    
    ; RETURN sum
    %32 = load %Test.Point3D* %sum
    ret %Test.Point3D %32
}

define %Test.Point3D @Test.Point3DRec.Sub(%Test.Point3D* %this, %Test.Point3D %$that) nounwind {
    
    %that = alloca %Test.Point3D
    store %Test.Point3D %$that, %Test.Point3D* %that
    
    %dif = alloca %Test.Point3D
    
    ; NEW(dif)
    %1 = getelementptr inbounds %Test.Point3DRec* null, i32 1
    %2 = ptrtoint %Test.Point3DRec* %1 to i32
    %3 = call i8* (i32)* @GC_malloc(i32 %2) nounwind
    %4 = bitcast i8* %3 to %Test.Point3DRec*
    store %Test.Point3DRec {i8* bitcast ([6 x i8*]* @.rec1 to i8*), %Test.PointType zeroinitializer, %Test.PointType zeroinitializer, %Test.PointType zeroinitializer}, %Test.Point3DRec* %4
    store %Test.Point3DRec* %4, %Test.Point3DRec** %dif
    
    ; dif^.x := this^.x - that^.x
    %5 = load %Test.Point3D* %dif
    %6 = load %Test.Point3D* %this
    %7 = getelementptr inbounds %Test.Point3DRec* %6, i32 0, i32 1
    %8 = load %Test.PointType* %7
    %9 = load %Test.Point3D* %that
    %10 = getelementptr inbounds %Test.Point3DRec* %9, i32 0, i32 1
    %11 = load %Test.PointType* %10
    %12 = fsub double %8, %11
    %13 = getelementptr inbounds %Test.Point3DRec* %5, i32 0, i32 1
    store %Test.PointType %12, %Test.PointType* %13
    
    ; dif^.y := this^.y - that^.y
    %14 = load %Test.Point3D* %dif
    %15 = load %Test.Point3D* %this
    %16 = getelementptr inbounds %Test.Point3DRec* %15, i32 0, i32 2
    %17 = load %Test.PointType* %16
    %18 = load %Test.Point3D* %that
    %19 = getelementptr inbounds %Test.Point3DRec* %18, i32 0, i32 2
    %20 = load %Test.PointType* %19
    %21 = fsub double %17, %20
    %22 = getelementptr inbounds %Test.Point3DRec* %14, i32 0, i32 2
    store %Test.PointType %21, %Test.PointType* %22
    
    ; dif^.z := this^.z - that^.z
    %23 = load %Test.Point3D* %dif
    %24 = load %Test.Point3D* %this
    %25 = getelementptr inbounds %Test.Point3DRec* %24, i32 0, i32 3
    %26 = load %Test.PointType* %25
    %27 = load %Test.Point3D* %that
    %28 = getelementptr inbounds %Test.Point3DRec* %27, i32 0, i32 3
    %29 = load %Test.PointType* %28
    %30 = fsub double %26, %29
    %31 = getelementptr inbounds %Test.Point3DRec* %23, i32 0, i32 3
    store %Test.PointType %30, %Test.PointType* %31
    
    ; RETURN dif
    %32 = load %Test.Point3D* %dif
    ret %Test.Point3D %32
}

define i32 @main() nounwind {
    
    ; a.New(5.200000e+000, 6, 1.240000e+000)
    %1 = load %Test.Point3D* @a
    %2 = icmp eq %Test.Point3D %1, null
    br i1 %2, label %10, label %3
    
; <label>:3                                       ; preds = %0
    %4 = getelementptr inbounds %Test.Point3D %1, i32 0, i32 0
    %5 = load i8** %4
    %6 = bitcast i8* %5 to [6 x i8*]*
    %7 = getelementptr inbounds [6 x i8*]* %6, i32 0, i32 2
    %8 = load i8** %7
    %9 = bitcast i8* %8 to void (%Test.Point3D*, %Test.PointType, %Test.PointType, %Test.PointType)*
    br label %10
    
; <label>:10                                      ; preds = %0, %3
    %11 = phi void (%Test.Point3D*, %Test.PointType, %Test.PointType, %Test.PointType)* [@Test.Point3DRec.New, %0], [%9, %3]
    call void (%Test.Point3D*, %Test.PointType, %Test.PointType, %Test.PointType)* %11(%Test.Point3D* @a, %Test.PointType 5.200000e+000, %Test.PointType -6.000000e+000, %Test.PointType 1.240000e+000) nounwind
    
    ; b.New(3.824000e+000, 3.100000e+002, 12)
    %12 = load %Test.Point3D* @b
    %13 = icmp eq %Test.Point3D %12, null
    br i1 %13, label %21, label %14
    
; <label>:14                                      ; preds = %10
    %15 = getelementptr inbounds %Test.Point3D %12, i32 0, i32 0
    %16 = load i8** %15
    %17 = bitcast i8* %16 to [6 x i8*]*
    %18 = getelementptr inbounds [6 x i8*]* %17, i32 0, i32 2
    %19 = load i8** %18
    %20 = bitcast i8* %19 to void (%Test.Point3D*, %Test.PointType, %Test.PointType, %Test.PointType)*
    br label %21
    
; <label>:21                                      ; preds = %10, %14
    %22 = phi void (%Test.Point3D*, %Test.PointType, %Test.PointType, %Test.PointType)* [@Test.Point3DRec.New, %10], [%20, %14]
    call void (%Test.Point3D*, %Test.PointType, %Test.PointType, %Test.PointType)* %22(%Test.Point3D* @b, %Test.PointType 3.824000e+000, %Test.PointType 3.100000e+002, %Test.PointType 1.200000e+001) nounwind
    
    ; a.Print()
    %23 = load %Test.Point3D* @a
    %24 = icmp eq %Test.Point3D %23, null
    br i1 %24, label %32, label %25
    
; <label>:25                                      ; preds = %21
    %26 = getelementptr inbounds %Test.Point3D %23, i32 0, i32 0
    %27 = load i8** %26
    %28 = bitcast i8* %27 to [6 x i8*]*
    %29 = getelementptr inbounds [6 x i8*]* %28, i32 0, i32 3
    %30 = load i8** %29
    %31 = bitcast i8* %30 to void (%Test.Point3D*)*
    br label %32
    
; <label>:32                                      ; preds = %21, %25
    %33 = phi void (%Test.Point3D*)* [@Test.Point3DRec.Print, %21], [%31, %25]
    call void (%Test.Point3D*)* %33(%Test.Point3D* @a) nounwind
    
    ; Out.Ln()
    %34 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)) nounwind
    
    ; b.Print()
    %35 = load %Test.Point3D* @b
    %36 = icmp eq %Test.Point3D %35, null
    br i1 %36, label %44, label %37
    
; <label>:37                                      ; preds = %32
    %38 = getelementptr inbounds %Test.Point3D %35, i32 0, i32 0
    %39 = load i8** %38
    %40 = bitcast i8* %39 to [6 x i8*]*
    %41 = getelementptr inbounds [6 x i8*]* %40, i32 0, i32 3
    %42 = load i8** %41
    %43 = bitcast i8* %42 to void (%Test.Point3D*)*
    br label %44
    
; <label>:44                                      ; preds = %32, %37
    %45 = phi void (%Test.Point3D*)* [@Test.Point3DRec.Print, %32], [%43, %37]
    call void (%Test.Point3D*)* %45(%Test.Point3D* @b) nounwind
    
    ; Out.Ln()
    %46 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)) nounwind
    
    ; c := a.Add(b)
    %47 = load %Test.Point3D* @a
    %48 = icmp eq %Test.Point3D %47, null
    br i1 %48, label %56, label %49
    
; <label>:49                                      ; preds = %44
    %50 = getelementptr inbounds %Test.Point3D %47, i32 0, i32 0
    %51 = load i8** %50
    %52 = bitcast i8* %51 to [6 x i8*]*
    %53 = getelementptr inbounds [6 x i8*]* %52, i32 0, i32 4
    %54 = load i8** %53
    %55 = bitcast i8* %54 to %Test.Point3D (%Test.Point3D*, %Test.Point3D)*
    br label %56
    
; <label>:56                                      ; preds = %44, %49
    %57 = phi %Test.Point3D (%Test.Point3D*, %Test.Point3D)* [@Test.Point3DRec.Add, %44], [%55, %49]
    %58 = load %Test.Point3D* @b
    %59 = call %Test.Point3D (%Test.Point3D*, %Test.Point3D)* %57(%Test.Point3D* @a, %Test.Point3D %58) nounwind
    store %Test.Point3D %59, %Test.Point3D* @c
    
    ; c.Print()
    %60 = load %Test.Point3D* @c
    %61 = icmp eq %Test.Point3D %60, null
    br i1 %61, label %69, label %62
    
; <label>:62                                      ; preds = %56
    %63 = getelementptr inbounds %Test.Point3D %60, i32 0, i32 0
    %64 = load i8** %63
    %65 = bitcast i8* %64 to [6 x i8*]*
    %66 = getelementptr inbounds [6 x i8*]* %65, i32 0, i32 3
    %67 = load i8** %66
    %68 = bitcast i8* %67 to void (%Test.Point3D*)*
    br label %69
    
; <label>:69                                      ; preds = %56, %62
    %70 = phi void (%Test.Point3D*)* [@Test.Point3DRec.Print, %56], [%68, %62]
    call void (%Test.Point3D*)* %70(%Test.Point3D* @c) nounwind
    
    ; Out.Ln()
    %71 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)) nounwind
    
    ret i32 0
}

