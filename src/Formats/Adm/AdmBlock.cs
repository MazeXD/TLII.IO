using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TLII.IO.Formats.Adm
{
    public class AdmBlock
    {
        public int NameHash;
        public List<AdmField> Fields = new List<AdmField>();
        public List<AdmBlock> Childs = new List<AdmBlock>();
    }
}
