; Generated 10/08/2013 18:21:16
; GlobalUID 92fe1046-d9cd-4ccf-a887-447750376b00
; 
; LLVM IR file for module "Simple"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET  = type i64
%Simple.Vector1 = type {i8*, i32}
%Simple.Vector2 = type {i8*, i32, i32}
%Simple.Vector3 = type {i8*, i32, i32, i32}

@.rec0 = global [5 x i8*] [
    i8* getelementptr inbounds ([8 x %CHAR]* @.str0, i32 0, i32 0),
    i8* null,
    i8* bitcast (void (%Simple.Vector1*, i32)* @Simple.Vector1.SetX to i8*),
    i8* bitcast (i32 (%Simple.Vector1*)* @Simple.Vector1.GetX to i8*),
    i8* bitcast (void (%Simple.Vector1*)* @Simple.Vector1.Print to i8*)
]

@.rec1 = global [7 x i8*] [
    i8* getelementptr inbounds ([8 x  %CHAR]* @.str1, i32 0, i32 0),
    i8* bitcast ([5 x i8*]* @.rec0 to i8*),
    i8* bitcast (void (%Simple.Vector1*, i32)* @Simple.Vector1.SetX to i8*),
    i8* bitcast (i32 (%Simple.Vector1*)* @Simple.Vector1.GetX to i8*),
    i8* bitcast (void (%Simple.Vector2*)* @Simple.Vector2.Print to i8*),
    i8* bitcast (void (%Simple.Vector2*, i32)* @Simple.Vector2.SetY to i8*),
    i8* bitcast (i32 (%Simple.Vector2*)* @Simple.Vector2.GetY to i8*)
]

@.rec2 = global [9 x i8*] [
    i8* getelementptr inbounds ([8 x  %CHAR]* @.str2, i32 0, i32 0),
    i8* bitcast ([7 x i8*]* @.rec1 to i8*),
    i8* bitcast (void (%Simple.Vector1*, i32)* @Simple.Vector1.SetX to i8*),
    i8* bitcast (i32 (%Simple.Vector1*)* @Simple.Vector1.GetX to i8*),
    i8* bitcast (void (%Simple.Vector3*)* @Simple.Vector3.Print to i8*),
    i8* bitcast (void (%Simple.Vector2*, i32)* @Simple.Vector2.SetY to i8*),
    i8* bitcast (i32 (%Simple.Vector2*)* @Simple.Vector2.GetY to i8*),
    i8* bitcast (void (%Simple.Vector3*, i32)* @Simple.Vector3.SetZ to i8*),
    i8* bitcast (i32 (%Simple.Vector3*)* @Simple.Vector3.GetZ to i8*)
]

@.str0 = private constant [8 x i8] c"Vector1\00"
@.str1 = private constant [8 x i8] c"Vector2\00"
@.str2 = private constant [8 x i8] c"Vector3\00"
@.str3 = private constant [2 x i8] c"(\00"
@.str4 = private constant [3 x i8] c"%s\00"
@.str5 = private constant [3 x i8] c"%i\00"
@.str6 = private constant [2 x i8] c")\00"
@.str7 = private constant [2 x i8] c"\0A\00"
@.str8 = private constant [2 x i8] c",\00"

declare i32     @printf(%CHAR*, ...) nounwind 
declare noalias i8*    @GC_malloc(i32) 

@a = private global %Simple.Vector1 {
    i8* bitcast ([5 x i8*]* @.rec0 to i8*),
    i32 zeroinitializer
}

@b = private global %Simple.Vector2 {
    i8* bitcast ([7 x i8*]* @.rec1 to i8*),
    i32 zeroinitializer,
    i32 zeroinitializer
}

@c = private global %Simple.Vector3 {
    i8* bitcast ([9 x i8*]* @.rec2 to i8*),
    i32 zeroinitializer,
    i32 zeroinitializer,
    i32 zeroinitializer
}

define void @Simple.Vector1.SetX(%Simple.Vector1* %this, i32 %$val) nounwind {
    
    %val  = alloca i32
    store i32 %$val, i32* %val
    
    %1    = load i32* %val
    %2    = getelementptr inbounds %Simple.Vector1* %this, i32 0, i32 1
    store i32    %1,  i32* %2
    
    ret void 
}

define i32  @Simple.Vector1.GetX(%Simple.Vector1* %this) nounwind {
    
    %1  = getelementptr inbounds %Simple.Vector1* %this, i32 0, i32 1
    %2  = load i32* %1
    ret i32    %2
}

