	.def	 _Simple_main;
	.scl	2;
	.type	32;
	.endef
	.text
	.globl	_Simple_main
	.align	16, 0x90
_Simple_main:                           # @Simple_main
	.cfi_startproc
# BB#0:
	pushl	%ebx
Ltmp4:
	.cfi_def_cfa_offset 8
	pushl	%edi
Ltmp5:
	.cfi_def_cfa_offset 12
	pushl	%esi
Ltmp6:
	.cfi_def_cfa_offset 16
	pushl	%eax
Ltmp7:
	.cfi_def_cfa_offset 20
Ltmp8:
	.cfi_offset %esi, -16
Ltmp9:
	.cfi_offset %edi, -12
Ltmp10:
	.cfi_offset %ebx, -8
	movb	$5, %al
	movw	$5, %cx
	movw	$13, %dx
	movw	$15, %si
	addw	%cx, %dx
	subw	$6, %dx
	movzwl	%si, %edi
	movzwl	%dx, %ebx
	addl	%ebx, %edi
	movb	%al, 3(%esp)            # 1-byte Spill
	movl	%edi, %eax
	addl	$4, %esp
	popl	%esi
	popl	%edi
	popl	%ebx
	ret
	.cfi_endproc


