MODULE Test;
    IMPORT Out;

    TYPE
        PointType*  = LONGREAL;
        Point2D*    = POINTER TO Point2DRec;
        Point2DRec* = RECORD
            x* : PointType;
            y* : PointType;
        END;
        Point3D*    = POINTER TO Point3DRec;
        Point3DRec* = RECORD (Point2DRec)
            z* : PointType;
        END;
    VAR
        a : Point3D;
        b : Point3D;
        c : Point3D;

    PROCEDURE (this : Point2D) New* (x : PointType; y : PointType);
    BEGIN
        NEW(this);
        this.x := x;
        this.y := y;
    END Point2D;

    PROCEDURE (this : Point3D) New* (x : PointType; y : PointType; z : PointType);
    BEGIN
        NEW(this);
        this.x := x;
        this.y := y;
        this.z := z;
    END Point3D;

    PROCEDURE (this : Point2D) Print*;
    BEGIN
        Out.String("(");
        Out.Real(this.x);
        Out.String(" ");
        Out.Real(this.y);
        Out.String(")");
    END Print;

    PROCEDURE (this : Point3D) Print*;
    BEGIN
        Out.String("(");
        Out.Real(this.x);
        Out.String(" ");
        Out.Real(this.y);
        Out.String(" ");
        Out.Real(this.z);
        Out.String(")");
    END Print;

    PROCEDURE (this : Point2D) Add* (that : Point2D) : Point2D;
        VAR sum : Point2D;
    BEGIN
        NEW(sum);
        sum.x := this.x + that.x;
        sum.y := this.y + that.y;
        RETURN sum;
    END Add;

    PROCEDURE (this : Point2D) Sub* (that : Point2D) : Point2D;
        VAR dif : Point2D;
    BEGIN
        NEW(dif);
        dif.x := this.x - that.x;
        dif.y := this.y - that.y;
        RETURN dif;
    END Sub;

    PROCEDURE (this : Point3D) Add* (that : Point3D) : Point3D;
        VAR sum : Point3D;
    BEGIN
        NEW(sum);
        sum.x := this.x + that.x;
        sum.y := this.y + that.y;
        sum.z := this.z + that.z;
        RETURN sum;
    END Add;

    PROCEDURE (this : Point3D) Sub* (that : Point3D) : Point3D;
        VAR dif : Point3D;
    BEGIN
        NEW(dif);
        dif.x := this.x - that.x;
        dif.y := this.y - that.y;
        dif.z := this.z - that.z;
        RETURN dif;
    END Sub;
BEGIN
    a.New(5.2, -6, 12.4E-1);
    b.New(3.824, 3.1D2, 12);

    a.Print; Out.Ln;
    b.Print; Out.Ln;

    c := a.Add(b);

    c.Print; Out.Ln;
END Test.
