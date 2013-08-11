; Generated 11/08/2013 15:09:39
; GlobalUID e8d43b81-6c3b-48d6-9348-dd7d81a27f96
; 
; LLVM IR file for module "Simple"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET = type i64
%Simple.Vector1Rec = type {i8*, i32}
%Simple.Vector1 = type %Simple.Vector1Rec*
%Simple.Vector2Rec = type {i8*, i32, i32}
%Simple.Vector2 = type %Simple.Vector2Rec*
%Simple.Vector3Rec = type {i8*, i32, i32, i32}
%Simple.Vector3 = type %Simple.Vector3Rec*

@.rec0 = global [5 x i8*] [
    i8* getelementptr inbounds ([11 x %CHAR]* @.str0, i32 0, i32 0),
    i8* null,
    i8* bitcast (void (%Simple.Vector1*, i32)* @Simple.Vector1Rec.SetX to i8*),
    i8* bitcast (i32 (%Simple.Vector1*)* @Simple.Vector1Rec.GetX to i8*),
    i8* bitcast (void (%Simple.Vector1*)* @Simple.Vector1Rec.Print to i8*)
]

@.rec1 = global [7 x i8*] [
    i8* getelementptr inbounds ([11 x %CHAR]* @.str1, i32 0, i32 0),
    i8* bitcast ([5 x i8*]* @.rec0 to i8*),
    i8* bitcast (void (%Simple.Vector1*, i32)* @Simple.Vector1Rec.SetX to i8*),
    i8* bitcast (i32 (%Simple.Vector1*)* @Simple.Vector1Rec.GetX to i8*),
    i8* bitcast (void (%Simple.Vector2*)* @Simple.Vector2Rec.Print to i8*),
    i8* bitcast (void (%Simple.Vector2*, i32)* @Simple.Vector2Rec.SetY to i8*),
    i8* bitcast (i32 (%Simple.Vector2*)* @Simple.Vector2Rec.GetY to i8*)
]

@.rec2 = global [9 x i8*] [
    i8* getelementptr inbounds ([11 x %CHAR]* @.str2, i32 0, i32 0),
    i8* bitcast ([7 x i8*]* @.rec1 to i8*),
    i8* bitcast (void (%Simple.Vector1*, i32)* @Simple.Vector1Rec.SetX to i8*),
    i8* bitcast (i32 (%Simple.Vector1*)* @Simple.Vector1Rec.GetX to i8*),
    i8* bitcast (void (%Simple.Vector3*)* @Simple.Vector3Rec.Print to i8*),
    i8* bitcast (void (%Simple.Vector2*, i32)* @Simple.Vector2Rec.SetY to i8*),
    i8* bitcast (i32 (%Simple.Vector2*)* @Simple.Vector2Rec.GetY to i8*),
    i8* bitcast (void (%Simple.Vector3*, i32)* @Simple.Vector3Rec.SetZ to i8*),
    i8* bitcast (i32 (%Simple.Vector3*)* @Simple.Vector3Rec.GetZ to i8*)
]

@.str0 = private constant [11 x i8] c"Vector1Rec\00"
@.str1 = private constant [11 x i8] c"Vector2Rec\00"
@.str2 = private constant [11 x i8] c"Vector3Rec\00"
@.str3 = private constant [2 x i8] c"(\00"
@.str4 = private constant [3 x i8] c"%s\00"
@.str5 = private constant [3 x i8] c"%i\00"
@.str6 = private constant [2 x i8] c")\00"
@.str7 = private constant [2 x i8] c"\0A\00"
@.str8 = private constant [2 x i8] c" \00"

declare i32 @printf(%CHAR*, ...) nounwind 
declare noalias i8* @GC_malloc(i32) 

@Simple.A = global %Simple.Vector1 null
@Simple.B = global %Simple.Vector2 null
@Simple.C = global %Simple.Vector3 null

define void @Simple.Vector1Rec.SetX(%Simple.Vector1* %this, i32 %$val) nounwind {
    
    %val = alloca i32
    store i32 %$val, i32* %val
    
    ; this^.x := val
    %1 = load %Simple.Vector1* %this
    %2 = load i32* %val
    %3 = getelementptr inbounds %Simple.Vector1Rec* %1, i32 0, i32 1
    store i32 %2, i32* %3
    
    ret void 
}

