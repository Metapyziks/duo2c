﻿using System;
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
                var ruleset = Ruleset.FromString(File.ReadAllText("oberon2.txt"));
                try {
                    var tree = ruleset.ParseFile(args[0]);
                    var outpath = Path.GetDirectoryName(args[0])
                        + Path.DirectorySeparatorChar + "output.txt";
                    File.WriteAllText(outpath, tree.ToString());
                } catch (Parsers.ParserException e) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }
            }
        }
    }
}
