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
    public class ReleaseSprint : ISprint
    {
        private string _name;
        private DateTime _startDate;
        private DateTime _endDate;
        private ISprintState _state;
        private readonly Project _project;
        private readonly PersonModel _scrumMaster;
        private readonly List<PersonModel> _developers;
        private readonly List<BacklogItem> _sprintBacklogItems;
        private ReviewModel _review;

        public ReleaseSprint(string name, DateTime startDate, DateTime endDate, Project project, PersonModel scrumMaster, List<PersonModel> developers)
        {
            this._name = name;
            this._startDate = startDate;
            this._endDate = endDate;
            this._project = project;
            this._scrumMaster = scrumMaster;
            this._developers = developers;
            this._sprintBacklogItems = new List<BacklogItem>();

            this._state = new InitializedState(this);
        }
        public void ChangeState(ISprintState state)
        {
            this._state = state;
        }

        public ISprintState GetState()
        {
            return _state;
        }

        public PersonModel GetScrumMaster()
        {
            return this._scrumMaster;
        }

        public List<PersonModel> GetDevelopers()
        {
            return this._developers;
        }

        public void AddDeveloper(PersonModel person)
        {
            if (this._developers.Contains(person))
                throw new NotSupportedException("Can't add the same person twice");
            this._developers.Add(person);
        }

        public void AddToSprintBacklog(BacklogItem backlogItem)
        {
            if (_sprintBacklogItems.Contains(backlogItem)) return;
            backlogItem.SetSprint(this);
            _sprintBacklogItems.Add(backlogItem);
        }

        public List<BacklogItem> GetBacklogItems()
        {
            return _sprintBacklogItems;
        }

        public Project GetProject()
        {
            return this._project;
        }

        public string GetName()
        {
            return this._name;
        }

        public void SetName(string name)
        {
            this._name = name;
        }

        public DateTime GetStartDate()
        {
            return this._startDate;
        }

        public void SetStartDate(DateTime startDate)
        {
            this._startDate = startDate;
        }

        public DateTime GetEndDate()
        {
            return this._endDate;
        }

        public void SetReview(ReviewModel review)
        {
            this._review = review;
        }

        public ReviewModel GetReview()
        {
            return this._review;
        }

        public void SetEndDate(DateTime endDate)
        {
            this._endDate = endDate;
        }

        public ReportModel GenerateReport(EReportBranding branding, List<string> contents, string version, DateTime date, EReportFormat format)
        {
            return branding == EReportBranding.Avans ? ReportDirector.BuildAvansReport(this, contents, version, date, format) : ReportDirector.BuildAvansPlusReport(this, contents, version, date, format);

        }
    }
}
