using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps;
using AvansDevOps.Backlog;
using AvansDevOps.Backlog.BacklogItemStates;
using AvansDevOps.Channel;
using AvansDevOps.Notification;
using AvansDevOps.Person;
using AvansDevOps.Sprint;
using Task = AvansDevOps.Backlog.Task;
using Xunit;

namespace AvansDevOpsTests
{
    public class NotificationTests
    {
        [Fact]
        public void Backlog_Can_Register_One_Observer()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Piet", ERole.Developer);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);

            var backlog = new BacklogModel(project);

            var backlogItem = new BacklogItem("User can login into the platform", "Foo", p2, 3, backlog);

            backlogItem.AssignPerson(p2);
            backlog.AddBacklogItem(backlogItem);

            sprint.AddToSprintBacklog(backlogItem);

            project.AddBacklog(backlog);

            var task1 = new Task("Bar", p1);
            backlogItem.GetState().AddTask(task1);

            var backlogItemObserver = new BacklogItemObserver();

            // Act
            backlogItem.Register(backlogItemObserver);

            // Assert
            Assert.Single(backlogItem.GetObservers());
        }

        [Fact]
        public void Backlog_Can_Register_Multiple_Observers()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Piet", ERole.Developer);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);

            var backlog = new BacklogModel(project);

            var backlogItem = new BacklogItem("User can login into the platform", "Foo", p2, 3, backlog);

            backlogItem.AssignPerson(p2);
            backlog.AddBacklogItem(backlogItem);

            sprint.AddToSprintBacklog(backlogItem);

            project.AddBacklog(backlog);

            var task1 = new Task("Bar", p1);
            backlogItem.GetState().AddTask(task1);

            var backlogItemObserver = new BacklogItemObserver();

            // Act
            // TODO: Add sprint observer
            backlogItem.Register(backlogItemObserver);

            // Assert
            Assert.Single(backlogItem.GetObservers());
        }

        [Fact]
        public void Backlog_Can_Unregister_Observer()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Piet", ERole.Developer);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);

            var backlog = new BacklogModel(project);

            var backlogItem = new BacklogItem("User can login into the platform", "Foo", p2, 3, backlog);

            backlogItem.AssignPerson(p2);
            backlog.AddBacklogItem(backlogItem);

            sprint.AddToSprintBacklog(backlogItem);

            project.AddBacklog(backlog);

            var task1 = new Task("Bar", p1);
            backlogItem.GetState().AddTask(task1);

            var backlogItemObserver = new BacklogItemObserver();

            // Act
            backlogItem.Register(backlogItemObserver);
            backlogItem.Unregister(backlogItemObserver);

            // Assert
            Assert.Empty(backlogItem.GetObservers());
        }

        [Fact]
        public void Backlog_Can_Not_Unregister_Observer()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Piet", ERole.Developer);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);

            var backlog = new BacklogModel(project);

            var backlogItem = new BacklogItem("User can login into the platform", "Foo", p2, 3, backlog);

            backlogItem.AssignPerson(p2);
            backlog.AddBacklogItem(backlogItem);

            sprint.AddToSprintBacklog(backlogItem);

            project.AddBacklog(backlog);

            var task1 = new Task("Bar", p1);
            backlogItem.GetState().AddTask(task1);

            var backlogItemObserver = new BacklogItemObserver();

            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => backlogItem.Unregister(backlogItemObserver));
            Assert.Empty(backlogItem.GetObservers());
        }

        [Fact]
        public void Backlog_Can_Not_Register_The_Same_Observer()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Piet", ERole.Developer);

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);

            var backlog = new BacklogModel(project);

            var backlogItem = new BacklogItem("User can login into the platform", "Foo", p2, 3, backlog);

            backlogItem.AssignPerson(p2);
            backlog.AddBacklogItem(backlogItem);

            sprint.AddToSprintBacklog(backlogItem);

            project.AddBacklog(backlog);

            var task1 = new Task("Bar", p1);
            backlogItem.GetState().AddTask(task1);

            var backlogItemObserver = new BacklogItemObserver();

            // Act
            backlogItem.Register(backlogItemObserver);

            // Assert
            Assert.Throws<NotSupportedException>(() => backlogItem.Register(backlogItemObserver));
            Assert.Single(backlogItem.GetObservers());
        }

        [Fact]
        public void Notification_Todo_State_Update()
        {
            // Arrange
            Project project = new Project("Test Project", new PersonModel("Tom", ERole.Lead));
            SprintFactory factory = new SprintFactory();
            ChannelFactory channel = new ChannelFactory();

            PersonModel p1 = new PersonModel("Henk", ERole.Developer);
            PersonModel p2 = new PersonModel("Piet", ERole.Developer);

            p1.AddChannel(channel.CreateSlackChannel("@Henk"));

            ISprint sprint = factory.MakeReleaseSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), project, p1, new List<PersonModel>() { p2 });
            project.AddSprint(sprint);

            var backlog = new BacklogModel(project);

            var backlogItem = new BacklogItem("User can login into the platform", "Foo", p2, 3, backlog);

            backlogItem.AssignPerson(p2);
            backlog.AddBacklogItem(backlogItem);

            sprint.AddToSprintBacklog(backlogItem);

            project.AddBacklog(backlog);

            var task1 = new Task("Bar", p1);
            backlogItem.GetState().AddTask(task1);

            var backlogItemObserver = new BacklogItemObserver();

            // Act
            backlogItem.Register(backlogItemObserver);
            backlogItem.GetState().NextState();
            // Backlog is in DoingState, set tasks

            backlogItem.GetTasks().First().NextState();
            backlogItem.GetTasks().First().NextState();

            task1.NextState();
            task1.NextState();

            backlogItem.GetState().NextState();
            backlogItem.GetState().NextState();

            // Backlog is in TestingState
            backlogItem.GetState().NextState();

            // Assert
            Assert.NotEmpty(backlogItem.GetObservers());
        }
    }

}
