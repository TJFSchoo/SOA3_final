using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Pipeline.PipelineStates.Source
{
    public interface ISourceStateBehaviour
    {
        bool Execute();
        string GetErrorMessage();
    }
}
