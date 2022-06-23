using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Backlog.BacklogItemStates
{
    public class TestingState : IBacklogItemState
    {
        private readonly BacklogItem _backlogItem;
        private static string TAG = "TestingState";

        public TestingState(BacklogItem backlogItem)
        {
            _backlogItem = backlogItem;
        }

        public void AddTask(Task task)
        {
            throw new NotSupportedException("[" + TAG + "] " + "Can't add more tasks when BacklogItem is being tested");

        }

        public void RemoveTask(Task task)
        {
            throw new NotSupportedException("[" + TAG + "] " + "Can't remove task when BacklogItem is being tested");

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
            foreach (var task in _backlogItem.GetTasks())
            {
                if (task.GetState() != ETaskState.Done)
                    throw new NotSupportedException(
                        "[" + TAG + "] " + "Can't set backlogItem state to done because not all tasks are done.");
            }

            _backlogItem.ChangeState(new TestingState(_backlogItem));
        }

        public void PreviousState()
        {
            // If the lead developer is not satisfied, the backlog item goes back to Ready to Test state and all testers are notified.
            _backlogItem.ChangeState(new ReadyToTestState(_backlogItem));
        }
    }
}
