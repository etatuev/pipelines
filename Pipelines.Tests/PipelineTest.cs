using System.Collections.Generic;
using System.Threading.Tasks;
using Pipelines.Tests.Stubs;
using Xunit;
using Xunit.Abstractions;

namespace Pipelines.Tests
{
    public class PipelineTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public PipelineTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }


        [Fact]
        public async Task ExecuteAsync_WhenSeveralSteps_ForwardResultToNextStep()
        {
            // Arrange
            var pipeline = new Pipeline<string, string>(new List<IPipelineStep>
            {
                new ConcatStringPipelineStep("123"),
                new ConcatStringPipelineStep("456")
            });

            // Act
            string result = await pipeline.ExecuteAsync(string.Empty);

            // Assert
            Assert.Equal("123456", result);
        }

        [Fact]
        public async Task ExecuteAsync_WhenStepsHasDifferentInputOutputTypes_ForwardResultToNextStep()
        {
            // Arrange
            var pipeline = new Pipeline<string, int>(new List<IPipelineStep>
            {
                new ConcatStringPipelineStep("123"),
                new StringLengthPipelineStep()
            });

            // Act
            int totalStringLength = await pipeline.ExecuteAsync(string.Empty);
            _testOutputHelper.WriteLine(totalStringLength.ToString());

            // Assert
            Assert.Equal(3, totalStringLength);
        }

        [Fact]
        public async Task ExecuteAsync_ForwardArgumentToFirstStepAsInput()
        {
            // Arrange
            var pipeline = new Pipeline<string, string>(new List<IPipelineStep>
            {
                new ConcatStringPipelineStep("received!"),
            });

            // Act
            string result = await pipeline.ExecuteAsync("Argument ");

            // Assert
            Assert.Equal("Argument received!", result);
        }

        [Fact]
        public async Task ExecuteAsync_WhenPipelineStepExists_WillExecuteAllSubSteps()
        {
            // Arrange
            var subPipeline = new Pipeline<string, string>(new List<IPipelineStep>
            {
                new ConcatStringPipelineStep("123"),
                new ConcatStringPipelineStep("456")
            });
            
            var pipeline = new Pipeline<string, string>(new List<IPipelineStep>
            {
                new ConcatStringPipelineStep("will be ["),
                subPipeline,
                new ConcatStringPipelineStep("] !")
            });
            
            // Act
            string result = await pipeline.ExecuteAsync("Numbers ");

            // Assert
            Assert.Equal("Numbers will be [123456] !", result);
        }
    }
}