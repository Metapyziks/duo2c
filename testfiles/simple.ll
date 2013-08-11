; Generated 12/08/2013 00:14:06
; GlobalUID 4d6eecfc-48d6-4172-a145-4c06b0eb755c
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
@.str3 = private constant [4 x i8] c"NIL\00"
@.str4 = private constant [3 x i8] c"%s\00"
@.str5 = private constant [2 x i8] c"\0A\00"
@.str6 = private constant [2 x i8] c"(\00"
@.str7 = private constant [3 x i8] c"%i\00"
@.str8 = private constant [2 x i8] c")\00"
@.str9 = private constant [2 x i8] c" \00"
@.str10 = private constant [14 x i8] c" is a Vector3\00"
@.str11 = private constant [14 x i8] c" is a Vector2\00"
@.str12 = private constant [14 x i8] c" is a Vector1\00"
@.str13 = private constant [17 x i8] c" is not a vector\00"
@.str14 = private constant [5 x i8] c"C = \00"
@.str15 = private constant [2 x i8] c"C\00"
@.str16 = private constant [5 x i8] c"A = \00"
@.str17 = private constant [2 x i8] c"A\00"
@.str18 = private constant [5 x i8] c"B = \00"
@.str19 = private constant [2 x i8] c"B\00"

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
    
    ; IF this = NIL THEN
    %1 = load %Simple.Vector1* %this
    %2 = icmp eq %Simple.Vector1 %1, null
    br i1 %2, label %3, label %7
    
; <label>:3                                       ; preds = %0
    
    ; Out.String("NIL")
    %4 = getelementptr inbounds [4 x %CHAR]* @.str3, i32 0, i32 0
    %5 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %4) nounwind
    
    ; Out.Ln()
    %6 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str5, i32 0, i32 0)) nounwind
    
    ; RETURN
    ret void
    
; <label>:7                                       ; preds = %0
    
    ; Out.String("(")
    %8 = getelementptr inbounds [2 x %CHAR]* @.str6, i32 0, i32 0
    %9 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %8) nounwind
    
    ; Out.Integer(this^.x)
    %10 = load %Simple.Vector1* %this
    %11 = getelementptr inbounds %Simple.Vector1Rec* %10, i32 0, i32 1
    %12 = load i32* %11
    %13 = sext i32 %12 to i64
    %14 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str7, i32 0, i32 0), i64 %13) nounwind
    
    ; Out.String(")")
    %15 = getelementptr inbounds [2 x %CHAR]* @.str8, i32 0, i32 0
    %16 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %15) nounwind
    
    ; Out.Ln()
    %17 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str5, i32 0, i32 0)) nounwind
    
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
    
    ; IF this = NIL THEN
    %1 = load %Simple.Vector2* %this
    %2 = icmp eq %Simple.Vector2 %1, null
    br i1 %2, label %3, label %7
    
; <label>:3                                       ; preds = %0
    
    ; Out.String("NIL")
    %4 = getelementptr inbounds [4 x %CHAR]* @.str3, i32 0, i32 0
    %5 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %4) nounwind
    
    ; Out.Ln()
    %6 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str5, i32 0, i32 0)) nounwind
    
    ; RETURN
    ret void
    
