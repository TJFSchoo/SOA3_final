using System;
using AvansDevOps.Pipeline.PipelineState;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Pipeline.PipelineStates.Build
{
    public class BuildBehaviour : IStateBehaviour
    {
        private readonly IPipeline _pipeline;
        private string _errors;
        private static string TAG = "BuildBehaviour";

        public BuildBehaviour(IPipeline pipeline)
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
