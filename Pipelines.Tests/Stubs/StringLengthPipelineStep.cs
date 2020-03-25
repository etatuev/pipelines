using System.Threading.Tasks;

namespace Pipelines.Tests.Stubs
{
    internal class StringLengthPipelineStep : PipelineStep<string, int>
    {
        public override Task<int> ExecuteAsync(string input)
        {
            return Task.FromResult(input.Length);
        }
    }
}