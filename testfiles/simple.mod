MODULE Simple;
    IMPORT Out;

    TYPE
        TestRec = RECORD
            x : INTEGER;
        END;
        AnotherRec = RECORD (TestRec)
            y : INTEGER;
        END;

    VAR a : TestRec;
    VAR b : AnotherRec;

BEGIN
    a.x := 5;

    b.x := 12;
    b.y := a.x + b.x * 2;

    Out.String("b.y: "); Out.Integer(b.y); Out.Ln;
END Simple.
