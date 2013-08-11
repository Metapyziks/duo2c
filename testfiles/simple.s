	.def	 _Simple.Vector1Rec.SetX;
	.scl	2;
	.type	32;
	.endef
	.text
	.globl	_Simple.Vector1Rec.SetX
	.align	16, 0x90
_Simple.Vector1Rec.SetX:                # @Simple.Vector1Rec.SetX
# BB#0:
	pushl	%eax
	movl	12(%esp), %eax
	movl	%eax, (%esp)
	movl	8(%esp), %ecx
	movl	(%ecx), %ecx
	movl	%eax, 4(%ecx)
	popl	%eax
	ret

	.def	 _Simple.Vector1Rec.GetX;
	.scl	2;
	.type	32;
	.endef
	.globl	_Simple.Vector1Rec.GetX
	.align	16, 0x90
_Simple.Vector1Rec.GetX:                # @Simple.Vector1Rec.GetX
# BB#0:
	movl	4(%esp), %eax
	movl	(%eax), %eax
	movl	4(%eax), %eax
	ret

	.def	 _Simple.Vector1Rec.Print;
	.scl	2;
	.type	32;
	.endef
	.globl	_Simple.Vector1Rec.Print
	.align	16, 0x90
_Simple.Vector1Rec.Print:               # @Simple.Vector1Rec.Print
# BB#0:
	pushl	%esi
	subl	$12, %esp
	movl	20(%esp), %esi
	cmpl	$0, (%esi)
	je	LBB2_1
# BB#3:
	movl	$L_.str6, 4(%esp)
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	(%esi), %eax
	movl	4(%eax), %eax
	movl	%eax, %ecx
	sarl	$31, %ecx
	movl	%ecx, 8(%esp)
	movl	%eax, 4(%esp)
	movl	$L_.str7, (%esp)
	calll	_printf
	movl	$L_.str8, 4(%esp)
	jmp	LBB2_2
LBB2_1:
	movl	$L_.str3, 4(%esp)
LBB2_2:
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	$L_.str5, (%esp)
	calll	_printf
	addl	$12, %esp
	popl	%esi
	ret

	.def	 _Simple.Vector2Rec.SetY;
	.scl	2;
	.type	32;
	.endef
	.globl	_Simple.Vector2Rec.SetY
	.align	16, 0x90
_Simple.Vector2Rec.SetY:                # @Simple.Vector2Rec.SetY
# BB#0:
	pushl	%eax
	movl	12(%esp), %eax
	movl	%eax, (%esp)
	movl	8(%esp), %ecx
	movl	(%ecx), %ecx
	movl	%eax, 8(%ecx)
	popl	%eax
	ret

	.def	 _Simple.Vector2Rec.GetY;
	.scl	2;
	.type	32;
	.endef
	.globl	_Simple.Vector2Rec.GetY
	.align	16, 0x90
_Simple.Vector2Rec.GetY:                # @Simple.Vector2Rec.GetY
# BB#0:
	movl	4(%esp), %eax
	movl	(%eax), %eax
	movl	8(%eax), %eax
	ret

	.def	 _Simple.Vector2Rec.Print;
	.scl	2;
	.type	32;
	.endef
	.globl	_Simple.Vector2Rec.Print
	.align	16, 0x90
_Simple.Vector2Rec.Print:               # @Simple.Vector2Rec.Print
# BB#0:
	pushl	%esi
	subl	$12, %esp
	movl	20(%esp), %esi
	cmpl	$0, (%esi)
	je	LBB5_1
# BB#3:
	movl	$L_.str6, 4(%esp)
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	(%esi), %eax
	movl	4(%eax), %eax
	movl	%eax, %ecx
	sarl	$31, %ecx
	movl	%ecx, 8(%esp)
	movl	%eax, 4(%esp)
	movl	$L_.str7, (%esp)
	calll	_printf
	movl	$L_.str9, 4(%esp)
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	(%esi), %eax
	movl	8(%eax), %eax
	movl	%eax, %ecx
	sarl	$31, %ecx
	movl	%ecx, 8(%esp)
	movl	%eax, 4(%esp)
	movl	$L_.str7, (%esp)
	calll	_printf
	movl	$L_.str8, 4(%esp)
	jmp	LBB5_2
LBB5_1:
	movl	$L_.str3, 4(%esp)
LBB5_2:
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	$L_.str5, (%esp)
	calll	_printf
	addl	$12, %esp
	popl	%esi
	ret

	.def	 _Simple.Vector3Rec.SetZ;
	.scl	2;
	.type	32;
	.endef
	.globl	_Simple.Vector3Rec.SetZ
	.align	16, 0x90
