MODULE Lists;

    (*** declare global constants, types and variables ***)

    TYPE
        Int32    = Integer;
        List*    = POINTER TO ListNode;
        ListNode = RECORD
            value : Int32;
            next  : List;
        END;

    (*** declare procedures ***)

    PROCEDURE (l : List) Add* (v : Int32);
    BEGIN
        IF l = NIL THEN
            NEW(l);                  (* create record instance *)
            l.value := v
        ELSE
            l.next.Add(v(Int32))      (* recursive call to .add(n) *)
        END;
    END Add;

    PROCEDURE TestMeth (l : List) : List;
    BEGIN
        RETURN l;
    END TestMeth;

    PROCEDURE (l : List) Get* () : Int32;
    VAR
        v : Int32;
    BEGIN
        IF l = NIL THEN
            RETURN 0           (* .get() must always return an INTEGER *)
        ELSE
            v := l.value;      (* this line will crash if l is NIL *)
            l := l.next;
            RETURN v
        END
    END Get;

END Lists.
