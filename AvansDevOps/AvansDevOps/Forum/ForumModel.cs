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
        private static string TAG = "ForumModel";

        public ForumModel()
        {
            _threads = new List<ThreadModel>();
        }

        public void NewThread(ThreadModel thread)
        {
            if (thread.GetTask().GetState() == ETaskState.Done)
                throw new NotSupportedException("[" + TAG + "] " + "Can't add thread when the task is marked as done.");

            if (string.IsNullOrWhiteSpace(thread.GetTitle()))
                throw new ArgumentNullException(thread.GetTitle(), "[" + TAG + "] " + "Title of thread can't be empty.");

            _threads.Add(thread);
        }

        public void ArchiveThread(ThreadModel thread)
        {
            if(!_threads.Exists(thread.Equals))
                throw new NotSupportedException("[" + TAG + "] " + "Can't remove thread that does not exists.");

            _threads.Remove(thread);
        }
        
        public List<ThreadModel> GetThreads()
        {
            return this._threads;
        }
    }
}
