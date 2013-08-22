bin/gltest.exe: out.mod gl.mod glut.mod gltest.mod
	../bin/Debug/duo2c.exe -o bin/gltest.exe -e GLTest -K ir \
		out.mod gl.mod glut.mod gltest.mod \
		-lfreeglut -lglu32 -lopengl32

bin/pong.exe: out.mod gl.mod glut.mod pong.mod
	../bin/Debug/duo2c.exe -o bin/pong.exe -e Pong -K ir \
		out.mod gl.mod glut.mod pong.mod \
		-lfreeglut -lglu32 -lopengl32

bin/vector.exe: out.mod vector.mod
	../bin/Debug/duo2c.exe -o bin/vector.exe -e Vector -K ir \
		out.mod vector.mod
		