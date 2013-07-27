	.def	 _main;
	.scl	2;
	.type	32;
	.endef
	.text
	.globl	_main
	.align	16, 0x90
_main:                                  # @main
	.cfi_startproc
# BB#0:
	pushl	%ebp
Ltmp2:
	.cfi_def_cfa_offset 8
Ltmp3:
	.cfi_offset %ebp, -8
	movl	%esp, %ebp
Ltmp4:
	.cfi_def_cfa_register %ebp
	subl	$8, %esp
	calll	___main
	movl	$5, 4(%esp)
	movl	$L_.str, (%esp)
	calll	_printf
	movl	$10, 4(%esp)
	movl	$L_.str, (%esp)
	calll	_printf
	xorl	%eax, %eax
	addl	$8, %esp
	popl	%ebp
	ret
	.cfi_endproc

	.data
L_.str:                                 # @.str
	.asciz	 "%i\n"


