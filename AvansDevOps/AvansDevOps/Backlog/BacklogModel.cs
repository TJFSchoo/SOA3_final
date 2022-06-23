using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Backlog
{
    public class BacklogModel
    {
        private readonly Project _project;
        private readonly List<BacklogItem> _backlogItems;
        private static string TAG = "BacklogModel";

        public BacklogModel(Project project)
        {
            _project = project;
            _backlogItems = new List<BacklogItem>();
        }

        public Project GetProject()
        {
            return _project;
        }

        public void AddBacklogItem(BacklogItem backlogItem)
        {
            if (_backlogItems.Contains(backlogItem))
                throw new NotSupportedException("[" + TAG + "] " + "Unable to add the same Backlog Item twice.");

            _backlogItems.Add(backlogItem);
        }

        public List<BacklogItem> GetBacklogItems()
        {
            return _backlogItems;
        }
    }
}
