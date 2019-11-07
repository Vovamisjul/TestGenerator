using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGenerator.ClassMembers
{
    class NamespaceInfo
    {
        public NamespaceInfo(string name, List<ClassInfo> classes)
        {
            Name = name;
            Classes = classes;
        }
        public string Name { get; set; }
        public List<ClassInfo> Classes { get; set; }
    }
}
