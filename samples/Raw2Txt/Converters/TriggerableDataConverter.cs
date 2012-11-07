using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLII.IO.Formats.Raw;
using TLII.IO.Formats.Raw.Data;

namespace Raw2Txt.Converters
{
    static class TriggerableDataConverter
    {
        public static string GetString(TriggerableData triggerableData)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("[TRIGGERABLES]");

            if (triggerableData.Triggerables.Count > 0)
            {
                foreach (var triggerable in triggerableData.Triggerables)
                {
                    builder.AppendTriggerable(triggerable);
                }
            }

            builder.Append("[/TRIGGERABLES]");

            return builder.ToString();
        }

        private static void AppendTriggerable(this StringBuilder builder, Triggerable triggerable)
        {
            builder.AppendLine("   [TRIGGERABLE]");

            builder.AppendFormat("      <STRING>Name:{0}\r\n", triggerable.Name);
            builder.AppendFormat("      <STRING>File:{0}\r\n", triggerable.File);

            builder.AppendLine("   [/TRIGGERABLE]");
        }
    }
}
