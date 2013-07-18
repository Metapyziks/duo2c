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
        bird.sound := "Quack!"
    END SetDuckSound;

    PROCEDURE SetCuckooSound* (bird : Cuckoo);
    BEGIN
        bird.sound := "Cuckoo!"
    END SetCuckooSound;

    PROCEDURE SetSound* (bird : Bird);
    BEGIN
        WITH bird : Cuckoo DO
             SetCuckooSound(bird)
           | bird : Duck DO
             SetDuckSound(bird)
        ELSE
             bird.sound := "Tweet!"
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

(* -------------------------------------- *)
(* Pass dynamic type to procedure         *)

    pb := pd;

    SetDuckSound(pb(Duck));
    Out.Ln; Out.String(pb^.sound); Out.Ln;

    pb := pc;

    SetCuckooSound(pb(Cuckoo));
    Out.Ln; Out.String(pb^.sound); Out.Ln;

(* -------------------------------------- *)

    SetSound(pb);
    Out.Ln; Out.String(pb^.sound); Out.Ln;

    pb := pd;

    SetSound(pb);
    Out.Ln; Out.String(pb^.sound); Out.Ln;

(* -------------------------------------- *)

    NEW(pb);

    SetSound(pb);
    Out.Ln; Out.String(pb^.sound); Out.Ln
END PointerBirds.
