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
        IF l^.next = NIL THEN
            NEW(l^.next);
            l^.value := v;
        ELSE
            l^.next.Add(v);
        END;
    END Add;

    PROCEDURE (l : List) Get* : INTEGER;
    VAR
        v : INTEGER;
    BEGIN
        IF l^.next = NIL THEN
            RETURN 0;
        ELSE
            v := l^.value;
            l := l^.next;
            RETURN v;
        END
    END Get;

BEGIN
    NEW(test);
    
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
