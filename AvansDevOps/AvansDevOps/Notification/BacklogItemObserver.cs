using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Backlog;
using AvansDevOps.Person;

namespace AvansDevOps.Notification
{
    public class BacklogItemObserver : IObserver
    {
        public void Update(NotificationSubject subject)
        {
            if (!(subject is BacklogItem backlogItem)) return;

            switch (backlogItem.GetState().GetType().Name)
            {
                case "TodoState":
                    TodoState(backlogItem);
                    break;

                case "DoingState":
                    DoingState(backlogItem);
                    break;

                case "ReadyToTestState":
                    ReadyToTestState(backlogItem);
                    break;

                case "TestingState":
                    TestingState(backlogItem);
                    break;

                case "DoneState":
                    DoneState(backlogItem);
                    break;
            }
        }

        private void TodoState(BacklogItem backlogItem)
        {
            var scrumMaster = backlogItem.GetSprint().GetScrumMaster();

            scrumMaster.SendNotification($"Hello scrum master {scrumMaster.GetName()}, BacklogItem {backlogItem.GetDescription()} is in ToDo state.");
        }

        private void DoingState(BacklogItem backlogItem)
        {
            var tempList = new List<PersonModel>();

            if (backlogItem.GetTasks().Count >= 1)
                foreach (var task in backlogItem.GetTasks())
                {
                    tempList.Add(task.GetAssignedPerson());
                }

            if (backlogItem.GetAssignedPerson() != null)
                tempList.Add(backlogItem.GetAssignedPerson());

            // Send messages
            foreach (var involvedPerson in tempList)
            {
                involvedPerson.SendNotification($"Hi {involvedPerson.GetName()}, backlog item {backlogItem.GetName()} is in doing state.");
            }
        }

        private void ReadyToTestState(BacklogItem backlogItem)
        {
            // BacklogItem is ready to test, notify testers.
            foreach (var tester in backlogItem.GetBacklog().GetProject().GetTesters())
            {
                tester.SendNotification($"Hello tester {tester.GetName()}, BacklogItem {backlogItem.GetDescription()} is ready to test");
            }
        }

        private void TestingState(BacklogItem backlogItem)
        {
            var scrumMaster = backlogItem.GetSprint().GetScrumMaster();

            scrumMaster.SendNotification($"Hello scrum master {scrumMaster.GetName()}, BacklogItem {backlogItem.GetDescription()} is in Testing state.");
        }

        private void DoneState(BacklogItem backlogItem)
        {
            // BR: Let's send every developer from the backlogItem a notification that their item is approved.

            var tempList = new List<PersonModel>();

            if (backlogItem.GetTasks().Count >= 1)
                foreach (var task in backlogItem.GetTasks())
                {
                    tempList.Add(task.GetAssignedPerson());
                }

            if (backlogItem.GetAssignedPerson() != null)
                tempList.Add(backlogItem.GetAssignedPerson());

            // Send messages
            foreach (var involvedPerson in tempList)
            {
                involvedPerson.SendNotification($"Hi {involvedPerson.GetName()}, backlog item {backlogItem.GetName()} is done! Kudos!");
            }
        }
    }
}
