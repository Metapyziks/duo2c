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

    PROCEDURE (this : TestRec) SetX (val : INTEGER);
    BEGIN
        this.x := val;
    END;

BEGIN
    a.SetX(13);

    Out.String("a.x: "); Out.Integer(a.x); Out.Ln;
END Simple.
