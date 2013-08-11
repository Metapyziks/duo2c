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
    
    FOR i := 1 TO 10 DO
        test.Add(i);
    END;

    REPEAT
        i := test.Get();
        Out.Integer(i); Out.Ln;
    UNTIL i = 0;

END List.
