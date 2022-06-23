using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Backlog.BacklogItemStates
{
    public class DoingState : IBacklogItemState
    {
        private readonly BacklogItem _backlogItem;
        private static string TAG = "DoingState";

        public DoingState(BacklogItem backlogItem)
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
            // Only allow ready to test state when all tasks are active or done
            foreach (var task in _backlogItem.GetTasks())
            {
                if (task.GetState() == ETaskState.Todo)
                {
                    throw new NotSupportedException(
                        "[" + TAG + "] " + "Backlog item can't go to Ready to test because there are still tasks with todo status");
                }
            }

            _backlogItem.ChangeState(new ReadyToTestState(_backlogItem));
        }

        public void PreviousState()
        {
            _backlogItem.ChangeState(new ToDoState(_backlogItem));
        }
    }
}
