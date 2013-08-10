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
        A* : Vector1Rec;
        B* : Vector2Rec;
        C* : Vector3Rec;

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
        Out.String("("); Out.Integer(this^.x);
        Out.String(","); Out.Integer(this^.y);
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
        Out.String("("); Out.Integer(this^.x);
        Out.String(","); Out.Integer(this^.y);
        Out.String(","); Out.Integer(this^.z);
        Out.String(")"); Out.Ln;
    END;
BEGIN
    C.SetX(1);
    C.SetY(2);
    C.SetZ(3);
    C.Print;
END Simple.
