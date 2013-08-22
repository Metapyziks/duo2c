MODULE Vector;
    IMPORT Out;

    CONST N = 16;

    TYPE Vector = VECTOR N OF INTEGER;

    VAR a : Vector;
    VAR b : Vector;
    VAR c : Vector;
    VAR i : INTEGER;

BEGIN
    FOR i := 0 TO N - 1 DO a[i] := i; END;

    b := 2;
    c := a * b + 1;

    Out.String("< ");
    FOR i := 0 TO N - 1 DO
        Out.Integer(c[i]); Out.String(" ");
    END;
    Out.String(">"); Out.Ln;

END Vector.
