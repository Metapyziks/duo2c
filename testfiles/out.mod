MODULE Out;
    TYPE
        String* = ARRAY OF CHAR;

    PROCEDURE ^ Ln*;
    PROCEDURE ^ String*(val : String);
    PROCEDURE ^ Integer*(val : LONGINT);
    PROCEDURE ^ Real*(val : LONGREAL);
END Out.
