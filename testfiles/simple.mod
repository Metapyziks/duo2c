MODULE Simple;
    IMPORT Out;

    TYPE
        Test = POINTER TO INTEGER;

    VAR PI : LONGREAL;

    PROCEDURE FindPI* (VAR pi : LONGREAL; iters : INTEGER);
    VAR
        i : INTEGER;
    BEGIN        
        FOR i := 1 TO iters DO
            pi := pi - 4.0D0 / (i * 4 - 1) + 4.0D0 / (i * 4 + 1);
        END;
    END;

BEGIN
    PI := 8;
    FindPI(PI, 1000000);
    FindPI(PI, 1000000);
    Out.Real(PI / 2); Out.Ln;
END Simple.
