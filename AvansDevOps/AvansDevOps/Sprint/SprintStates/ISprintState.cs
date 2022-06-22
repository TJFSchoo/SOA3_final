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
    public interface ISprintState
    {
        void SetName(string name);
        void SetReview(ReviewModel review);
        void SetStartDate(DateTime startDate);
        void SetEndDate(DateTime endDate);
        void AddDeveloper(PersonModel developer);
        void AddToSprintBacklog(BacklogItem backlogItem);
        void StartStateAction();
        void ToNextState();
        void ToPreviousState();

    }
}
