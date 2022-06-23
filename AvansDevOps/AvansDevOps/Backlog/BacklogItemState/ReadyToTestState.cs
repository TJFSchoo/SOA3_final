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
        private static string TAG = "ReadyToTestState";

        public ReadyToTestState(BacklogItem backlogItem)
        {
            _backlogItem = backlogItem;

            foreach (var tester in backlogItem.GetBacklog().GetProject().GetTesters())
            {
                tester.SendNotification($"Attention {tester.GetName()}, backlog item {backlogItem.GetDescription()} is in Ready to Test state.");
            }

        }

        public void AddTask(Task task)
        {
            throw new NotSupportedException("[" + TAG + "] " + "Unable to add more tasks when backlog item is in Ready to Test state.");
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
            // Only allow ready to test state when all tasks are active or done
            foreach (var task in _backlogItem.GetTasks())
            {
                if (task.GetState() != ETaskState.Done)
                {
                    throw new NotSupportedException(
                        "[" + TAG + "] " + "Backlog item is unable to go to Testing state due to existing tasks in ToDo or Active state.");
                }
            }

            _backlogItem.ChangeState(new TestingState(_backlogItem));
        }

        public void PreviousState()
        {
            // If a tester is not satisfied with the backlog item, it can only go back to ToDo state.
            // We also notify the scrum master
            var scrumMaster = _backlogItem.GetSprint().GetScrumMaster();
            scrumMaster.SendNotification($"Attention {scrumMaster.GetName()}, tester has assigned backlog item from Ready to Test state to ToDo state. Please check.");
            _backlogItem.ChangeState(new ToDoState(_backlogItem));
        }
    }
}
