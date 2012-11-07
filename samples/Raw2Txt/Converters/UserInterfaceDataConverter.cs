using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLII.IO.Formats.Raw;
using TLII.IO.Formats.Raw.Data;

namespace Raw2Txt.Converters
{
    static class UserInterfaceDataConverter
    {
        public static string GetString(UserInterfaceData userInterfaceData)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("[USERINTERFACES]");

            if (userInterfaceData.UserInterfaces.Count > 0)
            {
                foreach (var userInterface in userInterfaceData.UserInterfaces)
                {
                    builder.AppendUserInterface(userInterface);
                }
            }

            builder.Append("[/USERINTERFACES]");

            return builder.ToString();
        }

        private static void AppendUserInterface(this StringBuilder builder, UserInterface userInterface)
        {
            builder.AppendLine("   [USERINTERFACE]");

            builder.AppendFormat("      <STRING>Name:{0}\r\n", userInterface.Name);
            builder.AppendFormat("      <STRING>File:{0}\r\n", userInterface.File);
            builder.AppendFormat("      <INTEGER>Unknown:{0}\r\n", userInterface.Unknown);
            builder.AppendFormat("      <INTEGER>Unknown2:{0}\r\n", userInterface.Unknown2);
            builder.AppendFormat("      <SHORT>Unknown3:{0}\r\n", userInterface.Unknown3);
            builder.AppendFormat("      <BOOL>Unknown4:{0:X2}\r\n", userInterface.Unknown4.Value ? "True" : "False");
            builder.AppendFormat("      <STRING>Unknown5:{0}\r\n", userInterface.Unknown5);

            builder.AppendLine("   [/USERINTERFACE]");
        }
    }
}
