using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Pipeline.PipelineStates.Dependencies;

namespace AvansDevOps.Pipeline.PipelineStates.Source
{
    public class SourceState : IPipelineState
    {

        private readonly IPipeline _pipeline;
        private readonly ISourceStateBehaviour _behaviour;
        public SourceState(IPipeline pipeline)
        {
            _pipeline = pipeline;
            _behaviour = new SourceBehaviour(_pipeline);
        }

        public void Execute()
        {
            var pipelineResult = _behaviour.Execute();

            if (!pipelineResult || _behaviour.GetErrorMessage() != null)
                throw new NotSupportedException($"Pipeline failed: SourceState: {_behaviour.GetErrorMessage()} ");

            this.NextState();
        }

        public void NextState()
        {
            _pipeline.SetState(new InstallDependenciesState(_pipeline));
        }
    }
}
