using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.SCM
{
    public class Source
    {
        private readonly string _name;
        private readonly List<ICommit> _commits;

        public Source(string name)
        {
            _name = name;
            _commits = new List<ICommit>();
        }

        public string GetName()
        {
            return _name;
        }

        public List<ICommit> GetCommits()
        {
            return _commits;
        }

        public void AddCommit(ICommit commit)
        {
            if (_commits.Contains(commit))
                throw new NotSupportedException("Can't add the same commit twice");

            _commits.Add(commit);
        }
    }
}
