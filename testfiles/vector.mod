MODULE Vector;
    IMPORT Out;

    VAR a : VECTOR 8 OF INTEGER;
    VAR b : VECTOR 8 OF INTEGER;
    VAR c : VECTOR 8 OF INTEGER;
    VAR i : INTEGER;

BEGIN

    a := <0, 1, 2, 3, 4, 5, 6, 7>;
    b := 2;

    c := a * b + 1;

    Out.String("< ");
    FOR i := 0 TO 7 DO
        Out.Integer(c[i]); Out.String(" ");
    END;
    Out.String(">"); Out.Ln;

END Vector.
