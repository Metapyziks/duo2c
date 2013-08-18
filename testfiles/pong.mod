MODULE Pong;
    IMPORT GL, GLUT;

    CONST
        Width = 800;
        Height = 600;

    PROCEDURE VecToScreen(VAR x, y : REAL);
    BEGIN
        x := x / (Width * 0.5);
        y := -y / (Height * 0.5);
    END VecToScreen;

    PROCEDURE PosToScreen(VAR x, y : REAL);
    BEGIN
        VecToScreen(x, y);
        x := x - 1;
        y := 1 + y;
    END PosToScreen;

    PROCEDURE DrawRect(x, y, w, h : REAL);
    BEGIN
        PosToScreen(x, y);
        VecToScreen(w, h);

        GL.Begin(GL.Quads);
            GL.Color3f(1.0, 1.0, 1.0);
            GL.Vertex2f(x,     y    );
            GL.Vertex2f(x + w, y    );
            GL.Vertex2f(x + w, y + h);
            GL.Vertex2f(x,     y + h);
        GL.End;
    END DrawRect;

    PROCEDURE IdleHandler;
    BEGIN
        RETURN;
    END IdleHandler;

    PROCEDURE DisplayHandler;
    BEGIN
        GL.Clear(GL.ColorBufferBit);

        DrawRect(32, 32, 128, 128);
        
        GLUT.SwapBuffers;
    END DisplayHandler;

BEGIN
    GLUT.Init;
    GLUT.InitDisplayMode(GLUT.RGB OR GLUT.Double);
    GLUT.InitWindowSize(Width, Height);
    GLUT.InitWindowPosition(300, 200);
    GLUT.CreateWindow("OBERpONg");
    GLUT.IdleFunc(IdleHandler);
    GLUT.DisplayFunc(DisplayHandler);
    GLUT.MainLoop;
END Pong.
