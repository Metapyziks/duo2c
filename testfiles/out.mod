MODULE Out;
    TYPE
        String* = ARRAY OF CHAR;

    PROCEDURE ^ Ln*;
    PROCEDURE ^ String*(val : String);
    PROCEDURE ^ Byte*(val : BYTE);
    PROCEDURE ^ ShortInt*(val : SHORTINT);
    PROCEDURE ^ Integer*(val : INTEGER);
    PROCEDURE ^ LongInt*(val : LONGINT);
    PROCEDURE ^ Real*(val : REAL);
    PROCEDURE ^ LongReal*(val : LONGREAL);
END Out.
