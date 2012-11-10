using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TLII.IO.Formats.Raw;
using TLII.IO.Formats.Raw.Data;
using Txt2Raw.Exceptions;

namespace Txt2Raw.Converters
{
    static class UnitDataConverter
    {
        public static UnitData GetData(string data)
        {
            UnitData unitData = new UnitData();

            StringReader reader = new StringReader(data);

            string line = "";
            List<Unit> unitList = null;
            int lineNumber = 0;

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (line == "[/UNITDATA]")
                {
                    break;
                }
                else if (line == "[ITEMS]")
                {
                    unitList = unitData.Items;
                }
                else if (line == "[MONSTERS]")
                {
                    unitList = unitData.Monsters;
                }
                else if (line == "[PLAYERS]")
                {
                    unitList = unitData.Players;
                }
                else if (line == "[PROPS]")
                {
                    unitList = unitData.Props;
                }
                else if (line == "[UNIT]")
                {
                    if (unitList == null)
                    {
                        throw new TxtConverterException(String.Format("Units need to be in a group! (Line: {0})", lineNumber));
                    }

                    unitList.Add(ReadUnit(ref reader, ref lineNumber));
                }
            }

            reader.Close();
            reader.Dispose();

            return unitData;
        }

        private static Unit ReadUnit(ref StringReader reader, ref int lineNumber)
        {
            Unit unit = new Unit();

            string line;
            int startLine = lineNumber;
            bool[] hasSet = new bool[10];

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (line == "[/UNIT]")
                {
                    break;
                }
                else if (line.StartsWith("<INTEGER64>GUID:"))
                {
                    line = line.Replace("<INTEGER64>GUID:", "").Trim();

                    long guid;
                    if (long.TryParse(line, out guid))
                    {
                        unit.GUID = guid;
                        hasSet[0] = true;
                    }
                    else
                    {
                        throw new TxtConverterException(String.Format("GUID must be a 64bit Integer. (Line: {0} | Current: {1})", lineNumber, line));
                    }
                }
                else if (line.StartsWith("<STRING>Name:"))
                {
                    line = line.Replace("<STRING>Name:", "").Trim();

                    unit.Name = line;
                    hasSet[1] = true;
                }
                else if (line.StartsWith("<STRING>File:"))
                {
                    line = line.Replace("<STRING>File:", "").Trim();

                    unit.File = line;
                    hasSet[2] = true;
                }
                else if (line.StartsWith("<STRING>UnitType:"))
                {
                    line = line.Replace("<STRING>UnitType:", "").Trim();

                    unit.Type = line;
                    hasSet[3] = true;
                }
                else if (line.StartsWith("<BINARY>Unknown:"))
                {
                    line = line.Replace("<BINARY>Unknown:", "").Trim();

                    if(line.Length != 2)
                    {
                        throw new TxtConverterException(String.Format("Unknown must be a single hex byte. (Line: {0} | Current: {1})", lineNumber, line));
                    }

                    byte unknown;
                    if(byte.TryParse(line, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out unknown))
                    {
                        unit.Unknown = unknown;
                        hasSet[4] = true;
                    }else{
                        throw new TxtConverterException(String.Format("Unknown is not a valid hex byte! (Line: {0} | Current: {1})", lineNumber, line));
                    }
                }
                else if (line.StartsWith("<INTEGER>Level:"))
                {
                    line = line.Replace("<INTEGER>Level:", "").Trim();

                    int level;
                    if (int.TryParse(line, out level))
                    {
                        unit.Level = level;
                        hasSet[5] = true;
                    }
                    else
                    {
                        throw new TxtConverterException(String.Format("Level must be a 32bit Integer. (Line: {0} | Current: {1})", lineNumber, line));
                    }
                }
                else if (line.StartsWith("<INTEGER>MinLevel:"))
                {
                    
                    line = line.Replace("<INTEGER>MinLevel:", "").Trim();

                    int level;
                    if (int.TryParse(line, out level))
                    {
                        unit.MinLevel = level;
                        hasSet[6] = true;
                    }
                    else
                    {
                        throw new TxtConverterException(String.Format("MinLevel must be a 32bit Integer. (Line: {0} | Current: {1})", lineNumber, line));
                    }
                }
                else if (line.StartsWith("<INTEGER>MaxLevel:"))
                {
                    
                    line = line.Replace("<INTEGER>MaxLevel:", "").Trim();

                    int level;
                    if (int.TryParse(line, out level))
                    {
                        unit.MaxLevel = level;
                        hasSet[7] = true;
                    }
                    else
                    {
                        throw new TxtConverterException(String.Format("MaxLevel must be a 32bit Integer. (Line: {0} | Current: {1})", lineNumber, line));
                    }
                }
                else if (line.StartsWith("<INTEGER>Rarity:"))
                {
                    
                    line = line.Replace("<INTEGER>Rarity:", "").Trim();

                    int rarity;
                    if (int.TryParse(line, out rarity))
                    {
                        unit.Rarity = rarity;
                        hasSet[8] = true;
                    }
                    else
                    {
                        throw new TxtConverterException(String.Format("Rarity must be a 32bit Integer. (Line: {0} | Current: {1})", lineNumber, line));
                    }
                }
                else if (line.StartsWith("<INTEGER>RarityHC:"))
                {
                    
                    line = line.Replace("<INTEGER>RarityHC:", "").Trim();

                    int rarity;
                    if (int.TryParse(line, out rarity))
                    {
                        unit.RarityHC = rarity;
                        hasSet[9] = true;
                    }
                    else
                    {
                        throw new TxtConverterException(String.Format("RarityHC must be a 32bit Integer. (Line: {0} | Current: {1})", lineNumber, line));
                    }
                }
				else if(line.StartsWith("//") || line.StartsWith("#"))
				{
					continue;
				}
                else
                {
                    throw new TxtConverterException(String.Format("Unknown unit property. (Line: {0})", lineNumber));
                }
            }

            Validate(hasSet, startLine);

            return unit;
        }

        private static void Validate(bool[] hasSet, int lineNumber)
        {
            if (!hasSet[0])
            {
                throw new TxtConverterException(String.Format("GUID must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[1])
            {
                throw new TxtConverterException(String.Format("Name must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[2])
            {
                throw new TxtConverterException(String.Format("File must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[3])
            {
                throw new TxtConverterException(String.Format("UnitType must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[4])
            {
                throw new TxtConverterException(String.Format("Unknown must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[5])
            {
                throw new TxtConverterException(String.Format("Level must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[6])
            {
                throw new TxtConverterException(String.Format("MinLevel must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[7])
            {
                throw new TxtConverterException(String.Format("MaxLevel must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[8])
            {
                throw new TxtConverterException(String.Format("Rarity must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[9])
            {
                throw new TxtConverterException(String.Format("RarityHC must be specified. (Line: {0})", lineNumber));
            }
        }
    }
}
