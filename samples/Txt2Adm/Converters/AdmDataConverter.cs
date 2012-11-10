using System;
using System.Globalization;
using System.IO;
using TLII.IO.Formats.Adm;
using TLII.IO.Formats.Adm.Fields;
using TLII.IO.Utilities;
using Txt2Adm.Contexts;
using Txt2Adm.Exceptions;

namespace Txt2Adm.Converters
{
    static class AdmDataConverter
    {
        public static AdmData GetData(string data)
        {
            AdmData admData = new AdmData();

            var context = new AdmConverterContext();
            context.Reader = new StringReader(data);

            string line = "";

            while ((line = context.Reader.ReadLine()) != null)
            {
                context.LineNumber++;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    if (line.StartsWith("[/"))
                    {
                        line = line.Substring(2, line.Length - 3);

                        if (line != context.BlockStack.Pop())
                        {
                            throw new AdmConverterException(String.Format("Can't close outer block before inner! (Line: {0})", context.LineNumber));
                        }

                        break;
                    }

                    line = line.Substring(1, line.Length - 2);

                    context.BlockStack.Push(line);

                    admData.MainBlock = ReadBlock(line, ref context);
                }
            }


            context.Reader.Close();
            context.Reader.Dispose();

            return admData;
        }

        private static AdmBlock ReadBlock(string name, ref AdmConverterContext context)
        {
            AdmBlock block = new AdmBlock();
            block.NameHash = name.StartsWith("0x") ? int.Parse(name.Remove(0, 2), NumberStyles.HexNumber) : HashUtility.GenerateHash(name);

            string line = "";
            while ((line = context.Reader.ReadLine()) != null)
            {
                context.LineNumber++;

                if (line.StartsWith("//") || line.StartsWith("#"))
                {
                    continue;
                }
                else if (line.StartsWith("<") && line.IndexOf(">") != -1 && line.IndexOf(":") != -1)
                {
                    block.Fields.Add(ReadField(line, ref context));
                }
                else if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    if (line.StartsWith("[/"))
                    {
                        line = line.Substring(2, line.Length - 3);

                        if (line != context.BlockStack.Pop())
                        {
                            throw new AdmConverterException(String.Format("Can't close outer block before inner! (Line: {0})", context.LineNumber));
                        }

                        break;
                    }

                    line = line.Substring(1, line.Length - 2);

                    context.BlockStack.Push(line);

                    block.Childs.Add(ReadBlock(line, ref context));
                }
                else
                {
                    throw new AdmConverterException(String.Format("Unknown line format! (Line: {0})", context.LineNumber));
                }
            }

            return block;
        }

        private static AdmField ReadField(string line, ref AdmConverterContext context)
        {
            string type = line.Substring(1, line.IndexOf(">") - 1);

            var field = GetFieldByType(type);

            if (field == null)
            {
                throw new AdmConverterException(String.Format("Invalid field type! (Line: {0})", context.LineNumber));
            }

            string name = line.Substring(line.IndexOf(">") + 1, line.IndexOf(":") - line.IndexOf(">") - 1);

            field.NameHash = name.StartsWith("0x") ? int.Parse(name.Remove(0,2), NumberStyles.HexNumber) : HashUtility.GenerateHash(name);

            string value = line.Substring(line.IndexOf(":") + 1);
            try
            {
                switch (field.Type)
                {
                    case AdmFieldType.Integer:
                        field.Value = ReadInteger(value);
                        break;
                    case AdmFieldType.Float:
                        field.Value = ReadFloat(value);
                        break;
                    case AdmFieldType.Double:
                        field.Value = ReadDouble(value);
                        break;
                    case AdmFieldType.UnsignedInteger:
                        field.Value = ReadUnsignedInt(value);
                        break;
                    case AdmFieldType.String:
                        field.Value = ReadString(value);
                        break;
                    case AdmFieldType.Boolean:
                        field.Value = ReadBoolean(value);
                        break;
                    case AdmFieldType.Integer64:
                        field.Value = ReadInteger64(value);
                        break;
                    case AdmFieldType.Translate:
                        field.Value = ReadTranslate(value);
                        break;
                }
            }
            catch (InvalidDataException e)
            {
                throw new AdmConverterException(string.Format("{0} (Line: {1}", e.Message, context.LineNumber));
            }

            return field;
        }

        private static int ReadInteger(string value)
        {
            int result;
            if (!int.TryParse(value, out result))
            {
                throw new InvalidDataException("Fields of type Integer must be an integer");
            }

            return result;
        }

        private static float ReadFloat(string value)
        {
            float result;
            if (!float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                throw new InvalidDataException("Fields of type Float must be a float");
            }

            return result;
        }

        private static double ReadDouble(string value)
        {
            double result;
            if (!double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                throw new InvalidDataException("Fields of type Double must be a double");
            }

            return result;
        }

        private static uint ReadUnsignedInt(string value)
        {
            uint result;
            if (!uint.TryParse(value, out result))
            {
                throw new InvalidDataException("Fields of type Unsigned Int must be an unsigned integer");
            }

            return result;
        }

        private static string ReadString(string value)
        {
            return value;
        }

        private static bool ReadBoolean(string value)
        {
            value = value.ToUpperInvariant();

            if (value == "TRUE" || value == "1")
            {
                return true;
            }
            else if (value == "FALSE" || value == "0")
            {
                return false;
            }
            else
            {
                throw new InvalidDataException("Fields of type Boolean must be TRUE or FALSE");
            }
        }

        private static long ReadInteger64(string value)
        {
            long result;
            if(!long.TryParse(value, out result))
            {
                throw new InvalidDataException("Fields of type Integer64 must be an 64bit Integer");
            }

            return result;
        }

        private static string ReadTranslate(string value)
        {
            return value;
        }

        private static AdmField GetFieldByType(string type)
        {
            switch (type.ToUpperInvariant())
            {
                case "INTEGER":
                    return new AdmIntegerField();
                case "FLOAT":
                    return new AdmFloatField();
                case "DOUBLE":
                    return new AdmDoubleField();
                case "UNSIGNED INT":
                    return new AdmUnsignedIntegerField();
                case "STRING":
                    return new AdmStringField();
                case "BOOLEAN":
                    return new AdmBooleanField();
                case "INTEGER64":
                    return new AdmInteger64Field();
                case "TRANSLATE":
                    return new AdmTranslateField();
            }

            return null;
        }
    }
}
