using System;
using System.IO;
using TLII.IO.Formats.Raw;
using TLII.IO.Formats.Raw.Data;
using Txt2Raw.Exceptions;

namespace Txt2Raw.Converters
{
    static class UserInterfaceDataConverter
    {
        public static UserInterfaceData GetData(string data)
        {
            UserInterfaceData userInterfaceData = new UserInterfaceData();

            StringReader reader = new StringReader(data);

            string line = "";
            int lineNumber = 0;

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (line == "[/USERINTERFACES]")
                {
                    break;
                }
                else if (line == "[USERINTERFACE]")
                {
                    userInterfaceData.UserInterfaces.Add(ReadUserInterface(ref reader, ref lineNumber));
                }
            }

            return userInterfaceData;
        }

        private static UserInterface ReadUserInterface(ref StringReader reader, ref int lineNumber)
        {
            UserInterface userInterface = new UserInterface();

            string line;
            int startLine = lineNumber;
            bool[] hasSet = new bool[7];

            /*
            
            builder.AppendFormat("      <STRING>Name:{0}\r\n", userInterface.Name);
            builder.AppendFormat("      <STRING>File:{0}\r\n", userInterface.File);
            builder.AppendFormat("      <INTEGER>Unknown:{0}\r\n", userInterface.Unknown);
            builder.AppendFormat("      <INTEGER>Unknown2:{0}\r\n", userInterface.Unknown2);
            builder.AppendFormat("      <SHORT>Unknown3:{0}\r\n", userInterface.Unknown3);
            builder.AppendFormat("      <BOOL>Unknown4:{0:X2}\r\n", userInterface.Unknown4.Value ? "True" : "False");
            builder.AppendFormat("      <STRING>Unknown5:{0}\r\n", userInterface.Unknown5);
            
            */
            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (line == "[/USERINTERFACE]")
                {
                    break;
                }
                else if (line.StartsWith("<STRING>Name:"))
                {
                    line = line.Replace("<STRING>Name:", "").Trim();

                    userInterface.Name = line;
                    hasSet[0] = true;
                }
                else if (line.StartsWith("<STRING>File:"))
                {
                    line = line.Replace("<STRING>File:", "").Trim();

                    userInterface.File = line;
                    hasSet[1] = true;
                }
                else if (line.StartsWith("<INTEGER>Unknown:"))
                {
                    line = line.Replace("<INTEGER>Unknown:", "").Trim();

                    int unknown;
                    if (int.TryParse(line, out unknown))
                    {
                        userInterface.Unknown = unknown;
                        hasSet[2] = true;
                    }
                    else
                    {
                        throw new TxtConverterException(String.Format("Unknown must be an Integer. (Line: {0} | Current: {1})", lineNumber, line));
                    }
                }
                else if (line.StartsWith("<INTEGER>Unknown2:"))
                {
                    line = line.Replace("<INTEGER>Unknown2:", "").Trim();

                    int unknown;
                    if (int.TryParse(line, out unknown))
                    {
                        userInterface.Unknown2 = unknown;
                        hasSet[3] = true;
                    }
                    else
                    {
                        throw new TxtConverterException(String.Format("Unknown2 must be an Integer. (Line: {0} | Current: {1})", lineNumber, line));
                    }
                }
                else if (line.StartsWith("<SHORT>Unknown3:"))
                {
                    line = line.Replace("<SHORT>Unknown3:", "").Trim();

                    short unknown;
                    if (short.TryParse(line, out unknown))
                    {
                        userInterface.Unknown3 = unknown;
                        hasSet[4] = true;
                    }
                    else
                    {
                        throw new TxtConverterException(String.Format("Unknown3 must be an 16bit Integer. (Line: {0} | Current: {1})", lineNumber, line));
                    }
                }
                else if (line.StartsWith("<BOOL>Unknown4:"))
                {
                    line = line.Replace("<BOOL>Unknown4:", "").Trim();

                    if (line.ToUpperInvariant() == "TRUE")
                    {
                        userInterface.Unknown4 = true;
                        hasSet[5] = true;
                    }
                    else if (line.ToUpperInvariant() == "FALSE")
                    {
                        userInterface.Unknown4 = false;
                        hasSet[5] = true;
                    }
                    else
                    {
                        throw new TxtConverterException(String.Format("Unknown4 must be a Boolean [True/False]. (Line: {0} | Current: {1})", lineNumber, line));
                    }
                }
                else if (line.StartsWith("<STRING>Unknown5:"))
                {
                    line = line.Replace("<STRING>Unknown5:", "").Trim();

                    userInterface.Unknown5 = line;
                    hasSet[6] = true;
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

            return userInterface;
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
                throw new TxtConverterException(String.Format("Unknown must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[3])
            {
                throw new TxtConverterException(String.Format("Unknown2 must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[4])
            {
                throw new TxtConverterException(String.Format("Unknown3 must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[5])
            {
                throw new TxtConverterException(String.Format("Unknown4 must be specified. (Line: {0})", lineNumber));
            }

            if (!hasSet[6])
            {
                throw new TxtConverterException(String.Format("Unknown5 must be specified. (Line: {0})", lineNumber));
            }
        }
    }
}
