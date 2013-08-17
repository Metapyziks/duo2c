MODULE GLUT;
    TYPE
        ExitType = PROCEDURE (v : INTEGER);

    PROCEDURE Exit (errorCode : INTEGER);
    EXTERNAL "exit";

    PROCEDURE LookAt* (eyeX,    eyeY,    eyeZ,
                       centerX, centerY, centerZ,
                       upX,     upY,     upZ : LONGREAL);
    EXTERNAL "gluLookAt";

    PROCEDURE StrokeCharacter* (font : POINTER TO BYTE; character : CHAR);
    EXTERNAL IMPORT "glutStrokeCharacter";

    PROCEDURE SwapBuffers*;
    EXTERNAL IMPORT "glutSwapBuffers";

    PROCEDURE InitInternal (argc : INTEGER;
        argv : POINTER TO POINTER TO CHAR; VAR exit : ExitType);
    EXTERNAL IMPORT "__glutInitWithExit";

    PROCEDURE Init*;
    BEGIN
        InitInternal(0, NIL, Exit);
    END Init;

    PROCEDURE InitDisplayMode* (mode : INTEGER);
    EXTERNAL IMPORT "glutInitDisplayMode";

    PROCEDURE InitWindowSize* (width, height : INTEGER);
    EXTERNAL IMPORT "glutInitWindowSize";

    PROCEDURE InitWindowPosition* (x, y : INTEGER);
    EXTERNAL IMPORT "glutInitWindowPosition";

    PROCEDURE CreateWindowInternal (title : ARRAY OF CHAR; VAR exit : ExitType);
    EXTERNAL IMPORT "__glutCreateWindowWithExit";

    PROCEDURE CreateWindow* (title : ARRAY OF CHAR);
    BEGIN
        CreateWindowInternal(title, Exit);
    END CreateWindow;

    PROCEDURE DisplayFunc* (VAR func : PROCEDURE);
    EXTERNAL IMPORT "glutDisplayFunc";

    PROCEDURE MainLoop*;
    EXTERNAL IMPORT "glutMainLoop";

END GLUT.
