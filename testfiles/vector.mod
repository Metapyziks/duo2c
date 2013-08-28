MODULE Vector;
    IMPORT Out;

    CONST N = 16;

    TYPE IntVector = VECTOR N OF INTEGER;

    VAR a : IntVector;
    VAR b : IntVector;
    VAR c : IntVector;
    VAR i : INTEGER;

BEGIN
    FOR i := 0 TO N - 1 DO a[i] := i; END;

    b := a * 2 + 1;
    c := <6, 3, -1, 12, b[3], 5, 1, 8, 0, 10, 3, a[1] + 3, 13, b[10], b[4] * 2, 9>;

    Out.String("a\tb\tc"); Out.Ln; Out.Ln;

    FOR i := 0 TO N - 1 DO
        Out.Integer(a[i]); Out.String("\t");
        Out.Integer(b[i]); Out.String("\t");
        Out.Integer(c[i]); Out.Ln;
    END;

END Vector.
