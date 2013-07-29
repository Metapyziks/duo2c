MODULE Simple;
    IMPORT Out;
    VAR x : INTEGER;
BEGIN
    x := 4;
    Out.Integer(x + x * (x + x));
    x := x + 1;
END Simple.
