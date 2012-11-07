using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLII.IO.Formats.Raw;
using TLII.IO.Formats.Raw.Data;

namespace Raw2Txt.Converters
{
    public static class UnitDataConverter
    {
        public static string GetString(UnitData unitData)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("[UNITDATA]");

            if (unitData.Items.Count > 0)
            {
                builder.AppendLine("   [ITEMS]");
                foreach (var unit in unitData.Items)
                {
                    builder.AppendUnit(unit);
                }
                builder.AppendLine("   [/ITEMS]");
            }

            if (unitData.Monsters.Count > 0)
            {
                builder.AppendLine("   [MONSTERS]");
                foreach (var unit in unitData.Monsters)
                {
                    builder.AppendUnit(unit);
                }
                builder.AppendLine("   [/MONSTERS]");
            }

            if (unitData.Players.Count > 0)
            {
                builder.AppendLine("   [PLAYERS]");
                foreach (var unit in unitData.Players)
                {
                    builder.AppendUnit(unit);
                }
                builder.AppendLine("   [/PLAYERS]");
            }

            if (unitData.Props.Count > 0)
            {
                builder.AppendLine("   [PROPS]");
                foreach (var unit in unitData.Props)
                {
                    builder.AppendUnit(unit);
                }
                builder.AppendLine("   [/PROPS]");
            }

            builder.Append("[/UNITDATA]");

            return builder.ToString();
        }

        private static void AppendUnit(this StringBuilder builder, Unit unit)
        {
            builder.AppendLine("      [UNIT]");

            builder.AppendFormat("         <INTEGER64>GUID:{0}\r\n", unit.GUID);
            builder.AppendFormat("         <STRING>Name:{0}\r\n", unit.Name);
            builder.AppendFormat("         <STRING>File:{0}\r\n", unit.File);
            builder.AppendFormat("         <STRING>UnitType:{0}\r\n", unit.Type);
            builder.AppendFormat("         <BINARY>Unknown:{0:X2}\r\n", unit.Unknown);
            builder.AppendFormat("         <INTEGER>Level:{0}\r\n", unit.Level);
            builder.AppendFormat("         <INTEGER>MinLevel:{0}\r\n", unit.MinLevel);
            builder.AppendFormat("         <INTEGER>MaxLevel:{0}\r\n", unit.MaxLevel);
            builder.AppendFormat("         <INTEGER>Rarity:{0}\r\n", unit.Rarity);
            builder.AppendFormat("         <INTEGER>RarityHC:{0}\r\n", unit.RarityHC);

            builder.AppendLine("      [/UNIT]");
        }
    }
}
