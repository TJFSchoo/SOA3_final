using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvansDevOps;
using AvansDevOps.Backlog;
using AvansDevOps.Backlog.BacklogItemStates;
using AvansDevOps.Channel;
using AvansDevOps.Person;
using AvansDevOps.Sprint;
using Xunit;

namespace AvansDevOpsTests
{
    public class BacklogTests
    {
        [Fact]
        public void Adding_Backlog_To_Project_Gives_No_Exception()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            // Assert
            Assert.Equal(backlog, project.GetBacklog());

        }

        [Fact]
        public void Adding_Backlog_Items_To_Project_Gives_No_Exception()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);

            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);

            // Act
            var backlogItem1 = new BacklogItem("User can login into the platform", "Foo", p2, 3, backlog);
            backlogItem1.AssignPerson(p2);
            backlog.AddBacklogItem(backlogItem1);

            // Assert
            Assert.Contains(backlogItem1, project.GetBacklog().GetBacklogItems());
            Assert.Equal("TodoState", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetState().GetType().Name);
        }

        [Fact]
        public void Adding_Duplicate_Backlog_Items_To_Backlog_Gives_NotSupportedException()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);

            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);

            // Act
            var backlogItem1 = new BacklogItem("User can login into the platform", "Foo", p2, 3, backlog);
            
            backlog.AddBacklogItem(backlogItem1);

            // Assert
            Assert.Throws<NotSupportedException>(() => backlog.AddBacklogItem(backlogItem1));
        }

        [Fact]
        public void Adding_Tasks_To_Backlog_Item_Converts_BacklogItem_To_Different_Task()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);

            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);

            // Act
            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);
            backlog.AddBacklogItem(backlogItem1);

            var newTask = new Task("Don't look here", p1);

            backlogItem1.GetState().AddTask(newTask);

            // Assert
            Assert.Equal(2, backlogItem1.GetTasks().Count);
            Assert.Equal(2, backlog.GetBacklogItems().First().GetTasks().Count);
            Assert.Null(backlog.GetBacklogItems().First().GetAssignedPerson());
            Assert.Equal("Look here",
                backlog.GetBacklogItems().First().GetTasks().Find((task => task.GetDescription() == "Look here"))
                    .GetDescription());

            Assert.Equal(p2,
                backlog.GetBacklogItems().First().GetTasks().Find((task => task.GetAssignedPerson() == p2))
                    .GetAssignedPerson());

            Assert.Equal(p1,
                backlog.GetBacklogItems().First().GetTasks().Find((task => task.GetAssignedPerson() == p1))
                    .GetAssignedPerson());

        }

        [Fact]
        public void Changing_Backlog_Item_State_Without_Sprint_Gives_NotSupportedException()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);

            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);

            // Act
            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);
            backlog.AddBacklogItem(backlogItem1);

            // Assert
            Assert.Throws<NotSupportedException>(() => backlogItem1.ChangeState(new DoingState(backlogItem1)));
        }
    }

    public partial class TodoStateBacklogItemsTests
    {
        [Fact]
        public void Adding_Removing_Tasks_From_Backlog_Item_In_InitialState_Gives_No_Exception()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);

            // Assert
            Assert.Equal(4, backlogItem1.GetTasks().Count);
            backlogItem1.GetState().RemoveTask(task1);
            Assert.Equal(3, backlogItem1.GetTasks().Count);
            Assert.NotNull(backlogItem1.GetTasks().Find((task => task.GetDescription() == "Look here" )));
            Assert.NotNull(backlogItem1.GetTasks().Find((task => task.GetDescription() == "lorem" )));
            Assert.NotNull(backlogItem1.GetTasks().Find((task => task.GetDescription() == "ipsum" )));
        }

        [Fact]
        public void Changing_Attributes_Gives_No_Exception()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);

            backlogItem1.GetState().SetName("Test1");
            backlogItem1.GetState().SetDescription("Test2");
            backlogItem1.GetState().SetEffort(1);

            // Assert
            Assert.Equal("Test1", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetName());
            Assert.Equal("Test2", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetDescription());
            Assert.Equal(1, project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetEffort());
        }

        [Fact]
        public void Previous_State_Change_Gives_NotSupportedException()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);

            // Assert
            Assert.Throws<NotSupportedException>(() => backlogItem1.GetState().PreviousState());
        }

        [Fact]
        public void To_Next_State_Gives_No_Exception()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);

            backlogItem1.GetState().NextState();

            // Assert
            Assert.Equal("DoingState", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetState().GetType().Name);
        }
    }

    public partial class DoingStateBacklogItemsTests
    {
        [Fact]
        public void Adding_And_Removing_Tasks_From_BacklogItem_In_DoingState_Gives_No_Exception()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            backlogItem1.GetState().NextState();

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);

            // Assert
            Assert.Equal(4, backlogItem1.GetTasks().Count);
            backlogItem1.GetState().RemoveTask(task1);
            Assert.Equal(3, backlogItem1.GetTasks().Count);
            Assert.NotNull(backlogItem1.GetTasks().Find((task => task.GetDescription() == "Look here")));
            Assert.NotNull(backlogItem1.GetTasks().Find((task => task.GetDescription() == "lorem")));
            Assert.NotNull(backlogItem1.GetTasks().Find((task => task.GetDescription() == "ipsum")));
        }

        [Fact]
        public void Changing_Name_or_Effort_or_Description_Gives_No_Exception()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            backlogItem1.GetState().NextState();

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);

            backlogItem1.GetState().SetName("Test1");
            backlogItem1.GetState().SetDescription("Test2");
            backlogItem1.GetState().SetEffort(1);

            // Assert
            Assert.Equal("Test1", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetName());
            Assert.Equal("Test2", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetDescription());
            Assert.Equal(1, project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetEffort());
        }

        [Fact]
        public void To_Previous_State_Gives_No_Exception()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            backlogItem1.GetState().NextState();

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);


            backlogItem1.GetState().PreviousState();

            // Assert
            Assert.Equal("TodoState", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetState().GetType().Name);
        }

        [Fact]
        public void To_Next_State_With_Tasks_On_Todo_Gives_NotSupportedException()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);

            backlogItem1.GetState().NextState();

            // Assert
            Assert.Throws<NotSupportedException>(() => backlogItem1.GetState().NextState());
            Assert.Equal("DoingState", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetState().GetType().Name);
        }

        [Fact]
        public void To_Next_State_With_No_Tasks_On_Todo_Gives_No_Exception()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);
            backlogItem1.GetState().NextState();

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetState().NextState();

            // Assert
            Assert.Equal("ReadyToTestState", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetState().GetType().Name);
        }
    }

    public partial class ReadyToTestStateBacklogItemsTests
    {
        [Fact]
        public void Removing_Tasks_From_BacklogItem_In_ReadyToTestState_Gives_No_Exception()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            backlogItem1.GetState().NextState();

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);

            backlogItem1.GetTasks().First().NextState();

            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetState().NextState();

            // Assert
            Assert.Equal(4, backlogItem1.GetTasks().Count);
            backlogItem1.GetState().RemoveTask(task1);
            Assert.Equal(3, backlogItem1.GetTasks().Count);
            Assert.NotNull(backlogItem1.GetTasks().Find((task => task.GetDescription() == "Look here")));
            Assert.NotNull(backlogItem1.GetTasks().Find((task => task.GetDescription() == "lorem")));
            Assert.NotNull(backlogItem1.GetTasks().Find((task => task.GetDescription() == "ipsum")));
        }

        [Fact]
        public void Adding_Tasks_From_BacklogItem_In_ReadyToTestState_Gives_NotSupportedException()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            backlogItem1.GetState().NextState();

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);

            backlogItem1.GetTasks().First().NextState();

            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetState().NextState();

            // Assert
            Assert.Equal(3, backlogItem1.GetTasks().Count);
            Assert.NotNull(backlogItem1.GetTasks().Find((task => task.GetDescription() == "Look here")));
            Assert.NotNull(backlogItem1.GetTasks().Find((task => task.GetDescription() == "lorem")));
            Assert.Throws<NotSupportedException>(() => backlogItem1.GetState().AddTask(task3));
            Assert.Null(backlogItem1.GetTasks().Find((task => task.GetDescription() == "ipsum")));
        }

        [Fact]
        public void Changing_Name_or_Effort_or_Description_Gives_No_Exception()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            backlogItem1.GetState().NextState();

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetState().NextState();

            backlogItem1.GetState().SetName("Test1");
            backlogItem1.GetState().SetDescription("Test2");
            backlogItem1.GetState().SetEffort(1);

            // Assert
            Assert.Equal("Test1", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetName());
            Assert.Equal("Test2", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetDescription());
            Assert.Equal(1, project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetEffort());
        }

        [Fact]
        public void To_Previous_State_Gives_No_Exception()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            backlogItem1.GetState().NextState();

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetState().NextState();

            // Assert
            backlogItem1.GetState().PreviousState();
            Assert.Equal("TodoState", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetState().GetType().Name);
        }

        [Fact]
        public void To_Next_State_With_Tasks_On_Todo_Or_Doing_Gives_NotSupportedException()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);

            backlogItem1.GetState().NextState();

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            task1.NextState();
            task2.NextState();

            backlogItem1.GetState().NextState();

            // Assert
            Assert.Throws<NotSupportedException>(() => backlogItem1.GetState().NextState());
            Assert.Equal("ReadyToTestState", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetState().GetType().Name);
        }

        [Fact]
        public void To_Next_State_With_No_Tasks_On_Todo_Or_Doing_Gives_No_Exception()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);
            backlogItem1.GetState().NextState();

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetState().NextState();

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetState().NextState();

            // Assert
            Assert.Equal("TestingState", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetState().GetType().Name);
        }
    }

    public partial class TestingStateBacklogItemsTest
    {
        [Fact]
        public void Adding_And_Removing_Tasks_From_Backlog_Item_In_TestState_Gives_NotSupportedException()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            backlogItem1.GetState().NextState();

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetState().NextState();
            backlogItem1.GetState().NextState();

            // Assert
            Assert.Equal("TestingState", backlogItem1.GetState().GetType().Name);
            Assert.Throws<NotSupportedException>(() => backlogItem1.GetState().RemoveTask(task1));
            Assert.Throws<NotSupportedException>(() => backlogItem1.GetState().AddTask(new Task("This shouldn't work", p4)));
        }

        [Fact]
        public void Changing_Backlog_Item_Name_In_TestState_Gives_No_Exception()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            backlogItem1.GetState().NextState();

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetState().NextState();
            backlogItem1.GetState().NextState();

            // Act
            backlogItem1.GetState().SetName("Foo bar");

            // Assert
            Assert.Equal("TestingState", backlogItem1.GetState().GetType().Name);
            Assert.Equal("Foo bar", backlogItem1.GetName());
        }


        [Fact]
        public void Adding_Tasks_To_Backlog_Item_In_TestingState_Gives_NotSupportedException()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            backlogItem1.GetState().NextState();

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetState().NextState();
            backlogItem1.GetState().NextState();

            // Assert
            Assert.Equal("TestingState", backlogItem1.GetState().GetType().Name);
            Assert.Equal(3, backlogItem1.GetTasks().Count);
            Assert.NotNull(backlogItem1.GetTasks().Find((task => task.GetDescription() == "Look here")));
            Assert.NotNull(backlogItem1.GetTasks().Find((task => task.GetDescription() == "lorem")));
            Assert.Throws<NotSupportedException>(() => backlogItem1.GetState().AddTask(task3));
            Assert.Null(backlogItem1.GetTasks().Find((task => task.GetDescription() == "ipsum")));
        }

        [Fact]
        public void Changing_Attributes_Gives_No_Exception()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            backlogItem1.GetState().NextState();

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetState().NextState();
            backlogItem1.GetState().NextState();

            backlogItem1.GetState().SetName("Test1");
            backlogItem1.GetState().SetDescription("Test2");
            backlogItem1.GetState().SetEffort(1);

            // Assert
            Assert.Equal("TestingState", backlogItem1.GetState().GetType().Name);
            Assert.Equal("Test1", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetName());
            Assert.Equal("Test2", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetDescription());
            Assert.Equal(1, project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetEffort());
        }

        [Fact]
        public void Change_To_Previous_State_Gives_No_Exception()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            backlogItem1.GetState().NextState();

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetState().NextState();
            backlogItem1.GetState().NextState();

            // Assert
            backlogItem1.GetState().PreviousState();
            Assert.Equal("ReadyToTestState", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetState().GetType().Name);
        }

        [Fact]
        public void Change_To_Next_State_With_Tasks_Gives_NotSupportedException()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Bas", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Tom", ERole.Developer);
            p1.AddChannel(new EmailChannel("tom@test.nl"));
            p1.AddChannel(new SlackChannel("tomslack"));
            PersonModel p2 = new PersonModel("Henk", ERole.Developer);
            p2.AddChannel(new EmailChannel("henk@test.nl"));
            PersonModel p3 = new PersonModel("Peter", ERole.Tester);
            p3.AddChannel(new SlackChannel("Peterslack"));
            PersonModel p4 = new PersonModel("Monique", ERole.Developer);
            p4.AddChannel(new EmailChannel("monique@test.nl"));
            PersonModel p5 = new PersonModel("Jan", ERole.Developer);
            p5.AddChannel(new EmailChannel("jan@test.nl"));


            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);
            project.AddTester(p3);

            // Act
            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);


            var backlogItem1 = new BacklogItem("User can login into the platform", "Look here", p2, 3, backlog);

            backlog.AddBacklogItem(backlogItem1);
            sprint.AddToSprintBacklog(backlogItem1);

            var task1 = new Task("Bar", p4);
            var task2 = new Task("lorem", p5);
            var task3 = new Task("ipsum", p5);
            backlogItem1.GetState().AddTask(task1);
            backlogItem1.GetState().AddTask(task2);
            backlogItem1.GetState().AddTask(task3);


            backlogItem1.GetState().NextState();

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetTasks().First().NextState();
            task1.NextState();
            task2.NextState();
            task3.NextState();

            backlogItem1.GetState().NextState();
            backlogItem1.GetState().NextState();
            task1.PreviousState();

            // Assert
            Assert.Throws<NotSupportedException>(() => backlogItem1.GetState().NextState());
            Assert.Equal("TestingState", project.GetBacklog().GetBacklogItems().Find(item => item == backlogItem1).GetState().GetType().Name);
        }

    }
}
