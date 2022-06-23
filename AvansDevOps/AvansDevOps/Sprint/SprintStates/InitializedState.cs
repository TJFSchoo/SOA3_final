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
    public class InitializedState : ISprintState
    {
        private readonly ISprint _sprint;
        private static string TAG = "InitializedState";

        public InitializedState(ISprint sprint)
        {
            this._sprint = sprint;
        }

        public void SetName(string name)
        {
            this._sprint.SetName(name);
        }

        public void SetReview(ReviewModel review)
        {
            throw new NotSupportedException("[" + TAG + "] " + "Unable to add a review in Initialized state.");
        }

        public void SetStartDate(DateTime startDate)
        {
            this._sprint.SetStartDate(startDate);
        }

        public void SetEndDate(DateTime endDate)
        {
            this._sprint.SetEndDate(endDate);
        }

        public void AddDeveloper(PersonModel developer)
        {
            this._sprint.AddDeveloper(developer);
        }

        public void AddToSprintBacklog(BacklogItem backlogItem)
        {
            this._sprint.AddToSprintBacklog(backlogItem);
        }

        public void StartStateAction()
        {
            throw new NotImplementedException();
        }

        public void ToNextState()
        {
            this._sprint.ChangeState(new ActiveState(_sprint));
                
        }

        public void ToPreviousState()
        {
            throw new NotSupportedException("[" + TAG + "] " + "Unable to go to older state.");
        }


    }
}
