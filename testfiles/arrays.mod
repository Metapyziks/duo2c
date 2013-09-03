MODULE Arrays;
    IMPORT Out;

    CONST
        N = 16;

    VAR 
        i : INTEGER;
        a : ARRAY N OF INTEGER;

    PROCEDURE PrintArray(array : ARRAY OF INTEGER; start : INTEGER; count : INTEGER);
    VAR 
        j : INTEGER;
    BEGIN
        FOR j := start TO start + count - 1 DO
            Out.Integer(array[j]); Out.Ln;
        END;
    END PrintArray;

BEGIN
    FOR i := 0 TO N - 1 DO
        a[i] := i + 1;
    END;

    PrintArray(a, 0, 4);
    PrintArray(a, 8, 8);

END Arrays.
