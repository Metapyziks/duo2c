; Generated 10/08/2013 23:33:33
; GlobalUID 5b0ef5e9-03d1-420b-9263-7311053aacf4
; 
; LLVM IR file for module "Simple"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET  = type i64
%Simple.Vector1Rec = type {i8*, i32}
%Simple.Vector1 = type %Simple.Vector1Rec*
%Simple.Vector2Rec = type {i8*, i32, i32}
%Simple.Vector2 = type %Simple.Vector2Rec*
%Simple.Vector3Rec = type {i8*, i32, i32, i32}
%Simple.Vector3 = type %Simple.Vector3Rec*

@.rec0 = global [5 x i8*] [
    i8* getelementptr inbounds ([11 x %CHAR]* @.str0, i32 0, i32 0),
    i8* null,
    i8* bitcast (void (%Simple.Vector1, i32)* @Simple.Vector1Rec.SetX to i8*),
    i8* bitcast (i32 (%Simple.Vector1)* @Simple.Vector1Rec.GetX to i8*),
    i8* bitcast (void (%Simple.Vector1)* @Simple.Vector1Rec.Print to i8*)
]

@.rec1 = global [7 x i8*] [
    i8* getelementptr inbounds ([11 x  %CHAR]* @.str1, i32 0, i32 0),
    i8* bitcast ([5 x i8*]* @.rec0 to i8*),
    i8* bitcast (void (%Simple.Vector1, i32)* @Simple.Vector1Rec.SetX to i8*),
    i8* bitcast (i32 (%Simple.Vector1)* @Simple.Vector1Rec.GetX to i8*),
    i8* bitcast (void (%Simple.Vector2)* @Simple.Vector2Rec.Print to i8*),
    i8* bitcast (void (%Simple.Vector2, i32)* @Simple.Vector2Rec.SetY to i8*),
    i8* bitcast (i32 (%Simple.Vector2)* @Simple.Vector2Rec.GetY to i8*)
]

@.rec2 = global [9 x i8*] [
    i8* getelementptr inbounds ([11 x  %CHAR]* @.str2, i32 0, i32 0),
    i8* bitcast ([7 x i8*]* @.rec1 to i8*),
    i8* bitcast (void (%Simple.Vector1, i32)* @Simple.Vector1Rec.SetX to i8*),
    i8* bitcast (i32 (%Simple.Vector1)* @Simple.Vector1Rec.GetX to i8*),
    i8* bitcast (void (%Simple.Vector3)* @Simple.Vector3Rec.Print to i8*),
    i8* bitcast (void (%Simple.Vector2, i32)* @Simple.Vector2Rec.SetY to i8*),
    i8* bitcast (i32 (%Simple.Vector2)* @Simple.Vector2Rec.GetY to i8*),
    i8* bitcast (void (%Simple.Vector3, i32)* @Simple.Vector3Rec.SetZ to i8*),
    i8* bitcast (i32 (%Simple.Vector3)* @Simple.Vector3Rec.GetZ to i8*)
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

declare i32     @printf(%CHAR*, ...) nounwind 
declare noalias i8*    @GC_malloc(i32) 

@testRec = private global %Simple.Vector1Rec {i8* bitcast ([5 x i8*]* @.rec0 to i8*), i32 zeroinitializer}
@testPtr = private global %Simple.Vector1 null

define void @Simple.Vector1Rec.SetX(%Simple.Vector1 %$this, i32 %$val) nounwind {
    
    %this = alloca %Simple.Vector1
    store %Simple.Vector1 %$this, %Simple.Vector1* %this
    
    %val  = alloca i32
    store i32 %$val, i32* %val
    
    ; this.x := val
    %1    = load %Simple.Vector1* %this
    %2    = load i32* %val
    %3    = getelementptr inbounds %Simple.Vector1Rec* %1, i32 0, i32 1
    store i32    %2,  i32* %3
    
    ret void 
}

define i32  @Simple.Vector1Rec.GetX(%Simple.Vector1 %$this) nounwind {
    
    %this = alloca %Simple.Vector1
    store %Simple.Vector1 %$this, %Simple.Vector1* %this
    
    ; RETURN this.x
    %1  = load %Simple.Vector1* %this
    %2  = getelementptr inbounds %Simple.Vector1Rec* %1, i32 0, i32 1
    %3  = load i32* %2
    ret i32    %3
}

define void @Simple.Vector1Rec.Print(%Simple.Vector1 %$this) nounwind {
    
    %this = alloca %Simple.Vector1
    store %Simple.Vector1 %$this, %Simple.Vector1* %this
    
    ; Out.String("(")
    %1 = getelementptr inbounds [2 x %CHAR]* @.str3, i32 0, i32 0
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %1) nounwind
    
    ; Out.Integer(this.x)
    %3 = load %Simple.Vector1* %this
    %4 = getelementptr inbounds %Simple.Vector1Rec* %3, i32 0, i32 1
    %5 = load i32* %4
    %6 = sext i32  %5 to i64
    %7 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %6) nounwind
    
    ; Out.String(")")
    %8 = getelementptr inbounds [2 x %CHAR]* @.str6, i32 0, i32 0
    %9 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %8) nounwind
    
    ; Out.Ln()
    %10 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)) nounwind
    
    ret void 
}

