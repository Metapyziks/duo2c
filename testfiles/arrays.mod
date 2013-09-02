MODULE Arrays;
    IMPORT Out;

    TYPE
        TestRec = RECORD x : INTEGER; END;
        Test = POINTER TO TestRec;

    VAR
        a : ARRAY 5 OF INTEGER;
        i : INTEGER;

BEGIN
    NEW(a);

    FOR i := 0 TO 4 DO
        a[i] := i + 1;
        Out.Integer(a[i]); Out.Ln;
    END;

END Arrays.
