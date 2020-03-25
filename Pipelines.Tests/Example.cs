using System;
using System.Threading.Tasks;
using Pipelines.Tests.Stubs;
using Xunit;
using Xunit.Abstractions;

namespace Pipelines.Tests
{
    public class Example
    {
        // So example can be easily copy-pasted
        // ReSharper disable once InconsistentNaming
        private readonly ITestOutputHelper Console;

        public Example(ITestOutputHelper console)
        {
            Console = console;
        }

        [Fact]
        public async Task ExampleTest()
        {
            T LogStep<T>(T prev)
            {
                Console.WriteLine(prev.ToString());
                return prev;
            }

            Pipeline<string, int> stringLengthPipeline = PipelineBuilder
                .Create<string>()
                .AddStep(new ConcatStringPipelineStep(", let's count "))
                .AddStep(new ConcatStringPipelineStep("this string length!"))
                .AddStep(LogStep)
                .AddStep(new StringLengthPipelineStep())
                .Build();

            Pipeline<string, string> pipeline = PipelineBuilder
                .Create<string>()
                .AddStep(LogStep)
                .AddStep(stringLengthPipeline)
                .AddStep(length => $"So, result is {length}")
                .Build();

            string result = await pipeline.ExecuteAsync("Hello");

            Console.WriteLine(result);

            Assert.Equal("So, result is 38", result);
        }
    }
}