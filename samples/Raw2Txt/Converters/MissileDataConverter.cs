using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLII.IO.Formats.Raw.Data;
using TLII.IO.Formats.Raw;

namespace Raw2Txt.Converters
{
    static class MissileDataConverter
    {
        public static string GetString(MissileData missileData)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("[MISSILES]");

            if (missileData.Missiles.Count > 0)
            {
                foreach (var missile in missileData.Missiles)
                {
                    builder.AppendMissile(missile);
                }
            }

            builder.Append("[/MISSILES]");

            return builder.ToString();
        }

        private static void AppendMissile(this StringBuilder builder, Missile missile)
        {
            builder.AppendLine("   [MISSILE]");

            builder.AppendFormat("      <STRING>File:{0}\r\n", missile.File);

            if (missile.Names.Count > 0)
            {
                builder.AppendLine("      [NAMES]");
                foreach (var name in missile.Names)
                {
                    builder.AppendFormat("         <STRING>{0}\r\n", name);
                }
                builder.AppendLine("      [/NAMES]");
            }

            builder.AppendLine("   [/MISSILE]");
        }
    }
}
