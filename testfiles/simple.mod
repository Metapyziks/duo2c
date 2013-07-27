MODULE Simple;
    IMPORT Out;
    VAR
        x  : BYTE;
        y  : SHORTINT;
        z- : INTEGER;
BEGIN
    x := 5;
    Out.Integer(x);
    y := 8 + x;
    z := y + 2 - (3 - x + 2) + (y + x - 6);

    Out.Integer(2 + y - x);
END Simple.
