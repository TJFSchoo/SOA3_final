using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Pipeline.PipelineStates.Build;

namespace AvansDevOps.Pipeline.PipelineStates.Dependencies
{
    public class InstallDependenciesState : IPipelineState
    {

        private readonly IPipeline _pipeline;
        private readonly IInstallDependenciesStateBehaviour _behaviour;
        public InstallDependenciesState(IPipeline pipeline)
        {
            _pipeline = pipeline;
            _behaviour = new InstallDependenciesBehaviour(_pipeline);
        }

        public void Execute()
        {
            var pipelineResult = _behaviour.Execute();

            if (!pipelineResult || _behaviour.GetErrorMessage() != null)
                throw new NotSupportedException($"Pipeline failed: Install Dependencies State: {_behaviour.GetErrorMessage()} ");

            this.NextState();
        }

        public void NextState()
        {
            _pipeline.SetState(new BuildState(_pipeline));
        }
    }
}
