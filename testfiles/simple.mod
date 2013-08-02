MODULE Simple;
    IMPORT Out;

    TYPE
        Test = POINTER TO INTEGER;

    PROCEDURE FindPI* (iters : INTEGER) : LONGREAL;
    VAR
        i : INTEGER;
        pi : LONGREAL;
    BEGIN
        i := 1;
        pi := 4;
        
        WHILE i < iters DO
            pi := pi - 4.0D0 / (i * 4 - 1) + 4.0D0 / (i * 4 + 1);
            i := i + 1;
        END;

        RETURN pi;
    END;

BEGIN
    Out.Real(FindPI(1000000)); Out.Ln;
END Simple.
