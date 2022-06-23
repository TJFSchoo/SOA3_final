using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Backlog.BacklogItemStates
{
    public class ToDoState : IBacklogItemState
    {

        private readonly BacklogItem _backlogItem;
        private static string TAG = "TodoState";

        public ToDoState(BacklogItem backlogItem)
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

        public void SetName(string name)
        {
            _backlogItem.SetName(name);
        }
        public void SetDescription(string description)
        {
            _backlogItem.SetDescription(description);
        }

        public void SetEffort(int effort)
        {
            _backlogItem.SetEffort(effort);
        }

        public void NextState()
        {
            _backlogItem.ChangeState(new DoingState(_backlogItem));
        }

        public void PreviousState()
        {
            throw new NotSupportedException("[" + TAG + "] " + "There is no previous state.");
        }
    }
}
