using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps;
using AvansDevOps.Backlog;
using AvansDevOps.Person;
using AvansDevOps.SCM;
using Xunit;

namespace AvansDevOpsTests
{
    public class SCMTests
    {
        [Fact]
        public void Adding_Source_To_Project_Should_Not_Throw_Exception()
        {
            // Arrange
            var p1 = new PersonModel("Tom", ERole.Lead);
            var p2 = new PersonModel("Henk", ERole.Developer);

            var project = new Project("AvansDevOps", p1);
            
            // Act
            var source = new Source("AvansDevOps_Web");
            project.AddSource(source);

            // Assert
            Assert.Contains(source, project.GetSources());
            Assert.Equal("AvansDevOps_Web", project.GetSources().First().GetName());

        }

        [Fact]
        public void Adding_Same_Source_To_Project_Should_Throw_NotSupportedException()
        {
            // Arrange
            var p1 = new PersonModel("Tom", ERole.Lead);
            var p2 = new PersonModel("Henk", ERole.Developer);

            var project = new Project("AvansDevOps", p1);

            // Act
            var source = new Source("AvansDevOps_Web");
            project.AddSource(source);

            // Assert
            Assert.Throws<NotSupportedException>(() => project.AddSource(source));

        }

        [Fact] public void Adding_Commit_To_Source_Should_Not_Throw_Exception()
        {
            // Arrange
            var p1 = new PersonModel("Tom", ERole.Lead);
            var p2 = new PersonModel("Henk", ERole.Developer);

            var project = new Project("AvansDevOps", p1);
            var source = new Source("AvansDevOps_Web");
            project.AddSource(source);

            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);

            backlog.AddBacklogItem(new BacklogItem("User should be able to log into the web interface", "foo", p2, 3, backlog));
            
            // Act
            var factory = new CommitFactory();
            var commit = factory.MakeCommit("Adding login section to website", "Reference to backlogItem",
                project.GetBacklog().GetBacklogItems().Find(backlogItem => backlogItem.GetDescription() == "foo"));
            project.GetSources().First().AddCommit(commit);


            // Assert
            Assert.Equal("Adding login section to website", project.GetSources().First().GetCommits().Find((foundCommit) => foundCommit == commit).GetTitle());
            Assert.Equal("Reference to backlogItem", project.GetSources().First().GetCommits().Find((foundCommit) => foundCommit == commit).GetDescription());
            Assert.Equal(project.GetBacklog().GetBacklogItems().Find(backlogItem => backlogItem.GetDescription() == "foo"), project.GetSources().First().GetCommits().Find((foundCommit) => foundCommit == commit).GetBacklogItem());
            Assert.Contains(commit, project.GetSources().First().GetCommits());

        }

        [Fact]
        public void Adding_Same_Commit_To_Source_Should_Throw_NotSupportedException()
        {
            // Arrange
            var p1 = new PersonModel("Tom", ERole.Lead);
            var p2 = new PersonModel("Henk", ERole.Developer);

            var project = new Project("AvansDevOps", p1);
            var source = new Source("AvansDevOps_Web");
            project.AddSource(source);

            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);

            backlog.AddBacklogItem(new BacklogItem("User should be able to log into the web interface", "foo", p2, 3, backlog));

            // Act
            var factory = new CommitFactory();
            var commit = factory.MakeCommit("Adding login section to website", "Reference to backlogItem",
                project.GetBacklog().GetBacklogItems().Find(backlogItem => backlogItem.GetDescription() == "foo"));
            project.GetSources().First().AddCommit(commit);


            // Assert
            Assert.Contains(commit, project.GetSources().First().GetCommits());
            Assert.Throws<NotSupportedException>(() => project.GetSources().First().AddCommit(commit));

        }

        [Fact]
        public void Deleting_An_Existing_Source_From_A_Project_Should_Not_Throw_Exception()
        {
            // Arrange
            var p1 = new PersonModel("Tom", ERole.Lead);
            var p2 = new PersonModel("Henk", ERole.Developer);

            var project = new Project("AvansDevOps", p1);
            var source = new Source("AvansDevOps_Web");
            project.AddSource(source);

            var backlog = new BacklogModel(project);
            project.AddBacklog(backlog);

            backlog.AddBacklogItem(new BacklogItem("User should be able to log into the web interface", "foo", p2, 3, backlog));

            // Act
            var factory = new CommitFactory();
            var commit = factory.MakeCommit("Adding login section to website", "Reference to backlogItem",
                project.GetBacklog().GetBacklogItems().Find(backlogItem => backlogItem.GetDescription() == "foo"));
            project.GetSources().First().AddCommit(commit);


            // Assert
            project.RemoveSource(source);
            Assert.Empty(project.GetSources());
        }
    }
}
