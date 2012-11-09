using System;

namespace TLII.IO.Utilities
{
    public static class GUIDUtility
    {
        public static long GenerateGUID()
        {
            var guid = Guid.NewGuid();
            byte[] guidBytes = guid.ToByteArray();

            return BitConverter.ToInt64(guidBytes, 4);
        }
    }
}
