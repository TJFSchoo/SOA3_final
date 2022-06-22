using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using AvansDevOps.Backlog;
using AvansDevOps.Person;

namespace AvansDevOps.Forum
{
    public class ThreadModel
    {
        private readonly List<CommentModel> _comments;
        private readonly string _title;
        private readonly DateTime _date;
        private readonly PersonModel _author;
        private readonly Task _taskReference;

        public ThreadModel(string title, DateTime date, PersonModel author, Task task)
        {
            _title = title;
            _date = date;
            _author = author;
            _taskReference = task;
            _comments = new List<CommentModel>();
        }
        
        public void AddComment(CommentModel comment)
        {
            if (_taskReference.GetState() == ETaskState.Done)
                throw new NotSupportedException("Can't add a comment to thread when the task is marked as done.");

            if (string.IsNullOrWhiteSpace(comment.GetContent()))
                throw new ArgumentNullException(comment.GetContent(), "Content of comment cannot be empty.");


            foreach (var foundComment in _comments)
            {
                foundComment.GetAuthor().SendNotification($"Hi {foundComment.GetAuthor().GetName()}, a new comment from {comment.GetAuthor()} has been posted on a thread called {_title} on which you have activity");
            }

            _author.SendNotification($"Hello {_author.GetName()}, your thread received a new comment from {comment.GetAuthor().GetName()}. Please check your thread for new additions.");

            _comments.Add(comment);
        }

        public void DeleteComment(CommentModel comment)
        {
            if (_taskReference.GetState() == ETaskState.Done)
                throw new NotSupportedException("Can't remove a comment to thread when the task is marked as done.");

            if (!_comments.Exists(comment.Equals))
                throw new NotSupportedException("Can't remove comment that does not exists.");

            _comments.Remove(comment);
        }

        public List<CommentModel> GetComments()
        {
            return _comments;
        }

        public Task GetTask()
        {
            return _taskReference;
        }

        public string GetTitle()
        {
            return _title;
        }

        public DateTime GetDate()
        {
            return _date;
        }
    }
}
