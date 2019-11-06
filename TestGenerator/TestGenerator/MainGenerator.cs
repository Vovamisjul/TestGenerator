using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        
        private List<MethodMetadata> GetMethods(ClassDeclarationSyntax Class)
        {
            var methods = Class.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Where(method => method.Modifiers
                .Any(modifier => modifier.ToString() == "public"));

            var result = new List<MethodMetadata>();

            foreach (var method in methods)
            {
                result.Add(new MethodMetadata() { Name = method.Identifier.ToString(), ReturnType = method.ReturnType, parameters = GetParametersMetadata(method) });
            }

            return result;
        }

        private IEnumerable<ParameterMetadata> GetParameters(MethodDeclarationSyntax method)
        {
            return method.ParameterList.Parameters.Select(param => new ParameterMetadata()
            { Name = param.Type.GetTypeName(), Type = param.Type });
        }

        private IEnumerable<ParameterMetadata> GetClassDependencies(ClassDeclarationSyntax Class)
        {
            var constructors = Class.DescendantNodes()
                .OfType<ConstructorDeclarationSyntax>()
                .Where(method => method.Modifiers
                .Any(modifier => modifier.ToString() == "public"));

            foreach (var constructor in constructors)
            {
                var dependencies = constructor.ParameterList.Parameters.Where(param =>
                param.Type.GetTypeName()
                .StartsWith("I"));

                if (!dependencies.Count().Equals(0))
                    return dependencies.Select(param => new ParameterMetadata()
                    {
                        Name = param.Identifier.Value.ToString(),
                        Type = param.Type
                    });
            }

            // no dependencies were found

            return null;
        }

        private string GenerateTestFromTree(SyntaxNode root)
        {
            //there may be more than one class in one file
            var classesInfo = new List<ClassMetadata>();

            var classes = new List<ClassDeclarationSyntax>(root.DescendantNodes().OfType<ClassDeclarationSyntax>());

            var usings = new List<UsingDirectiveSyntax>(root.DescendantNodes().OfType<UsingDirectiveSyntax>());

            foreach (var Class in classes)
            {
                classesInfo.Add(new ClassMetadata()
                {
                    Methods = GetClassMethods(Class),
                    Name = Class.Identifier.ToString(),
                    NameSpace = ((NamespaceDeclarationSyntax)Class.Parent).Name.ToString(),
                    Dependencies = GetClassDependencies(Class),

                });
            }

            return TestClassesGenerator.Generate(classesInfo, usings);
        }
    }
}
