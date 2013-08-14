MODULE Link;
    IMPORT Out, List;

    VAR
        i, n : INTEGER;
        test : List.List;

BEGIN
    n := 1;
    FOR i := 1 TO 10 DO
        n := (n * 8723 + 181) MOD 256;
        test.Add(n);
    END;

    REPEAT
        i := test.Get();
        IF i > 0 THEN Out.Integer(i); Out.Ln; END;
    UNTIL i = 0;
END Link.