; <label>:7                                       ; preds = %0
    
    ; Out.String("(")
    %8 = getelementptr inbounds [2 x %CHAR]* @.str6, i32 0, i32 0
    %9 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %8) nounwind
    
    ; Out.Integer(this^.x)
    %10 = load %Simple.Vector2* %this
    %11 = getelementptr inbounds %Simple.Vector2Rec* %10, i32 0, i32 1
    %12 = load i32* %11
    %13 = sext i32 %12 to i64
    %14 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str7, i32 0, i32 0), i64 %13) nounwind
    
    ; Out.String(" ")
    %15 = getelementptr inbounds [2 x %CHAR]* @.str9, i32 0, i32 0
    %16 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %15) nounwind
    
    ; Out.Integer(this^.y)
    %17 = load %Simple.Vector2* %this
    %18 = getelementptr inbounds %Simple.Vector2Rec* %17, i32 0, i32 2
    %19 = load i32* %18
    %20 = sext i32 %19 to i64
    %21 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str7, i32 0, i32 0), i64 %20) nounwind
    
    ; Out.String(")")
    %22 = getelementptr inbounds [2 x %CHAR]* @.str8, i32 0, i32 0
    %23 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %22) nounwind
    
    ; Out.Ln()
    %24 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str5, i32 0, i32 0)) nounwind
    
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
    
    ; IF this = NIL THEN
    %1 = load %Simple.Vector3* %this
    %2 = icmp eq %Simple.Vector3 %1, null
    br i1 %2, label %3, label %7
    
; <label>:3                                       ; preds = %0
    
    ; Out.String("NIL")
    %4 = getelementptr inbounds [4 x %CHAR]* @.str3, i32 0, i32 0
    %5 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %4) nounwind
    
    ; Out.Ln()
    %6 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str5, i32 0, i32 0)) nounwind
    
    ; RETURN
    ret void
    
; <label>:7                                       ; preds = %0
    
    ; Out.String("(")
    %8 = getelementptr inbounds [2 x %CHAR]* @.str6, i32 0, i32 0
    %9 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %8) nounwind
    
    ; Out.Integer(this^.x)
    %10 = load %Simple.Vector3* %this
    %11 = getelementptr inbounds %Simple.Vector3Rec* %10, i32 0, i32 1
    %12 = load i32* %11
    %13 = sext i32 %12 to i64
    %14 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str7, i32 0, i32 0), i64 %13) nounwind
    
    ; Out.String(" ")
    %15 = getelementptr inbounds [2 x %CHAR]* @.str9, i32 0, i32 0
    %16 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %15) nounwind
    
    ; Out.Integer(this^.y)
    %17 = load %Simple.Vector3* %this
    %18 = getelementptr inbounds %Simple.Vector3Rec* %17, i32 0, i32 2
    %19 = load i32* %18
    %20 = sext i32 %19 to i64
    %21 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str7, i32 0, i32 0), i64 %20) nounwind
    
    ; Out.String(" ")
    %22 = getelementptr inbounds [2 x %CHAR]* @.str9, i32 0, i32 0
    %23 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %22) nounwind
    
    ; Out.Integer(this^.z)
    %24 = load %Simple.Vector3* %this
    %25 = getelementptr inbounds %Simple.Vector3Rec* %24, i32 0, i32 3
    %26 = load i32* %25
    %27 = sext i32 %26 to i64
    %28 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str7, i32 0, i32 0), i64 %27) nounwind
    
    ; Out.String(")")
    %29 = getelementptr inbounds [2 x %CHAR]* @.str8, i32 0, i32 0
    %30 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %29) nounwind
    
    ; Out.Ln()
    %31 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str5, i32 0, i32 0)) nounwind
    
    ret void 
}

