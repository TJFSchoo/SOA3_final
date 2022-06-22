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

        public TestingState(BacklogItem backlogItem)
        {
            _backlogItem = backlogItem;
        }

        public void AddTask(Task task)
        {
            throw new NotSupportedException("Can't add more tasks when BacklogItem is being tested");

        }

        public void RemoveTask(Task task)
        {
            throw new NotSupportedException("Can't remove task when BacklogItem is being tested");

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
            // BR: Only allow done state when all tasks are done.

            foreach (var task in _backlogItem.GetTasks())
            {
                if (task.GetState() != ETaskState.Done)
                    throw new NotSupportedException(
                        "Can't set backlogItem state to done because not all tasks are done.");
            }

            _backlogItem.ChangeState(new TestingState(_backlogItem));
        }

        public void PreviousState()
        {
            // BR: When the Lead Dev checks the DOD and finds something wrong, the backlogItem goes back to ready to test and all testers are notified again.
            _backlogItem.ChangeState(new ReadyToTestState(_backlogItem));
        }
    }
}
