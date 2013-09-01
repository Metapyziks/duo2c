MODULE Arrays;
    IMPORT Out;

    TYPE
        TestRec = RECORD x : INTEGER; END;
        Test = POINTER TO TestRec;

    VAR a : ARRAY 5 OF INTEGER;

BEGIN
    NEW(a);

    Out.String("Hello world!\n"); Out.Ln;

END Arrays.
