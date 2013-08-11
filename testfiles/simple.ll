; Generated 11/08/2013 03:03:51
; GlobalUID 0b470034-7d4b-49b2-bb39-dbab590d5349
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
    
    ; C.SetX(5)
    %5 = load %Simple.Vector3* @Simple.C
    %6 = icmp eq %Simple.Vector3 %5, null
    br i1 %6, label %14, label %7
    
; <label>:7                                       ; preds = %0
    %8 = getelementptr inbounds %Simple.Vector3 %5, i32 0, i32 0
    %9 = load i8** %8
    %10 = bitcast i8* %9 to [9 x i8*]*
    %11 = getelementptr inbounds [9 x i8*]* %10, i32 0, i32 2
    %12 = load i8** %11
    %13 = bitcast i8* %12 to void (%Simple.Vector1*, i32)*
    br label %14
    
; <label>:14                                      ; preds = %0, %7
    %15 = select i1 %6, void (%Simple.Vector1*, i32)* @Simple.Vector1Rec.SetX, void (%Simple.Vector1*, i32)* %13
    %16 = bitcast %Simple.Vector3* @Simple.C to %Simple.Vector1*
    call void (%Simple.Vector1*, i32)* %15(%Simple.Vector1* %16, i32 5) nounwind
    
    ; C.SetY(3)
    %17 = load %Simple.Vector3* @Simple.C
    %18 = icmp eq %Simple.Vector3 %17, null
    br i1 %18, label %26, label %19
    
; <label>:19                                      ; preds = %14
    %20 = getelementptr inbounds %Simple.Vector3 %17, i32 0, i32 0
    %21 = load i8** %20
    %22 = bitcast i8* %21 to [9 x i8*]*
    %23 = getelementptr inbounds [9 x i8*]* %22, i32 0, i32 5
    %24 = load i8** %23
    %25 = bitcast i8* %24 to void (%Simple.Vector2*, i32)*
    br label %26
    
; <label>:26                                      ; preds = %14, %19
    %27 = select i1 %18, void (%Simple.Vector2*, i32)* @Simple.Vector2Rec.SetY, void (%Simple.Vector2*, i32)* %25
    %28 = bitcast %Simple.Vector3* @Simple.C to %Simple.Vector2*
    call void (%Simple.Vector2*, i32)* %27(%Simple.Vector2* %28, i32 -3) nounwind
    
    ; C.SetZ(8)
    %29 = load %Simple.Vector3* @Simple.C
    %30 = icmp eq %Simple.Vector3 %29, null
    br i1 %30, label %38, label %31
    
; <label>:31                                      ; preds = %26
    %32 = getelementptr inbounds %Simple.Vector3 %29, i32 0, i32 0
    %33 = load i8** %32
    %34 = bitcast i8* %33 to [9 x i8*]*
    %35 = getelementptr inbounds [9 x i8*]* %34, i32 0, i32 7
    %36 = load i8** %35
    %37 = bitcast i8* %36 to void (%Simple.Vector3*, i32)*
    br label %38
    
; <label>:38                                      ; preds = %26, %31
    %39 = select i1 %30, void (%Simple.Vector3*, i32)* @Simple.Vector3Rec.SetZ, void (%Simple.Vector3*, i32)* %37
    call void (%Simple.Vector3*, i32)* %39(%Simple.Vector3* @Simple.C, i32 8) nounwind
    
    ; C.Print()
    %40 = load %Simple.Vector3* @Simple.C
    %41 = icmp eq %Simple.Vector3 %40, null
    br i1 %41, label %49, label %42
    
; <label>:42                                      ; preds = %38
    %43 = getelementptr inbounds %Simple.Vector3 %40, i32 0, i32 0
    %44 = load i8** %43
    %45 = bitcast i8* %44 to [9 x i8*]*
    %46 = getelementptr inbounds [9 x i8*]* %45, i32 0, i32 4
    %47 = load i8** %46
    %48 = bitcast i8* %47 to void (%Simple.Vector3*)*
    br label %49
    
; <label>:49                                      ; preds = %38, %42
    %50 = select i1 %41, void (%Simple.Vector3*)* @Simple.Vector3Rec.Print, void (%Simple.Vector3*)* %48
    call void (%Simple.Vector3*)* %50(%Simple.Vector3* @Simple.C) nounwind
    
    ; B := C
    %51 = load %Simple.Vector3* @Simple.C
    %52 = bitcast %Simple.Vector3 %51 to %Simple.Vector2
    store %Simple.Vector2 %52, %Simple.Vector2* @Simple.B
    
    ; B.Print()
    %53 = load %Simple.Vector2* @Simple.B
    %54 = icmp eq %Simple.Vector2 %53, null
    br i1 %54, label %62, label %55
    
; <label>:55                                      ; preds = %49
    %56 = getelementptr inbounds %Simple.Vector2 %53, i32 0, i32 0
    %57 = load i8** %56
    %58 = bitcast i8* %57 to [7 x i8*]*
    %59 = getelementptr inbounds [7 x i8*]* %58, i32 0, i32 4
    %60 = load i8** %59
    %61 = bitcast i8* %60 to void (%Simple.Vector2*)*
    br label %62
    
; <label>:62                                      ; preds = %49, %55
    %63 = select i1 %54, void (%Simple.Vector2*)* @Simple.Vector2Rec.Print, void (%Simple.Vector2*)* %61
    call void (%Simple.Vector2*)* %63(%Simple.Vector2* @Simple.B) nounwind
    
    ; A := C
    %64 = load %Simple.Vector3* @Simple.C
    %65 = bitcast %Simple.Vector3 %64 to %Simple.Vector1
    store %Simple.Vector1 %65, %Simple.Vector1* @Simple.A
    
    ; A.Print()
    %66 = load %Simple.Vector1* @Simple.A
    %67 = icmp eq %Simple.Vector1 %66, null
    br i1 %67, label %75, label %68
    
; <label>:68                                      ; preds = %62
    %69 = getelementptr inbounds %Simple.Vector1 %66, i32 0, i32 0
    %70 = load i8** %69
    %71 = bitcast i8* %70 to [5 x i8*]*
    %72 = getelementptr inbounds [5 x i8*]* %71, i32 0, i32 4
    %73 = load i8** %72
    %74 = bitcast i8* %73 to void (%Simple.Vector1*)*
    br label %75
    
; <label>:75                                      ; preds = %62, %68
    %76 = select i1 %67, void (%Simple.Vector1*)* @Simple.Vector1Rec.Print, void (%Simple.Vector1*)* %74
    call void (%Simple.Vector1*)* %76(%Simple.Vector1* @Simple.A) nounwind
    
    ret i32 0
}

