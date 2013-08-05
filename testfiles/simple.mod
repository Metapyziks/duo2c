MODULE Simple;
    IMPORT Out;

    PROCEDURE Test;
    VAR
        i : INTEGER;
        n : INTEGER;
        stra : ARRAY OF CHAR;
        strb : ARRAY OF CHAR;
    BEGIN
        stra := "#";
        strb := "-";

        FOR i := 1 TO 9 DO
            FOR n := 0 TO 16 DO
                IF n < i * i - 10 * i + 25 THEN
                    Out.String(stra);
                ELSE
                    Out.String(strb);
                END;
            END;
            Out.Ln;
        END;
    END;

BEGIN
    Test();
END Simple.
