using System.Collections.Generic;

namespace TLII.IO.Contexts
{
    class AdmReadingContext
    {
        public Dictionary<int, string> StringTable = new Dictionary<int, string>();

        public string GetStringByKey(int key)
        {
            if (!StringTable.ContainsKey(key))
            {
                return key.ToString();
            }

            return StringTable[key];
        }
    }
}