define void @Simple.Vector2Rec.SetY(%Simple.Vector2 %$this, i32 %$val) nounwind {
    
    %this = alloca %Simple.Vector2
    store %Simple.Vector2 %$this, %Simple.Vector2* %this
    
    %val  = alloca i32
    store i32 %$val, i32* %val
    
    ; this.y := val
    %1    = load %Simple.Vector2* %this
    %2    = load i32* %val
    %3    = getelementptr inbounds %Simple.Vector2Rec* %1, i32 0, i32 2
    store i32    %2,  i32* %3
    
    ret void 
}

define i32  @Simple.Vector2Rec.GetY(%Simple.Vector2 %$this) nounwind {
    
    %this = alloca %Simple.Vector2
    store %Simple.Vector2 %$this, %Simple.Vector2* %this
    
    ; RETURN this.y
    %1  = load %Simple.Vector2* %this
    %2  = getelementptr inbounds %Simple.Vector2Rec* %1, i32 0, i32 2
    %3  = load i32* %2
    ret i32    %3
}

define void @Simple.Vector2Rec.Print(%Simple.Vector2 %$this) nounwind {
    
    %this = alloca %Simple.Vector2
    store %Simple.Vector2 %$this, %Simple.Vector2* %this
    
    ; Out.String("(")
    %1 = getelementptr inbounds [2 x %CHAR]* @.str3, i32 0, i32 0
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %1) nounwind
    
    ; Out.Integer(this.x)
    %3 = load %Simple.Vector2* %this
    %4 = getelementptr inbounds %Simple.Vector2Rec* %3, i32 0, i32 1
    %5 = load i32* %4
    %6 = sext i32  %5 to i64
    %7 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %6) nounwind
    
    ; Out.String(" ")
    %8 = getelementptr inbounds [2 x %CHAR]* @.str8, i32 0, i32 0
    %9 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %8) nounwind
    
    ; Out.Integer(this.y)
    %10 = load %Simple.Vector2* %this
    %11 = getelementptr inbounds %Simple.Vector2Rec* %10, i32 0, i32 2
    %12 = load i32* %11
    %13 = sext i32  %12 to i64
    %14 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %13) nounwind
    
    ; Out.String(")")
    %15 = getelementptr inbounds [2 x %CHAR]* @.str6, i32 0, i32 0
    %16 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %15) nounwind
    
    ; Out.Ln()
    %17 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)) nounwind
    
    ret void 
}

define void @Simple.Vector3Rec.SetZ(%Simple.Vector3 %$this, i32 %$val) nounwind {
    
    %this = alloca %Simple.Vector3
    store %Simple.Vector3 %$this, %Simple.Vector3* %this
    
    %val  = alloca i32
    store i32 %$val, i32* %val
    
    ; this.z := val
    %1    = load %Simple.Vector3* %this
    %2    = load i32* %val
    %3    = getelementptr inbounds %Simple.Vector3Rec* %1, i32 0, i32 3
    store i32    %2,  i32* %3
    
    ret void 
}

define i32  @Simple.Vector3Rec.GetZ(%Simple.Vector3 %$this) nounwind {
    
    %this = alloca %Simple.Vector3
    store %Simple.Vector3 %$this, %Simple.Vector3* %this
    
    ; RETURN this.z
    %1  = load %Simple.Vector3* %this
    %2  = getelementptr inbounds %Simple.Vector3Rec* %1, i32 0, i32 3
    %3  = load i32* %2
    ret i32    %3
}

