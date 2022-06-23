using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Pipeline.PipelineState;
using AvansDevOps.Pipeline.PipelineStates.Build;

namespace AvansDevOps.Pipeline.PipelineStates.Dependencies
{
    public class InstallDependenciesState : IPipelineState
    {

        private readonly IPipeline _pipeline;
        private readonly IStateBehaviour _behaviour;
        private static string TAG = "InstallDependenciesState";
        public InstallDependenciesState(IPipeline pipeline)
        {
            _pipeline = pipeline;
            _behaviour = new InstallDependenciesBehaviour(_pipeline);
        }

        public void Execute()
        {
            var pipelineResult = _behaviour.Execute();

            if (!pipelineResult || _behaviour.GetErrorMessage() != null)
                throw new NotSupportedException("[" + TAG + "] " + $"Pipeline failed: {_behaviour.GetErrorMessage()} ");

            this.NextState();
        }

        public void NextState()
        {
            _pipeline.SetState(new BuildState(_pipeline));
        }
    }
}
