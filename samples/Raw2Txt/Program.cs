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

        static void Main(string[] args)
        {
            Console.Title = "Raw2Txt";

            if (args.Length <= 0 || Path.GetExtension(args[0]).ToUpperInvariant() != ".RAW" || !File.Exists(args[0]))
            {
                Console.WriteLine("Usage: {0} raw_file [output_path]", Process.GetCurrentProcess().ProcessName);
                Console.WriteLine("        Default output_path: directory of raw_file");
                return;
            }

            var fileName = args[0];
            var type = Path.GetFileNameWithoutExtension(fileName);

            if (!_supportedTypes.Contains(type.ToUpperInvariant()))
            {
                Console.WriteLine("That type({0}) of raw file is not supported!", type);
                Console.WriteLine("Supported types:");

                foreach (string suppType in _supportedTypes)
                {
                    Console.WriteLine("  - " + suppType);
                }
                return;
            }

            var reader = new RawReader(fileName);
            
            RawData file = null;
            try
            {
                file = reader.Read();
            }
            catch (RawReaderException e)
            {   
                Console.WriteLine(e.Message);
                Console.WriteLine("Reason: {0}", e.InnerException.Message);
            }

            string data = "";

            if (file is UnitData)
            {
                var unitData = (UnitData)file;

                data = UnitDataConverter.GetString(unitData);
            }
            else if (file is SkillData)
            {
                var skillData = (SkillData)file;

                data = SkillDataConverter.GetString(skillData);
            }
            else if (file is AffixData)
            {
                var affixData = (AffixData)file;

                data = AffixDataConverter.GetString(affixData);
            }
            else if (file is MissileData)
            {
                var missileData = (MissileData)file;

                data = MissileDataConverter.GetString(missileData);
            }
            else if (file is RoomPieceData)
            {
                var roomPieceData = (RoomPieceData)file;

                data = RoomPieceDataConverter.GetString(roomPieceData);
            }
            else if (file is TriggerableData)
            {
                var triggerableData = (TriggerableData)file;

                data = TriggerableDataConverter.GetString(triggerableData);
            }
            else if (file is UserInterfaceData)
            {
                var userInterfaceData = (UserInterfaceData)file;

                data = UserInterfaceDataConverter.GetString(userInterfaceData);
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

            outputPath = Path.Combine(outputPath, fileName.Replace(Path.GetExtension(fileName), ".txt"));

            File.WriteAllText(outputPath, data);
            Console.WriteLine("Converted {0} into a human readable format", Path.GetFileName(fileName));
            Console.Write("Press a key to exit...");
            Console.ReadKey();
        }
    }
}
