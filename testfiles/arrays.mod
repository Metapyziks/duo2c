MODULE Arrays;
    IMPORT Out;

    CONST
        N = 16;

    VAR
        a : ARRAY N OF INTEGER;
        b : ARRAY OF INTEGER;
        i : INTEGER;

BEGIN
    FOR i := 0 TO N - 1 DO
        a[i] := i + 1;
    END;

    b := a;

    FOR i := 0 TO N - 1 DO
        Out.Integer(b[i]); Out.Ln;
    END;

END Arrays.
