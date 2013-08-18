MODULE GLTest;
    IMPORT GL, GLUT, Out;

    VAR r, g, b : REAL;
    VAR lastDraw : LONGINT;

    PROCEDURE GetTickCount : LONGINT;
    EXTERNAL "GetTickCount";

    PROCEDURE IdleHandler;
    VAR curTime : LONGINT;
    BEGIN
        curTime := GetTickCount();
        IF curTime - lastDraw > 16 THEN
            r := r + 1.0 / 16.0;
            IF r >= 2 THEN
                r := 0;
                g := g + 1.0 / 16.0;
                IF g >= 2 THEN
                    g := 0;
                    b := b + 1.0 / 16.0;
                    IF b >= 2 THEN
                        b := 0;
                    END;
                END;
            END;

            lastDraw := curTime;
            GLUT.PostRedisplay;
        END;
    END;

    PROCEDURE DisplayHandler;
        VAR rn, gn, bn : REAL;
    BEGIN
        GL.Clear(GL.ColorBufferBit OR GL.DepthBufferBit);
        GL.Enable(GL.DepthTest);

        GL.MatrixMode(GL.Projection);
        GL.LoadIdentity;
        GL.Ortho(-2.0, 2.0, -2.0, 2.0, -2.0, 500.0);

        GL.MatrixMode(GL.Modelview);
        GL.LoadIdentity;
        GLUT.LookAt(2, 2, 2, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0);
        GL.Scalef(0.005, 0.005, 0.005);
        GL.Rotatef(20, 0, 1, 0);
        GL.Rotatef(30, 0, 0, 1);
        GL.Rotatef(5, 1, 0, 0);
        GL.Translatef(-300, 0, 0);

        IF r < 1 THEN rn := r; ELSE rn := 2 - r; END;
        IF g < 1 THEN gn := g; ELSE gn := 2 - g; END;
        IF b < 1 THEN bn := b; ELSE bn := 2 - b; END;

        GL.Color3f(rn, gn, bn);
        GLUT.StrokeCharacter(NIL, 'H');
        GLUT.StrokeCharacter(NIL, 'e');
        GLUT.StrokeCharacter(NIL, 'l');
        GLUT.StrokeCharacter(NIL, 'l');
        GLUT.StrokeCharacter(NIL, 'a');

        GLUT.StrokeCharacter(NIL, 'V');
        GLUT.StrokeCharacter(NIL, 'u');
        GLUT.StrokeCharacter(NIL, 'r');
        GLUT.StrokeCharacter(NIL, 'l');
        GLUT.StrokeCharacter(NIL, 'd');
        GLUT.StrokeCharacter(NIL, '!');
            
        GLUT.SwapBuffers;
    END DisplayHandler;

BEGIN
    GLUT.Init;
    GLUT.InitDisplayMode(GLUT.RGB OR GLUT.Double OR GLUT.Depth);
    GLUT.InitWindowSize(500, 500);
    GLUT.InitWindowPosition(300, 200);
    GLUT.CreateWindow("Hello World!");
    GLUT.DisplayFunc(DisplayHandler);
    GLUT.IdleFunc(IdleHandler);
    GLUT.MainLoop;
END GLTest.
