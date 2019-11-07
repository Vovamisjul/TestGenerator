using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestGenerator;

namespace TestGeneratorTest
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public async Task kek()
        {
            Generator gen = new Generator(new List<string> { "D:\\spp\\TestGenerator\\TestGenerator\\TestGenerator.Test\\SimpleClass.cs", "D:\\spp\\TestGenerator\\TestGenerator\\TestGenerator.Test\\ManyClasses.cs", "D:\\spp\\TestGenerator\\TestGenerator\\TestGenerator.Test\\ManyNamespaces.cs" }, "D:\\spp\\TestGenerator\\generatedClasses", 10, 10, 10);
            await gen.GenerateAsync();
        }
    }
}
