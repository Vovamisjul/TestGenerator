﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGenerator.ClassMembers
{
    class MethodInfo
    {
        public string Name { get; set; }
        public TypeSyntax ReturnType { get; set; }
        public List<ParameterInfo> Parameters { get; set; }
        public MethodInfo(string name, TypeSyntax returnType, List<ParameterInfo> parameters)
        {
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
        }
    }
}
