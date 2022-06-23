using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Pipeline.PipelineState
{
    internal interface IStateBehaviour
    {
        bool Execute();
        string GetErrorMessage();
    
    }
}
