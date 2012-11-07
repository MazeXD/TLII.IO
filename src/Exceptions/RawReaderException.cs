using System;

namespace TLII.IO.Exceptions
{
    public class RawReaderException : Exception
    {
        public RawReaderException(string message) : base(message) { }
        public RawReaderException(string message, Exception innerException) : base(message, innerException) { }
    }
}
