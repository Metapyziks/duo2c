MODULE Out;
    PROCEDURE ^ Ln*;
    PROCEDURE ^ String*(val : ARRAY OF CHAR);
    PROCEDURE ^ Integer*(val : LONGINT);
    PROCEDURE ^ Real*(val : LONGREAL);
    PROCEDURE ^ Boolean*(val : BOOLEAN);
BEGIN
    Out.String("Hello from Out!"); Out.Ln;
END Out.
