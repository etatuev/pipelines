using System.Threading.Tasks;

namespace Pipelines
{
    public interface IPipelineStep
    {
        Task<object> ExecuteAsync(object input);
    }
    
    public interface IPipelineStep<in T, TOut> : IPipelineStep
    {
        Task<TOut> ExecuteAsync(T input);
    }
}