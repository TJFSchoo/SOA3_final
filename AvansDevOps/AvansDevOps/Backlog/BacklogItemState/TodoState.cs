using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Backlog.BacklogItemStates
{
    public class TodoState : IBacklogItemState
    {

        private readonly BacklogItem _backlogItem;
        public TodoState(BacklogItem backlogItem)
        {
            _backlogItem = backlogItem;
        }

        public void AddTask(Task task)
        {
            _backlogItem.AddTask(task);
        }

        public void RemoveTask(Task task)
        {
            _backlogItem.RemoveTask(task);
        }

        public void SetName(string newName)
        {
            _backlogItem.SetName(newName);
        }
        public void SetDescription(string description)
        {
            _backlogItem.SetDescription(description);
        }

        public void SetEffort(int newEffort)
        {
            _backlogItem.SetEffort(newEffort);
        }

        public void NextState()
        {
            _backlogItem.ChangeState(new DoingState(_backlogItem));
        }

        public void PreviousState()
        {
            throw new NotSupportedException("Can't change TodoState. There is no previous state");
        }
    }
}
