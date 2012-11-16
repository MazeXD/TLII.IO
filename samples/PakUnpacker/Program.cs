using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using TLII.IO;
using TLII.IO.Exceptions;
using TLII.IO.Formats.Pak;

namespace PakUnpacker
{
    class Program
    {
        private static string _pakFile = "";
        private static string _outputDir = "";
        private static bool _hasErrored = false;

        static void Main(string[] args)
        {
            Console.Title = "Pak Extractor";

            ParseOptions(args);

            if (_pakFile == "")
            {
                Usage();
            }

            if (Path.GetExtension(_outputDir) != "")
            {
                _outputDir = Path.GetDirectoryName(_outputDir);
            }

            if (_outputDir != "" && !Directory.Exists(_outputDir))
            {
                Directory.CreateDirectory(_outputDir);
            }

            ExtractPackage();

            if (_hasErrored)
            {
                ForceExit();
            }
        }

        private static void ParseOptions(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i].Trim();

                if (arg.StartsWith("--"))
                {
                    arg = arg.Remove(0, 2);

                    if (arg.ToUpperInvariant() == "OUT")
                    {
                        i++;

                        if (i < args.Length)
                        {
                            _outputDir = args[i];
                        }
                    }
                }
                else
                {
                    string file = arg;
                    string name = Path.GetFileNameWithoutExtension(file);

                    if (Path.GetExtension(file).ToUpperInvariant() != ".PAK")
                    {
                        continue;
                    }
                    else
                    {
                        _pakFile = arg;
                    }
                }
            }
        }

        private static void ExtractPackage()
        {
            PakReader reader = null;

            try
            {
                reader = new PakReader(_pakFile);
            }
            catch (FileNotFoundException e)
            {
                _hasErrored = true;
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine("File: '{0}'", e.FileName);
            }

            PakFile file = null;
            try
            {
                file = reader.Read();
            }
            catch (PakReaderException e)
            {
                _hasErrored = true;
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine("Reason: {0}", e.InnerException.Message);
                return;
            }

            if (_outputDir == "")
            {
                _outputDir = Path.GetDirectoryName(_pakFile);
            }

            for (int i = 0; i < file.Entries.Count; i++)
            {
                var entry = file.Entries[i];

                entry.Extract(_outputDir);
                Console.WriteLine("[{0}/{1}] Extracted {2}", i+1, file.Entries.Count, entry.ArchivePath);

                Thread.Sleep(10);
            }

            Console.WriteLine("Extracted all files successfully!");            
        }

        private static void Usage()
        {
            Console.WriteLine("Usage: {0} [--out output_path] pak_file", Process.GetCurrentProcess().ProcessName);
            Console.WriteLine("        Default output_path: directory of raw_file");

            ForceExit();
        }

        private static void ForceExit()
        {
            Console.WriteLine("Press a key to exit...");

            try
            {
                Console.ReadKey();
            }
            catch (InvalidOperationException) { }

            Environment.Exit(0);
        }
    }
}
