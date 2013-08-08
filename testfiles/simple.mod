MODULE Simple;
    IMPORT Out;

    TYPE
        Vector1 = RECORD
            x : INTEGER;
        END;
        Vector2 = RECORD (Vector1)
            y : INTEGER;
        END;
        Vector3 = RECORD (Vector2)
            z : INTEGER;
        END;

    VAR a : Vector1;
    VAR b : Vector2;
    VAR c : Vector3;

    PROCEDURE (this : Vector1) SetX (val : INTEGER);
    BEGIN
        this.x := val;
    END;

    PROCEDURE (this : Vector1) GetX : INTEGER;
    BEGIN
        RETURN this.x;
    END;

    PROCEDURE (this : Vector1) Print;
    BEGIN
        Out.String("("); Out.Integer(this.x);
        Out.String(")"); Out.Ln;
    END;

    PROCEDURE (this : Vector2) SetY (val : INTEGER);
    BEGIN
        this.y := val;
    END;

    PROCEDURE (this : Vector2) GetY : INTEGER;
    BEGIN
        RETURN this.y;
    END;

    PROCEDURE (this : Vector2) Print;
    BEGIN
        Out.String("("); Out.Integer(this.x);
        Out.String(","); Out.Integer(this.y);
        Out.String(")"); Out.Ln;
    END;

    PROCEDURE (this : Vector3) SetZ (val : INTEGER);
    BEGIN
        this.z := val;
    END;

    PROCEDURE (this : Vector3) GetZ : INTEGER;
    BEGIN
        RETURN this.z;
    END;

    PROCEDURE (this : Vector3) Print;
    BEGIN
        Out.String("("); Out.Integer(this.x);
        Out.String(","); Out.Integer(this.y);
        Out.String(","); Out.Integer(this.z);
        Out.String(")"); Out.Ln;
    END;

BEGIN
    a.SetX(13);

    b.SetX(8);
    b.SetY(-3);

    c.SetX(8);
    c.SetY(-3);
    c.SetZ(92);

    a.Print;
    b.Print;
    c.Print;
END Simple.
