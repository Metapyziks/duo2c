MODULE GL;

    PROCEDURE Clear* (buffer : INTEGER);
    EXTERNAL "glClear";

    PROCEDURE Enable* (cap : INTEGER);
    EXTERNAL "glEnable";

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

END GL.
