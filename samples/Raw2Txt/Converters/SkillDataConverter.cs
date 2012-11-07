using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLII.IO.Formats.Raw;
using TLII.IO.Formats.Raw.Data;

namespace Raw2Txt.Converters
{
    static class SkillDataConverter
    {
        public static string GetString(SkillData skillData)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("[SKILLS]");

            if (skillData.Skills.Count > 0)
            {
                foreach (var skill in skillData.Skills)
                {
                    builder.AppendSkill(skill);
                }
            }

            builder.Append("[/SKILLS]");

            return builder.ToString();
        }

        private static void AppendSkill(this StringBuilder builder, Skill skill)
        {
            builder.AppendLine("   [SKILL]");

            builder.AppendFormat("      <INTEGER64>GUID:{0}\r\n", skill.GUID);
            builder.AppendFormat("      <STRING>Name:{0}\r\n", skill.Name);
            builder.AppendFormat("      <STRING>File:{0}\r\n", skill.File);

            builder.AppendLine("   [/SKILL]");
        }
    }
}
