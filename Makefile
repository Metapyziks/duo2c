CSC = mcs

CSVERSION = 5

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
	$(SRCDIR)/Parsers/*.cs \
	$(SRCDIR)/CodeGen/*.cs \
	$(SRCDIR)/CodeGen/LLVM/*.cs

TARGETDIR = bin/Debug

TARGET = $(TARGETDIR)/DUO2C.exe

debug:
	mkdir -p $(TARGETDIR)
	rm -f $(TARGET)
	$(CSC) -langversion:$(CSVERSION) $(SRC) -d:$(DEF) \
		-t:exe -debug -out:$(TARGET)
	cp oberon2.txt $(TARGETDIR)/

clean:
	rm -r bin/
