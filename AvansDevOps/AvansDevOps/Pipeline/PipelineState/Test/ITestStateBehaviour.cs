using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Pipeline.PipelineStates.Test
{
    public interface ITestStateBehaviour
    {
        bool Execute();
        string GetErrorMessage();
    }
}
