; Generated 11/08/2013 23:46:39
; GlobalUID 93cebb73-9cb5-450f-9431-12c1959a85b2
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
@.str9 = private constant [15 x i8] c"B is a Vector2\00"
@.str10 = private constant [19 x i8] c"B is not a Vector2\00"

declare i32 @printf(%CHAR*, ...) nounwind 
declare noalias i8* @GC_malloc(i32) 

@Simple.A = global %Simple.Vector1 zeroinitializer
@Simple.B = global %Simple.Vector2 null
@Simple.C = global %Simple.Vector3 zeroinitializer

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
    
    ; NEW(B)
    %1 = getelementptr inbounds %Simple.Vector2Rec* null, i32 1
    %2 = ptrtoint %Simple.Vector2Rec* %1 to i32
    %3 = call i8* (i32)* @GC_malloc(i32 %2) nounwind
    %4 = bitcast i8* %3 to %Simple.Vector2Rec*
    store %Simple.Vector2Rec {i8* bitcast ([7 x i8*]* @.rec1 to i8*), i32 zeroinitializer, i32 zeroinitializer}, %Simple.Vector2Rec* %4
    store %Simple.Vector2Rec* %4, %Simple.Vector2Rec** @Simple.B
    
    ; IF BVector1 THEN
    %5 = load %Simple.Vector2* @Simple.B
    %6 = icmp eq %Simple.Vector2 %5, null
    br i1 %6, label %18, label %7
    
; <label>:7                                       ; preds = %0
    %8 = getelementptr inbounds %Simple.Vector2 %5, i32 0, i32 0
    %9 = load i8** %8
    br label %10
    
; <label>:10                                      ; preds = %7, %13
    %11 = phi i8* [%9, %7], [%17, %13]
    %12 = icmp eq i8* %11, null
    br i1 %12, label %18, label %13
    
; <label>:13                                      ; preds = %10
    %14 = icmp eq i8* %11, bitcast ([5 x i8*]* @.rec0 to i8*)
    %15 = bitcast i8* %11 to i8**
    %16 = getelementptr inbounds i8** %15, i32 1
    %17 = load i8** %16
    br i1 %14, label %18, label %10
    
; <label>:18                                      ; preds = %0, %10, %13
    %19 = phi i1 [0, %0], [0, %10], [1, %13]
    br i1 %19, label %20, label %24
    
; <label>:20                                      ; preds = %18
    
    ; Out.String("B is a Vector2")
    %21 = getelementptr inbounds [15 x %CHAR]* @.str9, i32 0, i32 0
    %22 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %21) nounwind
    
    ; Out.Ln()
    %23 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)) nounwind
    
    br label %28
    
; <label>:24                                      ; preds = %18
    
    ; Out.String("B is not a Vector2")
    %25 = getelementptr inbounds [19 x %CHAR]* @.str10, i32 0, i32 0
    %26 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %25) nounwind
    
    ; Out.Ln()
    %27 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)) nounwind
    
    br label %28
    
; <label>:28                                      ; preds = %20, %24
    
    ret i32 0
}

