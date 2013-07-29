MODULE Simple;
    IMPORT Out;
    VAR x : INTEGER;
    VAR y : SHORTINT;
    VAR z : INTEGER;
    VAR n : REAL;
BEGIN
    x := 1;
    y := 1 - 2;
    z := 3;
    Out.Integer(x + y * (z + 1)); Out.Ln;

    n := 12.5 / 3;
    Out.LongReal(n); Out.Ln;
END Simple.
