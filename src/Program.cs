using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using DUO2C.Nodes;
using DUO2C.Semantics;

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
                    var timer = new Stopwatch();
                    timer.Start();
                    var module = (NModule) ruleset.ParseFile(args[0]);
                    timer.Stop();
                    Console.WriteLine("File parsed in {0}ms", timer.ElapsedMilliseconds);

                    var root = new RootScope();
                    // Includes would be here

                    module.FindDeclarations(root);

                    var errors = module.FindTypeErrors(root);
                    if (errors.Count() > 0) {
                        var src = File.ReadAllText(args[0]);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Encountered {0} error{1} while compiling:",
                            errors.Count(), errors.Count() != 1 ? "s" : "");
                        foreach (var error in errors) {
                            error.FindLocationInfo(src);
                            error.SetSourcePath(args[0]);
                            Console.WriteLine("- {0}", error.Message);
                        }
                        Console.ResetColor();
                    } else {
                        var outpath = Path.GetDirectoryName(args[0])
                            + Path.DirectorySeparatorChar
                            + Path.GetFileNameWithoutExtension(args[0])
                            + ".syntax";
                        File.WriteAllText(outpath, module.ToString());
                    }
                } catch (ParserException e) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }
            }
        }
    }
}
