using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DUO2C.Tests
{
    [TestClass]
    public class SyntaxTests
    {
        private Ruleset _ruleset;

        [TestInitialize]
        public void Initialize()
        {
            _ruleset = Ruleset.FromString(File.ReadAllText("../../oberon2.txt"));
        }

        [TestMethod]
        public void SmallFileTest()
        {
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

            Assert.IsTrue(_ruleset.IsMatch(src));
        }
    }
}