define i32 @Simple.Vector1Rec.GetX(%Simple.Vector1* %this) nounwind {
    
    ; RETURN this^.x
    %1 = load %Simple.Vector1* %this
    %2 = getelementptr inbounds %Simple.Vector1Rec* %1, i32 0, i32 1
    %3 = load i32* %2
    ret i32 %3
}

define void @Simple.Vector1Rec.Print(%Simple.Vector1* %this) nounwind {
    
    ; Out.String("(")
    %1 = getelementptr inbounds [2 x %CHAR]* @.str3, i32 0, i32 0
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %1) nounwind
    
    ; Out.Integer(this^.x)
    %3 = load %Simple.Vector1* %this
    %4 = getelementptr inbounds %Simple.Vector1Rec* %3, i32 0, i32 1
    %5 = load i32* %4
    %6 = sext i32 %5 to i64
    %7 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %6) nounwind
    
    ; Out.String(")")
    %8 = getelementptr inbounds [2 x %CHAR]* @.str6, i32 0, i32 0
    %9 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %8) nounwind
    
    ; Out.Ln()
    %10 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)) nounwind
    
    ret void 
}

define void @Simple.Vector2Rec.SetY(%Simple.Vector2* %this, i32 %$val) nounwind {
    
    %val = alloca i32
    store i32 %$val, i32* %val
    
    ; this^.y := val
    %1 = load %Simple.Vector2* %this
    %2 = load i32* %val
    %3 = getelementptr inbounds %Simple.Vector2Rec* %1, i32 0, i32 2
    store i32 %2, i32* %3
    
    ret void 
}

define i32 @Simple.Vector2Rec.GetY(%Simple.Vector2* %this) nounwind {
    
    ; RETURN this^.y
    %1 = load %Simple.Vector2* %this
    %2 = getelementptr inbounds %Simple.Vector2Rec* %1, i32 0, i32 2
    %3 = load i32* %2
    ret i32 %3
}

define void @Simple.Vector2Rec.Print(%Simple.Vector2* %this) nounwind {
    
    ; Out.String("(")
    %1 = getelementptr inbounds [2 x %CHAR]* @.str3, i32 0, i32 0
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %1) nounwind
    
    ; Out.Integer(this^.x)
    %3 = load %Simple.Vector2* %this
    %4 = getelementptr inbounds %Simple.Vector2Rec* %3, i32 0, i32 1
    %5 = load i32* %4
    %6 = sext i32 %5 to i64
    %7 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %6) nounwind
    
    ; Out.String(" ")
    %8 = getelementptr inbounds [2 x %CHAR]* @.str8, i32 0, i32 0
    %9 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %8) nounwind
    
    ; Out.Integer(this^.y)
    %10 = load %Simple.Vector2* %this
    %11 = getelementptr inbounds %Simple.Vector2Rec* %10, i32 0, i32 2
    %12 = load i32* %11
    %13 = sext i32 %12 to i64
    %14 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %13) nounwind
    
    ; Out.String(")")
    %15 = getelementptr inbounds [2 x %CHAR]* @.str6, i32 0, i32 0
    %16 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %15) nounwind
    
    ; Out.Ln()
    %17 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)) nounwind
    
    ret void 
}

define void @Simple.Vector3Rec.SetZ(%Simple.Vector3* %this, i32 %$val) nounwind {
    
    %val = alloca i32
    store i32 %$val, i32* %val
    
    ; this^.z := val
    %1 = load %Simple.Vector3* %this
    %2 = load i32* %val
    %3 = getelementptr inbounds %Simple.Vector3Rec* %1, i32 0, i32 3
    store i32 %2, i32* %3
    
    ret void 
}

define i32 @Simple.Vector3Rec.GetZ(%Simple.Vector3* %this) nounwind {
    
    ; RETURN this^.z
    %1 = load %Simple.Vector3* %this
    %2 = getelementptr inbounds %Simple.Vector3Rec* %1, i32 0, i32 3
    %3 = load i32* %2
    ret i32 %3
}

