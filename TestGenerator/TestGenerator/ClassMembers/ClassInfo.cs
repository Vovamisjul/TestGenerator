using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGenerator.ClassMembers
{
    class ClassInfo
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public List<MethodInfo> Methods { get; set; }
        public ClassInfo(string name, string nameSpace, List<MethodInfo> methods)
        {
            Name = name;
            NameSpace = nameSpace;
            Methods = methods;
        }
    }
}