define void @TestType(%Simple.Vector1 %$vec, {i32, %CHAR*} %$name) nounwind {
    
    %vec = alloca %Simple.Vector1
    store %Simple.Vector1 %$vec, %Simple.Vector1* %vec
    
    %name = alloca {i32, %CHAR*}
    store {i32, %CHAR*} %$name, {i32, %CHAR*}* %name
    
    ; IF vec IS Vector3 THEN
    %1 = load %Simple.Vector1* %vec
    %2 = icmp eq %Simple.Vector1 %1, null
    br i1 %2, label %14, label %3
    
; <label>:3                                       ; preds = %0
    %4 = getelementptr inbounds %Simple.Vector1 %1, i32 0, i32 0
    %5 = load i8** %4
    br label %6
    
; <label>:6                                       ; preds = %3, %9
    %7 = phi i8* [%5, %3], [%13, %9]
    %8 = icmp eq i8* %7, null
    br i1 %8, label %14, label %9
    
; <label>:9                                       ; preds = %6
    %10 = icmp eq i8* %7, bitcast ([9 x i8*]* @.rec2 to i8*)
    %11 = bitcast i8* %7 to i8**
    %12 = getelementptr inbounds i8** %11, i32 1
    %13 = load i8** %12
    br i1 %10, label %14, label %6
    
; <label>:14                                      ; preds = %0, %6, %9
    %15 = phi i1 [0, %0], [0, %6], [1, %9]
    br i1 %15, label %16, label %23
    
; <label>:16                                      ; preds = %14
    
    ; Out.String(name)
    %17 = getelementptr inbounds {i32, %CHAR*}* %name, i32 0, i32 1
    %18 = load %CHAR** %17
    %19 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %18) nounwind
    
    ; Out.String(" is a Vector3")
    %20 = getelementptr inbounds [14 x %CHAR]* @.str10, i32 0, i32 0
    %21 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %20) nounwind
    
    ; Out.Ln()
    %22 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str5, i32 0, i32 0)) nounwind
    
    br label %78
    
; <label>:23                                      ; preds = %14
    
    ; IF vec IS Vector2 THEN
    %24 = load %Simple.Vector1* %vec
    %25 = icmp eq %Simple.Vector1 %24, null
    br i1 %25, label %37, label %26
    
; <label>:26                                      ; preds = %23
    %27 = getelementptr inbounds %Simple.Vector1 %24, i32 0, i32 0
    %28 = load i8** %27
    br label %29
    
; <label>:29                                      ; preds = %26, %32
    %30 = phi i8* [%28, %26], [%36, %32]
    %31 = icmp eq i8* %30, null
    br i1 %31, label %37, label %32
    
; <label>:32                                      ; preds = %29
    %33 = icmp eq i8* %30, bitcast ([7 x i8*]* @.rec1 to i8*)
    %34 = bitcast i8* %30 to i8**
    %35 = getelementptr inbounds i8** %34, i32 1
    %36 = load i8** %35
    br i1 %33, label %37, label %29
    
; <label>:37                                      ; preds = %23, %29, %32
    %38 = phi i1 [0, %23], [0, %29], [1, %32]
    br i1 %38, label %39, label %46
    
; <label>:39                                      ; preds = %37
    
    ; Out.String(name)
    %40 = getelementptr inbounds {i32, %CHAR*}* %name, i32 0, i32 1
    %41 = load %CHAR** %40
    %42 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %41) nounwind
    
    ; Out.String(" is a Vector2")
    %43 = getelementptr inbounds [14 x %CHAR]* @.str11, i32 0, i32 0
    %44 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %43) nounwind
    
    ; Out.Ln()
    %45 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str5, i32 0, i32 0)) nounwind
    
    br label %77
    
; <label>:46                                      ; preds = %37
    
    ; IF vec IS Vector1 THEN
    %47 = load %Simple.Vector1* %vec
    %48 = icmp eq %Simple.Vector1 %47, null
    br i1 %48, label %60, label %49
    
; <label>:49                                      ; preds = %46
    %50 = getelementptr inbounds %Simple.Vector1 %47, i32 0, i32 0
    %51 = load i8** %50
    br label %52
    
; <label>:52                                      ; preds = %49, %55
    %53 = phi i8* [%51, %49], [%59, %55]
    %54 = icmp eq i8* %53, null
    br i1 %54, label %60, label %55
    
; <label>:55                                      ; preds = %52
    %56 = icmp eq i8* %53, bitcast ([5 x i8*]* @.rec0 to i8*)
    %57 = bitcast i8* %53 to i8**
    %58 = getelementptr inbounds i8** %57, i32 1
    %59 = load i8** %58
    br i1 %56, label %60, label %52
    
