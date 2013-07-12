using System;
using System.Diagnostics;
using System.IO;

namespace DUO2C
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) {
                // TODO: Improve useage statement
                Console.WriteLine("Usage: {0} <source-file-path>",
                    Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName));
            } else {
                var ruleset = Ruleset.FromString(File.ReadAllText("oberon2.txt"));
                ruleset.AddSubstitutionNS("DUO2C.Nodes.Oberon2", true);

                try {
                    var tree = ruleset.ParseFile(args[0]);
                    var outpath = Path.GetDirectoryName(args[0])
                        + Path.DirectorySeparatorChar
                        + Path.GetFileNameWithoutExtension(args[0])
                        + ".syntax";
                    File.WriteAllText(outpath, tree.ToString());
                } catch (ParserException e) {
                    // TODO: Clean up
                    if (e.Line == -1) {
                        e.FindLocationInfo(File.ReadAllText(args[0]));
                        e.SetSourcePath(args[0]);
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }
            }
        }
    }
}
