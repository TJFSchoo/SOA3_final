using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Person;

namespace AvansDevOps.Sprint
{
    public class SprintFactory
    {

        public ISprint MakeReleaseSprint(string name, DateTime startDate, DateTime endDate, Project project, PersonModel scrumMaster,
            List<PersonModel> developers)
        {
            return new ReleaseSprint(name, startDate, endDate, project, scrumMaster, developers);
        }

        public ISprint MakeReviewSprint(string name, DateTime startDate, DateTime endDate, Project project, PersonModel scrumMaster,
            List<PersonModel> developers)
        {
            return new ReviewSprint(name, startDate, endDate, project, scrumMaster, developers);
        }

    }
}
