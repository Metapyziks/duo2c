MODULE List;
    IMPORT Out;

    TYPE
        List*     = POINTER TO ListNode;
        ListNode* = RECORD
            value* : INTEGER;
            next*  : List;
        END;

    VAR
        test : List;
        i : INTEGER;
        n : INTEGER;

    PROCEDURE (l : List) Add* (v : INTEGER);
    BEGIN
        IF l = NIL THEN
            NEW(l);
            l.value := v;
        ELSE
            l.next.Add(v);
        END;
    END Add;

    PROCEDURE (l : List) Get* : INTEGER;
    VAR
        v : INTEGER;
    BEGIN
        IF l = NIL THEN
            RETURN 0;
        ELSE
            v := l.value;
            l := l.next;
            RETURN v;
        END
    END Get;

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

END List.
