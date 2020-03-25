using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Pipelines
{
    public static class PipelineBuilder
    {
        public static PipelineBuilder<T, T> Create<T>()
        {
            return new PipelineBuilder<T, T>();
        }
    }
    
    /// <summary>
    /// Sequential pipeline builder.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public class PipelineBuilder<T, TOut>
    {
        private readonly List<IPipelineStep> _steps;
        private bool _inserted;

        public PipelineBuilder()
        {
            _steps = new List<IPipelineStep>();
        }

        public PipelineBuilder([NotNull] IEnumerable<IPipelineStep> steps)
        {
            _steps = new List<IPipelineStep>(steps);
        }

        public IReadOnlyList<IPipelineStep> Steps => _steps.AsReadOnly();

        /// <summary>
        /// Add step to the sequence, and return new instance of a <see cref="PipelineBuilder{T,TOut}"/>.
        /// </summary>
        /// <param name="stepFunc">The work to execute.</param>
        /// <typeparam name="TNextOut"></typeparam>
        /// <returns>New instance of a <see cref="PipelineBuilder{T,TOut}"/></returns>
        [PublicAPI]
        [MustUseReturnValue("It's designed to be used in a \"chain\" way, e.g. 'builder.AddStep(..).AddStep(..)'")]
        public PipelineBuilder<T, TNextOut> AddStep<TNextOut>(Func<TOut, TNextOut> stepFunc)
        {
            return AddStep(PipelineStep.From(stepFunc));
        }

        /// <summary>
        /// Add step to the sequence, and return new instance of a <see cref="PipelineBuilder{T,TOut}"/>.
        /// </summary>
        /// <param name="stepFunc">The work to execute.</param>
        /// <typeparam name="TNextOut"></typeparam>
        /// <returns>New instance of a <see cref="PipelineBuilder{T,TOut}"/></returns>
        [PublicAPI]
        [MustUseReturnValue("It's designed to be used in a \"chain\" way, e.g. 'builder.AddStep(..).AddStep(..)'")]
        public PipelineBuilder<T, TNextOut> AddStep<TNextOut>(Func<TOut, Task<TNextOut>> stepFunc)
        {
            return AddStep(PipelineStep.From(stepFunc));
        }

        /// <summary>
        /// Add step to the sequence, and return new instance of a <see cref="PipelineBuilder{T,TOut}"/>.
        /// </summary>
        /// <param name="pipelineStep">The work to execute.</param>
        /// <typeparam name="TNextOut"></typeparam>
        /// <returns>New instance of a <see cref="PipelineBuilder{T,TOut}"/></returns>
        [PublicAPI]
        [MustUseReturnValue("It's designed to be used in a \"chain\" way, e.g. 'builder.AddStep(..).AddStep(..)'")]
        public PipelineBuilder<T, TNextOut> AddStep<TNextOut>(IPipelineStep<TOut, TNextOut> pipelineStep)
        {
            if (!_inserted)
            {
                _steps.Add(pipelineStep);
                _inserted = true;
            }
            else
            {
                _steps[_steps.Count - 1] = pipelineStep;
            }

            return new PipelineBuilder<T, TNextOut>(Steps);
        }

        /// <summary>
        /// Finalize build, and prepare executable pipeline.
        /// </summary>
        /// <returns></returns>
        public Pipeline<T, TOut> Build()
        {
            return new Pipeline<T, TOut>(Steps);
        }
    }
}