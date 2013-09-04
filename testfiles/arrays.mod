MODULE Arrays;
    IMPORT Out;

    CONST
        N = 10;

    VAR 
        i, j : INTEGER;
        a : ARRAY N, N OF INTEGER;

BEGIN
    FOR i := 0 TO N - 1 DO
        FOR j := 0 TO N - 1 DO
            a[i][j] := (i + 1) * (j + 1);
        END;
    END;

    Out.Integer(a[3 - 1][8 - 1]); Out.Ln;
END Arrays.
