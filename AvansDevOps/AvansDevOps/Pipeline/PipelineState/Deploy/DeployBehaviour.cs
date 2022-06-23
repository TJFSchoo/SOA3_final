using System;
using System.Collections.Generic;
using AvansDevOps.Pipeline.PipelineState;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Pipeline.PipelineStates.Deploy
{
    public class DeployBehaviour : IStateBehaviour
    {
        private readonly IPipeline _pipeline;
        private string _errors;
        private static string TAG = "DeployBehaviour";

        public DeployBehaviour(IPipeline pipeline)
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
