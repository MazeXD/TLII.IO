using System.Collections.Generic;

namespace TLII.IO.Formats.Raw.Data
{
    public class Affix
    {
        public string File;
        public string Name;

        public int MinSpawnRange;
        public int MaxSpawnRange;

        public int Weight;

        public int DifficultiesAllowed;

        public List<string> UnitTypes = new List<string>();
        public List<string> NotUnitTypes = new List<string>();

        public override string ToString()
        {
            return Name;
        }
    }
}
