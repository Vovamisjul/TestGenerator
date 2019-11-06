using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGenerator
{
    struct CSFile
    {
        public CSFile(string fileName, string text)
        {
            FileName = fileName;
            Text = text;
        }
        public string FileName;
        public string Text;
    }
}
