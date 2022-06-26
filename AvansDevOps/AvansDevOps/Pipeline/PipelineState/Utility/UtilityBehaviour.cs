using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Pipeline.PipelineState;

namespace AvansDevOps.Pipeline.PipelineStates.Utility
{
    public class UtilityBehaviour : IStateBehaviour
    {
        private readonly IPipeline _pipeline;
        private string _errors;
        private static string TAG = "UtilityBehaviour";

        public UtilityBehaviour(IPipeline pipeline)
        {
            _pipeline = pipeline;
        }

        public bool Execute()
        {
            Console.WriteLine("[" + TAG + "] " + "Utility pipeline step run");
            return true;
        }

        public string GetErrorMessage()
        {
            return _errors;
        }
    }
}
