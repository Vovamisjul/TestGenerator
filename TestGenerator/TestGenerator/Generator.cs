using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TestGenerator
{
    public class Generator
    {
        private List<string> _testables;
        private string _outFolder;
        private int _maxFileToRead;
        private int _maxFileToWrite;
        private int _maxThreads;
        public Generator(List<string> testables, string outFolder, int maxFileToRead, int maxFileToWrite, int maxThreads)
        {
            _testables = testables;
            _outFolder = outFolder;
            _maxFileToRead = maxFileToRead;
            _maxFileToWrite = maxFileToWrite;
            _maxThreads = maxThreads;
        }
        public void Generate()
        {
            ExecutionDataflowBlockOptions maxFilesToLoadTasks = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = _maxFileToRead
            };

            ExecutionDataflowBlockOptions maxTasksExecutedTasks = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = _maxThreads
            };

            ExecutionDataflowBlockOptions maxFilesToWriteTasks = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = _maxFileToWrite
            };

            var loadFiles = new TransformBlock<string, CSFile>(
               new Func<string, Task<CSFile>>(LoadTextFromFile), maxFilesToLoadTasks);

            var getTestClasses = new TransformBlock<CSFile, CSFile>(
               new Func<CSFile, Task<CSFile>>(GetTestClass), maxTasksExecutedTasks);

            var writeResult = new ActionBlock<CSFile>(async input =>
            {
                await WriteResult(input);
            }, maxFilesToWriteTasks);

            loadFiles.LinkTo(getTestClasses, new DataflowLinkOptions() { PropagateCompletion = true });
            getTestClasses.LinkTo(writeResult, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (var testClass in _testables)
            {
                loadFiles.Post(testClass);
            }
            loadFiles.Complete();

            loadFiles.Completion.Wait();

            getTestClasses.Complete();

            getTestClasses.Completion.Wait();

            writeResult.Complete();

            writeResult.Completion.Wait();
        }

        public async Task GenerateAsync()
        {
            await Task.Run((Action)Generate);
        }
        private async Task<CSFile> LoadTextFromFile(string inputFile)
        {
            using (var reader = new StreamReader(new FileStream(inputFile, FileMode.Open)))
                return new CSFile(inputFile, await reader.ReadToEndAsync());
        }

        private async Task<CSFile> GetTestClass(CSFile file)
        {
            return await new MainGenerator().CreateTest(file);
        }

        private async Task WriteResult(CSFile testClass)
        {
            using (var writer = new StreamWriter(new FileStream(_outFolder + Path.DirectorySeparatorChar + testClass.FileName, FileMode.Create)))
                await writer.WriteAsync(testClass.Text);
        }
    }
}
