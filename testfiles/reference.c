#include <GL/glut.h>            /* Open GL Util    OpenGL*/

void displayCall() {
  glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
  glEnable(GL_DEPTH_TEST);

  glMatrixMode(GL_PROJECTION);
  glLoadIdentity();
  glOrtho(-2.0, 2.0, -2.0, 2.0, -2.0, 500.0);

  glMatrixMode(GL_MODELVIEW);
  glLoadIdentity();
  gluLookAt(2, 2, 2, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0);
  glScalef(.005,.005,.005);
  glRotatef(20, 0, 1, 0);
  glRotatef(30, 0, 0, 1);
  glRotatef(5, 1, 0, 0);
  glTranslatef(-300, 0, 0);
    
  glColor3f(1,1,1);
  glutStrokeCharacter(GLUT_STROKE_ROMAN, 'H');
  glutStrokeCharacter(GLUT_STROKE_ROMAN, 'e');
  glutStrokeCharacter(GLUT_STROKE_ROMAN, 'l');
  glutStrokeCharacter(GLUT_STROKE_ROMAN, 'l');
  glutStrokeCharacter(GLUT_STROKE_ROMAN, 'o');
  
  glutStrokeCharacter(GLUT_STROKE_ROMAN, 'W');
  glutStrokeCharacter(GLUT_STROKE_ROMAN, 'o');
  glutStrokeCharacter(GLUT_STROKE_ROMAN, 'r');
  glutStrokeCharacter(GLUT_STROKE_ROMAN, 'l');
  glutStrokeCharacter(GLUT_STROKE_ROMAN, 'd');
  glutStrokeCharacter(GLUT_STROKE_ROMAN, '!');
        
  glutSwapBuffers();
}

int main(int argc, char *argv[]) {
  glutInit(&argc, argv);
  glutInitDisplayMode(GLUT_RGB | GLUT_DOUBLE | GLUT_DEPTH);
  glutInitWindowSize(500, 500);
  glutInitWindowPosition(300, 200);
  glutCreateWindow("Hello World!");
  glutDisplayFunc(displayCall);
  glutMainLoop();
  return 0;
}
