MODULE Vector;
    IMPORT Out;

    CONST N = 16;

    TYPE IntVector = VECTOR N OF INTEGER;

    VAR
        vec : IntVector;
        arr : ARRAY N OF INTEGER;

        i : INTEGER;

BEGIN
    FOR i := 0 TO N - 1 DO vec[i] := i; END;

    vec := vec * 2 + 1;

    VECSTORE(arr, vec);

    FOR i := 0 TO N - 1 DO Out.Integer(arr[i]); Out.Ln; END;

END Vector.
