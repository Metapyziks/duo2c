using System;
using System.Diagnostics;
using System.IO;

namespace DUO2C
{
    class Program
    {
        static void Main(string[] args)
        {
            var ruleset = Ruleset.Parse(File.ReadAllText("oberon2.bnf"));

            var src = @"
MODULE Birds;
    TYPE
        Bird* = RECORD
            sound* : ARRAY 10 OF Char;
        END;
END Birds.

MODULE Ducks;
    IMPORT Birds;

    TYPE
        Duck* = RECORD (Birds.Bird) END;

    PROCEDURE SetSound* (VAR bird : Duck);
    BEGIN
        bird.sound := ""Quack!""
    END SetSound;
END Ducks.

MODULE Cuckoos;
    IMPORT Birds;

    TYPE
        Cuckoo* = RECORD (Birds.Bird) END;

    PROCEDURE SetSound* (VAR bird : Cuckoo);
    BEGIN
        bird.sound := ""Cuckoo!""
    END SetSound;
END Cuckoos.

MODULE Test;
    IMPORT Out, Birds, Cuckoos, Ducks;

    TYPE
        SomeBird* = RECORD (Birds.Bird) END;

    VAR
        sb : SomeBird;
        c  : Cuckoos.Cuckoo;
        d  : Ducks.Duck;

    PROCEDURE SetSound* (VAR bird : Birds.Bird);
    BEGIN
        WITH bird : Cuckoos.Cuckoo DO
             bird.sound := ""Cuckoo!""
           | bird : Ducks.Duck DO
             bird.sound := ""Quack!""
        ELSE
             bird.sound := ""Tweet!""
        END
    END SetSound;

    PROCEDURE MakeSound* (VAR b : Birds.Bird);
    BEGIN
        Out.Ln;
        Out.String(b.sound);
        Out.Ln
    END MakeSound;

BEGIN
    SetSound(c);
    SetSound(d);
    SetSound(sb);

    MakeSound(c);
    MakeSound(d);
    MakeSound(sb)
END Test.

MODULE PointerBirds;
    IMPORT Out;

    TYPE
        BirdRec*   = RECORD
            sound* : ARRAY 10 OF Char;
        END;
        DuckRec*   = RECORD (BirdRec) END;
        CuckooRec* = RECORD (BirdRec) END;

        Bird   = POINTER TO BirdRec;
        Cuckoo = POINTER TO CuckooRec;
        Duck   = POINTER TO DuckRec;

   VAR
       pb : Bird;
       pc : Cuckoo;
       pd : Duck;

    PROCEDURE SetDuckSound* (bird : Duck);
    BEGIN
        bird.sound := ""Quack!""
    END SetDuckSound;

    PROCEDURE SetCuckooSound* (bird : Cuckoo);
    BEGIN
        bird.sound := ""Cuckoo!""
    END SetCuckooSound;

    PROCEDURE SetSound* (bird : Bird);
    BEGIN
        WITH bird : Cuckoo DO
             SetCuckooSound(bird)
           | bird : Duck DO
             SetDuckSound(bird)
        ELSE
             bird.sound := ""Tweet!""
        END
    END SetSound;

BEGIN
    NEW(pc);
    NEW(pd);

    SetCuckooSound(pc);
    SetDuckSound(pd);

    Out.Ln; Out.String(pc^.sound); Out.Ln;
    Out.Ln; Out.String(pd^.sound); Out.Ln;

    SetSound(pc);
    SetSound(pd);

    Out.Ln; Out.String(pc^.sound); Out.Ln;
    Out.Ln; Out.String(pd^.sound); Out.Ln;

    pb := pd;

    SetDuckSound(pb(Duck));
    Out.Ln; Out.String(pb^.sound); Out.Ln;

    pb := pc;

    SetCuckooSound(pb(Cuckoo));
    Out.Ln; Out.String(pb^.sound); Out.Ln;

    SetSound(pb);
    Out.Ln; Out.String(pb^.sound); Out.Ln;

    pb := pd;

    SetSound(pb);
    Out.Ln; Out.String(pb^.sound); Out.Ln;

    NEW(pb);

    SetSound(pb);
    Out.Ln; Out.String(pb^.sound); Out.Ln
END PointerBirds.
            ";

            var tree = Parser.Parse(src, ruleset);

            if (tree == null) {
                Console.WriteLine("Could not parse!");
            } else {
                File.WriteAllText("output.txt", tree.ToString());
                Console.WriteLine("Parsed successfully");
            }

            Console.ReadKey();
        }
    }
}
