using System;

namespace TLII.IO.Exceptions
{
    public class AdmWriterException : Exception
    {
        public AdmWriterException(string message) : base(message) { }
        public AdmWriterException(string message, Exception innerException) : base(message, innerException) { }
    }
}
