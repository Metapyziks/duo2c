MODULE Simple;
    IMPORT Out;

    PROCEDURE WriteLine (str : ARRAY OF CHAR);
    BEGIN
        Out.String(str); Out.Ln;
    END;

BEGIN
    Out.String("Hello world!"); Out.Ln;
    WriteLine("Hello to you too!");
END Simple.