define void @Simple.Vector1.Print(%Simple.Vector1* %this) nounwind {
    
    %1 = getelementptr inbounds [2 x %CHAR]* @.str3, i32 0, i32 0
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %1) nounwind
    
    %3 = getelementptr inbounds %Simple.Vector1* %this, i32 0, i32 1
    %4 = load i32* %3
    %5 = sext i32  %4 to i64
    %6 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %5) nounwind
    
    %7 = getelementptr inbounds [2 x %CHAR]* @.str6, i32 0, i32 0
    %8 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %7) nounwind
    
    %9 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)) nounwind
    
    ret void 
}

define void @Simple.Vector2.SetY(%Simple.Vector2* %this, i32 %$val) nounwind {
    
    %val  = alloca i32
    store i32 %$val, i32* %val
    
    %1    = load i32* %val
    %2    = getelementptr inbounds %Simple.Vector2* %this, i32 0, i32 2
    store i32    %1,  i32* %2
    
    ret void 
}

define i32  @Simple.Vector2.GetY(%Simple.Vector2* %this) nounwind {
    
    %1  = getelementptr inbounds %Simple.Vector2* %this, i32 0, i32 2
    %2  = load i32* %1
    ret i32    %2
}

define void @Simple.Vector2.Print(%Simple.Vector2* %this) nounwind {
    
    %1 = getelementptr inbounds [2 x %CHAR]* @.str3, i32 0, i32 0
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %1) nounwind
    
    %3 = getelementptr inbounds %Simple.Vector2* %this, i32 0, i32 1
    %4 = load i32* %3
    %5 = sext i32  %4 to i64
    %6 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %5) nounwind
    
    %7 = getelementptr inbounds [2 x %CHAR]* @.str8, i32 0, i32 0
    %8 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %7) nounwind
    
    %9  = getelementptr inbounds %Simple.Vector2* %this, i32 0, i32 2
    %10 = load i32* %9
    %11 = sext i32  %10 to i64
    %12 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %11) nounwind
    
    %13 = getelementptr inbounds [2 x %CHAR]* @.str6, i32 0, i32 0
    %14 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %13) nounwind
    
    %15 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)) nounwind
    
    ret void 
}

define void @Simple.Vector3.SetZ(%Simple.Vector3* %this, i32 %$val) nounwind {
    
    %val  = alloca i32
    store i32 %$val, i32* %val
    
    %1    = load i32* %val
    %2    = getelementptr inbounds %Simple.Vector3* %this, i32 0, i32 3
    store i32    %1,  i32* %2
    
    ret void 
}

define i32  @Simple.Vector3.GetZ(%Simple.Vector3* %this) nounwind {
    
    %1  = getelementptr inbounds %Simple.Vector3* %this, i32 0, i32 3
    %2  = load i32* %1
    ret i32    %2
}

define void @Simple.Vector3.Print(%Simple.Vector3* %this) nounwind {
    
    %1 = getelementptr inbounds [2 x %CHAR]* @.str3, i32 0, i32 0
    %2 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %1) nounwind
    
    %3 = getelementptr inbounds %Simple.Vector3* %this, i32 0, i32 1
    %4 = load i32* %3
    %5 = sext i32  %4 to i64
    %6 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %5) nounwind
    
    %7 = getelementptr inbounds [2 x %CHAR]* @.str8, i32 0, i32 0
    %8 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %7) nounwind
    
    %9  = getelementptr inbounds %Simple.Vector3* %this, i32 0, i32 2
    %10 = load i32* %9
    %11 = sext i32  %10 to i64
    %12 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %11) nounwind
    
    %13 = getelementptr inbounds [2 x %CHAR]* @.str8, i32 0, i32 0
    %14 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %13) nounwind
    
    %15 = getelementptr inbounds %Simple.Vector3* %this, i32 0, i32 3
    %16 = load i32* %15
    %17 = sext i32  %16 to i64
    %18 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str5, i32 0, i32 0), i64 %17) nounwind
    
    %19 = getelementptr inbounds [2 x %CHAR]* @.str6, i32 0, i32 0
    %20 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), %CHAR* %19) nounwind
    
    %21 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str7, i32 0, i32 0)) nounwind
    
    ret void 
}