define void @Simple.Vector3Rec.Print(%Simple.Vector3* %this) nounwind {
    
    ; Out.String("(")
    %1 = getelementptr inbounds [2 x %CHAR]* @.str3, i32 0, i32 0
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %1) nounwind
    
    ; Out.Integer(this^.x)
    %3 = load %Simple.Vector3* %this
    %4 = getelementptr inbounds %Simple.Vector3Rec* %3, i32 0, i32 1
    %5 = load i32* %4
    %6 = sext i32 %5 to i64
    %7 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %6) nounwind
    
    ; Out.String(" ")
    %8 = getelementptr inbounds [2 x %CHAR]* @.str8, i32 0, i32 0
    %9 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %8) nounwind
    
    ; Out.Integer(this^.y)
    %10 = load %Simple.Vector3* %this
    %11 = getelementptr inbounds %Simple.Vector3Rec* %10, i32 0, i32 2
    %12 = load i32* %11
    %13 = sext i32 %12 to i64
    %14 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %13) nounwind
    
    ; Out.String(" ")
    %15 = getelementptr inbounds [2 x %CHAR]* @.str8, i32 0, i32 0
    %16 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %15) nounwind
    
    ; Out.Integer(this^.z)
    %17 = load %Simple.Vector3* %this
    %18 = getelementptr inbounds %Simple.Vector3Rec* %17, i32 0, i32 3
    %19 = load i32* %18
    %20 = sext i32 %19 to i64
    %21 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %20) nounwind
    
    ; Out.String(")")
    %22 = getelementptr inbounds [2 x %CHAR]* @.str6, i32 0, i32 0
    %23 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %22) nounwind
    
    ; Out.Ln()
    %24 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)) nounwind
    
    ret void 
}

