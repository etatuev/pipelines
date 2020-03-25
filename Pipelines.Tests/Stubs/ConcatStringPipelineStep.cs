using System.Threading.Tasks;

namespace Pipelines.Tests.Stubs
{
    internal class ConcatStringPipelineStep : PipelineStep<string, string>
    {
        private readonly string _argument;

        public ConcatStringPipelineStep(string argument)
        {
            _argument = argument;
        }

        public override Task<string> ExecuteAsync(string input)
        {
            return Task.FromResult($"{input}{_argument}");
        }
    }
}