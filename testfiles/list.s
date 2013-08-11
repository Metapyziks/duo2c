	.def	 _Lists.ListNode.Add;
	.scl	2;
	.type	32;
	.endef
	.text
	.globl	_Lists.ListNode.Add
	.align	16, 0x90
_Lists.ListNode.Add:                    # @Lists.ListNode.Add
# BB#0:
	pushl	%esi
	subl	$12, %esp
	movl	24(%esp), %eax
	movl	%eax, 8(%esp)
	movl	20(%esp), %esi
	cmpl	$0, (%esi)
	je	LBB0_1
# BB#2:
	movl	(%esi), %eax
	movl	8(%eax), %edx
	movl	$_Lists.ListNode.Add, %ecx
	testl	%edx, %edx
	je	LBB0_4
# BB#3:
	movl	(%edx), %ecx
	movl	8(%ecx), %ecx
LBB0_4:
	movl	8(%esp), %edx
	movl	%edx, 4(%esp)
	addl	$8, %eax
	movl	%eax, (%esp)
	calll	*%ecx
	jmp	LBB0_5
LBB0_1:
	movl	$12, (%esp)
	calll	_GC_malloc
	movl	$0, 8(%eax)
	movl	$0, 4(%eax)
	movl	$_.rec0, (%eax)
	movl	%eax, (%esi)
	movl	8(%esp), %ecx
	movl	%ecx, 4(%eax)
LBB0_5:
	addl	$12, %esp
	popl	%esi
	ret

	.def	 _Lists.ListNode.Get;
	.scl	2;
	.type	32;
	.endef
	.globl	_Lists.ListNode.Get
	.align	16, 0x90
_Lists.ListNode.Get:                    # @Lists.ListNode.Get
# BB#0:
	pushl	%eax
	movl	8(%esp), %eax
	cmpl	$0, (%eax)
	je	LBB1_1
# BB#2:
	movl	(%eax), %ecx
	movl	4(%ecx), %ecx
	movl	%ecx, (%esp)
	movl	(%eax), %ecx
	movl	8(%ecx), %ecx
	movl	%ecx, (%eax)
	movl	(%esp), %eax
	popl	%edx
	ret
LBB1_1:
	xorl	%eax, %eax
	popl	%edx
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
	subl	$12, %esp
	calll	___main
	movl	$1, L_n
	movl	$1, L_i
	jmp	LBB2_1
	.align	16, 0x90
LBB2_4:                                 #   in Loop: Header=BB2_1 Depth=1
	movl	L_n, %ecx
	movl	%ecx, 4(%esp)
	movl	$L_test, (%esp)
	calll	*%eax
	incl	L_i
LBB2_1:                                 # =>This Inner Loop Header: Depth=1
	cmpl	$10, L_i
	jg	LBB2_5
# BB#2:                                 #   in Loop: Header=BB2_1 Depth=1
	imull	$8723, L_n, %ecx        # imm = 0x2213
	leal	181(%ecx), %eax
	movl	%eax, %edx
	sarl	$31, %edx
	shrl	$24, %edx
	leal	181(%ecx,%edx), %ecx
	andl	$-256, %ecx
	subl	%ecx, %eax
	movl	%eax, L_n
	movl	$_Lists.ListNode.Add, %eax
	movl	L_test, %ecx
	testl	%ecx, %ecx
	je	LBB2_4
# BB#3:                                 #   in Loop: Header=BB2_1 Depth=1
	movl	(%ecx), %eax
	movl	8(%eax), %eax
	jmp	LBB2_4
	.align	16, 0x90
LBB2_5:                                 # =>This Inner Loop Header: Depth=1
	movl	$_Lists.ListNode.Get, %eax
	movl	L_test, %ecx
	testl	%ecx, %ecx
	je	LBB2_7
# BB#6:                                 #   in Loop: Header=BB2_5 Depth=1
	movl	(%ecx), %eax
	movl	12(%eax), %eax
LBB2_7:                                 #   in Loop: Header=BB2_5 Depth=1
	movl	$L_test, (%esp)
	calll	*%eax
	movl	%eax, L_i
	testl	%eax, %eax
	jle	LBB2_9
# BB#8:                                 #   in Loop: Header=BB2_5 Depth=1
	movl	L_i, %eax
	movl	%eax, %ecx
	sarl	$31, %ecx
	movl	%ecx, 8(%esp)
	movl	%eax, 4(%esp)
	movl	$L_.str1, (%esp)
	calll	_printf
	movl	$L_.str2, (%esp)
	calll	_printf
LBB2_9:                                 #   in Loop: Header=BB2_5 Depth=1
	cmpl	$0, L_i
	jne	LBB2_5
# BB#10:
	xorl	%eax, %eax
	addl	$12, %esp
	popl	%ebp
	ret

	.data
	.globl	_.rec0                  # @.rec0
	.align	4
_.rec0:
	.long	L_.str0
	.long	0
	.long	_Lists.ListNode.Add
	.long	_Lists.ListNode.Get

L_.str0:                                # @.str0
	.asciz	 "ListNode"

L_.str1:                                # @.str1
	.asciz	 "%i"

L_.str2:                                # @.str2
	.asciz	 "\n"

	.lcomm	L_i,4,4                 # @i
	.lcomm	L_n,4,4                 # @n
	.lcomm	L_test,4,4              # @test

