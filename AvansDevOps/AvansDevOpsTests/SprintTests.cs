using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using AvansDevOps;
using AvansDevOps.Person;
using AvansDevOps.Report;
using AvansDevOps.Review;
using AvansDevOps.Sprint;
using Xunit;

namespace AvansDevOpsTests
{
    public class SprintTests
    {
        [Fact]
        public void Generate_A_Report_Gives_No_Exception()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            ISprint sprint2 = factory.MakeReleaseSprint("Sprint 2", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddSprint(sprint2);

            // Act
            DateTime now = DateTime.Now;
            ReportModel sprint1GeneratedPublicReport = sprint.GenerateReport(EReportBranding.Public, new List<string>() { "Burndown chart: foo", "Velocity: 21" }, "v1.0", now, EReportFormat.PDF);
            ReportModel sprint1GeneratedFacultyReport = sprint.GenerateReport(EReportBranding.Faculty, new List<string>() { "Burndown chart: bar", "Velocity: 13" }, "v2.0", now, EReportFormat.PNG);


            ReportModel sprint2GeneratedPublicReport = sprint.GenerateReport(EReportBranding.Public, new List<string>() { "Burndown chart: foo", "Velocity: 21" }, "v1.0", now, EReportFormat.PDF);
            ReportModel sprint2GeneratedFacultyReport = sprint.GenerateReport(EReportBranding.Faculty, new List<string>() { "Burndown chart: bar", "Velocity: 13" }, "v2.0", now, EReportFormat.PNG);


            // Assert
            Assert.Equal(now, sprint1GeneratedPublicReport.Header.Date);
            Assert.Equal("Public", sprint1GeneratedPublicReport.Header.HeaderName);
            Assert.Equal(new List<string>() { "Burndown chart: foo", "Velocity: 21" }, sprint1GeneratedPublicReport.Contents);
            Assert.Equal("v1.0", sprint1GeneratedPublicReport.Header.ReportVersion);
            Assert.Equal(EReportFormat.PDF, sprint1GeneratedPublicReport.Format);

            Assert.Equal(now, sprint1GeneratedFacultyReport.Header.Date);
            Assert.Equal("Faculty", sprint1GeneratedFacultyReport.Header.HeaderName);
            Assert.Equal(new List<string>() { "Burndown chart: bar", "Velocity: 13" }, sprint2GeneratedFacultyReport.Contents);
            Assert.Equal("v2.0", sprint1GeneratedFacultyReport.Header.ReportVersion);
            Assert.Equal(EReportFormat.PNG, sprint1GeneratedFacultyReport.Format);

            Assert.Equal(now, sprint2GeneratedPublicReport.Header.Date);
            Assert.Equal("Public", sprint2GeneratedPublicReport.Header.HeaderName);
            Assert.Equal(new List<string>() { "Burndown chart: foo", "Velocity: 21" }, sprint2GeneratedPublicReport.Contents);
            Assert.Equal("v1.0", sprint2GeneratedPublicReport.Header.ReportVersion);
            Assert.Equal(EReportFormat.PDF, sprint1GeneratedPublicReport.Format);

            Assert.Equal(now, sprint2GeneratedFacultyReport.Header.Date);
            Assert.Equal("Faculty", sprint2GeneratedFacultyReport.Header.HeaderName);
            Assert.Equal(new List<string>() { "Burndown chart: bar", "Velocity: 13" }, sprint2GeneratedFacultyReport.Contents);
            Assert.Equal("v2.0", sprint2GeneratedFacultyReport.Header.ReportVersion);
            Assert.Equal(EReportFormat.PNG, sprint2GeneratedFacultyReport.Format);
        }

    }

    public partial class FeedbackSprint_InitializedState_Tests {
        [Fact]
        public void Change_Sprint_Name_Gives_No_Exception_In_InitializedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            // Act

            sprint.GetState().SetName("foo");

            // Assert
            Assert.Equal("foo", project.GetSprints().First().GetName());

        }

        [Fact]
        public void Change_Start_Date_Gives_No_Exception_In_InitializedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            // Act

