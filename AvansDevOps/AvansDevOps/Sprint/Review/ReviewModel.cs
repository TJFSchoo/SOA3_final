using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Person;
using AvansDevOps.Sprint;

namespace AvansDevOps.Review
{
    public class ReviewModel
    {
        private string _review;
        private readonly PersonModel _author;
        private readonly ISprint _sprint;

        public ReviewModel(ISprint sprint, PersonModel author, string review)
        {
            this._sprint = sprint;
            this._author = author;
            this._review = review;
        }

        public PersonModel GetAuthor()
        {
            return this._author;
        }
    }
}
