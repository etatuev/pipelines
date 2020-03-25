using System.Threading.Tasks;

namespace Pipelines
{
    public abstract class PipelineStep<T, TOut> : IPipelineStep<T, TOut>
    {
        public abstract Task<TOut> ExecuteAsync(T input);
        public async Task<object> ExecuteAsync(object input) => await ExecuteAsync((T) input);
    }
}