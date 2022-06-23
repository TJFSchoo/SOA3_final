using AvansDevOps.Pipeline.PipelineState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Pipeline.PipelineStates.Analyze
{
    public class AnalyzeBehaviour : IStateBehaviour
    {
        private readonly IPipeline _pipeline;
        private readonly string _errors;

        public AnalyzeBehaviour(IPipeline pipeline)
        {
            _pipeline = pipeline;
        }

        public bool Execute()
        {
            // Stub execution step
            Console.WriteLine("Analyze pipeline step run");
            return true;
        }

        public string GetErrorMessage()
        {
            return _errors;
        }
    }
}
