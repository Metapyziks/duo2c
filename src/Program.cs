using System;
using System.IO;

namespace DUO2C
{
    class Program
    {
        static void Main(string[] args)
        {
            var ruleset = Ruleset.FromString(Properties.Resources.oberon2);

            var src = @"
MODULE Lists;

    (*** declare global constants, types and variables ***)

    TYPE
        List*    = POINTER TO ListNode;
        ListNode = RECORD
            value : Integer;
            next  : List;
        END;

    (*** declare procedures ***)

    PROCEDURE (l : List) Add* (v : Integer);
    BEGIN
        IF l = NIL THEN
            NEW(l);             (* create record instance *)
            l.value := v
        ELSE
            l.next.Add(v)      (* recursive call to .add(n) *)
        END
    END Add;

    PROCEDURE (l : List) Get* () : Integer;
    VAR
        v : Integer;
    BEGIN
        IF l = NIL THEN
            RETURN 0           (* .get() must always return an INTEGER *)
        ELSE
            v := l.value;       (* this line will crash if l is NIL *)
            l := l.next;
            RETURN v
        END
    END Get;

END Lists.
            ";

            var tree = ruleset.Parse(src);

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
