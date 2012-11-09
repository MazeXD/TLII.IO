using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Raw2Txt.Converters;
using TLII.IO;
using TLII.IO.Exceptions;
using TLII.IO.Formats.Raw;

namespace Raw2Txt
{
    class Program
    {
        private static List<string> _supportedTypes = new List<string>()
        {
            "UNITDATA",
            "SKILLS",
            "AFFIXES",
            "MISSILES",
            "ROOMPIECES",
            "TRIGGERABLES",
            "UI",
        };

        private static string _outputDir = "";
        private static List<string> _files = new List<string>();
        private static List<string> _unsupportedFiles = new List<string>();
        private static bool _hasErrored = false;

        static void Main(string[] args)
        {
            Console.Title = "Raw2Txt";


            ParseOptions(args);

            if (_files.Count <= 0)
            {
                Usage();
            }

            if(Path.GetExtension(_outputDir) != "")
            {
                _outputDir = Path.GetDirectoryName(_outputDir);
            }

            if(_outputDir != "" && !Directory.Exists(_outputDir))
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
                    var type = Path.GetFileNameWithoutExtension(_unsupportedFiles[i]);

                    types += type;
                    if (i != _unsupportedFiles.Count - 1)
                    {
                        types += ",";
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Those types({0}) of raw files aren't supported!", types);
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

                    if (Path.GetExtension(file).ToUpperInvariant() != ".RAW")
                    {
                        continue;
                    }
                    else if (!_supportedTypes.Contains(name.ToUpperInvariant()) || !File.Exists(file))
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
            var reader = new RawReader(fileName);

            RawData file = null;
            try
            {
                file = reader.Read();
            }
            catch (RawReaderException e)
            {
                _hasErrored = true;
                Console.WriteLine("File: {0}", fileName);
                Console.WriteLine(e.Message);
                Console.WriteLine("Reason: {0}", e.InnerException.Message);
                return;
            }

            string data = "";

            switch (file.Type)
            {
                case RawType.UnitData:
                    data = UnitDataConverter.GetString((UnitData)file);
                    break;
                case RawType.SkillData:
                    data = SkillDataConverter.GetString((SkillData)file);
                    break;
                case RawType.AffixData:
                    data = AffixDataConverter.GetString((AffixData)file);
                    break;
                case RawType.MissileData:
                    data = MissileDataConverter.GetString((MissileData)file);
                    break;
                case RawType.RoomPieceData:
                    data = RoomPieceDataConverter.GetString((RoomPieceData)file);
                    break;
                case RawType.TriggerableData:
                    data = TriggerableDataConverter.GetString((TriggerableData)file);
                    break;
                case RawType.UserInterfaceData:
                    data = UserInterfaceDataConverter.GetString((UserInterfaceData)file);
                    break;
            }

            string outputPath = _outputDir != "" ? Path.Combine(_outputDir, Path.GetFileNameWithoutExtension(fileName) + ".txt") : fileName.Replace(Path.GetExtension(fileName), ".txt");

            File.WriteAllText(outputPath, data);
            Console.WriteLine("Converted {0} into a human readable format", Path.GetFileName(fileName));
        }

        private static void Usage()
        {
            Console.WriteLine("Usage: {0} [--out output_path] raw_file", Process.GetCurrentProcess().ProcessName);
            Console.WriteLine("        Default output_path: directory of raw_file");
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