_Simple.Vector3Rec.SetZ:                # @Simple.Vector3Rec.SetZ
# BB#0:
	pushl	%eax
	movl	12(%esp), %eax
	movl	%eax, (%esp)
	movl	8(%esp), %ecx
	movl	(%ecx), %ecx
	movl	%eax, 12(%ecx)
	popl	%eax
	ret

	.def	 _Simple.Vector3Rec.GetZ;
	.scl	2;
	.type	32;
	.endef
	.globl	_Simple.Vector3Rec.GetZ
	.align	16, 0x90
_Simple.Vector3Rec.GetZ:                # @Simple.Vector3Rec.GetZ
# BB#0:
	movl	4(%esp), %eax
	movl	(%eax), %eax
	movl	12(%eax), %eax
	ret

	.def	 _Simple.Vector3Rec.Print;
	.scl	2;
	.type	32;
	.endef
	.globl	_Simple.Vector3Rec.Print
	.align	16, 0x90
_Simple.Vector3Rec.Print:               # @Simple.Vector3Rec.Print
# BB#0:
	pushl	%esi
	subl	$12, %esp
	movl	20(%esp), %esi
	cmpl	$0, (%esi)
	je	LBB8_1
# BB#3:
	movl	$L_.str6, 4(%esp)
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	(%esi), %eax
	movl	4(%eax), %eax
	movl	%eax, %ecx
	sarl	$31, %ecx
	movl	%ecx, 8(%esp)
	movl	%eax, 4(%esp)
	movl	$L_.str7, (%esp)
	calll	_printf
	movl	$L_.str9, 4(%esp)
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	(%esi), %eax
	movl	8(%eax), %eax
	movl	%eax, %ecx
	sarl	$31, %ecx
	movl	%ecx, 8(%esp)
	movl	%eax, 4(%esp)
	movl	$L_.str7, (%esp)
	calll	_printf
	movl	$L_.str9, 4(%esp)
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	(%esi), %eax
	movl	12(%eax), %eax
	movl	%eax, %ecx
	sarl	$31, %ecx
	movl	%ecx, 8(%esp)
	movl	%eax, 4(%esp)
	movl	$L_.str7, (%esp)
	calll	_printf
	movl	$L_.str8, 4(%esp)
	jmp	LBB8_2
LBB8_1:
	movl	$L_.str3, 4(%esp)
LBB8_2:
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	$L_.str5, (%esp)
	calll	_printf
	addl	$12, %esp
	popl	%esi
	ret

	.def	 _TestType;
	.scl	2;
	.type	32;
	.endef
	.globl	_TestType
	.align	16, 0x90
_TestType:                              # @TestType
# BB#0:
	pushl	%ebp
	movl	%esp, %ebp
	andl	$-8, %esp
	pushl	%esi
	subl	$20, %esp
	movl	8(%ebp), %eax
	movl	%eax, 16(%esp)
	movl	16(%ebp), %eax
	movl	%eax, 12(%esp)
	movl	12(%ebp), %eax
	movl	%eax, 8(%esp)
	xorb	%al, %al
	movl	16(%esp), %ecx
	testl	%ecx, %ecx
	je	LBB9_4
# BB#1:
	movl	(%ecx), %edx
	movl	$_.rec2, %ecx
	.align	16, 0x90
LBB9_2:                                 # =>This Inner Loop Header: Depth=1
	xorb	%al, %al
	testl	%edx, %edx
	je	LBB9_4
# BB#3:                                 #   in Loop: Header=BB9_2 Depth=1
	movl	4(%edx), %esi
	movb	$1, %al
	cmpl	%ecx, %edx
	movl	%esi, %edx
	jne	LBB9_2
LBB9_4:
	cmpb	$1, %al
	jne	LBB9_6
# BB#5:
	movl	12(%esp), %eax
	movl	%eax, 4(%esp)
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	$L_.str10, 4(%esp)
	jmp	LBB9_19
LBB9_6:
	xorb	%al, %al
	movl	16(%esp), %ecx
	testl	%ecx, %ecx
	je	LBB9_10
# BB#7:
	movl	(%ecx), %edx
	movl	$_.rec1, %ecx
	.align	16, 0x90
LBB9_8:                                 # =>This Inner Loop Header: Depth=1
	xorb	%al, %al
	testl	%edx, %edx
	je	LBB9_10
# BB#9:                                 #   in Loop: Header=BB9_8 Depth=1
	movl	4(%edx), %esi
	movb	$1, %al
	cmpl	%ecx, %edx
	movl	%esi, %edx
	jne	LBB9_8
LBB9_10:
	cmpb	$1, %al
	jne	LBB9_12
