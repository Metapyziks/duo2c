MODULE Arrays;
    IMPORT Out;

    CONST
        I = 10;
        J = 20;
        K = 30;

    VAR 
        i, j, k : INTEGER;
        a : ARRAY OF ARRAY OF ARRAY OF INTEGER;

BEGIN
    NEW(a, I, J, K);

    FOR i := 0 TO I - 1 DO
        FOR j := 0 TO J - 1 DO
            FOR k := 0 TO K - 1 DO
                a[i, j, k] := (i + 1) * (j + 1) * (k + 1);
            END;
        END;
    END;

    Out.Integer(a[0, 1, 2]); Out.Ln;
END Arrays.