; <label>:60                                      ; preds = %46, %52, %55
    %61 = phi i1 [0, %46], [0, %52], [1, %55]
    br i1 %61, label %62, label %69
    
; <label>:62                                      ; preds = %60
    
    ; Out.String(name)
    %63 = getelementptr inbounds {i32, %CHAR*}* %name, i32 0, i32 1
    %64 = load %CHAR** %63
    %65 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %64) nounwind
    
    ; Out.String(" is a Vector1")
    %66 = getelementptr inbounds [14 x %CHAR]* @.str12, i32 0, i32 0
    %67 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %66) nounwind
    
    ; Out.Ln()
    %68 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str5, i32 0, i32 0)) nounwind
    
    br label %76
    
; <label>:69                                      ; preds = %60
    
    ; Out.String(name)
    %70 = getelementptr inbounds {i32, %CHAR*}* %name, i32 0, i32 1
    %71 = load %CHAR** %70
    %72 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %71) nounwind
    
    ; Out.String(" is not a vector")
    %73 = getelementptr inbounds [17 x %CHAR]* @.str13, i32 0, i32 0
    %74 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %73) nounwind
    
    ; Out.Ln()
    %75 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str5, i32 0, i32 0)) nounwind
    
    br label %76
    
; <label>:76                                      ; preds = %62, %69
    
    br label %77
    
; <label>:77                                      ; preds = %39, %76
    
    br label %78
    
; <label>:78                                      ; preds = %16, %77
    
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
    
    ; C.SetX(56)
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
    %15 = phi void (%Simple.Vector1*, i32)* [@Simple.Vector1Rec.SetX, %0], [%13, %7]
    %16 = bitcast %Simple.Vector3* @Simple.C to %Simple.Vector1*
    call void (%Simple.Vector1*, i32)* %15(%Simple.Vector1* %16, i32 56) nounwind
    
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
    %27 = phi void (%Simple.Vector2*, i32)* [@Simple.Vector2Rec.SetY, %14], [%25, %19]
    %28 = bitcast %Simple.Vector3* @Simple.C to %Simple.Vector2*
    call void (%Simple.Vector2*, i32)* %27(%Simple.Vector2* %28, i32 -3) nounwind
    
    ; C.SetZ(12)
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
    %39 = phi void (%Simple.Vector3*, i32)* [@Simple.Vector3Rec.SetZ, %26], [%37, %31]
    call void (%Simple.Vector3*, i32)* %39(%Simple.Vector3* @Simple.C, i32 12) nounwind
    
    ; Out.String("C = ")
    %40 = getelementptr inbounds [5 x %CHAR]* @.str14, i32 0, i32 0
    %41 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %40) nounwind
    
    ; C.Print()
    %42 = load %Simple.Vector3* @Simple.C
    %43 = icmp eq %Simple.Vector3 %42, null
    br i1 %43, label %51, label %44
    
; <label>:44                                      ; preds = %38
    %45 = getelementptr inbounds %Simple.Vector3 %42, i32 0, i32 0
    %46 = load i8** %45
    %47 = bitcast i8* %46 to [9 x i8*]*
    %48 = getelementptr inbounds [9 x i8*]* %47, i32 0, i32 4
    %49 = load i8** %48
    %50 = bitcast i8* %49 to void (%Simple.Vector3*)*
    br label %51
    
