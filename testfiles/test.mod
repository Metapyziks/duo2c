MODULE Test;
    TYPE
        PointType*  = LongReal;
        Point2D*    = POINTER TO Point2DRec;
        Point2DRec* = RECORD
            x* : PointType;
            y* : PointType;
        END;
        Point3D*    = POINTER TO Point3DRec;
        Point3DRec* = RECORD (Point2DRec)
            z* : PointType;
        END;
        TestArray*  = ARRAY 12, 8 OF Integer;
    VAR
        a : Point3D;
        b : Point3D;
        c : Point3D;

    PROCEDURE Point2D* (x : PointType; y : PointType) : Point2D;
        VAR val : Point2D;
    BEGIN
        NEW(val);
        val.x := x;
        val.y := y;
        RETURN val;
    END Point2D;

    PROCEDURE Point3D* (x : PointType; y : PointType; z : PointType) : Point3D;
        VAR val : Point3D;
    BEGIN
        NEW(val);
        val.x := x;
        val.y := y;
        val.z := z;
        RETURN val;
    END Point3D;

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
    a := Point3D(5.2, -6, 12.4E-1);
    b := Point3D(3.824, 3.1D2, 12);

    c := a.Add(b);
END Test.
