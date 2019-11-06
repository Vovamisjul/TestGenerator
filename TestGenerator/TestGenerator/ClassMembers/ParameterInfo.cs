using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGenerator.ClassMembers
{
    class ParameterInfo
    {
        public string Name { get; set; }
        public TypeSyntax Type { get; set; }
        public ParameterInfo(string name, TypeSyntax type)
        {
            Name = name;
            Type = type;
        }
    }
}
