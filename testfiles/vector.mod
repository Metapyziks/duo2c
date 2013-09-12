MODULE Vector;
    IMPORT Out;

    CONST N = 16;

    TYPE IntVector = VECTOR N OF INTEGER;

    VAR
        vec : IntVector;
        arr : ARRAY N OF INTEGER;
        
        i : INTEGER;

BEGIN
    FOR i := 0 TO N - 1 DO arr[i] := i; END;

    VECLOAD(arr, vec, 0);

    FOR i := 0 TO N - 1 DO Out.Integer(vec[i]); Out.Ln; END;

END Vector.
