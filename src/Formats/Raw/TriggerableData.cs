using System.Collections.Generic;
using TLII.IO.Formats.Raw.Data;

namespace TLII.IO.Formats.Raw
{
    public class TriggerableData : RawData
    {
        public override RawType Type
        {
            get { return RawType.TriggerableData; }
        }

        public List<Triggerable> Triggerables = new List<Triggerable>();
    }
}
