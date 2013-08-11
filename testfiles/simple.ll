; Generated 11/08/2013 00:05:46
; GlobalUID 0b8b734e-5be4-4893-b98e-3c7c78553737
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

@Simple.A = global %Simple.Vector1 null
@Simple.B = global %Simple.Vector2 null
@Simple.C = global %Simple.Vector3 null

define void @Simple.Vector1Rec.SetX(%Simple.Vector1 %$this, i32 %$val) nounwind {
    
    %this = alloca %Simple.Vector1
    store %Simple.Vector1 %$this, %Simple.Vector1* %this
    
    %val  = alloca i32
    store i32 %$val, i32* %val
    
    ; this^.x := val
    %1    = load %Simple.Vector1* %this
    %2    = load i32* %val
    %3    = getelementptr inbounds %Simple.Vector1Rec* %1, i32 0, i32 1
    store i32    %2,  i32* %3
    
    ret void 
}

define i32  @Simple.Vector1Rec.GetX(%Simple.Vector1 %$this) nounwind {
    
    %this = alloca %Simple.Vector1
    store %Simple.Vector1 %$this, %Simple.Vector1* %this
    
    ; RETURN this^.x
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
    
    ; Out.Integer(this^.x)
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
    
    ; this^.y := val
    %1    = load %Simple.Vector2* %this
    %2    = load i32* %val
    %3    = getelementptr inbounds %Simple.Vector2Rec* %1, i32 0, i32 2
    store i32    %2,  i32* %3
    
    ret void 
}

define i32  @Simple.Vector2Rec.GetY(%Simple.Vector2 %$this) nounwind {
    
    %this = alloca %Simple.Vector2
    store %Simple.Vector2 %$this, %Simple.Vector2* %this
    
    ; RETURN this^.y
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
    
    ; Out.Integer(this^.x)
    %3 = load %Simple.Vector2* %this
    %4 = getelementptr inbounds %Simple.Vector2Rec* %3, i32 0, i32 1
    %5 = load i32* %4
    %6 = sext i32  %5 to i64
    %7 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %6) nounwind
    
    ; Out.String(" ")
    %8 = getelementptr inbounds [2 x %CHAR]* @.str8, i32 0, i32 0
    %9 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %8) nounwind
    
    ; Out.Integer(this^.y)
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
    
    ; this^.z := val
    %1    = load %Simple.Vector3* %this
    %2    = load i32* %val
    %3    = getelementptr inbounds %Simple.Vector3Rec* %1, i32 0, i32 3
    store i32    %2,  i32* %3
    
    ret void 
}

