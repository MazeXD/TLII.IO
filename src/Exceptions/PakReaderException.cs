using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TLII.IO.Exceptions
{
    public class PakReaderException : Exception
    {
        public PakReaderException(string message) : base(message) { }
        public PakReaderException(string message, Exception innerException) : base(message, innerException) { }
    }
}
