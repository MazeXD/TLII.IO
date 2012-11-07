using System.Collections.Generic;
using TLII.IO.Formats.Raw.Data;

namespace TLII.IO.Formats.Raw
{
    public class AffixData : RawData
    {
        public override RawType Type
        {
            get { return RawType.AffixData; }
        }

        public List<Affix> Affixes = new List<Affix>();
    }
}
