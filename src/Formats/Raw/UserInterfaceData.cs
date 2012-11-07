using System.Collections.Generic;
using TLII.IO.Formats.Raw.Data;

namespace TLII.IO.Formats.Raw
{
    public class UserInterfaceData : RawData
    {
        public override RawType Type
        {
            get { return RawType.UserInterfaceData; }
        }

        public List<UserInterface> UserInterfaces = new List<UserInterface>();
    }
}