define void @Simple.Vector3Rec.Print(%Simple.Vector3 %$this) nounwind {
    
    %this = alloca %Simple.Vector3
    store %Simple.Vector3 %$this, %Simple.Vector3* %this
    
    ; Out.String("(")
    %1 = getelementptr inbounds [2 x %CHAR]* @.str3, i32 0, i32 0
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %1) nounwind
    
    ; Out.Integer(this.x)
    %3 = load %Simple.Vector3* %this
    %4 = getelementptr inbounds %Simple.Vector3Rec* %3, i32 0, i32 1
    %5 = load i32* %4
    %6 = sext i32  %5 to i64
    %7 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %6) nounwind
    
    ; Out.String(" ")
    %8 = getelementptr inbounds [2 x %CHAR]* @.str8, i32 0, i32 0
    %9 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %8) nounwind
    
    ; Out.Integer(this.y)
    %10 = load %Simple.Vector3* %this
    %11 = getelementptr inbounds %Simple.Vector3Rec* %10, i32 0, i32 2
    %12 = load i32* %11
    %13 = sext i32  %12 to i64
    %14 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %13) nounwind
    
    ; Out.String(" ")
    %15 = getelementptr inbounds [2 x %CHAR]* @.str8, i32 0, i32 0
    %16 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %15) nounwind
    
    ; Out.Integer(this.z)
    %17 = load %Simple.Vector3* %this
    %18 = getelementptr inbounds %Simple.Vector3Rec* %17, i32 0, i32 3
    %19 = load i32* %18
    %20 = sext i32  %19 to i64
    %21 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %20) nounwind
    
    ; Out.String(")")
    %22 = getelementptr inbounds [2 x %CHAR]* @.str6, i32 0, i32 0
    %23 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %22) nounwind
    
    ; Out.Ln()
    %24 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)) nounwind
    
    ret void 
}

define i32 @main() {
    
    ; testRec.SetX(5)
    %1   = getelementptr inbounds %Simple.Vector1Rec* @testRec, i32 0, i32 0
    %2   = load i8** %1
    %3   = bitcast i8*  %2 to [5 x i8*]*
    %4   = getelementptr inbounds [5 x i8*]* %3, i32 0, i32 2
    %5   = load i8** %4
    %6   = bitcast i8*  %5 to void (%Simple.Vector1, i32)*
    call void (%Simple.Vector1, i32)* %6(%Simple.Vector1 @testRec, i32 5) nounwind
    
    ; Out.Integer(testRec.x)
    %7  = getelementptr inbounds %Simple.Vector1Rec* @testRec, i32 0, i32 1
    %8  = load i32* %7
    %9  = sext i32  %8 to i64
    %10 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %9) nounwind
    
    ; Out.Ln()
    %11 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)) nounwind
    
    ; NEW(testPtr)
    %12   = getelementptr inbounds %Simple.Vector1Rec* null, i32 1
    %13   = ptrtoint %Simple.Vector1Rec* %12 to i32
    %14   = call i8* (i32)* @GC_malloc(i32 %13) nounwind
    %15   = bitcast i8* %14 to %Simple.Vector1Rec*
    store %Simple.Vector1Rec {i8* bitcast ([5 x i8*]* @.rec0 to i8*), i32 zeroinitializer}, %Simple.Vector1Rec* %15
    store %Simple.Vector1Rec* %15, %Simple.Vector1Rec** @testPtr
    
    ; testPtr.SetX(5)
    %16  = load %Simple.Vector1* @testPtr
    %17  = getelementptr inbounds %Simple.Vector1Rec* %16, i32 0, i32 0
    %18  = load i8** %17
    %19  = bitcast i8*  %18 to [5 x i8*]*
    %20  = getelementptr inbounds [5 x i8*]* %19, i32 0, i32 2
    %21  = load i8** %20
    %22  = bitcast i8*  %21 to void (%Simple.Vector1, i32)*
    call void (%Simple.Vector1, i32)* %22(%Simple.Vector1 %16, i32 5) nounwind
    
    ; Out.Integer(testPtr.x)
    %23 = load %Simple.Vector1* @testPtr
    %24 = getelementptr inbounds %Simple.Vector1Rec* %23, i32 0, i32 1
    %25 = load i32* %24
    %26 = sext i32  %25 to i64
    %27 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %26) nounwind
    
    ; Out.Ln()
    %28 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)) nounwind
    
    ret i32 0
}

