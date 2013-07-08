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
VAR
    a : Real;
BEGIN
    a := 23.4 + 9 - 6 * 2.9 + (4 + 8) - 2
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
