MODULE Vector;
    IMPORT Out;

    VAR a : VECTOR 8 OF INTEGER;
    VAR b : VECTOR 8 OF INTEGER;
    VAR c : VECTOR 8 OF INTEGER;

BEGIN

    a := <0, 1, 2, 3, 4, 5, 6, 7>;
    b := <2, 2, 2, 2, 2, 2, 2, 2>;

    c := a + b;

END Vector.
