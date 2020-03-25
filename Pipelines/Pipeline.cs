using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Pipelines
{
    public class Pipeline<T, TOut> : PipelineStep<T, TOut>
    {
        private readonly List<IPipelineStep> _steps;

        [PublicAPI]
        public IReadOnlyList<IPipelineStep> Steps => _steps.AsReadOnly();
        
        public Pipeline([NotNull] IEnumerable<IPipelineStep> steps)
        {
            _steps = new List<IPipelineStep>(steps);
        }
        
        /// <summary>
        /// Execute all steps
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<TOut> ExecuteAsync(T input)
        {
            object payload = input;
            foreach (IPipelineStep step in Steps)
            {
                payload = await step.ExecuteAsync(payload);
            }

            return (TOut) payload;
        }
    }
}