using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Backlog.BacklogItemStates
{
    public class ReadyToTestState : IBacklogItemState
    {
        private readonly BacklogItem _backlogItem;

        public ReadyToTestState(BacklogItem backlogItem)
        {
            _backlogItem = backlogItem;

            // BacklogItem is ready to test, notify testers.
            foreach (var tester in backlogItem.GetBacklog().GetProject().GetTesters())
            {
                tester.SendNotification($"Hello tester {tester.GetName()}, BacklogItem {backlogItem.GetDescription()} is ready to test");
            }

        }

        public void AddTask(Task task)
        {
            throw new NotSupportedException("Can't add more tasks when BacklogItem is ready to test");
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
            // BR: Only allow ready to test state when all tasks are active or done
            foreach (var task in _backlogItem.GetTasks())
            {
                if (task.GetState() != ETaskState.Done)
                {
                    throw new NotSupportedException(
                        "Backlog item can't go to testing because there are still tasks with todo or active status");
                }
            }

            _backlogItem.ChangeState(new TestingState(_backlogItem));
        }

        public void PreviousState()
        {
            // BR: If a tester is not satisfied with the backlogItem, it can only go back to To-do, not to doing.
            // BR: We also notify the scrum master
            var scrumMaster = _backlogItem.GetSprint().GetScrumMaster();
            scrumMaster.SendNotification($"Hello {scrumMaster.GetName()}, your devs have messed up. Please check in with testers");
            _backlogItem.ChangeState(new TodoState(_backlogItem));
        }
    }
}
