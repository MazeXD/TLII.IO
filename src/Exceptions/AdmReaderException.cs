using System;

namespace TLII.IO.Exceptions
{
    public class AdmReaderException : Exception
    {
        public AdmReaderException(string message) : base(message) { }
        public AdmReaderException(string message, Exception innerException) : base(message, innerException) { }
    }
}
