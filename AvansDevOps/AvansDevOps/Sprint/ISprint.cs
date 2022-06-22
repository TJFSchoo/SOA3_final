using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Backlog;
using AvansDevOps.Person;
using AvansDevOps.Report;
using AvansDevOps.Review;
using AvansDevOps.Sprint.SprintState;

namespace AvansDevOps.Sprint
{
    public interface ISprint
    {
        void ChangeState(ISprintState state);
        ISprintState GetState();
        PersonModel GetScrumMaster();
        List<PersonModel> GetDevelopers();
        void AddDeveloper(PersonModel person);
        void AddToSprintBacklog(BacklogItem backlogItem);
        List<BacklogItem> GetBacklogItems();
        Project GetProject();
        string GetName();
        void SetName(string name);
        DateTime GetStartDate();
        void SetStartDate(DateTime startDate);
        DateTime GetEndDate();
        void SetReview(ReviewModel review);
        ReviewModel GetReview();
        void SetEndDate(DateTime endDate);
        ReportModel GenerateReport(EReportBranding branding, List<string> contents, string version, DateTime date, EReportFormat format);
    }
}
