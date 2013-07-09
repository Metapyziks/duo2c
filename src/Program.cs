using System;
using System.IO;
using System.Diagnostics;

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
#if LINUX
                var ruleset = Ruleset.FromFile(File.ReadAllText("oberon2.txt"));
#else
                var ruleset = Ruleset.FromString(Properties.Resources.oberon2);
#endif
#if !DEBUG
                try {
#endif
                    var tree = ruleset.ParseFile(args[0]);
                    var outpath = Path.GetDirectoryName(args[0])
                        + Path.DirectorySeparatorChar + "output.txt";
                    File.WriteAllText(outpath, tree.ToString());
                    Console.WriteLine("Parsed successfully");
#if !DEBUG
                } catch (Parsers.ParserException e) {
                    Console.WriteLine(e.Message);
                }
#endif
            }

#if DEBUG
            Console.ReadKey();
#endif
        }
    }
}
