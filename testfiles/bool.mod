MODULE Bool;
    IMPORT Out;

    PROCEDURE True(msg : ARRAY OF CHAR) : BOOLEAN;
    BEGIN
        Out.String("+"); Out.String(msg); Out.Ln;
        RETURN TRUE;
    END;

    PROCEDURE False(msg : ARRAY OF CHAR) : BOOLEAN;
    BEGIN
        Out.String("-"); Out.String(msg); Out.Ln;
        RETURN FALSE;
    END;

BEGIN
    Out.String("Start"); Out.Ln;
    IF True("A") & True("B") & False("C") & True("D") THEN
        Out.String("True"); Out.Ln;
    ELSE
        Out.String("False"); Out.Ln;
    END;
END Bool.
