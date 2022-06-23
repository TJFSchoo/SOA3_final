using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Backlog;
using AvansDevOps.Person;
using AvansDevOps.Review;

namespace AvansDevOps.Sprint.SprintState
{
    public class ActiveState : ISprintState
    {
        private readonly ISprint _sprint;
        private static string TAG = "ActiveState";

        public ActiveState(ISprint sprint)
        {
            this._sprint = sprint;
        }

        public void SetName(string name)
        {
            throw new NotSupportedException("[" + TAG + "] " + "Unable to change name when sprint is in active state.");
        }

        public void SetReview(ReviewModel review)
        {
            throw new NotSupportedException("[" + TAG + "] " + "Unable to add a review when sprint is in active state.");
        }

        public void SetStartDate(DateTime startDate)
        {
            throw new NotSupportedException("[" + TAG + "] " + "Unable to change startDate when sprint is in active state.");
        }

        public void SetEndDate(DateTime endDate)
        {
            throw new NotSupportedException("[" + TAG + "] " + "Unable to change endDate when sprint is in active state.");
        }

        public void AddDeveloper(PersonModel developer)
        {
            this._sprint.AddDeveloper(developer);
        }

        public void AddToSprintBacklog(BacklogItem backlogItem)
        {
            throw new NotSupportedException("[" + TAG + "] " + "Unable to add to sprint backlog when sprint is in active state.");
        }

        public void StartStateAction()
        {
            throw new NotImplementedException();
        }

        public void ToNextState()
        {
            this._sprint.ChangeState(new FinishedState(_sprint));
        }

        public void ToPreviousState()
        {
            this._sprint.ChangeState(new InitializedState(_sprint));
        }
    }
}