define i32 @main() nounwind {
    
    ; NEW(C)
    %1 = getelementptr inbounds %Simple.Vector3Rec* null, i32 1
    %2 = ptrtoint %Simple.Vector3Rec* %1 to i32
    %3 = call i8* (i32)* @GC_malloc(i32 %2) nounwind
    %4 = bitcast i8* %3 to %Simple.Vector3Rec*
    store %Simple.Vector3Rec {i8* bitcast ([9 x i8*]* @.rec2 to i8*), i32 zeroinitializer, i32 zeroinitializer, i32 zeroinitializer}, %Simple.Vector3Rec* %4
    store %Simple.Vector3Rec* %4, %Simple.Vector3Rec** @Simple.C
    
    ; C.Print()
    %5 = load %Simple.Vector3* @Simple.C
    %6 = icmp eq %Simple.Vector3 %5, null
    br i1 %6, label %14, label %7
    
; <label>:7                                       ; preds = %0
    %8 = getelementptr inbounds %Simple.Vector3 %5, i32 0, i32 0
    %9 = load i8** %8
    %10 = bitcast i8* %9 to [9 x i8*]*
    %11 = getelementptr inbounds [9 x i8*]* %10, i32 0, i32 4
    %12 = load i8** %11
    %13 = bitcast i8* %12 to void (%Simple.Vector3*)*
    br label %14
    
; <label>:14                                      ; preds = %0, %7
    %15 = phi void (%Simple.Vector3*)* [@Simple.Vector3Rec.Print, %0], [%13, %7]
    call void (%Simple.Vector3*)* %15(%Simple.Vector3* @Simple.C) nounwind
    
    ; C.SetY(3)
    %16 = load %Simple.Vector3* @Simple.C
    %17 = icmp eq %Simple.Vector3 %16, null
    br i1 %17, label %25, label %18
    
; <label>:18                                      ; preds = %14
    %19 = getelementptr inbounds %Simple.Vector3 %16, i32 0, i32 0
    %20 = load i8** %19
    %21 = bitcast i8* %20 to [9 x i8*]*
    %22 = getelementptr inbounds [9 x i8*]* %21, i32 0, i32 5
    %23 = load i8** %22
    %24 = bitcast i8* %23 to void (%Simple.Vector2*, i32)*
    br label %25
    
; <label>:25                                      ; preds = %14, %18
    %26 = phi void (%Simple.Vector2*, i32)* [@Simple.Vector2Rec.SetY, %14], [%24, %18]
    %27 = bitcast %Simple.Vector3* @Simple.C to %Simple.Vector2*
    call void (%Simple.Vector2*, i32)* %26(%Simple.Vector2* %27, i32 -3) nounwind
    
    ; C.SetZ(8)
    %28 = load %Simple.Vector3* @Simple.C
    %29 = icmp eq %Simple.Vector3 %28, null
    br i1 %29, label %37, label %30
    
; <label>:30                                      ; preds = %25
    %31 = getelementptr inbounds %Simple.Vector3 %28, i32 0, i32 0
    %32 = load i8** %31
    %33 = bitcast i8* %32 to [9 x i8*]*
    %34 = getelementptr inbounds [9 x i8*]* %33, i32 0, i32 7
    %35 = load i8** %34
    %36 = bitcast i8* %35 to void (%Simple.Vector3*, i32)*
    br label %37
    
; <label>:37                                      ; preds = %25, %30
    %38 = phi void (%Simple.Vector3*, i32)* [@Simple.Vector3Rec.SetZ, %25], [%36, %30]
    call void (%Simple.Vector3*, i32)* %38(%Simple.Vector3* @Simple.C, i32 8) nounwind
    
    ; C.Print()
    %39 = load %Simple.Vector3* @Simple.C
    %40 = icmp eq %Simple.Vector3 %39, null
    br i1 %40, label %48, label %41
    
; <label>:41                                      ; preds = %37
    %42 = getelementptr inbounds %Simple.Vector3 %39, i32 0, i32 0
    %43 = load i8** %42
    %44 = bitcast i8* %43 to [9 x i8*]*
    %45 = getelementptr inbounds [9 x i8*]* %44, i32 0, i32 4
    %46 = load i8** %45
    %47 = bitcast i8* %46 to void (%Simple.Vector3*)*
    br label %48
    
; <label>:48                                      ; preds = %37, %41
    %49 = phi void (%Simple.Vector3*)* [@Simple.Vector3Rec.Print, %37], [%47, %41]
    call void (%Simple.Vector3*)* %49(%Simple.Vector3* @Simple.C) nounwind
    
    ; B := C
    %50 = load %Simple.Vector3* @Simple.C
    %51 = bitcast %Simple.Vector3 %50 to %Simple.Vector2
    store %Simple.Vector2 %51, %Simple.Vector2* @Simple.B
    
    ; B.Print()
    %52 = load %Simple.Vector2* @Simple.B
    %53 = icmp eq %Simple.Vector2 %52, null
    br i1 %53, label %61, label %54
    
; <label>:54                                      ; preds = %48
    %55 = getelementptr inbounds %Simple.Vector2 %52, i32 0, i32 0
    %56 = load i8** %55
    %57 = bitcast i8* %56 to [7 x i8*]*
    %58 = getelementptr inbounds [7 x i8*]* %57, i32 0, i32 4
    %59 = load i8** %58
    %60 = bitcast i8* %59 to void (%Simple.Vector2*)*
    br label %61
    
; <label>:61                                      ; preds = %48, %54
    %62 = phi void (%Simple.Vector2*)* [@Simple.Vector2Rec.Print, %48], [%60, %54]
    call void (%Simple.Vector2*)* %62(%Simple.Vector2* @Simple.B) nounwind
    
    ; A := C
    %63 = load %Simple.Vector3* @Simple.C
    %64 = bitcast %Simple.Vector3 %63 to %Simple.Vector1
    store %Simple.Vector1 %64, %Simple.Vector1* @Simple.A
    
    ; A.Print()
    %65 = load %Simple.Vector1* @Simple.A
    %66 = icmp eq %Simple.Vector1 %65, null
    br i1 %66, label %74, label %67
    
; <label>:67                                      ; preds = %61
    %68 = getelementptr inbounds %Simple.Vector1 %65, i32 0, i32 0
    %69 = load i8** %68
    %70 = bitcast i8* %69 to [5 x i8*]*
    %71 = getelementptr inbounds [5 x i8*]* %70, i32 0, i32 4
    %72 = load i8** %71
    %73 = bitcast i8* %72 to void (%Simple.Vector1*)*
    br label %74
    
; <label>:74                                      ; preds = %61, %67
    %75 = phi void (%Simple.Vector1*)* [@Simple.Vector1Rec.Print, %61], [%73, %67]
    call void (%Simple.Vector1*)* %75(%Simple.Vector1* @Simple.A) nounwind
    
    ret i32 0
}

