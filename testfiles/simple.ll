; Generated 08/08/2013 23:22:51
; GlobalUID 102e3379-7464-4827-b9b8-1bef55733065
; 
; LLVM IR file for module "Simple"
; 
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p0:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-a0:0:64-n8:16:32-S32"

%CHAR = type i8
%SET  = type i64
%Simple.TestRec = type {i8*, i32}
%Simple.AnotherRec = type {i8*, i32, i32}

@.rec0 = global [3 x i8*] [
    i8* getelementptr inbounds ([8 x %CHAR]* @.str0, i32 0, i32 0),
    i8* null,
    i8* bitcast (void (%Simple.TestRec*, i32)* @Simple.TestRec.SetX to i8*)
]
@.rec1 = global [3 x i8*] [
    i8* getelementptr inbounds ([11 x  %CHAR]*        @.str1, i32 0, i32 0),
    i8* bitcast ([3 x i8*]* @.rec0 to i8*),
    i8* bitcast (void (%Simple.TestRec*, i32)* @Simple.TestRec.SetX to i8*)
]

@.str0 = private constant [8  x i8] c"TestRec\00"
@.str1 = private constant [11 x i8] c"AnotherRec\00"
@.str2 = private constant [6  x i8] c"a.x: \00"
@.str3 = private constant [3  x i8] c"%s\00"
@.str4 = private constant [3  x i8] c"%i\00"
@.str5 = private constant [2  x i8] c"\0A\00"

declare i32 @printf(%CHAR*, ...) nounwind 
@a = private global %Simple.TestRec {i8* bitcast ([3 x i8*]* @.rec0 to i8*), i32 zeroinitializer}

define void @Simple.TestRec.SetX(%Simple.TestRec* %this, i32 %$val) nounwind {
    
    %val  = alloca i32
    store i32      %$val, i32* %val
    
    
    ; this.x := val
    %1    = load i32* %val
    %2    = getelementptr inbounds %Simple.TestRec* %this, i32 0, i32 1
    store i32    %1,  i32* %2
    
    ret void 
}

define i32 @main() {
    
    ; a.SetX(13)
    %1   = getelementptr inbounds %Simple.TestRec* @a, i32 0, i32 0
    %2   = load    i8** %1
    %3   = bitcast i8*  %2 to [3 x i8*]*
    %4   = getelementptr inbounds [3    x  i8*]* %3, i32 0, i32 2
    %5   = load    i8** %4
    %6   = bitcast i8*  %5 to void (%Simple.TestRec*, i32)*
    call void (%Simple.TestRec*, i32)* %6(%Simple.TestRec* @a, i32 13) nounwind
    
    ; Out.String(a.x: )
    %7 = getelementptr inbounds [6 x %CHAR]* @.str2, i32 0, i32 0
    %8 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str3, i32 0, i32 0), %CHAR* %7) nounwind
    
    ; Out.Integer(a.x)
    %9  = getelementptr inbounds %Simple.TestRec* @a, i32 0, i32 1
    %10 = load i32* %9
    %11 = sext i32  %10 to i64
    %12 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), i64 %11) nounwind
    
    ; Out.Ln()
    %13 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str5, i32 0, i32 0)) nounwind
    
    ret i32 0
}

