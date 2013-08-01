MODULE Simple;
    IMPORT Out;

    TYPE
        Test = POINTER TO INTEGER;

    PROCEDURE FindPI* (iters : INTEGER) : LONGREAL;
    VAR
        i : INTEGER;
        PI : LONGREAL;
    BEGIN
        PI := 4;
        i := 0;
        
        WHILE i < iters DO
            PI := PI - 4.0D0 / (i * 4 - 1) + 4.0D0 / (i * 4 + 1);
            i := i + 1;
        END;
    END;

BEGIN
    Out.Real(FindPI(1000000)); Out.Ln;
END Simple.
