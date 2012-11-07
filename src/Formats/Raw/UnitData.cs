using System.Collections.Generic;
using TLII.IO.Formats.Raw.Data;

namespace TLII.IO.Formats.Raw
{
    public class UnitData : RawData
    {
        public override RawType Type
        {
            get { return RawType.UnitData; }
        }

        public List<Unit> Items = new List<Unit>();
        public List<Unit> Monsters = new List<Unit>();
        public List<Unit> Players = new List<Unit>();
        public List<Unit> Props = new List<Unit>();
    }
}
