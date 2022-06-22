using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Backlog;

namespace AvansDevOps.Forum
{
    public class ForumModel
    {
        private readonly List<ThreadModel> _threads;

        public ForumModel()
        {
            _threads = new List<ThreadModel>();
        }

        public void NewThread(ThreadModel thread)
        {
            if (thread.GetTask().GetState() == ETaskState.Done)
                throw new NotSupportedException("Can't add thread when the task is marked as done.");

            if (string.IsNullOrWhiteSpace(thread.GetTitle()))
                throw new ArgumentNullException(thread.GetTitle(), "Title of thread cannot be empty.");

            _threads.Add(thread);
        }

        public void ArchiveThread(ThreadModel thread)
        {
            if(!_threads.Exists(thread.Equals))
                throw new NotSupportedException("Can't remove thread that does not exists.");

            _threads.Remove(thread);
        }
        
        public List<ThreadModel> GetThreads()
        {
            return this._threads;
        }
    }
}
