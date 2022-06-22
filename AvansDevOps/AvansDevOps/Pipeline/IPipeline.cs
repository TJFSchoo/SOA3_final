using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Pipeline.PipelineStates;

namespace AvansDevOps.Pipeline
{
    public interface IPipeline
    {
        void SetState(IPipelineState state);
        void Execute();
        void OnError();
        void OnSuccess();

    }
}
