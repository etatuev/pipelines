using System;
using System.Threading.Tasks;
using Pipelines.Tests.Stubs;
using Xunit;
using Xunit.Abstractions;

namespace Pipelines.Tests
{
    
    public class ExampleTest
    {
        private readonly ITestOutputHelper _console;

        public ExampleTest(ITestOutputHelper console)
        {
            _console = console;
        }

        [Fact]
        public async Task Example()
        {
            
            
            T LogStep<T>(T prev)
            {
                _console.WriteLine(prev.ToString());
                return prev;
            }

            Pipeline<string,int> stringLengthPipeline = PipelineBuilder.Create<string>()
                .AddStep(new ConcatStringPipelineStep(", let's count "))
                .AddStep(new ConcatStringPipelineStep("this string length!"))
                .AddStep(LogStep)
                .AddStep(new StringLengthPipelineStep())
                .Build();


            Pipeline<string,string> pipeline = PipelineBuilder
                .Create<string>()
                .AddStep(LogStep)
                .AddStep(stringLengthPipeline)
                .AddStep(length => $"So, result is {length}")
                .Build();
            
            string result = await pipeline.ExecuteAsync("Hello");
            
            _console.WriteLine(result);

            // Assert
            Assert.Equal("So, result is 38", result);
        }
    }
}