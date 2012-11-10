using System.Collections.Generic;
using TLII.IO.Utilities;

namespace Adm2Txt.KeyTables
{
    abstract class KeyTableSource
    {
        private Dictionary<int, string> _keyTable = new Dictionary<int, string>();

        protected void AddEntry(string value)
        {
            int hash = HashUtility.GenerateHash(value);

            if (!_keyTable.ContainsKey(hash))
            {
                _keyTable.Add(hash, value);
            }
        }

        public void AddToKeyTable(ref Dictionary<int, string> keyTable)
        {
            foreach (var entry in _keyTable)
            {
                if (!keyTable.ContainsKey(entry.Key))
                {
                    keyTable.Add(entry.Key, entry.Value);
                }
            }
        }

        public abstract void Load();
    }
}