# BB#11:
	movl	12(%esp), %eax
	movl	%eax, 4(%esp)
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	$L_.str11, 4(%esp)
	jmp	LBB9_19
LBB9_12:
	xorb	%al, %al
	movl	16(%esp), %ecx
	testl	%ecx, %ecx
	je	LBB9_16
# BB#13:
	movl	(%ecx), %edx
	movl	$_.rec0, %ecx
	.align	16, 0x90
LBB9_14:                                # =>This Inner Loop Header: Depth=1
	xorb	%al, %al
	testl	%edx, %edx
	je	LBB9_16
# BB#15:                                #   in Loop: Header=BB9_14 Depth=1
	movl	4(%edx), %esi
	movb	$1, %al
	cmpl	%ecx, %edx
	movl	%esi, %edx
	jne	LBB9_14
LBB9_16:
	cmpb	$1, %al
	jne	LBB9_18
# BB#17:
	movl	12(%esp), %eax
	movl	%eax, 4(%esp)
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	$L_.str12, 4(%esp)
	jmp	LBB9_19
LBB9_18:
	movl	12(%esp), %eax
	movl	%eax, 4(%esp)
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	$L_.str13, 4(%esp)
LBB9_19:
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	$L_.str5, (%esp)
	calll	_printf
	addl	$20, %esp
	popl	%esi
	movl	%ebp, %esp
	popl	%ebp
	ret

	.def	 _main;
	.scl	2;
	.type	32;
	.endef
	.globl	_main
	.align	16, 0x90
_main:                                  # @main
# BB#0:
	pushl	%ebp
	movl	%esp, %ebp
	pushl	%esi
	subl	$12, %esp
	calll	___main
	movl	$16, (%esp)
	calll	_GC_malloc
	movl	$0, 12(%eax)
	movl	$0, 8(%eax)
	movl	$0, 4(%eax)
	movl	$_.rec2, (%eax)
	movl	%eax, _Simple.C
	movl	$_Simple.Vector1Rec.SetX, %ecx
	testl	%eax, %eax
	je	LBB10_2
# BB#1:
	movl	(%eax), %eax
	movl	8(%eax), %ecx
LBB10_2:
	movl	$56, 4(%esp)
	movl	$_Simple.C, (%esp)
	calll	*%ecx
	movl	$_Simple.Vector2Rec.SetY, %eax
	movl	_Simple.C, %ecx
	testl	%ecx, %ecx
	je	LBB10_4
# BB#3:
	movl	(%ecx), %eax
	movl	20(%eax), %eax
LBB10_4:
	movl	$-3, 4(%esp)
	movl	$_Simple.C, (%esp)
	calll	*%eax
	movl	$_Simple.Vector3Rec.SetZ, %eax
	movl	_Simple.C, %ecx
	testl	%ecx, %ecx
	je	LBB10_6
# BB#5:
	movl	(%ecx), %eax
	movl	28(%eax), %eax
LBB10_6:
	movl	$12, 4(%esp)
	movl	$_Simple.C, (%esp)
	calll	*%eax
	movl	$L_.str14, 4(%esp)
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	$_Simple.Vector3Rec.Print, %eax
	movl	_Simple.C, %ecx
	testl	%ecx, %ecx
	je	LBB10_8
# BB#7:
	movl	(%ecx), %eax
	movl	16(%eax), %eax
LBB10_8:
	movl	$_Simple.C, (%esp)
	calll	*%eax
	movl	_Simple.C, %eax
	movl	%eax, (%esp)
	movl	$L_.str15, 8(%esp)
	movl	$2, 4(%esp)
	calll	_TestType
	movl	$8, (%esp)
	calll	_GC_malloc
	movl	$0, 4(%eax)
	movl	$_.rec0, (%eax)
	movl	%eax, _Simple.A
	movl	$_Simple.Vector1Rec.SetX, %esi
	testl	%eax, %eax
	je	LBB10_10
# BB#9:
	movl	(%eax), %eax
	movl	8(%eax), %esi
LBB10_10:
	movl	$_Simple.Vector2Rec.GetY, %eax
	movl	_Simple.C, %ecx
	testl	%ecx, %ecx
	je	LBB10_12
# BB#11:
	movl	(%ecx), %eax
	movl	24(%eax), %eax
LBB10_12:
	movl	$_Simple.C, (%esp)
	calll	*%eax
	leal	(%eax,%eax,2), %eax
	movl	%eax, 4(%esp)
	movl	$_Simple.A, (%esp)
	calll	*%esi
	movl	$L_.str16, 4(%esp)
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	$_Simple.Vector1Rec.Print, %eax
	movl	_Simple.A, %ecx
	testl	%ecx, %ecx
	je	LBB10_14
