using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Threading;

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

        static IEnumerator<String> InsertArg(IEnumerator<String> iter, String str)
        {
            yield return str;
            while (iter.MoveNext()) {
                yield return iter.Current;
            }
        }

        static String[] ParseArgs(String[] args)
        {
            var iter = args.Where(x => true).GetEnumerator();
            var loose = new List<String>();

            while (iter.MoveNext()) {
                var arg = iter.Current;
                if (arg.StartsWith("-")) {
                    if (!arg.StartsWith("--") && arg.Length > 2) {
                        var val = arg.Substring(2);
                        arg = arg.Substring(0, 2);
                        iter = InsertArg(iter, val);
                    }
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

        static void RunTool(String name, params Object[] args)
        {
            var strArgs = args.SelectMany(x => x is IEnumerable<String>
                ? (IEnumerable<String>) x : new String[] { x.ToString() })
                .Select(x => x.StartsWith("\"") || x.StartsWith("-") ? x : String.Format("\"{0}\"", x));
            var startInfo = new ProcessStartInfo(name, String.Join(" ", strArgs));
#if DEBUG
            Console.WriteLine("Exec {0} {1}", startInfo.FileName, startInfo.Arguments);
#endif
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;

            using (var process = Process.Start(startInfo)) {
                while (!process.HasExited) {
                    while (!process.StandardOutput.EndOfStream) {
                        Console.WriteLine(process.StandardOutput.ReadLine());
                    }

                    while (!process.StandardError.EndOfStream) {
                        Console.WriteLine(process.StandardError.ReadLine());
                    }

                    Thread.Sleep(10);
                }

                if (process.ExitCode != 0) {
                    throw new Exception("Error while running " + name);
                }
            }
        }

        static IEnumerable<String> ListFileNames(String moduleIdent)
        {
            yield return moduleIdent + ".sym";
            yield return moduleIdent.ToLower() + ".sym";
        }

        static IEnumerable<String> ListPaths(String moduleIdent, String dir)
        {
            foreach (var fileName in ListFileNames(moduleIdent).Distinct()) {
                yield return dir + Path.DirectorySeparatorChar + fileName;
                yield return Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + fileName;
            }
        }

        static String FindSymbolFile(String moduleIdent, String dir)
        {
            return ListPaths(moduleIdent, dir).FirstOrDefault(x => File.Exists(x));
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
                String keepDir = null;
                var libs = new List<String>();
                bool keepIRFiles = false;
                bool link = true;

                AddOption((arg, iter) => {
                    iter.MoveNext();
                    outPath = iter.Current;
                }, "o", "out");

                AddOption((arg, iter) => {
                    iter.MoveNext();
                    entryModule = iter.Current;
                }, "e", "entry");

                AddOption((arg, iter) => {
                    keepIRFiles = true;
                    if (arg == "K" || arg == "keepdir") {
                        iter.MoveNext();
                        keepDir = iter.Current.TrimEnd('/', '\\');
                    }
                }, "k", "K", "keep", "keepdir");

                AddOption((arg, iter) => {
                    link = false;
                    keepIRFiles = true;
                }, "S");

                AddOption((arg, iter) => {
                    iter.MoveNext();
                    libs.Add(iter.Current);
                }, "l");

                var rulesetPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "oberon2.txt");

                var ruleset = Ruleset.FromString(File.ReadAllText(rulesetPath));
                ruleset.AddSubstitutionNS("DUO2C.Nodes.Oberon2", true);

                var cleanupFiles = new List<String>();

                try {
                    Queue<String> files = new Queue<string>(ParseArgs(args));

                    if (keepDir != null && !Directory.Exists(keepDir)) {
                        Directory.CreateDirectory(keepDir);
                    }

                    if (files.Count == 0) {
                        throw new Exception("No files to compile specified");
                    }

                    if (entryModule == null && outPath == null) {
                        throw new Exception("No entry module specified (-e <module>)");
                    } else if (entryModule == null) {
                        entryModule = Path.GetFileNameWithoutExtension(outPath);
                    } else if (outPath == null) {
                        outPath = entryModule + ".exe";
                    }

                    var modules = new Dictionary<String, NModule>();
                    var irFiles = new Dictionary<String, String>();
#if DEBUG
                    var timer = new Stopwatch();
                    timer.Start();
#endif
                    while (files.Count > 0) {
                        var file = files.Dequeue();
                        var module = (NModule) ruleset.ParseFile(file);
                        modules.Add(file, module);
                    }
#if DEBUG
                    timer.Stop();
                    Console.WriteLine("File(s) parsed in {0}ms", timer.ElapsedMilliseconds);

                    if (!modules.Any(x => x.Value.Identifier.ToLower() == entryModule.ToLower())) {
                        Console.WriteLine("Could not find entry module '{0}'", entryModule);
                        return 1;
                    } else {
                        entryModule = modules.First(x => x.Value.Identifier.ToLower() == entryModule.ToLower()).Value.Identifier;
                    }
#endif
                    while (modules.Count > 0) {
                        var pair = modules.FirstOrDefault(x => !x.Value.Imports.Any(y => modules.Values.Any(z => z.Identifier == y)));
                        var mdlpath = pair.Key;
                        var module = pair.Value;

                        if (module == null) {
                            throw new Exception("Cyclic dependency encountered between modules");
                        }

                        modules.Remove(mdlpath);

                        var root = new RootScope();

                        foreach (var import in module.Imports) {
                            var path = FindSymbolFile(import, Path.GetDirectoryName(Path.GetFullPath(modules.FirstOrDefault(x => x.Value.Identifier == import).Key ?? mdlpath)));
                            if (path == null) {
                                throw new Exception(String.Format("Could not find symbol file for module '{0}'", import));
                            }

                            var mdl = (NModule) ruleset.ParseFile(path);
                            mdl.FindDeclarations(root);
                        }

                        module.FindDeclarations(root);

                        var errors = module.FindTypeErrors(root);
                        if (errors.Count() > 0) {
                            var src = File.ReadAllText(mdlpath);
                            WriteErrorHeader("Encountered {0} error{1} while performing type checks:",
                                errors.Count(), errors.Count() != 1 ? "s" : "");
                            foreach (var error in errors) {
                                error.FindLocationInfo(src);
                                error.SetSourcePath(mdlpath);
                                WriteCompilerError(error);
                            }
                            Console.WriteLine();
                            return 1;
                        } else {
                            var guid = Guid.NewGuid();

                            var outpath = Path.GetDirectoryName(Path.GetFullPath(mdlpath))
                                + Path.DirectorySeparatorChar
                                + Path.GetFileNameWithoutExtension(mdlpath)
                                + ".sym";

                            string sym = SymbolCodeGenerator.Generate(module.Type, guid);
                            File.WriteAllText(outpath, sym);

                            if (keepIRFiles) {
                                outpath = (keepDir ?? Path.GetDirectoryName(Path.GetFullPath(mdlpath)))
                                + Path.DirectorySeparatorChar
                                + Path.GetFileNameWithoutExtension(mdlpath)
                                + ".ll";
                            } else {
                                outpath = Path.GetTempFileName();
                                cleanupFiles.Add(outpath);
                            }

                            string ir = IntermediaryCodeGenerator.Generate(module, guid, module.Identifier == entryModule);
                            File.WriteAllText(outpath, ir);

                            if (!keepIRFiles) {
                                cleanupFiles.Remove(outpath);
                                outpath = RenameTempFileExtension(outpath, "ll");
                                cleanupFiles.Add(outpath);
                            }

                            irFiles.Add(module.Identifier, outpath);
                        }
                    }

                    if (!link) return 0;

                    String linkedPath;
                    if (keepIRFiles) {
                        linkedPath = (keepDir ?? Path.GetDirectoryName(Path.GetFullPath(outPath)))
                            + Path.DirectorySeparatorChar
                            + Path.GetFileNameWithoutExtension(outPath)
                            + ".link.ll";
                    } else {
                        linkedPath = Path.GetTempFileName();
                        cleanupFiles.Add(linkedPath);
                    }

                    RunTool("llvm-link", irFiles.Values, "-S", "-o", linkedPath);

                    if (!keepIRFiles) {
                        cleanupFiles.Remove(linkedPath);
                        linkedPath = RenameTempFileExtension(linkedPath, "ll");
                        cleanupFiles.Add(linkedPath);
                    }

                    var assemblyPath = Path.GetTempFileName();
                    cleanupFiles.Add(assemblyPath);
                    RunTool("llc", linkedPath, "-load gc", libs.Select(x => "-load " + x), "-O3", "-o", assemblyPath);
                    cleanupFiles.Remove(linkedPath);
                    assemblyPath = RenameTempFileExtension(assemblyPath, "s");
                    cleanupFiles.Add(assemblyPath);

                    if (!Directory.Exists(Path.GetDirectoryName(Path.GetFullPath(outPath)))) {
                        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outPath)));
                    }

                    RunTool("gcc", assemblyPath, "-lgc", libs.Select(x => "-l" + x), "-o", outPath);

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
#if !DEBUG
                } catch (Exception e) {
                    WriteErrorHeader("Encountered 1 error while compiling file(s):");
                    WriteError(e);
                    Console.WriteLine();
                    return 1;
#endif
                } finally {
                    foreach (var file in cleanupFiles) {
                        File.Delete(file);
                    }
                }
            }
        }
    }
}
