MODULE Arrays;
    IMPORT Out;

    CONST
        N = 10;

    VAR 
        i, j, k : INTEGER;
        a : ARRAY OF ARRAY OF ARRAY OF INTEGER;

BEGIN
    NEW(a, N, N, N);

    FOR i := 0 TO N - 1 DO
        FOR j := 0 TO N - 1 DO
            FOR k := 0 TO N - 1 DO
                a[i, j, k] := (i + 1) * (j + 1) * (k + 1);
            END;
        END;
    END;

    Out.Integer(a[3, 8, 4]); Out.Ln;
END Arrays.
