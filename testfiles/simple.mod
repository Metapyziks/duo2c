MODULE Simple;
    IMPORT Out;

    TYPE
        TestRec = RECORD
            x : INTEGER;
        END;

    PROCEDURE (this : TestRec) Set (v : INTEGER);
    BEGIN
        Out.Integer(v); Out.Ln;
    END;

BEGIN
    Out.String("Hello world!"); Out.Ln;
END Simple.
