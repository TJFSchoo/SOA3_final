using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Backlog;
using AvansDevOps.Person;
using AvansDevOps.Pipeline;
using AvansDevOps.Review;

namespace AvansDevOps.Sprint.SprintState
{
    public class FinishedState : ISprintState
    {
        private readonly ISprint _sprint;
        private IPipeline _pipeline;
        private static string TAG = "FinishedState";

        public FinishedState(ISprint sprint)
        {
            this._sprint = sprint;
        }

        public void SetName(string name)
        {
            throw new NotSupportedException("[" + TAG + "] " + "Unable to modify name when sprint is in finished state.");
        }

        public void SetReview(ReviewModel review)
        {
            if (review.GetAuthor() == _sprint.GetScrumMaster())
            {
                this._sprint.SetReview(review);
            }
            else
            {
                throw new SecurityException("[" + TAG + "] " + "Only scrum master can add a review to a sprint.");
            }
        }

        public void SetStartDate(DateTime startDate)
        {
            throw new NotSupportedException("[" + TAG + "] " + "Unable to change start date when sprint is in finished state.");
        }

        public void SetEndDate(DateTime endDate)
        {
            throw new NotSupportedException("[" + TAG + "] " + "Unable to change end date when sprint is in finished state.");
        }

        public void AddDeveloper(PersonModel developer)
        {
            throw new NotSupportedException("[" + TAG + "] " + "Unable to add developer when sprint is in finished state.");
        }

        public void AddToSprintBacklog(BacklogItem backlogItem)
        {
            throw new NotSupportedException("[" + TAG + "] " + "Unable to add backlog item when sprint is in finished state.");
        }

        public void StartStateAction()
        {
            switch (_sprint.GetType().Name)
            {
                case "ReleaseSprint":
                    _pipeline = new DevelopmentPipeline(_sprint, EPipelineConfig.Automatic);
                    break;
                case "ReviewSprint":
                    _pipeline = new TestPipeline(_sprint, EPipelineConfig.Automatic);
                    break;
            }

            _pipeline.Execute();
        }

        public void ToNextState()
        {
            throw new NotImplementedException();
        }

        public void ToPreviousState()
        {
            this._sprint.ChangeState(new ActiveState(_sprint));
        }
    }
}
