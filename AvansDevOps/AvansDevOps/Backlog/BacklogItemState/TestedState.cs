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

            // BacklogItem is tested, notify owner.

            PersonModel owner = backlogItem.GetBacklog().GetProject().GetProductOwner();

            owner.SendNotification($"Hello owner {owner.GetName()}, BacklogItem {backlogItem.GetDescription()} is tested");
            

        }

        public void AddTask(Task task)
        {
            throw new NotSupportedException("Can't add more tasks when BacklogItem is already tested");
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
            // BR: If a tester is not satisfied with the already-tested backlogItem, it can only go back to To-do, not to doing.
            // BR: We also notify the scrum master
            var scrumMaster = _backlogItem.GetSprint().GetScrumMaster();
            scrumMaster.SendNotification($"Hello {scrumMaster.GetName()}, your devs have messed up. Please check in with testers");
            _backlogItem.ChangeState(new TodoState(_backlogItem));
        }
    }
}
