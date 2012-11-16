using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Adm2Txt.KeyTables;
using TLII.IO.Formats.Adm;

namespace Adm2Txt.Converters
{
    static class AdmDataConverter
    {
        private static Dictionary<int, string> _keyTable = new Dictionary<int, string>();

        static AdmDataConverter()
        {
            new ResourceKeyTableSource("KeyTable.txt").AddToKeyTable(ref _keyTable);
            new FileKeyTableSource("keytable.txt").AddToKeyTable(ref _keyTable);
        }

        public static string GetString(AdmData data)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendBlock(data.MainBlock);

            return builder.ToString();
        }

        private static void AppendBlock(this StringBuilder builder, AdmBlock block, int depth = 0)
        {
            string name = GetStringFromKeyTable(block.NameHash);

            builder.AppendFormat("{0}[{1}]\r\n", new String(' ', depth * 3), name);

            foreach (var field in block.Fields)
            {
                builder.AppendField(field, depth + 1);
            }

            foreach (var subBlock in block.Childs)
            {
                builder.AppendBlock(subBlock, depth + 1);
            }

            builder.AppendFormat("{0}[/{1}]", new String(' ', depth * 3), name);

            if (depth > 0)
            {
                builder.AppendLine();
            }
        }

        private static void AppendField(this StringBuilder builder, AdmField field, int depth)
        {
            string type = "";

            switch (field.Type)
            {
                case AdmFieldType.Integer:
                    type = "INTEGER";
                    break;
                case AdmFieldType.Float:
                    type = "FLOAT";
                    break;
                case AdmFieldType.Double:
                    type = "DOUBLE";
                    break;
                case AdmFieldType.UnsignedInteger:
                    type = "UNSIGNED INT";
                    break;
                case AdmFieldType.String:
                    type = "STRING";
                    break;
                case AdmFieldType.Boolean:
                    type = "BOOLEAN";
                    break;
                case AdmFieldType.Integer64:
                    type = "INTEGER64";
                    break;
                case AdmFieldType.Translate:
                    type = "TRANSLATE";
                    break;
            }

            object value = field.Value;

            if (value is bool)
            {
                value = (bool)value ? "True" : "False";
            }
            else if (value is float)
            {
                value = ((float)value).ToString(CultureInfo.InvariantCulture);
            }
            else if (value is double)
            {
                value = ((double)value).ToString(CultureInfo.InvariantCulture);
            }

            builder.AppendFormat("{0}<{1}>{2}:{3}\r\n", new String(' ', depth * 3), type, GetStringFromKeyTable(field.NameHash), value);
        }

        private static string GetStringFromKeyTable(int key)
        {
            if (_keyTable.ContainsKey(key))
            {
                return _keyTable[key];
            }

            return "0x" + key.ToString("X8");
        }
    }
}
