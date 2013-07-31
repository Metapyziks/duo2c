MODULE Simple;
    IMPORT Out;

    VAR
        i : INTEGER;
        PI * : LONGREAL;

BEGIN
    PI := 4;
    i := 1;
    
    REPEAT
        PI := PI - 4.0D0 / (i * 4 - 1) + 4.0D0 / (i * 4 + 1);
        i := i + 1;
    UNTIL i >= 1000000;

    Out.Real(PI); Out.Ln;
END Simple.
