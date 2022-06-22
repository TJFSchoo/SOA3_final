using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Pipeline.PipelineStates.Utility
{
    public interface IUtilityStateBehaviour
    {
        bool Execute();
        string GetErrorMessage();
    }
}
