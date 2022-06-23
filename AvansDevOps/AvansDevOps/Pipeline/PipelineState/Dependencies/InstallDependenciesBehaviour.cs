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
        private static string TAG = "InstallDependenciesBehaviour";

        public InstallDependenciesBehaviour(IPipeline pipeline)
        {
            _pipeline = pipeline;
        }

        public bool Execute()
        {
            Console.WriteLine("[" + TAG + "] " + "Executing.");
            return true;
        }

        public string GetErrorMessage()
        {
            return _errors;
        }
    }
}
