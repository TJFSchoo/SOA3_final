using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Backlog;

namespace AvansDevOps.SCM
{
    public class GitCommit : ICommit
    {
        private readonly string _title;
        private readonly string _description;
        private readonly BacklogItem _backlogItem;

        public GitCommit(string title, string description, BacklogItem backlogItem)
        {
            _title = title;
            _description = description;
            _backlogItem = backlogItem;
        }


        public string GetTitle()
        {
            return _title;
        }

        public string GetDescription()
        {
            return _description;
        }

        public BacklogItem GetBacklogItem()
        {
            return _backlogItem;
        }
    }
}
