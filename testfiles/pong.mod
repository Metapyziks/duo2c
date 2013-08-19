MODULE Pong;
    IMPORT GL, GLUT;

    CONST
        WindowWidth = 800;
        WindowHeight = 600;
        UpdateInterval = 16;
        Margin = 24;

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

        Human = POINTER TO HumanRec;
        HumanRec = RECORD (PlayerRec) END;

        Computer = POINTER TO ComputerRec;
        ComputerRec = RECORD (PlayerRec)
            destY : REAL;
            nextTry : LONGREAL;
        END;

    VAR
        lastUpdate : LONGINT;
        mouseX, mouseY : INTEGER;
        ball : Ball;
        player1 : Human;
        player2 : Computer;

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
        IF this = NIL THEN NEW(this); END;

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
            this.x := player2.x - (player2.width + this.width) / 2 - 8;
            this.y := player2.y;
        ELSE
            this.x := player1.x + (player1.width + this.width) / 2 + 8;
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

        IF (this.dy < 0) & (this.y < Margin + this.height / 2) THEN
            this.dy := -this.dy;
            this.y := Margin + this.height / 2;
        ELSIF (this.dy > 0) & (this.y >= WindowHeight - Margin - this.height / 2) THEN
            this.dy := -this.dy;
            this.y := WindowHeight - Margin - this.height / 2;
        END;

        IF (this.dx < 0) & this.IsTouching(player1) THEN
            this.Bounce(player1);
        ELSIF (this.dx > 0) & this.IsTouching(player2) THEN
            this.Bounce(player2);
        END;
    END Update;

    (* Player Procedures *)

    PROCEDURE (this : Player) Init(x, y : REAL);
    BEGIN
        this.x := x;
        this.y := y;

        this.width  := 16;
        this.height := 128;
    END New;

    PROCEDURE (this : Player) SetY(y : REAL);
    BEGIN
        IF y < Margin + this.height / 2 THEN
            this.y := Margin + this.height / 2;
        ELSIF y > WindowHeight - Margin - this.height / 2 THEN
            this.y := WindowHeight - Margin - this.height / 2;
        ELSE
            this.y := y;
        END;
    END SetY;

    PROCEDURE (this : Player) Update;
    BEGIN
        RETURN;
    END Update;

    (* Human Procedures *)

    PROCEDURE (this : Human) New(x, y : REAL);
    BEGIN
        NEW(this);
        this.Init(x, y);
    END New;

    PROCEDURE (this : Human) Update;
    BEGIN
        this.SetY(mouseY);
    END Update;

    (* Computer Procedures *)

    PROCEDURE (this : Computer) New(x, y : REAL);
    BEGIN
        NEW(this);
        this.Init(x, y);
    END New;

    PROCEDURE (this : Computer) Update;
    VAR curTime : LONGINT;
    VAR dy, guess : REAL;
    BEGIN
        curTime := GetTickCount();
        IF curTime >= this.nextTry THEN
            this.nextTry := curTime + 250 + Abs(ball.x - this.x);
            this.destY := ball.y + ball.dy * Abs(ball.x - this.x) * 1.5 / ball.dx;

            WHILE (this.destY < 0) OR (this.destY > WindowHeight) DO
                IF this.destY < 0 THEN
                    this.destY := 2 * (Margin + ball.height / 2) - this.destY;
                ELSE
                    this.destY := 2 * (WindowHeight - Margin - ball.height / 2) - this.destY;
                END;
            END;
        END;
        
        dy := (this.destY - this.y) * 0.1;

        IF dy >  16 THEN dy :=  16; END;
        IF dy < -16 THEN dy := -16; END;

        this.SetY(this.y + dy);
    END Update;

    (* Global Procedures *)

    PROCEDURE VecToScreen(VAR x, y : REAL);
    BEGIN
        x := ( x * 2) / WindowWidth;
        y := (-y * 2) / WindowHeight;
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

    PROCEDURE DrawScore(score : INTEGER; x, y : REAL; dx, height : REAL);
    VAR i : INTEGER;
    BEGIN
        IF dx < 0 THEN x := x; END;
        FOR i := 0 TO score - 1 DO
            DrawRect(x + dx * i, y, dx * 0.75, height);
        END;
    END;

    PROCEDURE MotionHandler(x, y : INTEGER);
    BEGIN
        mouseX := x;
        mouseY := y;
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

            DrawScore(player1.score, 4, 4, 12, 16);
            DrawScore(player2.score, WindowWidth - 4, 4, -12, 16);
        GL.End;

        GL.Color3f(0.25, 0.25, 0.25);
        
        GLUT.SwapBuffers;
    END DisplayHandler;

    PROCEDURE Abs(v : REAL) : REAL;
    BEGIN
        IF v < 0 THEN RETURN -v; END;
        RETURN v;
    END;

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
