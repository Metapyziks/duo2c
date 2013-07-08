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

            bool first = true;
            ruleset.GetRule("Statement").MatchTested += (sender, e) => {
                if (first) {
                    // Debugger.Break();
                    first = false;
                }
            };

            var src = @"
MODULE Lists;

    TYPE
        List*    = POINTER TO ListNode;
        ListNode = RECORD
            value : Integer;
            next  : List;
        END;

    PROCEDURE (l : List) Add* (v : Integer);
    BEGIN
        IF l = NIL THEN
            NEW(l);
            l.value := v
        ELSE
            l.next.Add(v)
        END
    END Add;

    PROCEDURE (l : List) Get* () : Integer;
    VAR
        v : Integer;
    BEGIN
        IF l = NIL THEN
            RETURN 0
        ELSE
            v := l.value;
            l := l.next;
            RETURN v
        END
    END Get;

END Lists.
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
