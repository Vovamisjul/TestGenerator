﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
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
        private string location;
        private string testFolder;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            location = GetType().Assembly.Location;
            location = location.Substring(0, location.LastIndexOf(Path.DirectorySeparatorChar) + 1) + Path.DirectorySeparatorChar;
            var listTests = new List<string>
            {
                location + "SimpleClass.cs",
                location + "ManyClasses.cs",
                location + "ManyNamespaces.cs"
            };
            testFolder = location + "generatedClasses";
            Directory.CreateDirectory(testFolder);
            var maxFileToRead = 10;
            var maxFileToWrite = 10;
            var maxThreads = 10;
            Generator gen = new Generator(listTests, testFolder, maxFileToRead, maxFileToWrite, maxThreads);
            await gen.GenerateAsync();
        }

        [Test]
        public void When_FileWithSimpleMethods_Excpect_GenerateTest()
        {
            using (var reader = new StreamReader(new FileStream(testFolder + "\\SimpleClassTest.cs", FileMode.Open)))
            {
                var testFile = reader.ReadToEnd();
                var expected = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MyNamespace1;
namespace MyNamespace1.Tests
{
    [TestFixture]
    public class SimpleClassTests
    {
        [Test]
        public void ATest()
        {
            Assert.Fail(""autogenerated"");
        }

        [Test]
        public void BTest()
        {
            Assert.Fail(""autogenerated"");
        }
    }
}";
                Assert.AreEqual(testFile, expected);
            }
        }

        [Test]
        public void When_FileWithTwoClasses_Excpect_GenerateTest()
        {
            using (var reader = new StreamReader(new FileStream(testFolder + "\\ManyClassesTest.cs", FileMode.Open)))
            {
                var testFile = reader.ReadToEnd();
                var expected = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Namespace2;
namespace Namespace2.Tests
{
    [TestFixture]
    public class Class1Tests
    {
        [Test]
        public void KekTest()
        {
            Assert.Fail(""autogenerated"");
        }
    }

    [TestFixture]
    public class Class2Tests
    {
        [Test]
        public void LolTest()
        {
            Assert.Fail(""autogenerated"");
        }
    }
}";
                Assert.AreEqual(testFile, expected);
            }
        }

        [Test]
        public void When_ClassWithTwoNamespaces_Excpect_GenerateTest()
        {
            using (var reader = new StreamReader(new FileStream(testFolder + "\\ManyNamespacesTest.cs", FileMode.Open)))
            {
                var testFile = reader.ReadToEnd();
                var expected = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Namespace3;
using Namespace4;
namespace Namespace3.Tests
{
    [TestFixture]
    public class Class1Tests
    {
        [Test]
        public void aTest()
        {
            Assert.Fail(""autogenerated"");
        }
    }
}
namespace Namespace4.Tests
{
    [TestFixture]
    public class Class2Tests
    {
        [Test]
        public void aTest()
        {
            Assert.Fail(""autogenerated"");
        }
    }
}";
                Assert.AreEqual(testFile, expected);
            }
        }
    }
}
