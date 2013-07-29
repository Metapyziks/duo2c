MODULE Simple;
    IMPORT Out;

    VAR x : INTEGER;
    VAR y : INTEGER;

BEGIN
    x := 5;
    y := 7;

    IF x > y THEN
        Out.Integer(6); Out.Ln;
    ELSE
        Out.Integer(12); Out.Ln;
    END;

END Simple.
