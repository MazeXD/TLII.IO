using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Adm2Txt.Converters;
using TLII.IO;
using TLII.IO.Exceptions;
using TLII.IO.Formats.Adm;

namespace Adm2Txt
{
    class Program
    {
        private static List<string> _supportedTypes = new List<string>()
        {
            "ANIMATION",
            "DAT",
            "HIE",
            "TEMPLATE"
        };

        private static string _outputDir = "";
        private static List<string> _files = new List<string>();
        private static List<string> _unsupportedFiles = new List<string>();
        private static bool _hasErrored = false;

        static void Main(string[] args)
        {
            Console.Title = "Adm2Txt";

            ParseOptions(args);

            if (_files.Count <= 0)
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

            foreach (var file in _files)
            {
                ConvertFile(file);
            }

            if (_unsupportedFiles.Count > 0)
            {
                string types = "";

                for (int i = 0; i < _unsupportedFiles.Count; i++)
                {
                    var type = Path.GetExtension(_unsupportedFiles[i]).Remove(0, 1);

                    types += type;
                    if (i != _unsupportedFiles.Count - 1)
                    {
                        types += ",";
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Those types({0}) of adm files aren't supported!", types);
                Console.WriteLine("Supported types:");

                foreach (string suppType in _supportedTypes)
                {
                    Console.WriteLine("  - " + suppType);
                }

                _hasErrored = true;
            }

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

                    if (!_supportedTypes.Contains(Path.GetExtension(file).ToUpperInvariant().Remove(0, 1)) || !File.Exists(file))
                    {
                        _unsupportedFiles.Add(arg);
                    }
                    else
                    {
                        _files.Add(arg);
                    }
                }
            }
        }

        private static void ConvertFile(string fileName)
        {
            var reader = new AdmReader(fileName);

            AdmData data = null;
            try
            {
                data = reader.Read();
            }
            catch (AdmReaderException e)
            {
                _hasErrored = true;
                Console.WriteLine("File: {0}", fileName);
                Console.WriteLine(e.Message);
                Console.WriteLine("Reason: {0}", e.InnerException.Message);
                return;
            }

            string content = AdmDataConverter.GetString(data);

            string outputPath = _outputDir != "" ? Path.Combine(_outputDir, fileName + ".txt") : fileName + ".txt";

            File.WriteAllText(outputPath, content);
            Console.WriteLine("Converted {0} into a human readable format", Path.GetFileName(fileName));
        }

        private static void Usage()
        {
            Console.WriteLine("Usage: {0} [--out output_path] adm_file", Process.GetCurrentProcess().ProcessName);
            Console.WriteLine("        Default output_path: directory of adm_file");
            Console.WriteLine("Supported types:");

            foreach (string suppType in _supportedTypes)
            {
                Console.WriteLine("  - " + suppType);
            }

            ForceExit();
        }

        private static void ForceExit()
        {
            Console.WriteLine("Press a key to exit...");
            Console.ReadKey();

            Environment.Exit(0);
        }
    }
}
