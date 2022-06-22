using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps;
using AvansDevOps.Person;
using AvansDevOps.Review;
using AvansDevOps.Sprint;
using Xunit;

namespace AvansDevOpsTests
{
    public class PipelineTests
    {
        [Fact]
        public void DevelopmentPipeline_Should_Fire_When_ReleaseSprint_Reaches_FinishedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1,
                new List<PersonModel>() {p2});
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            sprint.GetState().ToNextState();

            // Act
            sprint.GetState().StartStateAction();
            // Assert
        }

        [Fact]
        public void TestPipeline_Should_Fire_When_ReviewSprint_Reaches_FinishedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1,
                new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            sprint.GetState().ToNextState();

            // Act
            sprint.GetState().StartStateAction();
            // Assert
        }
    }
}
