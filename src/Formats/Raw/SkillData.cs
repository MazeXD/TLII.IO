using System;
using System.Collections.Generic;
using System.Linq;
using TLII.IO.Formats.Raw.Data;

namespace TLII.IO.Formats.Raw
{
    public class SkillData : RawData
    {
        public override RawType Type
        {
            get { return RawType.SkillData; }
        }

        public List<Skill> Skills = new List<Skill>();
    }
}
