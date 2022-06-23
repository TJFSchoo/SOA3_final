using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Pipeline.PipelineState;

namespace AvansDevOps.Pipeline.PipelineStates.Utility
{
    public class UtilityState : IPipelineState
    {

        private readonly IPipeline _pipeline;
        private readonly IStateBehaviour _behaviour;
        public UtilityState(IPipeline pipeline)
        {
            _pipeline = pipeline;
            _behaviour = new UtilityBehaviour(_pipeline);
        }

        public void Execute()
        {
            var pipelineResult = _behaviour.Execute();

            if (!pipelineResult || _behaviour.GetErrorMessage() != null)
                throw new NotSupportedException($"Pipeline failed: SourceState: {_behaviour.GetErrorMessage()} ");

            
        }

        public void NextState()
        {
            throw new NotSupportedException("Pipeline is already at the latest step");
        }
    }
}
