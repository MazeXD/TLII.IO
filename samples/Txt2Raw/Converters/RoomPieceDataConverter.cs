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
    static class RoomPieceDataConverter
    {
        public static RoomPieceData GetData(string data)
        {
            RoomPieceData roomPieceData = new RoomPieceData();

            StringReader reader = new StringReader(data);

            string line = "";
            int lineNumber = 0;

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (line == "[/ROOMPIECES]")
                {
                    break;
                }
                else if (line == "[LEVELSET]")
                {
                    roomPieceData.LevelSets.Add(ReadLevelSet(ref reader, ref lineNumber));
                }
            }

            return roomPieceData;
        }

        private static LevelSet ReadLevelSet(ref StringReader reader, ref int lineNumber)
        {
            LevelSet levelSet = new LevelSet();

            string line;
            int startLine = lineNumber;
            bool[] hasSet = new bool[1];

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (line == "[/LEVELSET]")
                {
                    break;
                }
                else if (line.StartsWith("<STRING>File:"))
                {
                    line = line.Replace("<STRING>File:", "").Trim();

                    levelSet.File = line;
                    hasSet[0] = true;
                }
                else if (line.StartsWith("[GUIDS]"))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        lineNumber++;

                        if (line == "[/GUIDS]")
                        {
                            break;
                        }
						else if(line.StartsWith("//") || line.StartsWith("#"))
						{
							continue;
						}
                        else
                        {
                            line = line.Replace("<INTEGER64>", "").Trim();

                            long guid;
                            if (long.TryParse(line, out guid))
                            {
                                levelSet.GUIDS.Add(guid);
                            }
                            else
                            {
                                throw new TxtConverterException(String.Format("Any GUID must be a 64bit Integer. (Line: {0} | Current: {1})", lineNumber, line));
                            }
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

            return levelSet;
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
