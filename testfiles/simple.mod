MODULE Simple;
    IMPORT Out;

    TYPE
        Test = POINTER TO INTEGER;

    VAR PI : LONGREAL;

    PROCEDURE FindPI* (VAR pi : LONGREAL; iters : LONGINT);
    VAR
        i : LONGINT;
    BEGIN        
        FOR i := 1 TO iters DO
            pi := pi - 4.0D0 / (i * 4 - 1) + 4.0D0 / (i * 4 + 1);
        END;
    END;

BEGIN
    PI := 4;
    FindPI(PI, 10000000);
    Out.Real(PI); Out.Ln;
END Simple.