# BB#13:
	movl	(%ecx), %eax
	movl	16(%eax), %eax
LBB10_14:
	movl	$_Simple.A, (%esp)
	calll	*%eax
	movl	_Simple.A, %eax
	movl	%eax, (%esp)
	movl	$L_.str17, 8(%esp)
	movl	$2, 4(%esp)
	calll	_TestType
	movl	_Simple.C, %eax
	movl	%eax, _Simple.B
	movl	$L_.str18, 4(%esp)
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	$_Simple.Vector2Rec.Print, %eax
	movl	_Simple.B, %ecx
	testl	%ecx, %ecx
	je	LBB10_16
# BB#15:
	movl	(%ecx), %eax
	movl	16(%eax), %eax
LBB10_16:
	movl	$_Simple.B, (%esp)
	calll	*%eax
	movl	_Simple.B, %eax
	movl	%eax, (%esp)
	movl	$L_.str19, 8(%esp)
	movl	$2, 4(%esp)
	calll	_TestType
	movl	$0, _Simple.A
	movl	$L_.str16, 4(%esp)
	movl	$L_.str4, (%esp)
	calll	_printf
	movl	$_Simple.Vector1Rec.Print, %eax
	movl	_Simple.A, %ecx
	testl	%ecx, %ecx
	je	LBB10_18
# BB#17:
	movl	(%ecx), %eax
	movl	16(%eax), %eax
LBB10_18:
	movl	$_Simple.A, (%esp)
	calll	*%eax
	movl	_Simple.A, %eax
	movl	%eax, (%esp)
	movl	$L_.str3, 8(%esp)
	movl	$4, 4(%esp)
	calll	_TestType
	xorl	%eax, %eax
	addl	$12, %esp
	popl	%esi
	popl	%ebp
	ret

	.data
	.globl	_.rec0                  # @.rec0
	.align	16
_.rec0:
	.long	L_.str0
	.long	0
	.long	_Simple.Vector1Rec.SetX
	.long	_Simple.Vector1Rec.GetX
	.long	_Simple.Vector1Rec.Print

	.globl	_.rec1                  # @.rec1
	.align	16
_.rec1:
	.long	L_.str1
	.long	_.rec0
	.long	_Simple.Vector1Rec.SetX
	.long	_Simple.Vector1Rec.GetX
	.long	_Simple.Vector2Rec.Print
	.long	_Simple.Vector2Rec.SetY
	.long	_Simple.Vector2Rec.GetY

	.globl	_.rec2                  # @.rec2
	.align	16
_.rec2:
	.long	L_.str2
	.long	_.rec1
	.long	_Simple.Vector1Rec.SetX
	.long	_Simple.Vector1Rec.GetX
	.long	_Simple.Vector3Rec.Print
	.long	_Simple.Vector2Rec.SetY
	.long	_Simple.Vector2Rec.GetY
	.long	_Simple.Vector3Rec.SetZ
	.long	_Simple.Vector3Rec.GetZ

L_.str0:                                # @.str0
	.asciz	 "Vector1Rec"

L_.str1:                                # @.str1
	.asciz	 "Vector2Rec"

L_.str2:                                # @.str2
	.asciz	 "Vector3Rec"

L_.str3:                                # @.str3
	.asciz	 "NIL"

L_.str4:                                # @.str4
	.asciz	 "%s"

L_.str5:                                # @.str5
	.asciz	 "\n"

L_.str6:                                # @.str6
	.asciz	 "("

L_.str7:                                # @.str7
	.asciz	 "%i"

L_.str8:                                # @.str8
	.asciz	 ")"

L_.str9:                                # @.str9
	.asciz	 " "

L_.str10:                               # @.str10
	.asciz	 " is a Vector3"

L_.str11:                               # @.str11
	.asciz	 " is a Vector2"

L_.str12:                               # @.str12
	.asciz	 " is a Vector1"

	.align	16                      # @.str13
L_.str13:
	.asciz	 " is not a vector"

L_.str14:                               # @.str14
	.asciz	 "C = "

L_.str15:                               # @.str15
	.asciz	 "C"

L_.str16:                               # @.str16
	.asciz	 "A = "

L_.str17:                               # @.str17
	.asciz	 "A"

L_.str18:                               # @.str18
	.asciz	 "B = "

L_.str19:                               # @.str19
	.asciz	 "B"

	.globl	_Simple.A               # @Simple.A
	.align	4
_Simple.A:
	.long	0

	.globl	_Simple.B               # @Simple.B
	.align	4
_Simple.B:
	.long	0

	.globl	_Simple.C               # @Simple.C
	.align	4
_Simple.C:
	.long	0


