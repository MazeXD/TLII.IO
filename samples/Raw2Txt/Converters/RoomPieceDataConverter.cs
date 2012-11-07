using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLII.IO.Formats.Raw.Data;
using TLII.IO.Formats.Raw;

namespace Raw2Txt.Converters
{
    static class RoomPieceDataConverter
    {
        public static string GetString(RoomPieceData roomPieceData)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("[ROOMPIECES]");

            if (roomPieceData.LevelSets.Count > 0)
            {
                foreach (var levelSet in roomPieceData.LevelSets)
                {
                    builder.AppendLevelSet(levelSet);
                }
            }

            builder.Append("[/ROOMPIECES]");

            return builder.ToString();
        }

        private static void AppendLevelSet(this StringBuilder builder, LevelSet levelSet)
        {
            builder.AppendLine("   [LEVELSET]");

            builder.AppendFormat("      <STRING>File:{0}\r\n", levelSet.File);

            if (levelSet.GUIDS.Count > 0)
            {
                builder.AppendLine("      [GUIDS]");

                foreach (var guid in levelSet.GUIDS)
                {
                    builder.AppendFormat("         <INTEGER64>{0}\r\n", guid);
                }

                builder.AppendLine("      [/GUIDS]");
            }

            builder.AppendLine("   [/LEVELSET]");
        }
    }
}
