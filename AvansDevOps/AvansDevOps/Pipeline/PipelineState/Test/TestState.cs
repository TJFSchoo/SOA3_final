using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Pipeline.PipelineStates.Analyze;
using AvansDevOps.Pipeline.PipelineState;

namespace AvansDevOps.Pipeline.PipelineStates.Test
{
    public class TestState : IPipelineState
    {

        private readonly IPipeline _pipeline;
        private readonly IStateBehaviour _behaviour;
        public TestState(IPipeline pipeline)
        {
            _pipeline = pipeline;
            _behaviour = new TestBehaviour(_pipeline);
        }

        public void Execute()
        {
            var pipelineResult = _behaviour.Execute();

            if (!pipelineResult || _behaviour.GetErrorMessage() != null)
                throw new NotSupportedException($"Pipeline failed: TestState: {_behaviour.GetErrorMessage()} ");

            this.NextState();
        }

        public void NextState()
        {
            _pipeline.SetState(new AnalyzeState(_pipeline));
        }
    }
}
