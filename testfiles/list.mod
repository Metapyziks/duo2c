MODULE Lists;

    (*** declare global constants, types and variables ***)

    TYPE
        (* Integer  = INTEGER; *)
        List*    = POINTER TO ListNode;
        ListNode = RECORD
            value : Integer;
            next  : List;
        END;

    VAR
        i, n : INTEGER;
        test : List;

    (*** declare procedures ***)

    PROCEDURE (l : List) Add* (v : Integer);
    BEGIN
        IF l = NIL THEN
            NEW(l);             (* create record instance *)
            l.value := v
        ELSE
            l.next.Add(v)      (* recursive call to .add(n) *)
        END
    END Add;

    PROCEDURE (l : List) Get* () : Integer;
    VAR
        v : Integer;
    BEGIN
        IF l = NIL THEN
            RETURN 0           (* .get() must always return an INTEGER *)
        ELSE
            v := l.value;       (* this line will crash if l is NIL *)
            l := l.next;
            RETURN v
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
