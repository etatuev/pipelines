using System;
using System.Threading.Tasks;

namespace Pipelines
{
    public class DelegatePipelineStep<T, TOut> : PipelineStep<T, TOut>
    {
        private readonly Func<T, TOut> _func;

        public DelegatePipelineStep(Func<T, TOut> func)
        {
            _func = func;
        }

        public override Task<TOut> ExecuteAsync(T input)
        {
            return Task.FromResult(_func(input));
        }
    }
}