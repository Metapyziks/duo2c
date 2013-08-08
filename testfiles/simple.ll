; Generated 08/08/2013 21:10:45
; GlobalUID a5439500-bcbb-4048-b11b-381805327716
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

@.rec0 = global [2 x i8*] [
    i8* getelementptr inbounds ([8 x %CHAR]* @.str0, i32 0, i32 0),
    i8* null
]
@.rec1 = global [2 x i8*] [
    i8* getelementptr inbounds ([11 x  %CHAR]*        @.str1, i32 0, i32 0),
    i8* bitcast ([2 x i8*]* @.rec0 to i8*)
]

@.str0 = private constant [8  x i8] c"TestRec\00"
@.str1 = private constant [11 x i8] c"AnotherRec\00"
@.str2 = private constant [6  x i8] c"b.y: \00"
@.str3 = private constant [3  x i8] c"%s\00"
@.str4 = private constant [3  x i8] c"%i\00"
@.str5 = private constant [2  x i8] c"\0A\00"

declare i32 @printf(%CHAR*, ...) nounwind 
@a = private global %Simple.TestRec {i8* bitcast ([2 x i8*]* @.rec0 to i8*), i32 zeroinitializer}
@b = private global %Simple.AnotherRec {i8* bitcast ([2 x i8*]* @.rec1 to i8*), i32 zeroinitializer, i32 zeroinitializer}


define i32 @main() {
    
    ; a.x := 5
    %1    = getelementptr inbounds %Simple.TestRec* @a, i32 0, i32 1
    store i32 5, i32* %1
    
    ; b.x := 12
    %2    = getelementptr inbounds %Simple.AnotherRec* @b, i32 0, i32 1
    store i32 12, i32* %2
    
    ; b.y := a.x + b.x * 2
    %3    = getelementptr inbounds %Simple.TestRec* @a, i32 0, i32 1
    %4    = load i32* %3
    %5    = getelementptr inbounds %Simple.AnotherRec* @b, i32 0, i32 1
    %6    = load i32* %5
    %7    = mul  i32  %6,  2
    %8    = add  i32  %4,  %7
    %9    = getelementptr inbounds %Simple.AnotherRec* @b, i32 0, i32 2
    store i32    %8,  i32* %9
    
    ; Out.String(b.y: )
    %10 = getelementptr inbounds [6 x %CHAR]* @.str2, i32 0, i32 0
    %11 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str3, i32 0, i32 0), %CHAR* %10) nounwind
    
    ; Out.Integer(b.y)
    %12 = getelementptr inbounds %Simple.AnotherRec* @b, i32 0, i32 2
    %13 = load i32* %12
    %14 = sext i32  %13 to i64
    %15 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([3 x %CHAR]* @.str4, i32 0, i32 0), i64 %14) nounwind
    
    ; Out.Ln()
    %16 = call i32 (%CHAR*, ...)* @printf(%CHAR* getelementptr inbounds ([2 x %CHAR]* @.str5, i32 0, i32 0)) nounwind
    
    ret i32 0
}

