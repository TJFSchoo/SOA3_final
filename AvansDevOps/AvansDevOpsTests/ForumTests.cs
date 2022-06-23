using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvansDevOps.Backlog;
using AvansDevOps.Channel;
using AvansDevOps.Forum;
using AvansDevOps.Person;
using Xunit;
using Xunit.Sdk;

namespace AvansDevOpsTests
{
    public class ForumTests
    {
        [Fact]
        public void Created_Thread_Can_Add_Thread()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string threadOneName = "ThreadOne title.";

            ThreadModel threadOne = new ThreadModel(threadOneName, currentDateTime, person, task);

            // Act
            forum.NewThread(threadOne);

            // Assert
            Assert.Contains(threadOne, forum.GetThreads());
        }

        [Fact]
        public void Created_Thread_Can_Remove_Multiple_Threads()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string threadOneName = "ThreadOne title.";

            ThreadModel threadOne = new ThreadModel(threadOneName, currentDateTime, person, task);

            // Act
            forum.NewThread(threadOne);
            forum.ArchiveThread(threadOne);

            // Assert
            Assert.DoesNotContain(threadOne, forum.GetThreads());
        }

        [Fact]
        public void Created_Thread_Can_Remove_Thread()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string threadOneName = "ThreadOne title.";

            ThreadModel threadOne = new ThreadModel(threadOneName, currentDateTime, person, task);

            // Act
            forum.NewThread(threadOne);
            forum.ArchiveThread(threadOne);

            // Assert
            Assert.DoesNotContain(threadOne, forum.GetThreads());
        }

        [Fact]
        public void Created_Thread_Can_Not_Remove_Thread_Gives_NotSupportedException()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string threadOneName = "ThreadOne title.";

            ThreadModel threadOne = new ThreadModel(threadOneName, currentDateTime, person, task);

            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => forum.ArchiveThread(threadOne));
            Assert.DoesNotContain(threadOne, forum.GetThreads());
        }

        [Fact]
        public void Created_Thread_Can_Add_Multiple_Thread()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string threadOneName = "ThreadOne title.";
            const string threadTwoName = "ThreadTwo title.";

            ThreadModel threadOne = new ThreadModel(threadOneName, currentDateTime, person, task);
            ThreadModel threadTwo = new ThreadModel(threadTwoName, currentDateTime, person, task);

            // Act
            forum.NewThread(threadOne);
            forum.NewThread(threadTwo);

            // Assert
            Assert.Contains(threadOne, forum.GetThreads());
            Assert.Contains(threadTwo, forum.GetThreads());
        }

        [Fact]
        public void Created_Thread_Can_Not_Remove_Multiple_Threads_Gives_NotSupportedException()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string threadOneName = "ThreadOne title.";
            const string threadTwoName = "ThreadTwo title.";

            ThreadModel threadOne = new ThreadModel(threadOneName, currentDateTime, person, task);
            ThreadModel threadTwo = new ThreadModel(threadTwoName, currentDateTime, person, task);

            // Act

            // Assert
            Assert.Throws<NotSupportedException>(() => forum.ArchiveThread(threadOne));
            Assert.Throws<NotSupportedException>(() => forum.ArchiveThread(threadTwo));
            Assert.DoesNotContain(threadOne, forum.GetThreads());
            Assert.DoesNotContain(threadTwo, forum.GetThreads());
        }

        [Fact]
        public void Created_Thread_Can_Add_Thread_On_Task_State_ToDo()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string threadOneName = "ThreadOne title.";

            ThreadModel threadOne = new ThreadModel(threadOneName, currentDateTime, person, task);

            // Act
            forum.NewThread(threadOne);

            // Assert
            Assert.Equal(ETaskState.Todo, task.GetState());
            Assert.Contains(threadOne, forum.GetThreads());
        }

        [Fact]
        public void Created_Thread_Can_Add_Thread_On_Task_State_Active()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string threadOneName = "ThreadOne title.";

            ThreadModel threadOne = new ThreadModel(threadOneName, currentDateTime, person, task);

            // Act
            task.NextState();
            forum.NewThread(threadOne);

            // Assert
            Assert.Equal(ETaskState.Active, task.GetState());
            Assert.Contains(threadOne, forum.GetThreads());
        }

        [Fact]
        public void Created_Thread_Cant_Add_Thread_On_Task_State_Done_Gives_NotSupportedException()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string threadOneName = "ThreadOne title.";

            ThreadModel threadOne = new ThreadModel(threadOneName, currentDateTime, person, task);

            // Act
            task.NextState();
            task.NextState();

            // Assert
            Assert.Equal(ETaskState.Done, task.GetState());
            Assert.Throws<NotSupportedException>(() => forum.NewThread(threadOne));
        }

        [Fact]
        public void Created_Thread_Cant_Add_Thread_With_Empty_Title_Gives_Null_Exception()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string threadOneName = "";

            ThreadModel threadOne = new ThreadModel(threadOneName, currentDateTime, person, task);

            // Act


            // Assert
            Assert.Throws<ArgumentNullException>(() => forum.NewThread(threadOne));
        }

        [Fact]
        public void Added_Comment_Can_Add_Comment_To_Existing_Thread()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string name = "ThreadOne title.";

            ThreadModel thread = new ThreadModel(name, currentDateTime, person, task);

            PersonModel personTwo = new PersonModel("Tom", ERole.Developer);
            const string contentCommentOne = "This is a test comment one.";

            CommentModel comment = new CommentModel(thread, personTwo, currentDateTime, contentCommentOne);

            // Act
            forum.NewThread(thread);
            thread.AddComment(comment);

            // Assert
            Assert.Contains(thread, forum.GetThreads());
            Assert.Contains(comment, thread.GetComments());
        }

        [Fact]
        public void Added_Comment_Can_Add_Multiple_Comments_To_Existing_Thread()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string name = "ThreadOne title.";

            ThreadModel thread = new ThreadModel(name, currentDateTime, person, task);

            PersonModel personTwo = new PersonModel("Tom", ERole.Developer);
            const string contentCommentOne = "This is a test comment one.";

            PersonModel personThree = new PersonModel("Jan", ERole.Developer);
            const string contentCommentTwo = "This is a test comment two.";

            CommentModel commentOne = new CommentModel(thread, personTwo, currentDateTime, contentCommentOne);
            CommentModel commentTwo = new CommentModel(thread, personThree, currentDateTime, contentCommentTwo);

            // Act
            forum.NewThread(thread);
            thread.AddComment(commentOne);
            thread.AddComment(commentTwo);

            // Assert
            Assert.Contains(thread, forum.GetThreads());
            Assert.Contains(commentOne, thread.GetComments());
            Assert.Contains(commentTwo, thread.GetComments());
        }

        [Fact]
        public void Added_Comment_Can_Remove_Comment_To_Existing_Thread()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string name = "ThreadOne title.";

            ThreadModel thread = new ThreadModel(name, currentDateTime, person, task);

            PersonModel personTwo = new PersonModel("Tom", ERole.Developer);
            const string contentCommentOne = "This is a test comment one.";

            CommentModel comment = new CommentModel(thread, personTwo, currentDateTime, contentCommentOne);

            // Act
            forum.NewThread(thread);
            thread.AddComment(comment);
            thread.DeleteComment(comment);

            // Assert
            Assert.Contains(thread, forum.GetThreads());
            Assert.DoesNotContain(comment, thread.GetComments());
        }

        [Fact]
        public void Added_Comment_Can_Remove_Multiple_Comments_To_Existing_Thread()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string name = "ThreadOne title.";

            ThreadModel thread = new ThreadModel(name, currentDateTime, person, task);

            PersonModel personTwo = new PersonModel("Tom", ERole.Developer);
            const string contentCommentOne = "This is a test comment one.";

            PersonModel personThree = new PersonModel("Jan", ERole.Developer);
            const string contentCommentTwo = "This is a test comment two.";

            CommentModel commentOne = new CommentModel(thread, personTwo, currentDateTime, contentCommentOne);
            CommentModel commentTwo = new CommentModel(thread, personThree, currentDateTime, contentCommentTwo);

            // Act
            forum.NewThread(thread);
            thread.AddComment(commentOne);
            thread.AddComment(commentTwo);
            thread.DeleteComment(commentOne);
            thread.DeleteComment(commentTwo);

            // Assert
            Assert.Contains(thread, forum.GetThreads());
            Assert.DoesNotContain(commentOne, thread.GetComments());
            Assert.DoesNotContain(commentTwo, thread.GetComments());
        }

        [Fact]
        public void Created_Thread_Cant_Add_Empty_Comment_On_Thread_Gives_Null_Exception()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string name = "ThreadOne title.";

            ThreadModel thread = new ThreadModel(name, currentDateTime, person, task);

            PersonModel personTwo = new PersonModel("Tom", ERole.Developer);
            const string contentCommentOne = "";

            CommentModel commentOne = new CommentModel(thread, personTwo, currentDateTime, contentCommentOne);

            // Act
            forum.NewThread(thread);

            // Assert
            Assert.DoesNotContain(commentOne, thread.GetComments());
            Assert.Throws<ArgumentNullException>(() => thread.AddComment(commentOne));
        }

        [Fact]
        public void Created_Thread_Cant_Add_Multiple_Empty_Comments_On_Thread_Gives_Null_()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string name = "ThreadOne title.";

            ThreadModel thread = new ThreadModel(name, currentDateTime, person, task);

            PersonModel personTwo = new PersonModel("Tom", ERole.Developer);
            const string contentCommentOne = "";

            PersonModel personThree = new PersonModel("Jan", ERole.Developer);
            const string contentCommentTwo = "";

            CommentModel commentOne = new CommentModel(thread, personTwo, currentDateTime, contentCommentOne);
            CommentModel commentTwo = new CommentModel(thread, personTwo, currentDateTime, contentCommentTwo);

            // Act
            forum.NewThread(thread);

            // Assert
            Assert.DoesNotContain(commentOne, thread.GetComments());
            Assert.Throws<ArgumentNullException>(() => thread.AddComment(commentOne));
            Assert.DoesNotContain(commentTwo, thread.GetComments());
            Assert.Throws<ArgumentNullException>(() => thread.AddComment(commentTwo));
        }

        [Fact]
        public void Created_Thread_Cant_Add_Comment_On_Thread_State_Done_Gives_NotSupportedException()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string name = "ThreadOne title.";

            ThreadModel thread = new ThreadModel(name, currentDateTime, person, task);

            PersonModel personTwo = new PersonModel("Tom", ERole.Developer);
            const string contentCommentOne = "ThreadOne title.";

            CommentModel commentOne = new CommentModel(thread, personTwo, currentDateTime, contentCommentOne);

            // Act
            forum.NewThread(thread);
            task.NextState();
            task.NextState();

            // Assert
            Assert.DoesNotContain(commentOne, thread.GetComments());
            Assert.Throws<NotSupportedException>(() => thread.AddComment(commentOne));
        }

        [Fact]
        public void Created_Thread_Cant_Delete_Comment_On_Thread_State_Done_Gives_NotSupportedException()
        {
            // Arrange
            ForumModel forum = new ForumModel();

            DateTime currentDateTime = DateTime.Now;
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            Task task = new Task("Sample Task", person);
            const string name = "ThreadOne title.";

            ThreadModel thread = new ThreadModel(name, currentDateTime, person, task);

            PersonModel personTwo = new PersonModel("Tom", ERole.Developer);
            const string contentCommentOne = "ThreadOne title.";

            CommentModel commentOne = new CommentModel(thread, personTwo, currentDateTime, contentCommentOne);

            // Act
            forum.NewThread(thread);
            task.NextState();
            task.NextState();

            // Assert
            Assert.DoesNotContain(commentOne, thread.GetComments());
            Assert.Throws<NotSupportedException>(() => thread.DeleteComment(commentOne));
        }
    }

}
