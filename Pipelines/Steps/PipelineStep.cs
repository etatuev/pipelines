using System;
using System.Threading.Tasks;

namespace Pipelines
{
    public static class PipelineStep
    {
        public static IPipelineStep<T, TOut> From<T, TOut> (Func<T, TOut> func)
        {
            return new DelegatePipelineStep<T, TOut>(func);
        }
        
        public static IPipelineStep<T, TOut> From<T, TOut> (Func<T, Task<TOut>> func)
        {
            return new AsyncDelegatePipelineStep<T, TOut>(func);
        }
    }
    
    public abstract class PipelineStep<T, TOut> : IPipelineStep<T, TOut>
    {
        public abstract Task<TOut> ExecuteAsync(T input);
        public async Task<object> ExecuteAsync(object input) => await ExecuteAsync((T) input);
    }
}