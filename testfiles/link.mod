MODULE Link;
    IMPORT Out, List;

    VAR
        i, n : INTEGER;
        test : List.List;

BEGIN
    n := 1;
    FOR i := 1 TO 256 DO
        n := (((n - 1) * 78721 + 1213) MOD 256) + 1;
        test.Add(n);
    END;

    n := 0;
    FOR i := 1 TO test.Count() DO
        IF test.Has(i) THEN
            n := n + 1;
        END;
    END;

    Out.String("Used "); Out.Integer(n);
    Out.String(" of ");
    Out.Integer(test.Count()); Out.Ln;
END Link.
