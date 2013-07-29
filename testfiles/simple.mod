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

    n := z / 2;
    Out.Real(n); Out.Ln;
END Simple.
