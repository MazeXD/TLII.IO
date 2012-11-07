using System.Collections.Generic;
using TLII.IO.Formats.Raw.Data;

namespace TLII.IO.Formats.Raw
{
    public class RoomPieceData : RawData
    {
        public override RawType Type
        {
            get { return RawType.RoomPieceData; }
        }

        public List<LevelSet> LevelSets = new List<LevelSet>();
    }
}
