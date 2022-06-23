using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Pipeline.PipelineStates.Deploy;
using AvansDevOps.Pipeline.PipelineState;

namespace AvansDevOps.Pipeline.PipelineStates.Analyze
{
    public class AnalyzeState : IPipelineState
    {

        private readonly IPipeline _pipeline;
        private readonly IStateBehaviour _behaviour;
        private static string TAG = "AnalyzeState";
        public AnalyzeState(IPipeline pipeline)
        {
            _pipeline = pipeline;
            _behaviour = new AnalyzeBehaviour(_pipeline);
        }

        public void Execute()
        {
            var pipelineResult = _behaviour.Execute();

            if (!pipelineResult || _behaviour.GetErrorMessage() != null)
                throw new Exception("[" + TAG + "] " + $"Pipeline failed: {_behaviour.GetErrorMessage()} ");

            this.NextState();
        }

        public void NextState()
        {
            _pipeline.SetState(new DeployState(_pipeline));
        }
    }
}
