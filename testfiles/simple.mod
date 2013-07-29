MODULE Simple;
    IMPORT Out;

    VAR
        i : INTEGER;

BEGIN
    i := 1;
    WHILE i <= 10 DO
        Out.Integer(i); Out.Ln;
        i := i + 1;
    END;
END Simple.
