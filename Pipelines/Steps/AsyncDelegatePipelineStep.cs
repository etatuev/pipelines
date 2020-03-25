using System;
using System.Threading.Tasks;

namespace Pipelines
{
    public class AsyncDelegatePipelineStep<T, TOut> : PipelineStep<T, TOut>
    {
        private readonly Func<T, Task<TOut>> _asyncFunc;

        public AsyncDelegatePipelineStep(Func<T, Task<TOut>> asyncFunc)
        {
            _asyncFunc = asyncFunc;
        }

        public override Task<TOut> ExecuteAsync(T input)
        {
            return _asyncFunc(input);
        }
    }
}