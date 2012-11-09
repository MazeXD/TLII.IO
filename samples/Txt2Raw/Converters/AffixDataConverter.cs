using System;
using System.IO;
using TLII.IO.Formats.Raw;
using TLII.IO.Formats.Raw.Data;
using Txt2Raw.Exceptions;

namespace Txt2Raw.Converters
{
    static class AffixDataConverter
    {
        public static AffixData GetData(string data)
        {
            AffixData affixData = new AffixData();

            StringReader reader = new StringReader(data);

            string line = "";
            int lineNumber = 0;

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (line == "[/AFFIXES]")
                {
                    break;
                }
                else if (line == "[AFFIX]")
                {
                    affixData.Affixes.Add(ReadAffix(ref reader, ref lineNumber));
                }
            }

            return affixData;
        }

        private static Affix ReadAffix(ref StringReader reader, ref int lineNumber)
        {
            Affix affix = new Affix();

            string line;
            int startLine = lineNumber;
            bool[] hasSet = new bool[6];

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (line == "[/AFFIX]")
                {
                    break;
                }
                else if (line.StartsWith("<STRING>Name:"))
                {
                    line = line.Replace("<STRING>Name:", "").Trim();

                    affix.Name = line;
                    hasSet[0] = true;
                }
                else if (line.StartsWith("<STRING>File:"))
                {
                    line = line.Replace("<STRING>File:", "").Trim();

                    affix.File = line;
                    hasSet[1] = true;
                }
                else if (line.StartsWith("<INTEGER>MinSpawnRange:"))
                {
                    line = line.Replace("<INTEGER>MinSpawnRange:", "").Trim();

                    int spawnRange;
                    if (int.TryParse(line, out spawnRange))
                    {
                        affix.MinSpawnRange = spawnRange;
                        hasSet[2] = true;
                    }
                    else
                    {
                        throw new TxtConverterException(String.Format("MinSpawnRange must be a 32bit Integer. (Line: {0} | Current: {1})", lineNumber, line));
                    }
                }
                else if (line.StartsWith("<INTEGER>MaxSpawnRange:"))
                {
                    line = line.Replace("<INTEGER>MaxSpawnRange:", "").Trim();

                    int spawnRange;
                    if (int.TryParse(line, out spawnRange))
                    {
                        affix.MaxSpawnRange = spawnRange;
                        hasSet[3] = true;
                    }
                    else
                    {
                        throw new TxtConverterException(String.Format("MaxSpawnRange must be a 32bit Integer. (Line: {0} | Current: {1})", lineNumber, line));
                    }
                }
                else if (line.StartsWith("<INTEGER>Weight:"))
                {
                    line = line.Replace("<INTEGER>Weight:", "").Trim();

                    int weight;
                    if (int.TryParse(line, out weight))
                    {
                        affix.Weight = weight;
                        hasSet[4] = true;
                    }
                    else
                    {
                        throw new TxtConverterException(String.Format("Weight must be a 32bit Integer. (Line: {0} | Current: {1})", lineNumber, line));
                    }
                }
                else if (line.StartsWith("<INTEGER>DifficultiesAllowed:"))
                {
                    line = line.Replace("<INTEGER>DifficultiesAllowed:", "").Trim();

                    int difficultiesAllowed;
                    if (int.TryParse(line, out difficultiesAllowed))
                    {
                        affix.DifficultiesAllowed = difficultiesAllowed;
                        hasSet[5] = true;
                    }
                    else
                    {
                        throw new TxtConverterException(String.Format("DifficultiesAllowed must be a 32bit Integer. (Line: {0} | Current: {1})", lineNumber, line));
                    }
                }
                else if (line.StartsWith("[UNITTYPES]"))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        lineNumber++;

                        if (line == "[/UNITTYPES]")
                        {
                            break;
                        }
                        else
                        {
                            affix.UnitTypes.Add(line.Replace("<STRING>", "").Trim());
                        }
                    }
                }
                else if (line.StartsWith("[NOTUNITTYPES]"))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        lineNumber++;

                        if (line == "[/NOTUNITTYPES]")
                        {
                            break;
                        }
                        else
                        {
                            affix.NotUnitTypes.Add(line.Replace("<STRING>", "").Trim());
                        }
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

            return affix;
        }

        private static void Validate(bool[] hasSet, int lineNumber)
        {
            if (!hasSet[0])
            {
                throw new TxtConverterException(String.Format("Name must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[1])
            {
                throw new TxtConverterException(String.Format("File must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[2])
            {
                throw new TxtConverterException(String.Format("MinSpawnRange must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[3])
            {
                throw new TxtConverterException(String.Format("MaxSpawnRange must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[4])
            {
                throw new TxtConverterException(String.Format("Weight must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[5])
            {
                throw new TxtConverterException(String.Format("DifficultiesAllowed must be specified. (Line: {0})", lineNumber));
            }
        }
    }
}
