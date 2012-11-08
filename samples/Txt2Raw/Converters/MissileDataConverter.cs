using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLII.IO.Formats.Raw.Data;
using TLII.IO.Formats.Raw;
using Txt2Raw.Exceptions;
using System.IO;

namespace Txt2Raw.Converters
{
    static class MissileDataConverter
    {
        public static MissileData GetData(string data)
        {
            MissileData missileData = new MissileData();

            StringReader reader = new StringReader(data);

            string line = "";
            int lineNumber = 0;

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (line == "[/MISSILES]")
                {
                    break;
                }
                else if (line == "[MISSILE]")
                {
                    missileData.Missiles.Add(ReadMissile(ref reader, ref lineNumber));
                }
            }

            return missileData;
        }

        private static Missile ReadMissile(ref StringReader reader, ref int lineNumber)
        {
            Missile missile = new Missile();

            string line;
            int startLine = lineNumber;
            bool[] hasSet = new bool[1];

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (line == "[/MISSILE]")
                {
                    break;
                }
                else if (line.StartsWith("<STRING>File:"))
                {
                    line = line.Replace("<STRING>File:", "").Trim();

                    missile.File = line;
                    hasSet[0] = true;
                }
                else if (line.StartsWith("[NAMES]"))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        lineNumber++;

                        if (line == "[/NAMES]")
                        {
                            break;
                        }
						else if(line.StartsWith("//") || line.StartsWith("#"))
						{
							continue;
						}
                        else
                        {
                            missile.Names.Add(line.Replace("<STRING>", "").Trim());
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

            return missile;
        }

        private static void Validate(bool[] hasSet, int lineNumber)
        {
            if (!hasSet[0])
            {
                throw new TxtConverterException(String.Format("File must be specified. (Line: {0})", lineNumber));
            }
        }
    }
}