; <label>:51                                      ; preds = %38, %44
    %52 = phi void (%Simple.Vector3*)* [@Simple.Vector3Rec.Print, %38], [%50, %44]
    call void (%Simple.Vector3*)* %52(%Simple.Vector3* @Simple.C) nounwind
    
    ; TestType(C, "C")
    %53 = load %Simple.Vector3* @Simple.C
    %54 = bitcast %Simple.Vector3 %53 to %Simple.Vector1
    call void (%Simple.Vector1, {i32, %CHAR*})* @TestType(%Simple.Vector1 %54, {i32, %CHAR*} {i32 2, %CHAR* getelementptr inbounds ([2 x %CHAR]* @.str15, i32 0, i32 0)}) nounwind
    
    ; NEW(A)
    %55 = getelementptr inbounds %Simple.Vector1Rec* null, i32 1
    %56 = ptrtoint %Simple.Vector1Rec* %55 to i32
    %57 = call i8* (i32)* @GC_malloc(i32 %56) nounwind
    %58 = bitcast i8* %57 to %Simple.Vector1Rec*
    store %Simple.Vector1Rec {i8* bitcast ([5 x i8*]* @.rec0 to i8*), i32 zeroinitializer}, %Simple.Vector1Rec* %58
    store %Simple.Vector1Rec* %58, %Simple.Vector1Rec** @Simple.A
    
    ; A.SetX(C.GetY() * 3)
    %59 = load %Simple.Vector1* @Simple.A
    %60 = icmp eq %Simple.Vector1 %59, null
    br i1 %60, label %68, label %61
    
; <label>:61                                      ; preds = %51
    %62 = getelementptr inbounds %Simple.Vector1 %59, i32 0, i32 0
    %63 = load i8** %62
    %64 = bitcast i8* %63 to [5 x i8*]*
    %65 = getelementptr inbounds [5 x i8*]* %64, i32 0, i32 2
    %66 = load i8** %65
    %67 = bitcast i8* %66 to void (%Simple.Vector1*, i32)*
    br label %68
    
; <label>:68                                      ; preds = %51, %61
    %69 = phi void (%Simple.Vector1*, i32)* [@Simple.Vector1Rec.SetX, %51], [%67, %61]
    %70 = load %Simple.Vector3* @Simple.C
    %71 = icmp eq %Simple.Vector3 %70, null
    br i1 %71, label %79, label %72
    
; <label>:72                                      ; preds = %68
    %73 = getelementptr inbounds %Simple.Vector3 %70, i32 0, i32 0
    %74 = load i8** %73
    %75 = bitcast i8* %74 to [9 x i8*]*
    %76 = getelementptr inbounds [9 x i8*]* %75, i32 0, i32 6
    %77 = load i8** %76
    %78 = bitcast i8* %77 to i32 (%Simple.Vector2*)*
    br label %79
    
; <label>:79                                      ; preds = %68, %72
    %80 = phi i32 (%Simple.Vector2*)* [@Simple.Vector2Rec.GetY, %68], [%78, %72]
    %81 = bitcast %Simple.Vector3* @Simple.C to %Simple.Vector2*
    %82 = call i32 (%Simple.Vector2*)* %80(%Simple.Vector2* %81) nounwind
    %83 = mul i32 %82, 3
    call void (%Simple.Vector1*, i32)* %69(%Simple.Vector1* @Simple.A, i32 %83) nounwind
    
    ; Out.String("A = ")
    %84 = getelementptr inbounds [5 x %CHAR]* @.str16, i32 0, i32 0
    %85 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %84) nounwind
    
    ; A.Print()
    %86 = load %Simple.Vector1* @Simple.A
    %87 = icmp eq %Simple.Vector1 %86, null
    br i1 %87, label %95, label %88
    
; <label>:88                                      ; preds = %79
    %89 = getelementptr inbounds %Simple.Vector1 %86, i32 0, i32 0
    %90 = load i8** %89
    %91 = bitcast i8* %90 to [5 x i8*]*
    %92 = getelementptr inbounds [5 x i8*]* %91, i32 0, i32 4
    %93 = load i8** %92
    %94 = bitcast i8* %93 to void (%Simple.Vector1*)*
    br label %95
    
