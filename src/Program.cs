using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using DUO2C.CodeGen;
using DUO2C.Nodes;
using DUO2C.Semantics;

namespace DUO2C
{
    class Program
    {
        static void WriteErrorHeader(String format, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = Console.BufferWidth; i > 0; --i) Console.Write("▄");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red;
            String text = String.Format(" " + format, args);
            Console.Write(text);
            for (int i = Console.BufferWidth; i > text.Length; --i) Console.Write(" ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = Console.BufferWidth; i > 0; --i) Console.Write("▀");
            Console.ResetColor();
        }

        static void WriteError(ParserException error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" " + error.Message);

            String snippet = error.LineSnippet.TrimStart();
            int trimmed = error.LineSnippet.Length - snippet.Length;
            snippet = snippet.TrimEnd();
            Console.WriteLine();
            int start = error.Column - 1 - trimmed;
            int end = start + error.SourceLength;
            Console.ResetColor();
            Console.Write("   " + snippet.Substring(0, start));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(snippet.Substring(start, end - start));
            Console.ResetColor();
            Console.WriteLine(snippet.Substring(end, snippet.Length - end));
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = trimmed - 2; i < error.Column; ++i) Console.Write(" ");
            for (int i = 0; i < Math.Max(1, error.SourceLength); ++i) Console.Write("~");
            Console.WriteLine();
            Console.WriteLine();
            Console.ResetColor();
        }

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

                    foreach (var import in module.Imports) {
                        String path = Path.GetDirectoryName(args[0]) + Path.DirectorySeparatorChar
                            + import.ToLower() + ".sym";
                        if (File.Exists(path)) {
                            var mdl = (NModule) ruleset.ParseFile(path);
                            mdl.FindDeclarations(root);
                        }
                    }

                    module.FindDeclarations(root);

                    var errors = module.FindTypeErrors(root);
                    if (errors.Count() > 0) {
                        var src = File.ReadAllText(args[0]);
                        WriteErrorHeader("Encountered {0} error{1} while performing type checks:",
                            errors.Count(), errors.Count() != 1 ? "s" : "");
                        foreach (var error in errors) {
                            error.FindLocationInfo(src);
                            error.SetSourcePath(args[0]);
                            WriteError(error);
                        }
                        Console.WriteLine();
                    } else {
                        var outpath = Path.GetDirectoryName(args[0])
                            + Path.DirectorySeparatorChar
                            + Path.GetFileNameWithoutExtension(args[0])
                            + ".sym";

                        File.WriteAllText(outpath, SymbolFileGenerator.Generate(module.Type));

                        outpath = Path.GetDirectoryName(args[0])
                            + Path.DirectorySeparatorChar
                            + Path.GetFileNameWithoutExtension(args[0])
                            + ".ll";

                        var ctx = new GenerationContext();
                        module.GenerateCode(ctx);
                        File.WriteAllText(outpath, ctx.GeneratedCode);
                    }
                } catch (ParserException e) {
                    WriteErrorHeader("Encountered 1 error while parsing:");
                    WriteError(e);
                    Console.WriteLine();
                }
            }
        }
    }
}
