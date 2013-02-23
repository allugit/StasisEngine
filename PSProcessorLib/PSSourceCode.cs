using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSProcessorLib
{
    public class PSSourceCode
    {
        private string _code;

        public string code { get { return _code; } }

        public PSSourceCode(string code)
        {
            _code = code;
        }
    }
}
