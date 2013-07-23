MODULE Test;
    TYPE
        PointType  = LongReal;
        Point2D*   = POINTER TO Point2DRec;
        Point2DRec = RECORD
            x : PointType;
            y : PointType;
        END;
        Point3D*   = POINTER TO Point3DRec;
        Point3DRec = RECORD (Point2DRec)
            z : PointType;
        END;
    VAR
        a : Point2D;
        b : Point3D;

    PROCEDURE CreatePoint2D* (x : PointType; y : PointType) : Point2D;
        VAR point : Point2D;
    BEGIN
        NEW(point);
        point.x := x;
        point.y := y;
        RETURN point;
    END CreatePoint2D;

    PROCEDURE CreatePoint3D* (x : PointType; y : PointType; z : PointType) : Point3D;
        VAR point : Point3D;
    BEGIN
        NEW(point);
        point.x := x;
        point.y := y;
        point.z := z;
        RETURN point;
    END CreatePoint3D;

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
    a := CreatePoint2D(5.2, 8.4);

    a := CreatePoint3D(6, 2, -1);
    b := CreatePoint3D(3.824, 3.1D2, 12);

    a := a.Add(b);

    b := CreatePoint2D(-4, 3)(Point3D);
    a := b.Sub(a(Point3D));
    b := a.Add(b)(Point3D);
    b := b.Sub(a(Point3D));
END Test.
