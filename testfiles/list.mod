MODULE List;
    IMPORT Out;

    (*** declare global constants, types and variables ***)

    TYPE
        Int32*  = INTEGER;
        List*     = POINTER TO ListNode;
        ListNode* = RECORD
            value : Int32;
            next  : List;
        END;

    (*** declare procedures ***)

    PROCEDURE (l : List) Add* (v : Int32);
    BEGIN
        IF l = NIL THEN
            NEW(l);             (* create record instance *)
            l.value := v
        ELSE
            l.next.Add(v)      (* recursive call to .add(n) *)
        END
    END Add;

    PROCEDURE (l : List) Get* () : Int32;
    VAR
        v : Int32;
    BEGIN
        IF l = NIL THEN
            RETURN 0           (* .get() must always return an INTEGER *)
        ELSE
            v := l.value;       (* this line will crash if l is NIL *)
            l := l.next;
            RETURN v
        END
    END Get;

    PROCEDURE (l : List) Has* (v : Int32) : BOOLEAN;
    BEGIN
        IF l = NIL THEN
            RETURN FALSE;
        ELSIF l.value = v THEN
            RETURN TRUE;
        ELSE
            RETURN l.next.Has(v);
        END
    END Has;

    PROCEDURE (l : List) Count* : INTEGER;
    BEGIN
        IF l = NIL THEN
            RETURN 0;
        ELSE
            RETURN 1 + l.next.Count();
        END;
    END Count;

BEGIN
    Out.String("Hello from List!"); Out.Ln;
END List.
