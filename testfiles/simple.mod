MODULE Simple;
    IMPORT Out;

    VAR x : BOOLEAN;
    VAR y : BOOLEAN;

BEGIN
    x := TRUE;
    y := FALSE;

    Out.Boolean(x); Out.Ln;
    Out.Boolean(y); Out.Ln;
END Simple.
