MODULE GLTest;
    IMPORT GL, GLUT, Out;

    PROCEDURE DisplayHandler;
    BEGIN
        GL.Clear(16640);
        GL.Enable(2929);

        GL.MatrixMode(5889);
        GL.LoadIdentity;
        GL.Ortho(-2.0, 2.0, -2.0, 2.0, -2.0, 500.0);

        GL.MatrixMode(5888);
        GL.LoadIdentity;
        GLUT.LookAt(2, 2, 2, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0);
        GL.Scalef(0.005, 0.005, 0.005);
        GL.Rotatef(20, 0, 1, 0);
        GL.Rotatef(30, 0, 0, 1);
        GL.Rotatef(5, 1, 0, 0);
        GL.Translatef(-300, 0, 0);

        GL.Color3f(1, 1, 1);
        GLUT.StrokeCharacter(NIL, 'H');
        GLUT.StrokeCharacter(NIL, 'e');
        GLUT.StrokeCharacter(NIL, 'l');
        GLUT.StrokeCharacter(NIL, 'l');
        GLUT.StrokeCharacter(NIL, 'o');

        GLUT.StrokeCharacter(NIL, 'W');
        GLUT.StrokeCharacter(NIL, 'o');
        GLUT.StrokeCharacter(NIL, 'r');
        GLUT.StrokeCharacter(NIL, 'l');
        GLUT.StrokeCharacter(NIL, 'd');
        GLUT.StrokeCharacter(NIL, '!');
            
        GLUT.SwapBuffers;
    END DisplayHandler;

BEGIN
    Out.String("Init"); Out.Ln;
    GLUT.Init;
    Out.String("InitDisplayMode"); Out.Ln;
    GLUT.InitDisplayMode(18);
    Out.String("InitWindowSize"); Out.Ln;
    GLUT.InitWindowSize(500, 500);
    Out.String("InitWindowPosition"); Out.Ln;
    GLUT.InitWindowPosition(300, 200);
    Out.String("CreateWindow"); Out.Ln;
    GLUT.CreateWindow("Hello World!");
    Out.String("DisplayFunc"); Out.Ln;
    GLUT.DisplayFunc(DisplayHandler);
    Out.String("MainLoop"); Out.Ln;
    GLUT.MainLoop;
END GLTest.
