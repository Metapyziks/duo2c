MODULE Simple;
    IMPORT Out;

    TYPE
        TestRec = RECORD
            x : INTEGER;
        END;
        AnotherRec = RECORD (TestRec)
            y : INTEGER;
        END;

    PROCEDURE (this : TestRec) SetX (v : INTEGER);
    BEGIN
        Out.Integer(v); Out.Ln;
    END;

    PROCEDURE (this : AnotherRec) SetY (v : INTEGER);
    BEGIN
        Out.Integer(v); Out.Ln;
    END;

BEGIN
    Out.String("Hello world!"); Out.Ln;
END Simple.
