using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Pipeline.PipelineState;

namespace AvansDevOps.Pipeline.PipelineStates.Test
{
    public class TestBehaviour : IStateBehaviour
    {
        private readonly IPipeline _pipeline;
        private string _errors;
        private static string TAG = "TestBehaviour";

        public TestBehaviour(IPipeline pipeline)
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
