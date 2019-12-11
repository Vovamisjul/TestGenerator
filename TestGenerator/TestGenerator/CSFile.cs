using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGenerator
{
    class CSFile
    {
        public string FileName { get; }
        public string Text { get; }
        public CSFile(string fileName, string text)
        {
            FileName = fileName;
            Text = text;
        }
    }
}
