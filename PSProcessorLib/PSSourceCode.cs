using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSProcessorLib
{
    public class PSSourceCode
    {
        private string _code;
        private string _filename;

        public string code { get { return _code; } }
        public string filename { get { return _filename; } }
        public string name { get { return System.IO.Path.GetFileNameWithoutExtension(_filename); } }

        public PSSourceCode(string code, string filename)
        {
            _code = code;
            _filename = filename;
        }
    }
}
