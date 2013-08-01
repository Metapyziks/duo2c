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
        
        WHILE i < 1000000 DO
            PI := PI - 4.0D0 / (i * 4 - 1) + 4.0D0 / (i * 4 + 1);
            i := i + 1;
        END;
    END;

BEGIN
    Out.Integer(193); Out.Ln;
    Out.Real(11.9); Out.Ln;
    Out.Boolean(TRUE); Out.Ln;
END Simple.
