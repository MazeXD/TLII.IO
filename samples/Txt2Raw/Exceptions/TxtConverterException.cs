using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Txt2Raw.Exceptions
{
    class TxtConverterException : Exception
    {
        public TxtConverterException(string message) : base(message) { }
    }
}
