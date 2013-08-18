MODULE GL;

    CONST
        Triangles      * = 00004H;
        Quads          * = 00007H;
        ColorBufferBit * = 04000H;
        DepthBufferBit * = 00100H;
        DepthTest      * = 00B71H;
        Projection     * = 01701H;
        Modelview      * = 01700H;

    PROCEDURE Clear* (buffer : INTEGER);
    EXTERNAL "glClear";

    PROCEDURE Enable* (cap : INTEGER);
    EXTERNAL "glEnable";

    PROCEDURE Begin* (mode : INTEGER);
    EXTERNAL "glBegin";

    PROCEDURE End*;
    EXTERNAL "glEnd";

    PROCEDURE MatrixMode* (mode : INTEGER);
    EXTERNAL "glMatrixMode";

    PROCEDURE LoadIdentity*;
    EXTERNAL "glLoadIdentity";

    PROCEDURE Ortho* (left, right, bottom, top, nearVal, farVal : LONGREAL);
    EXTERNAL "glOrtho";

    PROCEDURE Scalef* (x, y, z : REAL);
    EXTERNAL "glScalef";

    PROCEDURE Rotatef* (ang, x, y, z : REAL);
    EXTERNAL "glRotatef";

    PROCEDURE Translatef* (x, y, z : REAL);
    EXTERNAL "glTranslatef";

    PROCEDURE Color3f* (r, g, b : REAL);
    EXTERNAL "glColor3f";

    PROCEDURE Vertex2f* (x, y : REAL);
    EXTERNAL "glVertex2f";

    PROCEDURE Vertex3f* (x, y, z : REAL);
    EXTERNAL "glVertex3f";

END GL.
