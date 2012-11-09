using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TLII.IO.Formats.Raw;
using Txt2Raw.Converters;
using Txt2Raw.Exceptions;
using System.Text;
using TLII.IO;

namespace Txt2Raw
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
            "UI"
        };

        private static string _outputDir = "";
        private static List<string> _files = new List<string>();
        private static List<string> _unsupportedFiles = new List<string>();
        private static bool _hasErrored = false;

        static void Main(string[] args)
        {
            Console.Title = "Txt2Raw";

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
                    var type = Path.GetFileNameWithoutExtension(_unsupportedFiles[i]);

                    types += type;
                    if (i != _unsupportedFiles.Count - 1)
                    {
                        types += ",";
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Those types({0}) of text files aren't supported!", types);
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

                    if (Path.GetExtension(file).ToUpperInvariant() != ".TXT")
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
            List<string> lines = File.ReadAllLines(fileName).ToList();
            StripBlankLines(ref lines);

            string type = lines[0].TrimStart('[').TrimEnd(']');
            string data = MergeLines(ref lines);

            RawData rawFile = null;
            try
            {
                switch (type)
                {
                    case "UNITDATA":
                        rawFile = UnitDataConverter.GetData(data);
                        break;
                    case "SKILLS":
                        rawFile = SkillDataConverter.GetData(data);
                        break;
                    case "AFFIXES":
                        rawFile = AffixDataConverter.GetData(data);
                        break;
                    case "MISSILES":
                        rawFile = MissileDataConverter.GetData(data);
                        break;
                    case "ROOMPIECES":
                        rawFile = RoomPieceDataConverter.GetData(data);
                        break;
                    case "TRIGGERABLES":
                        rawFile = TriggerableDataConverter.GetData(data);
                        break;
                    case "USERINTERFACES":
                        rawFile = UserInterfaceDataConverter.GetData(data);
                        break;
                    default:
                        throw new TxtConverterException("Invalid format of " + type);
                }
            }
            catch (TxtConverterException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            string outputPath = _outputDir != "" ? _outputDir : Path.GetDirectoryName(fileName);

            RawWriter writer = new RawWriter(rawFile);
            writer.Write(outputPath);

            Console.WriteLine("Converted {0} into binary format", Path.GetFileName(fileName));
        }

        private static void StripBlankLines(ref List<string> lines)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                line = line.Replace('\t', ' ');
                line = line.Trim();

                if (!string.IsNullOrEmpty(line))
                {
                    result.Add(line);
                }
            }

            lines = result;
        }

        private static string MergeLines(ref List<string> lines)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < lines.Count; i++)
            {
                if (i != lines.Count - 1)
                {
                    builder.AppendLine(lines[i]);
                }
                else
                {
                    builder.Append(lines[i]);
                }
            }

            return builder.ToString();
        }

        private static void Usage()
        {
            Console.WriteLine("Usage: {0} [--out output_path] txt_file", Process.GetCurrentProcess().ProcessName);
            Console.WriteLine("        Default output_path: directory of txt_file");
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
