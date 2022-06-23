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

        public DeployBehaviour(IPipeline pipeline)
        {
            _pipeline = pipeline;
        }

        public bool Execute()
        {
            // Stub execution step
            Console.WriteLine("Deploy pipeline step run");
            return true;
        }

        public string GetErrorMessage()
        {
            return _errors;
        }
    }
}
