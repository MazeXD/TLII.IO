using System;
using System.IO;
using TLII.IO.Formats.Raw;
using TLII.IO.Formats.Raw.Data;
using Txt2Raw.Exceptions;

namespace Txt2Raw.Converters
{
    static class SkillDataConverter
    {
        public static SkillData GetData(string data)
        {
            SkillData skillData = new SkillData();

            StringReader reader = new StringReader(data);

            string line = "";
            int lineNumber = 0;

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (line == "[/SKILLS]")
                {
                    break;
                }
                else if (line == "[SKILL]")
                {
                    skillData.Skills.Add(ReadSkill(ref reader, ref lineNumber));
                }
            }

            return skillData;
        }

        private static Skill ReadSkill(ref StringReader reader, ref int lineNumber)
        {
            Skill skill = new Skill();

            string line;
            int startLine = lineNumber;
            bool[] hasSet = new bool[3];

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (line == "[/SKILL]")
                {
                    break;
                }
                else if (line.StartsWith("<INTEGER64>GUID:"))
                {
                    line = line.Replace("<INTEGER64>GUID:", "").Trim();

                    long guid;
                    if (long.TryParse(line, out guid))
                    {
                        skill.GUID = guid;
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

                    skill.Name = line;
                    hasSet[1] = true;
                }
                else if (line.StartsWith("<STRING>File:"))
                {
                    line = line.Replace("<STRING>File:", "").Trim();

                    skill.File = line;
                    hasSet[2] = true;
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

            return skill;
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
        }
    }
}
