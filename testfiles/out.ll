; Generated 29/07/2013 19:41:10
; GlobalUID 7b47b124-30cc-415b-abc6-4a83a862f062
;
; LLVM IR file for module "Out"
;
; WARNING: This file is automatically
; generated and should not be edited

target datalayout = "e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-f80:128:128-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32-S32"

@.printistr = private unnamed_addr constant [3 x i8] c"%i\00", align 1
@.printfstr = private unnamed_addr constant [3 x i8] c"%f\00", align 1
@.printnstr = private unnamed_addr constant [2 x i8] c"\0A\00", align 1

declare i32 @printf(i8*, ...) nounwind

; Begin type aliases
  
  ; CHAR = BYTE
  %_typeCHAR   = type i8
  
  ; SET = LONGINT
  %_typeSET    = type i64
  
  ; String = ARRAY OF CHAR
  %_typeString = type {i32, %_typeCHAR*}
  
; End type aliases


define i32 @main() {    
    
    ret i32 0
}
; Module end
