MODULE Pong;
    IMPORT GL, GLUT;

    CONST
        WindowWidth = 800;
        WindowHeight = 600;
        UpdateInterval = 16;

    TYPE
        Object = POINTER TO ObjectRec;
        ObjectRec = RECORD
            x, y, width, height : REAL;
        END;

        Ball = POINTER TO BallRec;
        BallRec = RECORD (ObjectRec)
            dx, dy : REAL;
        END;

        Player = POINTER TO PlayerRec;
        PlayerRec = RECORD (ObjectRec)
            score : INTEGER;
        END;

    VAR
        lastUpdate : LONGINT;

        ball    : Ball;
        player1 : Player;
        player2 : Player;

    (* External Procedures *)

    PROCEDURE GetTickCount : LONGINT;
    EXTERNAL "GetTickCount";

    (* Object Procedures *)

    PROCEDURE (this : Object) Bounds(VAR l, t, r, b : REAL);
    BEGIN
        l := this.x - this.width / 2;
        t := this.y - this.height / 2;
        r := l + this.width;
        b := t + this.height;
    END Bounds;

    PROCEDURE (this : Object) IsTouching(that : Object) : BOOLEAN;
    VAR l1, t1, r1, b1, l2, t2, r2, b2 : REAL;
    BEGIN
        this.Bounds(l1, t1, r1, b1);
        that.Bounds(l2, t2, r2, b2);
        RETURN (l1 <= r2) & (r1 >= l2) & (t1 <= b2) & (b1 >= t2);
    END IsTouching;

    PROCEDURE (this : Object) Draw;
    VAR l, t, r, b : REAL;
    BEGIN
        this.Bounds(l, t, r, b);
        DrawRect(l, t, r - l, b - t);
    END Draw;

    (* Ball Procedures *)

    PROCEDURE (this : Ball) New(x, y, dx, dy : REAL);
    BEGIN
        NEW(this);

        this.x  := x;
        this.y  := y;
        this.dx := dx;
        this.dy := dy;

        this.width  := 16;
        this.height := 16;
    END New;

    PROCEDURE (this : Ball) Bounce(ply : Player);
    VAR diff : REAL;
    BEGIN
        this.dx := -this.dx;
        diff := this.y - ply.y;
        this.dy := this.dy + diff / 16;
    END Bounce;

    PROCEDURE (this : Ball) Score(winner : Player);
    BEGIN
        winner.score := winner.score + 1;
        
        IF winner = player1 THEN
            this.x := player2.x - (player2.width + this.width) * 0.5 - 8;
            this.y := player2.y;
        ELSE
            this.x := player1.x + (player1.width + this.width) * 0.5 + 8;
            this.y := player1.y;
        END;
    

        this.dx := -this.dx;
        this.dy := 0;

    END Score;

    PROCEDURE (this : Ball) Update;
    BEGIN
        this.x := this.x + this.dx;
        this.y := this.y + this.dy;

        IF (this.dx < 0) & (this.x < this.width / 2) THEN
            this.Score(player2);
        ELSIF (this.dx > 0) & (this.x >= WindowWidth - this.width / 2) THEN
            this.Score(player1);
        END;

        IF (this.dy < 0) & (this.y < this.height / 2) THEN
            this.dy := -this.dy;
            this.y := this.height / 2;
        ELSIF (this.dy > 0) & (this.y >= WindowHeight - this.height / 2) THEN
            this.dy := -this.dy;
            this.y := WindowHeight - this.height / 2;
        END;

        IF (this.dx < 0) & this.IsTouching(player1) THEN
            this.Bounce(player1);
        ELSIF (this.dx > 0) & this.IsTouching(player2) THEN
            this.Bounce(player2);
        END;
    END Update;

    (* Player Procedures *)

    PROCEDURE (this : Player) New(x, y : REAL);
    BEGIN
        NEW(this);

        this.x := x;
        this.y := y;

        this.width  := 16;
        this.height := 128;
    END New;

    PROCEDURE (this : Player) Update;
    BEGIN
        RETURN;
    END Update;

    (* Global Procedures *)

    PROCEDURE VecToScreen(VAR x, y : REAL);
    BEGIN
        x :=  x / (WindowWidth * 0.5);
        y := -y / (WindowHeight * 0.5);
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

        GL.Color3f(1.0, 1.0, 1.0);
        GL.Vertex2f(x,     y    );
        GL.Vertex2f(x + w, y    );
        GL.Vertex2f(x + w, y + h);
        GL.Vertex2f(x,     y + h);
    END DrawRect;

    PROCEDURE MotionHandler(x, y : INTEGER);
    BEGIN
        player1.y := y;
    END MotionHandler;

    PROCEDURE IdleHandler;
    VAR curTime : LONGINT;
    BEGIN
        curTime := GetTickCount();
        IF curTime - lastUpdate >= UpdateInterval THEN
            lastUpdate := curTime;

            ball.Update;
            player1.Update;
            player2.Update;

            GLUT.PostRedisplay;
        END;
    END IdleHandler;

    PROCEDURE DisplayHandler;
    BEGIN
        GL.Clear(GL.ColorBufferBit);

        GL.Begin(GL.Quads);
            ball.Draw;
            player1.Draw;
            player2.Draw;
        GL.End;
        
        GLUT.SwapBuffers;
    END DisplayHandler;

BEGIN
    ball.New(WindowWidth / 2, WindowHeight / 2, 8, 0);

    player1.New(32, WindowHeight / 2);
    player2.New(WindowWidth - 32, WindowHeight / 2);

    GLUT.Init;

    GLUT.InitDisplayMode(GLUT.RGB OR GLUT.Double);
    GLUT.InitWindowSize(WindowWidth, WindowHeight);
    GLUT.InitWindowPosition(300, 200);

    GLUT.CreateWindow("OBERpONg");

    GLUT.IdleFunc(IdleHandler);
    GLUT.PassiveMotionFunc(MotionHandler);
    GLUT.DisplayFunc(DisplayHandler);

    GLUT.MainLoop;
END Pong.