; <label>:95                                      ; preds = %79, %88
    %96 = phi void (%Simple.Vector1*)* [@Simple.Vector1Rec.Print, %79], [%94, %88]
    call void (%Simple.Vector1*)* %96(%Simple.Vector1* @Simple.A) nounwind
    
    ; TestType(A, "A")
    %97 = load %Simple.Vector1* @Simple.A
    call void (%Simple.Vector1, {i32, %CHAR*})* @TestType(%Simple.Vector1 %97, {i32, %CHAR*} {i32 2, %CHAR* getelementptr inbounds ([2 x %CHAR]* @.str17, i32 0, i32 0)}) nounwind
    
    ; B := C
    %98 = load %Simple.Vector3* @Simple.C
    %99 = bitcast %Simple.Vector3 %98 to %Simple.Vector2
    store %Simple.Vector2 %99, %Simple.Vector2* @Simple.B
    
    ; Out.String("B = ")
    %100 = getelementptr inbounds [5 x %CHAR]* @.str18, i32 0, i32 0
    %101 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %100) nounwind
    
    ; B.Print()
    %102 = load %Simple.Vector2* @Simple.B
    %103 = icmp eq %Simple.Vector2 %102, null
    br i1 %103, label %111, label %104
    
; <label>:104                                     ; preds = %95
    %105 = getelementptr inbounds %Simple.Vector2 %102, i32 0, i32 0
    %106 = load i8** %105
    %107 = bitcast i8* %106 to [7 x i8*]*
    %108 = getelementptr inbounds [7 x i8*]* %107, i32 0, i32 4
    %109 = load i8** %108
    %110 = bitcast i8* %109 to void (%Simple.Vector2*)*
    br label %111
    
; <label>:111                                     ; preds = %95, %104
    %112 = phi void (%Simple.Vector2*)* [@Simple.Vector2Rec.Print, %95], [%110, %104]
    call void (%Simple.Vector2*)* %112(%Simple.Vector2* @Simple.B) nounwind
    
    ; TestType(B, "B")
    %113 = load %Simple.Vector2* @Simple.B
    %114 = bitcast %Simple.Vector2 %113 to %Simple.Vector1
    call void (%Simple.Vector1, {i32, %CHAR*})* @TestType(%Simple.Vector1 %114, {i32, %CHAR*} {i32 2, %CHAR* getelementptr inbounds ([2 x %CHAR]* @.str19, i32 0, i32 0)}) nounwind
    
    ; A := NIL
    store %Simple.Vector1 null, %Simple.Vector1* @Simple.A
    
    ; Out.String("A = ")
    %115 = getelementptr inbounds [5 x %CHAR]* @.str16, i32 0, i32 0
    %116 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %115) nounwind
    
    ; A.Print()
    %117 = load %Simple.Vector1* @Simple.A
    %118 = icmp eq %Simple.Vector1 %117, null
    br i1 %118, label %126, label %119
    
; <label>:119                                     ; preds = %111
    %120 = getelementptr inbounds %Simple.Vector1 %117, i32 0, i32 0
    %121 = load i8** %120
    %122 = bitcast i8* %121 to [5 x i8*]*
    %123 = getelementptr inbounds [5 x i8*]* %122, i32 0, i32 4
    %124 = load i8** %123
    %125 = bitcast i8* %124 to void (%Simple.Vector1*)*
    br label %126
    
; <label>:126                                     ; preds = %111, %119
    %127 = phi void (%Simple.Vector1*)* [@Simple.Vector1Rec.Print, %111], [%125, %119]
    call void (%Simple.Vector1*)* %127(%Simple.Vector1* @Simple.A) nounwind
    
    ; TestType(A, "NIL")
    %128 = load %Simple.Vector1* @Simple.A
    call void (%Simple.Vector1, {i32, %CHAR*})* @TestType(%Simple.Vector1 %128, {i32, %CHAR*} {i32 4, %CHAR* getelementptr inbounds ([4 x %CHAR]* @.str3, i32 0, i32 0)}) nounwind
    
    ret i32 0
}

