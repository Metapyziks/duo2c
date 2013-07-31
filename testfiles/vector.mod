MODULE Vector;
    IMPORT Out;

    VAR arr : ARRAY 8 OF INTEGER;

    VAR a : VECTOR 8 OF INTEGER;
    VAR b : VECTOR 8 OF INTEGER;
    VAR c : VECTOR 8 OF INTEGER;

    VAR d : Vector 8 OF INTEGER;

BEGIN
    (* Set all elements in a vector *)
    VECSET(a, 0);

    (* Alternatively *)
    a := 0;
    
    (* Set an indexed element in a vector *)
    a[2] := 5;

    (* Set each element in a vector *)
    VECSET(b, 1, 7, -1, 12, 93, 0, 4, 2);

    (* Alternatively *)
    b := <1, 7, -1, 12, 93, 0, 4, 2>;

    (* Copy 8 elements from an array starting at index 0,
     * to be placed at indices starting at 0 in a vector *)
    VECLOAD(c, arr, 0, 8, 0);

    (* Perform standard numeric operations on vectors *)
    d := a + b * c;

    (* Perhaps support operations between vectors and atomic types? *)
    d := d MOD 8;

    (* Copy 4 elements from a vector starting at index 0,
     * to be placed at indices starting at 2 in an array *)
    VECSTORE(d, arr, 0, 4, 2);

END Vector.
