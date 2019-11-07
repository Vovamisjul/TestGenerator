using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGenerator.ClassMembers;

namespace TestGenerator
{
    class MainGenerator
    {
        public async Task<CSFile> CreateTest(CSFile fileText)
        {
            var root = await CSharpSyntaxTree.ParseText(fileText.Text).GetRootAsync();
            return new CSFile(Path.GetFileNameWithoutExtension(fileText.FileName) + "Test.cs",
                GenerateTestFromTree(root));
        }
        
        private List<MethodInfo> GetMethods(ClassDeclarationSyntax Class)
        {
            var methods = Class.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Where(method => method.Modifiers
                .Any(modifier => modifier.ToString() == "public"));

            var result = new List<MethodInfo>();

            foreach (var method in methods)
            {
                result.Add(new MethodInfo(method.Identifier.ToString(), method.ReturnType, GetParameters(method)));
            }

            return result;
        }

        private List<ParameterInfo> GetParameters(MethodDeclarationSyntax method)
        {
            return method.ParameterList.Parameters.Select(param => new ParameterInfo(param.Identifier.Value.ToString(), param.Type)).ToList();
        }

        private string GenerateTestFromTree(SyntaxNode root)
        {
            //there may be more than one class in one file
            var namespacesInfo = new List<NamespaceInfo>();

            var classes = new List<ClassDeclarationSyntax>(root.DescendantNodes().OfType<ClassDeclarationSyntax>());

            var usings = new List<UsingDirectiveSyntax>(root.DescendantNodes().OfType<UsingDirectiveSyntax>());

            var namespaces = new List<NamespaceDeclarationSyntax>(root.DescendantNodes().OfType<NamespaceDeclarationSyntax>());

            foreach (var ns in namespaces)
            {
                var nsClasses = new List<ClassInfo>();
                foreach (var Class in classes.Where(Class => ((NamespaceDeclarationSyntax)Class.Parent).Name.ToString() == ns.Name.ToString()))
                {
                    nsClasses.Add(new ClassInfo(Class.Identifier.ToString(), GetMethods(Class)));
                }
                namespacesInfo.Add(new NamespaceInfo(ns.Name.ToString(), nsClasses));
            }

            return TestClassesGenerator.Generate(namespacesInfo, usings);
        }
    }
}
