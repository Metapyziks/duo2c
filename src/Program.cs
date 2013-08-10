using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using DUO2C.CodeGen;
using DUO2C.CodeGen.LLVM;
using DUO2C.Nodes.Oberon2;
using DUO2C.Semantics;

namespace DUO2C
{
    class Program
    {
        static void WriteTextWrapped(String text)
        {
            int margin = Console.CursorLeft;
            var segs = text.Split(' ');
            for (int i = 0; i < segs.Length; ++i) {
                var seg = segs[i];
                if (Console.CursorLeft != margin && Console.CursorLeft + seg.Length + 1 >= Console.WindowWidth) {
                    Console.WriteLine();
                    Console.CursorLeft = margin;
                }
                Console.Write("{0} ", seg);
                if (Console.CursorLeft == 0) {
                    Console.CursorLeft = margin;
                }
            }
            Console.WriteLine();
        }

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

        static void WriteError(CompilerException error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" {0} : ", error.Location);
            WriteTextWrapped(error.MessageNoLocation);

            String snippet = error.LineSnippet.TrimStart();
            int trimmed = error.LineSnippet.Length - snippet.Length;
            snippet = snippet.TrimEnd();
            Console.WriteLine();
            int start = error.Column - 1 - trimmed;
            int end = start + Math.Max(1, error.SourceLength);
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

        static int Main(string[] args)
        {
            if (args.Length == 0) {
                // TODO: Improve useage statement
                Console.WriteLine("Usage: {0} <source-file-path>",
                    Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName));
                return 1;
            } else {
                var ruleset = Ruleset.FromString(File.ReadAllText("oberon2.txt"));
                ruleset.AddSubstitutionNS("DUO2C.Nodes.Oberon2", true);

                try {
#if DEBUG
                    var timer = new Stopwatch();
                    timer.Start();
#endif
                    var module = (NModule) ruleset.ParseFile(args[0]);
#if DEBUG
                    timer.Stop();
                    Console.WriteLine("File parsed in {0}ms", timer.ElapsedMilliseconds);
#endif
                    var root = new RootScope();

                    root.DeclareSymbol("NEW", new ProcedureType(null,
                        new Parameter(true, "ptr", PointerType.Null)
                    ), AccessModifier.Private, DeclarationType.Global);

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
                        return 1;
                    } else {
                        var guid = Guid.NewGuid();
                        
                        var outpath = Path.GetDirectoryName(args[0])
                            + Path.DirectorySeparatorChar
                            + Path.GetFileNameWithoutExtension(args[0])
                            + ".sym";

                        string sym = SymbolCodeGenerator.Generate(module.Type, guid);
                        File.WriteAllText(outpath, sym);

                        outpath = Path.GetDirectoryName(args[0])
                            + Path.DirectorySeparatorChar
                            + Path.GetFileNameWithoutExtension(args[0])
                            + ".ll";

                        string ir = IntermediaryCodeGenerator.Generate(module, guid);
                        File.WriteAllText(outpath, ir);
                        return 0;
                    }
                } catch (CompilerException e) {
                    WriteErrorHeader("Encountered 1 error while parsing:");
                    WriteError(e);
                    Console.WriteLine();
                    return 1;
                }
            }
        }
    }
}
