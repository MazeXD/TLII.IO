using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLII.IO.Formats.Raw;
using TLII.IO.Formats.Raw.Data;

namespace Raw2Txt.Converters
{
    static class AffixDataConverter
    {
        public static string GetString(AffixData affixData)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("[AFFIXES]");

            if (affixData.Affixes.Count > 0)
            {
                foreach (var affix in affixData.Affixes)
                {
                    builder.AppendAffix(affix);
                }
            }

            builder.Append("[/AFFIXES]");

            return builder.ToString();
        }

        private static void AppendAffix(this StringBuilder builder, Affix affix)
        {
            builder.AppendLine("   [AFFIX]");

            builder.AppendFormat("      <STRING>Name:{0}\r\n", affix.Name);
            builder.AppendFormat("      <STRING>File:{0}\r\n", affix.File);
            builder.AppendFormat("      <INTEGER>MinSpawnRange:{0}\r\n", affix.MinSpawnRange);
            builder.AppendFormat("      <INTEGER>MaxSpawnRange:{0}\r\n", affix.MaxSpawnRange);
            builder.AppendFormat("      <INTEGER>Weight:{0}\r\n", affix.Weight);
            builder.AppendFormat("      <INTEGER>DifficultiesAllowed:{0}\r\n", affix.DifficultiesAllowed);

            if (affix.UnitTypes.Count > 0)
            {
                builder.AppendLine("      [UNITTYPES]");
                foreach (var unitType in affix.UnitTypes)
                {
                    builder.AppendFormat("         <STRING>{0}\r\n", unitType);
                }
                builder.AppendLine("      [/UNITTYPES]");
            }

            if (affix.NotUnitTypes.Count > 0)
            {
                builder.AppendLine("      [NOTUNITTYPES]");
                foreach (var unitType in affix.NotUnitTypes)
                {
                    builder.AppendFormat("         <STRING>{0}\r\n", unitType);
                }
                builder.AppendLine("      [/NOTUNITTYPES]");
            }
            builder.AppendLine("   [/AFFIX]");
        }
    }
}
