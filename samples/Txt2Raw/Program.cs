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

        static void Main(string[] args)
        {
            Console.Title = "Txt2Raw";

            if (args.Length <= 0 || Path.GetExtension(args[0]).ToUpperInvariant() != ".TXT" || !File.Exists(args[0]))
            {
                Console.WriteLine("Usage: {0} txt_file [output_path]", Process.GetCurrentProcess().ProcessName);
                Console.WriteLine("        Default output_path: directory of txt_file");
                return;
            }

            var fileName = args[0];
            var type = Path.GetFileNameWithoutExtension(fileName);

            if (!_supportedTypes.Contains(type.ToUpperInvariant()))
            {
                Console.WriteLine("That type({0}) of txt file is not supported!", type);
                Console.WriteLine("Supported types:");

                foreach (string suppType in _supportedTypes)
                {
                    Console.WriteLine("  - " + suppType);
                }
                return;
            }
            
            List<string> lines = File.ReadAllLines(fileName).ToList();
            StripBlankLines(ref lines);

            string Type = lines[0].TrimStart('[').TrimEnd(']');
            string data = MergeLines(ref lines);

            RawData rawFile = null;
            try
            {
                switch (Type)
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
            catch(TxtConverterException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            string outputPath;
            if (args.Length == 2)
            {
                outputPath = args[1];

                if (Path.GetExtension(outputPath) != "")
                {
                    outputPath = Path.GetDirectoryName(outputPath);
                }

                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
            }
            else
            {
                outputPath = Path.GetDirectoryName(fileName);
            }

            RawWriter writer = new RawWriter(rawFile);
            writer.Write(outputPath);

            Console.WriteLine("Converted {0} into binary format", Path.GetFileName(fileName));
            Console.Write("Press a key to exit...");
            Console.ReadKey();
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
    }
}
