MODULE Simple;
    IMPORT Out;

    VAR x : INTEGER;
    VAR y : INTEGER;

BEGIN
    x := 5;
    y := 6;

    Out.Boolean(x < y); Out.Ln;
    Out.Boolean(x > y); Out.Ln;
END Simple.
