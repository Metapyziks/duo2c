MODULE Pong;
    IMPORT GL, GLUT, Out;

    PROCEDURE IdleHandler;
    BEGIN
        RETURN;
    END IdleHandler;

    PROCEDURE DisplayHandler;
    BEGIN
        GL.Clear(16640);
        
        GLUT.SwapBuffers;
    END DisplayHandler;

BEGIN
    GLUT.Init;
    GLUT.InitDisplayMode(18);
    GLUT.InitWindowSize(800, 600);
    GLUT.InitWindowPosition(300, 200);
    GLUT.CreateWindow("OBERpONg");
    GLUT.IdleFunc(IdleHandler);
    GLUT.DisplayFunc(DisplayHandler);
    GLUT.MainLoop;
END Pong.
