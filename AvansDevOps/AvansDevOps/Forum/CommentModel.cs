using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Person;

namespace AvansDevOps.Forum
{
    public class CommentModel
    {
        private readonly ThreadModel _thread;
        private readonly PersonModel _author;
        private readonly DateTime _date;
        private readonly string _comment;

        public CommentModel(ThreadModel thread, PersonModel author, DateTime date, string comment)
        {
            _thread = thread;
            _author = author;
            _date = date;
            _comment = comment;
        }

        public ThreadModel GetThread()
        {
            return _thread;
        }

        public PersonModel GetAuthor()
        {
            return _author;
        }

        public DateTime GetDate()
        {
            return _date;
        }

        public string GetContent()
        {
            return _comment;
        }
    }
}
