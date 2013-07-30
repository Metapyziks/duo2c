MODULE Simple;
    IMPORT Out;

    VAR
        i : INTEGER;
        PI * : LONGREAL;

BEGIN
    PI := 4;
    i := 1;
    
    WHILE i < 1000000 DO
        PI := PI - 4.0D0 / (i * 4 - 1) + 4.0D0 / (i * 4 + 1);
        i := i + 1;
    END;

    Out.Real(PI); Out.Ln;
END Simple.
