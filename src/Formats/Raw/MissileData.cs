using System.Collections.Generic;
using TLII.IO.Formats.Raw.Data;

namespace TLII.IO.Formats.Raw
{
    public class MissileData : RawData
    {
        public override RawType Type
        {
            get { return RawType.MissileData; }
        }

        public List<Missile> Missiles = new List<Missile>();
    }
}
