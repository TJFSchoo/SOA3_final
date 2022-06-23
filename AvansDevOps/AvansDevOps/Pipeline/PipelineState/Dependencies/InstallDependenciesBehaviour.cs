using AvansDevOps.Pipeline.PipelineState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Pipeline.PipelineStates.Dependencies
{
    public class InstallDependenciesBehaviour : IStateBehaviour
    {
        private readonly IPipeline _pipeline;
        private string _errors;

        public InstallDependenciesBehaviour(IPipeline pipeline)
        {
            _pipeline = pipeline;
        }

        public bool Execute()
        {
            // Stub execution step
            Console.WriteLine("Install Dependencies pipeline step run");
            return true;
        }

        public string GetErrorMessage()
        {
            return _errors;
        }
    }
}
