using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DUO2C.CodeGen;
using DUO2C.CodeGen.LLVM;
using DUO2C.Nodes.Oberon2;
using DUO2C.Semantics;

namespace DUO2C
{
    class InvalidArgumentException : Exception
    {
        public String Arg { get; private set; }

        public InvalidArgumentException(String arg)
            : base(String.Format("Unrecognised argument '{0}'", arg))
        {
            Arg = arg;
        }
    }

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

        static void WriteCompilerError(CompilerException error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" {0} : ", error.Location);
            WriteTextWrapped(error.MessageNoLocation);

            String snippet = error.LineSnippet.TrimStart();
            int trimmed = error.LineSnippet.Length - snippet.Length;
            snippet = snippet.TrimEnd();
            Console.WriteLine();
            int start = error.Column - 1 - trimmed;
            int end = start + Math.Min(snippet.Length - start, Math.Max(1, error.SourceLength));
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

        static void WriteError(Exception error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" ");
            WriteTextWrapped(error.Message);
        }

        delegate void CmdLineOptionAction(String ident, IEnumerator<String> arg);

        static Dictionary<String, CmdLineOptionAction> _options = new Dictionary<string,CmdLineOptionAction>();
        static void AddOption(CmdLineOptionAction action, params String[] idents)
        {
            foreach (var ident in idents) {
                _options.Add("-" + (ident.Length == 1 || ident.StartsWith("-") ? ident : "-" + ident), action);
            }
        }

        static String[] ParseArgs(String[] args)
        {
            var iter = args.Where(x => true).GetEnumerator();
            var loose = new List<String>();

            while (iter.MoveNext()) {
                var arg = iter.Current;
                if (arg.StartsWith("-")) {
                    if (_options.ContainsKey(arg)) {
                        _options[arg](arg.TrimStart('-'), iter);
                    } else {
                        throw new InvalidArgumentException(arg);
                    }
                } else {
                    loose.Add(arg);
                }
            }

            return loose.ToArray();
        }

        static String RenameTempFileExtension(String path, String newExtension)
        {
            int index = path.LastIndexOf('.');
            var newPath = index == -1 || index < Math.Max(path.LastIndexOf('/'), path.LastIndexOf('\\'))
                ? path + "." + newExtension : path.Substring(0, index) + "." + newExtension;
            File.Copy(path, newPath); File.Delete(path);
            return newPath;
        }

        static int Main(string[] args)
        {
            if (args.Length == 0) {
                // TODO: Improve useage statement
                Console.WriteLine("Usage: {0} <source-file-path>",
                    Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName));
                return 1;
            } else {
                String outPath = null;
                String entryModule = null;

                AddOption((arg, iter) => {
                    iter.MoveNext();
                    outPath = iter.Current;
                }, "o", "out");

                AddOption((arg, iter) => {
                    iter.MoveNext();
                    entryModule = iter.Current;
                }, "e", "entry");

                var ruleset = Ruleset.FromString(File.ReadAllText("oberon2.txt"));
                ruleset.AddSubstitutionNS("DUO2C.Nodes.Oberon2", true);

                try {
                    String[] files = ParseArgs(args);

                    if (entryModule == null && outPath == null) {
                        throw new Exception("No entry module specified (-e <module>)");
                    } else if (entryModule == null) {
                        entryModule = Path.GetFileNameWithoutExtension(outPath);
                    } else if (outPath == null) {
                        outPath = entryModule + ".exe";
                    }

                    if (files.Length == 0) {
                        throw new Exception("No files to compile specified");
                    }

                    var modules = new Dictionary<String, NModule>();
                    var irFiles = new Dictionary<String, String>();
#if DEBUG
                    var timer = new Stopwatch();
                    timer.Start();
#endif
                    foreach (var file in files) {
                        var module = (NModule) ruleset.ParseFile(file);
                        modules.Add(module.Identifier, module);
                    }
#if DEBUG
                    timer.Stop();
                    Console.WriteLine("File(s) parsed in {0}ms", timer.ElapsedMilliseconds);

                    if (!modules.Any(x => x.Key == entryModule)) {
                        Console.WriteLine("Could not find entry module '{0}'", entryModule);
                        return 1;
                    }
#endif
                    while (modules.Count > 0) {
                        var module = modules.FirstOrDefault(x => !x.Value.Imports.Any(y => modules.ContainsKey(y))).Value;

                        if (module == null) {
                            throw new Exception("Cyclic dependency encountered between modules");
                        }

                        modules.Remove(module.Identifier);

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
                            var src = File.ReadAllText(files[0]);
                            WriteErrorHeader("Encountered {0} error{1} while performing type checks:",
                                errors.Count(), errors.Count() != 1 ? "s" : "");
                            foreach (var error in errors) {
                                error.FindLocationInfo(src);
                                error.SetSourcePath(files[0]);
                                WriteCompilerError(error);
                            }
                            Console.WriteLine();
                            return 1;
                        } else {
                            var guid = Guid.NewGuid();

                            var outpath = Path.GetDirectoryName(files[0])
                                + Path.DirectorySeparatorChar
                                + Path.GetFileNameWithoutExtension(files[0])
                                + ".sym";

                            string sym = SymbolCodeGenerator.Generate(module.Type, guid);
                            File.WriteAllText(outpath, sym);

                            outpath = Path.GetTempFileName();
                            string ir = IntermediaryCodeGenerator.Generate(module, guid, module.Identifier == entryModule);
                            File.WriteAllText(outpath, ir);

                            outpath = RenameTempFileExtension(outpath, "ll");
                            irFiles.Add(module.Identifier, outpath);
                        }
                    }

                    var linkedPath = Path.GetTempFileName();
                    using (var process = Process.Start("llvm-link", String.Format("\"{0}\" -S -o {1}",
                        String.Join("\" \"", irFiles.Values), linkedPath))) {

                        process.WaitForExit();

                        if (process.ExitCode > 0) {
                            Console.WriteLine("Error while calling llvm-link");
                            return 1;
                        }
                    }
                    linkedPath = RenameTempFileExtension(linkedPath, "ll");

                    var assemblyPath = Path.GetTempFileName();
                    using (var process = Process.Start("llc", String.Format("\"{0}\" -load gc -O3 -o {1}",
                        linkedPath, assemblyPath))) {

                        process.WaitForExit();

                        if (process.ExitCode > 0) {
                            Console.WriteLine("Error while calling llc");
                            return 1;
                        }
                    }
                    assemblyPath = RenameTempFileExtension(assemblyPath, "s");

                    using (var process = Process.Start("gcc", String.Format("\"{0}\" -lgc -o {1}",
                        assemblyPath, outPath))) {

                        process.WaitForExit();

                        if (process.ExitCode > 0) {
                            Console.WriteLine("Error while calling gcc");
                            return 1;
                        }
                    }

                    return 0;
                } catch (InvalidArgumentException e) {
                    WriteErrorHeader("Encountered 1 error while parsing arguments:");
                    WriteError(e);
                    Console.WriteLine();
                    return 1;
                } catch (CompilerException e) {
                    WriteErrorHeader("Encountered 1 error while parsing:");
                    WriteCompilerError(e);
                    Console.WriteLine();
                    return 1;
                /*} catch (Exception e) {
                    WriteErrorHeader("Encountered 1 error while compiling file(s):");
                    WriteError(e);
                    Console.WriteLine();
                    return 1;*/
                }
            }
        }
    }
}
