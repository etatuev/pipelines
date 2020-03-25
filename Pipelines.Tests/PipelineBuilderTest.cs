using System.Linq;
using System.Threading.Tasks;
using Pipelines.Tests.Stubs;
using Xunit;

namespace Pipelines.Tests
{
    public class PipelineBuilderTest
    {
        [Fact]
        public void AddStep_ShouldAddStep()
        {
            // Arrange
            var step1 = new ConcatStringPipelineStep("Hey");
            var builder = PipelineBuilder.Create<string>();

            // Act
            builder.AddStep(step1);

            // Assert
            Assert.Equal(1, builder.Steps.Count);
            Assert.Equal(step1, builder.Steps.First());
        }

        [Fact]
        public void AddStep_WhenCalledMultipleTimes_ShouldReplaceLastStep()
        {
            // Arrange
            var step1 = new ConcatStringPipelineStep("Hey");
            var step2 = new ConcatStringPipelineStep("Hey 2");
            PipelineBuilder<string, string> builder = PipelineBuilder.Create<string>();

            // Act
            builder.AddStep(step1);
            builder.AddStep(step2);

            // Assert
            Assert.Equal(1, builder.Steps.Count);
            Assert.Equal(step2, builder.Steps.First());
        }

        [Fact]
        public void AddStep_WhenCallsChained_ShouldForwardStepsToNextBuilder()
        {
            // Arrange
            var step1 = new ConcatStringPipelineStep("Hey");
            PipelineBuilder<string, string> builder = PipelineBuilder.Create<string>();

            // Act
            builder = builder.AddStep(step1);

            // Assert
            Assert.Equal(1, builder.Steps.Count);
            Assert.Equal(step1, builder.Steps.First());
        }

        [Fact]
        public void AddStep_WhenCallsChained_CanReuseSameSteps()
        {
            // Arrange
            var step1 = new ConcatStringPipelineStep("Hey");
            PipelineBuilder<string, string> builder = PipelineBuilder.Create<string>();

            // Act
            builder = builder
                .AddStep(step1)
                .AddStep(step1)
                .AddStep(step1);

            // Assert
            Assert.Equal(3, builder.Steps.Count);
        }

        [Fact]
        public void AddStep_WhenFuncPassed_ShouldAddDelegateStep()
        {
            // Arrange
            var builder = PipelineBuilder.Create<string>();

            // Act
            builder = builder
                .AddStep(arg => arg + "sync")
                .AddStep(async arg => await Task.FromResult(arg + "async"));

            // Assert
            Assert.IsType<DelegatePipelineStep<string, string>>(builder.Steps[0]);
            Assert.IsType<AsyncDelegatePipelineStep<string, string>>(builder.Steps[1]);
        }

        [Fact]
        public void Build_ShouldCreatePipelineWithCurrentSteps()
        {
            // Arrange
            PipelineBuilder<string, string> builder = PipelineBuilder.Create<string>()
                .AddStep(arg => arg + "sync")
                .AddStep(async arg => await Task.FromResult(arg + "async"));

            // Act
            Pipeline<string, string> pipeline = builder.Build();

            // Assert
            Assert.IsType<DelegatePipelineStep<string, string>>(pipeline.Steps[0]);
            Assert.IsType<AsyncDelegatePipelineStep<string, string>>(pipeline.Steps[1]);
        }

        [Fact]
        public void Build_WhenStepReplaced_WillNotReflectChanges()
        {
            // Arrange
            var builder = PipelineBuilder.Create<string>();
            builder.AddStep(arg => "string");
            Pipeline<string, string> pipeline = builder.Build();

            // Act
            // ReSharper disable once MustUseReturnValue
            builder.AddStep(_ => 0);

            // Assert
            Assert.IsType<DelegatePipelineStep<string, string>>(pipeline.Steps.First());
            Assert.IsType<DelegatePipelineStep<string, int>>(builder.Steps.First());
        }
    }
}