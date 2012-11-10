using System;

namespace Txt2Adm.Exceptions
{
    class AdmConverterException : Exception
    {
        public AdmConverterException(string message) : base(message) { }
    }
}
