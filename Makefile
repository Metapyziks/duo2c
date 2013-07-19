CSC = /usr/local/bin/mcs

CSVERSION = future

DEF = LINUX

SRCDIR = src

SRC = \
	$(SRCDIR)/*.cs \
	$(SRCDIR)/Nodes/*.cs \
	$(SRCDIR)/Nodes/Oberon2/*.cs \
	$(SRCDIR)/Nodes/Oberon2/Declarations/*.cs \
	$(SRCDIR)/Nodes/Oberon2/Expressions/*.cs \
	$(SRCDIR)/Nodes/Oberon2/Types/*.cs \
	$(SRCDIR)/Nodes/Oberon2/Statements/*.cs \
	$(SRCDIR)/Properties/*.cs \
	$(SRCDIR)/Semantics/*.cs \
	$(SRCDIR)/Parsers/*.cs

TARGET = bin/release/DUO2C.exe

release:
	mkdir -p bin/release
	rm -f $(TARGET)
	$(CSC) -langversion:$(CSVERSION) $(SRC) -d:$(DEF) \
		-t:exe -out:$(TARGET) -optimize+
	cp oberon2.txt bin/release/

clean:
	rm -r bin/
