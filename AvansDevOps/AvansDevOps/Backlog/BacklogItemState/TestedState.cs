using AvansDevOps.Backlog.BacklogItemStates;
using AvansDevOps.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Backlog.BacklogItemState
{
    public class TestedState : IBacklogItemState
    {
        private readonly BacklogItem _backlogItem;

        public TestedState(BacklogItem backlogItem)
        {
            _backlogItem = backlogItem;
            PersonModel owner = backlogItem.GetBacklog().GetProject().GetProductOwner();

            owner.SendNotification($"Attention {owner.GetName()}, backlog item {backlogItem.GetDescription()} has been successfully tested.");
            

        }

        public void AddTask(Task task)
        {
            throw new NotSupportedException("[" + TAG + "] " + "Unable to add more tasks when backlog item is already tested.");
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
            _backlogItem.ChangeState(new DoneState(_backlogItem));
        }

        public void PreviousState()
        {
            // If a tester is not satisfied with the already-tested backlogItem, it can only go back to ToDo state, not to Doing state.
            // We also notify the scrum master
            var scrumMaster = _backlogItem.GetSprint().GetScrumMaster();
            scrumMaster.SendNotification($"Attention {scrumMaster.GetName()}, tester has assigned backlog item from Tested state to ToDo state. Please check.");
            _backlogItem.ChangeState(new TodoState(_backlogItem));
        }
    }
}
