using AvansDevOps.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Backlog;
using AvansDevOps.Forum;
using AvansDevOps.SCM;
using AvansDevOps.Sprint;

namespace AvansDevOps
{
    public class Project
    {
        private readonly PersonModel _productOwner;
        private readonly string _name;
        private readonly List<ISprint> _sprints;
        private BacklogModel _backlog;
        private ForumModel _forum;
        private readonly List<PersonModel> _testers;
        private readonly List<Source> _sources;

        public Project(string name, PersonModel productOwner)
        {
            this._name = name;
            this._productOwner = productOwner;
            this._sprints = new List<ISprint>();
            this._testers = new List<PersonModel>();
            this._sources = new List<Source>();
        }

        public PersonModel GetProductOwner()
        {
            return this._productOwner;
        }

        public void AddBacklog(BacklogModel backlog)
        {
            _backlog = backlog;
        }

        public BacklogModel GetBacklog()
        {
            return _backlog;
        }

        public void AddForum(ForumModel forum)
        {
            _forum = forum;
        }

        public ForumModel GetForum()
        {
            return _forum;
        }

        public void AddSprint(ISprint sprint)
        {
            this._sprints.Add(sprint);
        }

        public List<ISprint> GetSprints()
        {
            return this._sprints;
        }

        public string GetName()
        {
            return this._name;
        }

        public void AddTester(PersonModel tester)
        {
            if (tester.GetRole() != ERole.Tester)
                throw new NotSupportedException(
                    "Can't add a person to project testers if he doesn't have the tester role.");

            if (_testers.Contains(tester))
                throw new NotSupportedException("Can't add the same person to project testers twice.");

            _testers.Add(tester);
        }

        public List<PersonModel> GetTesters()
        {
            return _testers;
        }

        public void AddSource(Source source)
        {
            if (_sources.Contains(source))
                throw new NotSupportedException("Can't add the same source twice");
            _sources.Add(source);
        }

        public bool RemoveSource(Source source)
        {
            return _sources.Remove(source);
        }

        public List<Source> GetSources()
        {
            return _sources;
        }
    }
}
