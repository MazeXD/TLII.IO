namespace TLII.IO.Utilities
{
    public static class HashUtility
    {
        public static int GenerateHash(string str)
        {
            int hash = str.Length;
            foreach (var c in str)
            {
                hash = (hash >> 27) ^ (hash << 5) ^ c;
            }

            return hash;
        }
    }
}
