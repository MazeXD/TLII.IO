using System.Collections.Generic;
using System.IO;

namespace Txt2Adm.Contexts
{
    class AdmConverterContext
    {
        public Stack<string> BlockStack = new Stack<string>();
        public int LineNumber = 0;
        public StringReader Reader;
    }
}
