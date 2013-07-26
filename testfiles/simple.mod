MODULE Simple;
    VAR
        x  : Byte;
        y  : ShortInt;
        z- : Integer;
BEGIN
    x := 5;
    y := 8 + x;
    z := y + 2 - (3 - x + 2) + (y + x - 6);
END Simple.
