MODULE Simple;
    IMPORT Out;

    TYPE
        Vector1Rec* = RECORD
            x : INTEGER;
        END;
        Vector1* = POINTER TO Vector1Rec;
        Vector2Rec* = RECORD (Vector1Rec)
            y : INTEGER;
        END;
        Vector2* = POINTER TO Vector2Rec;
        Vector3Rec* = RECORD (Vector2Rec)
            z : INTEGER;
        END;
        Vector3* = POINTER TO Vector3Rec;

    VAR
        A* : Vector1;
        B* : Vector2;
        C* : Vector3;

    PROCEDURE (this : Vector1) SetX (val : INTEGER);
    BEGIN
        this^.x := val;
    END;

    PROCEDURE (this : Vector1) GetX : INTEGER;
    BEGIN
        RETURN this^.x;
    END;

    PROCEDURE (this : Vector1) Print;
    BEGIN
        IF this = NIL THEN
            Out.String("NIL"); Out.Ln;
            RETURN;
        END;

        Out.String("("); Out.Integer(this^.x);
        Out.String(")"); Out.Ln;
    END;

    PROCEDURE (this : Vector2) SetY (val : INTEGER);
    BEGIN
        this^.y := val;
    END;

    PROCEDURE (this : Vector2) GetY : INTEGER;
    BEGIN
        RETURN this^.y;
    END;

    PROCEDURE (this : Vector2) Print;
    BEGIN
        IF this = NIL THEN
            Out.String("NIL"); Out.Ln;
            RETURN;
        END;

        Out.String("("); Out.Integer(this^.x);
        Out.String(" "); Out.Integer(this^.y);
        Out.String(")"); Out.Ln;
    END;

    PROCEDURE (this : Vector3) SetZ (val : INTEGER);
    BEGIN
        this^.z := val;
    END;

    PROCEDURE (this : Vector3) GetZ : INTEGER;
    BEGIN
        RETURN this^.z;
    END;

    PROCEDURE (this : Vector3) Print;
    BEGIN
        IF this = NIL THEN
            Out.String("NIL"); Out.Ln;
            RETURN;
        END;

        Out.String("("); Out.Integer(this^.x);
        Out.String(" "); Out.Integer(this^.y);
        Out.String(" "); Out.Integer(this^.z);
        Out.String(")"); Out.Ln;
    END;

    PROCEDURE TestType (vec : Vector1; name : ARRAY OF CHAR);
    BEGIN
        IF vec IS Vector3 THEN
            Out.String(name); Out.String(" is a Vector3"); Out.Ln;
        ELSIF vec IS Vector2 THEN
            Out.String(name); Out.String(" is a Vector2"); Out.Ln;
        ELSIF vec IS Vector1 THEN
            Out.String(name); Out.String(" is a Vector1"); Out.Ln;
        ELSE
            Out.String(name); Out.String(" is not a vector"); Out.Ln;
        END;
    END;

BEGIN
    NEW(C);

    C.SetX(56);
    C.SetY(-3);
    C.SetZ(12);

    Out.String("C = "); C.Print;
    TestType(C, "C");

    NEW(A);

    A.SetX(C.GetY() * 3);

    Out.String("A = "); A.Print;
    TestType(A, "A");

    B := C;

    Out.String("B = "); B.Print;
    TestType(B, "B");

    A := NIL;

    Out.String("A = "); A.Print;
    TestType(A, "NIL");

END Simple.
