using System;
using System.IO;
using TLII.IO.Formats.Raw;
using TLII.IO.Formats.Raw.Data;
using Txt2Raw.Exceptions;

namespace Txt2Raw.Converters
{
    static class TriggerableDataConverter
    {
        public static TriggerableData GetData(string data)
        {
            TriggerableData triggerableData = new TriggerableData();

            StringReader reader = new StringReader(data);

            string line = "";
            int lineNumber = 0;

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (line == "[/TRIGGERABLES]")
                {
                    break;
                }
                else if (line == "[TRIGGERABLE]")
                {
                    triggerableData.Triggerables.Add(ReadTriggerable(ref reader, ref lineNumber));
                }
            }

            return triggerableData;
        }

        private static Triggerable ReadTriggerable(ref StringReader reader, ref int lineNumber)
        {
            Triggerable triggerable = new Triggerable();

            string line;
            int startLine = lineNumber;
            bool[] hasSet = new bool[2];

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (line == "[/TRIGGERABLE]")
                {
                    break;
                }
                else if (line.StartsWith("<STRING>Name:"))
                {
                    line = line.Replace("<STRING>Name:", "").Trim();

                    triggerable.Name = line;
                    hasSet[0] = true;
                }
                else if (line.StartsWith("<STRING>File:"))
                {
                    line = line.Replace("<STRING>File:", "").Trim();

                    triggerable.File = line;
                    hasSet[1] = true;
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

            return triggerable;
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
        }
    }
}