            sprint.GetState().SetStartDate(DateTime.Parse("2010-10-10T00:00:00Z"));


            // Assert
            Assert.Equal(DateTime.Parse("2010-10-10T00:00:00Z"), project.GetSprints().First().GetStartDate());

        }

        [Fact]
        public void Change_End_Date_Gives_No_Exception_In_InitializedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Parse("2010-10-10T00:00:00Z"), DateTime.Parse("2010-10-10T00:00:00Z").AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            // Act

            sprint.GetState().SetEndDate(DateTime.Parse("2010-10-10T00:00:00Z").AddDays(7));


            // Assert
            Assert.Equal(DateTime.Parse("2010-10-17T00:00:00Z"), project.GetSprints().First().GetEndDate());

        }

        [Fact]
        public void Add_Developer_Gives_No_Exception_In_InitializedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);
            PersonModel p3 = new PersonModel("Nando", ERole.Developer);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Parse("2010-10-10T00:00:00Z"), DateTime.Parse("2010-10-10T00:00:00Z").AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);

            // Act
            sprint.GetState().AddDeveloper(p3);


            // Assert
            Assert.Contains(p3, sprint.GetDevelopers());

        }

        [Fact]
        public void Add_Review_Gives_NotSupportedException_In_InitializedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);

            ReviewModel review = new ReviewModel(sprint, p1, "No good");
            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().SetReview(review));

        }

        [Fact]
        public void Swap_To_New_State_Gives_No_Exception_In_InitializedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);

            // Act
            project.GetSprints().First().GetState().ToNextState();


            // Assert
            Assert.Equal("ActiveState", project.GetSprints().First().GetState().GetType().Name);

        }

        [Fact]
        public void Swap_To_Old_State_Gives_NotSupportedException_In_InitializedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().ToPreviousState());

        }
    }

    public partial class FeedbackSprint_ActiveState_Tests
    {
        [Fact]
        public void Change_Sprint_Name_Gives_NotSupportedException_In_ActiveState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            // Act
            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().SetName("Foo"));

        }

        [Fact]
        public void Change_Start_Date_Gives_NotSupportedException_In_ActiveState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().SetStartDate(DateTime.Parse("2010-10-10T00:00:00Z")));

        }

        [Fact]
        public void Change_End_Date_Gives_NotSupportedException_In_ActiveState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Parse("2010-10-10T00:00:00Z"), DateTime.Parse("2010-10-10T00:00:00Z").AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().SetEndDate(DateTime.Parse("2010-10-17T00:00:00Z")));

        }

        [Fact]
        public void Add_Developer_Gives_No_Exception_In_ActiveState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);
            PersonModel p3 = new PersonModel("Nando", ERole.Developer);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Parse("2010-10-10T00:00:00Z"), DateTime.Parse("2010-10-10T00:00:00Z").AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();

            // Act
            sprint.GetState().AddDeveloper(p3);


            // Assert
            Assert.Contains(p3, sprint.GetDevelopers());

        }

        [Fact]
        public void Add_Review_Gives_NotSupportedException_In_ActiveState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            ReviewModel review = new ReviewModel(sprint, p1, "No good");

            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().SetReview(review));

        }

        [Fact]
        public void Change_To_Old_State_Gives_No_Exception_In_ActiveState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();

            // Act
            project.GetSprints().First().GetState().ToPreviousState();


            // Assert
            Assert.Equal("InitializedState", project.GetSprints().First().GetState().GetType().Name);

        }
    }

    public partial class FeedbackSprint_FinishedState_Tests
    {
        [Fact]
        public void Change_Sprint_Name_Gives_NotSupportedException_In_ActiveState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            // Act
            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().SetName("Foo"));

        }

        [Fact]
        public void Change_Start_Date_Gives_NotSupportedException_In_ActiveState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().SetStartDate(DateTime.Parse("2010-10-10T00:00:00Z")));

        }

        [Fact]
        public void Change_End_Date_Gives_NotSupportedException_In_FinishedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Parse("2010-10-10T00:00:00Z"), DateTime.Parse("2010-10-10T00:00:00Z").AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            sprint.GetState().ToNextState();
            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().SetEndDate(DateTime.Parse("2010-10-17T00:00:00Z")));

        }

        [Fact]
        public void Add_Developer_Gives_NotSupportedException_In_FinishedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);
            PersonModel p3 = new PersonModel("Nando", ERole.Developer);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Parse("2010-10-10T00:00:00Z"), DateTime.Parse("2010-10-10T00:00:00Z").AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            sprint.GetState().ToNextState();

            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().AddDeveloper(p3));
            Assert.DoesNotContain(p3, sprint.GetDevelopers());

        }

        [Fact]
        public void Add_Review_Gives_No_Exception_In_FinishedState_While_Add_By_Scrum_Master()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            sprint.GetState().ToNextState();

            ReviewModel review = new ReviewModel(sprint, p1, "Good work guys!");

            // Act
            sprint.GetState().SetReview(review);


            // Assert
            Assert.Equal(review, sprint.GetReview());

        }

        [Fact]
        public void Add_Review_Gives_Security_Exception_In_Finished_State_When_Add_By_Non_ScrumMaster()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            sprint.GetState().ToNextState();

            ReviewModel review = new ReviewModel(sprint, p2, "Good work guys!");

            // Act

            // Assert
            Assert.Throws<SecurityException>(() => project.GetSprints().First().GetState().SetReview(review));

        }

        [Fact]
        public void Change_To_Old_State_Gives_No_Exception_In_FinishedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            sprint.GetState().ToNextState();

            // Act
            project.GetSprints().First().GetState().ToPreviousState();


            // Assert
            Assert.Equal("ActiveState", project.GetSprints().First().GetState().GetType().Name);

        }

    }

    public partial class ReleaseSprint_InitializedState_Tests
    {
        [Fact]
        public void Change_Sprint_Name_Gives_No_Exception_In_InitializedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            // Act

            sprint.GetState().SetName("foo");

            // Assert
            Assert.Equal("foo", project.GetSprints().First().GetName());

        }

        [Fact]
        public void Change_Start_Date_Gives_No_Exception_In_InitializedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            // Act

            sprint.GetState().SetStartDate(DateTime.Parse("2010-10-10T00:00:00Z"));


            // Assert
            Assert.Equal(DateTime.Parse("2010-10-10T00:00:00Z"), project.GetSprints().First().GetStartDate());

        }

        [Fact]
        public void Change_End_Date_Gives_No_Exception_In_InitializedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Parse("2010-10-10T00:00:00Z"), DateTime.Parse("2010-10-10T00:00:00Z").AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            // Act

            sprint.GetState().SetEndDate(DateTime.Parse("2010-10-10T00:00:00Z").AddDays(7));


            // Assert
            Assert.Equal(DateTime.Parse("2010-10-17T00:00:00Z"), project.GetSprints().First().GetEndDate());

        }

        [Fact]
        public void Add_Developer_Gives_No_Exception_In_InitializedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);
            PersonModel p3 = new PersonModel("Nando", ERole.Developer);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Parse("2010-10-10T00:00:00Z"), DateTime.Parse("2010-10-10T00:00:00Z").AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);

            // Act
            sprint.GetState().AddDeveloper(p3);


            // Assert
            Assert.Contains(p3, sprint.GetDevelopers());

        }

        [Fact]
        public void Add_Review_Gives_NotSupportedException_In_InitializedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);

            ReviewModel review = new ReviewModel(sprint, p1, "No good");
            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().SetReview(review));

        }

        [Fact]
        public void Change_To_New_State_Gives_No_Exception_In_InitializedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);

            // Act
            project.GetSprints().First().GetState().ToNextState();


            // Assert
            Assert.Equal("ActiveState", project.GetSprints().First().GetState().GetType().Name);

        }

        [Fact]
        public void Change_To_Old_State_Gives_NotSupportedException_In_InitializedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().ToPreviousState());

        }
    }

    public partial class ReleaseSprint_ActiveState_Tests
    {
        [Fact]
        public void Change_Sprint_Name_Gives_NotSupportedException_In_ActiveState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            // Act
            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().SetName("Foo"));

        }

        [Fact]
        public void Change_Start_Date_Gives_NotSupportedException_In_ActiveState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().SetStartDate(DateTime.Parse("2010-10-10T00:00:00Z")));

        }

        [Fact]
        public void Change_End_Date_Gives_NotSupportedException_In_ActiveState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Parse("2010-10-10T00:00:00Z"), DateTime.Parse("2010-10-10T00:00:00Z").AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().SetEndDate(DateTime.Parse("2010-10-17T00:00:00Z")));

        }

        [Fact]
        public void Add_Developer_Gives_No_Exception_In_ActiveState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);
            PersonModel p3 = new PersonModel("Nando", ERole.Developer);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Parse("2010-10-10T00:00:00Z"), DateTime.Parse("2010-10-10T00:00:00Z").AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();

            // Act
            sprint.GetState().AddDeveloper(p3);


            // Assert
            Assert.Contains(p3, sprint.GetDevelopers());

        }

        [Fact]
        public void Add_Review_Gives_NotSupportedException_In_ActiveState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            ReviewModel review = new ReviewModel(sprint, p1, "No good");

            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().SetReview(review));

        }

        [Fact]
        public void Change_To_Old_State_Gives_No_Exception_In_ActiveState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();

            // Act
            project.GetSprints().First().GetState().ToPreviousState();


            // Assert
            Assert.Equal("InitializedState", project.GetSprints().First().GetState().GetType().Name);

        }
    }

    public partial class ReleaseSprint_FinishedState_Tests
    {
        [Fact]
        public void Change_Sprint_Name_Gives_NotSupportedException_In_ActiveState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            // Act
            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().SetName("Foo"));

        }

        [Fact]
        public void Change_Start_Date_Gives_NotSupportedException_In_ActiveState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().SetStartDate(DateTime.Parse("2010-10-10T00:00:00Z")));

        }

        [Fact]
        public void Change_End_Date_Gives_NotSupportedException_In_FinishedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Parse("2010-10-10T00:00:00Z"), DateTime.Parse("2010-10-10T00:00:00Z").AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            sprint.GetState().ToNextState();
            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().SetEndDate(DateTime.Parse("2010-10-17T00:00:00Z")));

        }

        [Fact]
        public void Add_Developer_Gives_NotSupportedException_In_FinishedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);
            PersonModel p3 = new PersonModel("Nando", ERole.Developer);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Parse("2010-10-10T00:00:00Z"), DateTime.Parse("2010-10-10T00:00:00Z").AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            sprint.GetState().ToNextState();

            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => project.GetSprints().First().GetState().AddDeveloper(p3));
            Assert.DoesNotContain(p3, sprint.GetDevelopers());

        }

        [Fact]
        public void Add_Review_Gives_No_Exception_In_FinishedState_When_Add_By_Scrum_Master()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            sprint.GetState().ToNextState();

            ReviewModel review = new ReviewModel(sprint, p1, "Good work guys!");

            // Act
            sprint.GetState().SetReview(review);


            // Assert
            Assert.Equal(review, sprint.GetReview());

        }

        [Fact]
        public void Add_Review_Gives_Security_Exception_In_FinishedState_When_Add_By_Non_Scrum_Master()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            sprint.GetState().ToNextState();

            ReviewModel review = new ReviewModel(sprint, p2, "Good work guys!");

            // Act

            // Assert
            Assert.Throws<SecurityException>(() => project.GetSprints().First().GetState().SetReview(review));

        }

        [Fact]
        public void Change_To_Old_State_Gives_No_Exception_In_FinishedState()
        {
            // Arrange

            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Monique", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            sprint.GetState().ToNextState();
            sprint.GetState().ToNextState();

            // Act
            project.GetSprints().First().GetState().ToPreviousState();


            // Assert
            Assert.Equal("ActiveState", project.GetSprints().First().GetState().GetType().Name);

        }

    }
}
