using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Backlog.BacklogItemStates
{
    public interface IBacklogItemState
    {
        void AddTask(Task task);
        void RemoveTask(Task task);
        void SetName(string name);
        void SetDescription(string description);
        void SetEffort(int effort);
        void NextState();
        void PreviousState();
    }
}