define i32  @Simple.Vector3Rec.GetZ(%Simple.Vector3 %$this) nounwind {
    
    %this = alloca %Simple.Vector3
    store %Simple.Vector3 %$this, %Simple.Vector3* %this
    
    ; RETURN this^.z
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
    
    ; Out.Integer(this^.x)
    %3 = load %Simple.Vector3* %this
    %4 = getelementptr inbounds %Simple.Vector3Rec* %3, i32 0, i32 1
    %5 = load i32* %4
    %6 = sext i32  %5 to i64
    %7 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %6) nounwind
    
    ; Out.String(" ")
    %8 = getelementptr inbounds [2 x %CHAR]* @.str8, i32 0, i32 0
    %9 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %8) nounwind
    
    ; Out.Integer(this^.y)
    %10 = load %Simple.Vector3* %this
    %11 = getelementptr inbounds %Simple.Vector3Rec* %10, i32 0, i32 2
    %12 = load i32* %11
    %13 = sext i32  %12 to i64
    %14 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %13) nounwind
    
    ; Out.String(" ")
    %15 = getelementptr inbounds [2 x %CHAR]* @.str8, i32 0, i32 0
    %16 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %15) nounwind
    
    ; Out.Integer(this^.z)
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
    
    ; NEW(C)
    %1    = getelementptr inbounds %Simple.Vector3Rec* null, i32 1
    %2    = ptrtoint %Simple.Vector3Rec* %1 to i32
    %3    = call i8* (i32)* @GC_malloc(i32 %2) nounwind
    %4    = bitcast i8* %3 to %Simple.Vector3Rec*
    store %Simple.Vector3Rec {i8* bitcast ([9 x i8*]* @.rec2 to i8*), i32 zeroinitializer, i32 zeroinitializer, i32 zeroinitializer}, %Simple.Vector3Rec* %4
    store %Simple.Vector3Rec* %4, %Simple.Vector3Rec** @Simple.C
    
    ; C^.SetX(5)
    %5   = load %Simple.Vector3* @Simple.C
    %6   = getelementptr inbounds %Simple.Vector3Rec* %5, i32 0, i32 0
    %7   = load i8** %6
    %8   = bitcast i8*  %7 to [9 x i8*]*
    %9   = getelementptr inbounds [9 x i8*]* %8, i32 0, i32 2
    %10  = load i8** %9
    %11  = bitcast i8*  %10 to void (%Simple.Vector1, i32)*
    %12  = bitcast %Simple.Vector3Rec* %5 to %Simple.Vector1
    call void (%Simple.Vector1, i32)* %11(%Simple.Vector1 %12, i32 5) nounwind
    
    ; C^.SetY(3)
    %13  = load %Simple.Vector3* @Simple.C
    %14  = getelementptr inbounds %Simple.Vector3Rec* %13, i32 0, i32 0
    %15  = load i8** %14
    %16  = bitcast i8*  %15 to [9 x i8*]*
    %17  = getelementptr inbounds [9 x i8*]* %16, i32 0, i32 5
    %18  = load i8** %17
    %19  = bitcast i8*  %18 to void (%Simple.Vector2, i32)*
    %20  = bitcast %Simple.Vector3Rec* %13 to %Simple.Vector2
    call void (%Simple.Vector2, i32)* %19(%Simple.Vector2 %20, i32 -3) nounwind
    
    ; C^.SetZ(8)
    %21  = load %Simple.Vector3* @Simple.C
    %22  = getelementptr inbounds %Simple.Vector3Rec* %21, i32 0, i32 0
    %23  = load i8** %22
    %24  = bitcast i8*  %23 to [9 x i8*]*
    %25  = getelementptr inbounds [9 x i8*]* %24, i32 0, i32 7
    %26  = load i8** %25
    %27  = bitcast i8*  %26 to void (%Simple.Vector3, i32)*
    call void (%Simple.Vector3, i32)* %27(%Simple.Vector3 %21, i32 8) nounwind
    
    ; C^.Print()
    %28  = load %Simple.Vector3* @Simple.C
    %29  = getelementptr inbounds %Simple.Vector3Rec* %28, i32 0, i32 0
    %30  = load i8** %29
    %31  = bitcast i8*  %30 to [9 x i8*]*
    %32  = getelementptr inbounds [9 x i8*]* %31, i32 0, i32 4
    %33  = load i8** %32
    %34  = bitcast i8*  %33 to void (%Simple.Vector3)*
    call void (%Simple.Vector3)* %34(%Simple.Vector3 %28) nounwind
    
    ; B := C
    %35   = load %Simple.Vector3* @Simple.C
    %36   = bitcast %Simple.Vector3 %35 to %Simple.Vector2
    store %Simple.Vector2 %36, %Simple.Vector2* @Simple.B
    
    ; B^.Print()
    %37  = load %Simple.Vector2* @Simple.B
    %38  = getelementptr inbounds %Simple.Vector2Rec* %37, i32 0, i32 0
    %39  = load i8** %38
    %40  = bitcast i8*  %39 to [7 x i8*]*
    %41  = getelementptr inbounds [7 x i8*]* %40, i32 0, i32 4
    %42  = load i8** %41
    %43  = bitcast i8*  %42 to void (%Simple.Vector2)*
    call void (%Simple.Vector2)* %43(%Simple.Vector2 %37) nounwind
    
    ; A := C
    %44   = load %Simple.Vector3* @Simple.C
    %45   = bitcast %Simple.Vector3 %44 to %Simple.Vector1
    store %Simple.Vector1 %45, %Simple.Vector1* @Simple.A
    
    ; A^.Print()
    %46  = load %Simple.Vector1* @Simple.A
    %47  = getelementptr inbounds %Simple.Vector1Rec* %46, i32 0, i32 0
    %48  = load i8** %47
    %49  = bitcast i8*  %48 to [5 x i8*]*
    %50  = getelementptr inbounds [5 x i8*]* %49, i32 0, i32 4
    %51  = load i8** %50
    %52  = bitcast i8*  %51 to void (%Simple.Vector1)*
    call void (%Simple.Vector1)* %52(%Simple.Vector1 %46) nounwind
    
    ret i32 0
}

