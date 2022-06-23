using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Pipeline.PipelineStates;
using AvansDevOps.Pipeline.PipelineStates.Source;
using AvansDevOps.Sprint;

namespace AvansDevOps.Pipeline
{
    public class TestPipeline : IPipeline
    {
        private readonly ISprint _sprint;
        private readonly EPipelineConfig _config;
        private IPipelineState _state;

        public TestPipeline(ISprint sprint, EPipelineConfig config)
        {
            _sprint = sprint;
            _config = config;
            _state = new SourceState(this);
        }

        public void SetState(IPipelineState state)
        {
            _state = state;
        }

        public void Execute()
        {
            if (_config == EPipelineConfig.Manual)
                _state.Execute();
            while (_state.GetType().Name != "UtilityState")
            {
                _state.Execute();
            }

        }

        public void OnError()
        {
            // Pipeline failed, send notification
            _sprint.GetScrumMaster().SendNotification($"Attention {_sprint.GetScrumMaster().GetName()}, Test Pipeline for {_sprint.GetName()} failed.");
        }

        public void OnSuccess()
        {
            // Pipeline succeeded, send notification
            _sprint.GetScrumMaster().SendNotification($"Attention {_sprint.GetScrumMaster().GetName()}, Test Pipeline for {_sprint.GetName()} finished.");
        }
    }
}