define i32 @main() {
    
    %1   = getelementptr inbounds %Simple.Vector1* @a, i32 0, i32 0
    %2   = load i8** %1
    %3   = bitcast i8*  %2 to [5 x i8*]*
    %4   = getelementptr inbounds [5 x i8*]* %3, i32 0, i32 2
    %5   = load i8** %4
    %6   = bitcast i8*  %5 to void (%Simple.Vector1*, i32)*
    call void (%Simple.Vector1*, i32)* %6(%Simple.Vector1* @a, i32 13) nounwind
    
    %7   = getelementptr inbounds %Simple.Vector2* @b, i32 0, i32 0
    %8   = load i8** %7
    %9   = bitcast i8*  %8 to [7 x i8*]*
    %10  = getelementptr inbounds [7 x i8*]* %9, i32 0, i32 2
    %11  = load i8** %10
    %12  = bitcast i8*  %11 to void (%Simple.Vector1*, i32)*
    %13  = bitcast %Simple.Vector2* @b to %Simple.Vector1*
    call void (%Simple.Vector1*, i32)* %12(%Simple.Vector1* %13, i32 8) nounwind
    
    %14  = getelementptr inbounds %Simple.Vector2* @b, i32 0, i32 0
    %15  = load i8** %14
    %16  = bitcast i8*  %15 to [7 x i8*]*
    %17  = getelementptr inbounds [7 x i8*]* %16, i32 0, i32 5
    %18  = load i8** %17
    %19  = bitcast i8*  %18 to void (%Simple.Vector2*, i32)*
    call void (%Simple.Vector2*, i32)* %19(%Simple.Vector2* @b, i32 -3) nounwind
    
    %20  = getelementptr inbounds %Simple.Vector3* @c, i32 0, i32 0
    %21  = load i8** %20
    %22  = bitcast i8*  %21 to [9 x i8*]*
    %23  = getelementptr inbounds [9 x i8*]* %22, i32 0, i32 2
    %24  = load i8** %23
    %25  = bitcast i8*  %24 to void (%Simple.Vector1*, i32)*
    %26  = bitcast %Simple.Vector3* @c to %Simple.Vector1*
    call void (%Simple.Vector1*, i32)* %25(%Simple.Vector1* %26, i32 8) nounwind
    
    %27  = getelementptr inbounds %Simple.Vector3* @c, i32 0, i32 0
    %28  = load i8** %27
    %29  = bitcast i8*  %28 to [9 x i8*]*
    %30  = getelementptr inbounds [9 x i8*]* %29, i32 0, i32 5
    %31  = load i8** %30
    %32  = bitcast i8*  %31 to void (%Simple.Vector2*, i32)*
    %33  = bitcast %Simple.Vector3* @c to %Simple.Vector2*
    call void (%Simple.Vector2*, i32)* %32(%Simple.Vector2* %33, i32 -3) nounwind
    
    %34  = getelementptr inbounds %Simple.Vector3* @c, i32 0, i32 0
    %35  = load i8** %34
    %36  = bitcast i8*  %35 to [9 x i8*]*
    %37  = getelementptr inbounds [9 x i8*]* %36, i32 0, i32 7
    %38  = load i8** %37
    %39  = bitcast i8*  %38 to void (%Simple.Vector3*, i32)*
    call void (%Simple.Vector3*, i32)* %39(%Simple.Vector3* @c, i32 92) nounwind
    
    %40  = getelementptr inbounds %Simple.Vector1* @a, i32 0, i32 0
    %41  = load i8** %40
    %42  = bitcast i8*  %41 to [5 x i8*]*
    %43  = getelementptr inbounds [5 x i8*]* %42, i32 0, i32 4
    %44  = load i8** %43
    %45  = bitcast i8*  %44 to void (%Simple.Vector1*)*
    call void (%Simple.Vector1*)* %45(%Simple.Vector1* @a) nounwind
    
    %46  = getelementptr inbounds %Simple.Vector2* @b, i32 0, i32 0
    %47  = load i8** %46
    %48  = bitcast i8*  %47 to [7 x i8*]*
    %49  = getelementptr inbounds [7 x i8*]* %48, i32 0, i32 4
    %50  = load i8** %49
    %51  = bitcast i8*  %50 to void (%Simple.Vector2*)*
    call void (%Simple.Vector2*)* %51(%Simple.Vector2* @b) nounwind
    
    %52  = getelementptr inbounds %Simple.Vector3* @c, i32 0, i32 0
    %53  = load i8** %52
    %54  = bitcast i8*  %53 to [9 x i8*]*
    %55  = getelementptr inbounds [9 x i8*]* %54, i32 0, i32 4
    %56  = load i8** %55
    %57  = bitcast i8*  %56 to void (%Simple.Vector3*)*
    call void (%Simple.Vector3*)* %57(%Simple.Vector3* @c) nounwind
    
    ret i32 0
}

